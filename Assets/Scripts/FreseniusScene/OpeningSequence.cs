using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Fresenius
{
    public enum sequence
    {
        donothing = -100,
        openingText = 0,
        haveYouUseThisApp,
        wouldYouLikeToBeReminded,
        questionSkipTutor,
        startTutor = 4,
        startConfiguringSpeed = 5,
        startBallTesting = 6,
        finish = 7,
    }

    public class OpeningSequence : MonoBehaviour
    {
        [Header("Category Variable")]
        public string categoryUrl = "https://files.lfi.rwth-aachen.de/myscore/MyScoreAssets/Fresenius/Category.txt";
        public SimpleObjectPool categoryPool;
        public Transform categoryContainer;
        public List<string> categoryList = new List<string>();

        [Header("Vid Displayer")]
        public GameObject videoOp;
        public GameObject videoTutor;
        public bool videoFinish;

        [Header("Text Displayer")]
        public QuestionDisplayer questionDisplay;
        //text
        public TextMeshProUGUI text_WelcomeToVIA;
        public TextMeshProUGUI text_WouldYouLikeToBeReminded;
        public TextMeshProUGUI text_startTutor;
        public TextMeshProUGUI text_tutor_1;
        public TextMeshProUGUI text_tutor_2;
        public TextMeshProUGUI text_tutor_3;
        public TextMeshProUGUI text_tutor_4;
        public TextMeshProUGUI text_tutor_5;
        public TextMeshProUGUI text_tutor_6;
        public TextMeshProUGUI text_tutor_StartConfiguration;
        public TextMeshProUGUI text_tutor_8;
        public TextMeshProUGUI text_tutor_9;
        public TextMeshProUGUI text_Finish;

        public bool isHit = false;
        public bool isYes = false;

        [Header("Extra balls")]
        public GameObject ballYes;
        public GameObject ballNo;

        public GameObject ballFaster;
        public GameObject ballSlower;

        public List<GameObject> testBallsList = new List<GameObject>();


        public int currentSeq = -1;

        [Header("Language selection")]
        public GameObject ballEnglish;
        public GameObject ballGerman;

        
        [Header("Script Reference")]
        public GameObject roomManager;


        private void Start()
        {
            currentSeq = -1;

            //start the video
            /*
            videoOp.SetActive(true);
            StartCoroutine(waitToChangeSequence(3f, sequence.openingText));
            */
            StartCoroutine(InitCategoryBall());
        }


        private float nextActionTime = 0.0f;
        public float period = 0.1f;
        private void Update()
        { 
            if (Time.time > nextActionTime) 
            { 
                nextActionTime = Time.time + period;
                //execute something


                if (isHit)
                {
                    isHit = false;
                    EnableYesNoBall(false);

                    //increment the currentsequence.
                    currentSeq++;

                    if(currentSeq == (int)sequence.haveYouUseThisApp)
                    {
                        _ = isYes ? currentSeq = (int)sequence.wouldYouLikeToBeReminded : currentSeq = (int)sequence.startTutor;
                    }


                    //check if there is any special sequence needed.
                    if (currentSeq == (int)sequence.questionSkipTutor)
                    {
                        _ = isYes? currentSeq = (int)sequence.startTutor : currentSeq = (int)sequence.finish;
                    }


                    //do something here
                    ChangeSequence(currentSeq);
                }

                
            }
        }


        private float nextFupdateTime = 0f;
        public float periodFupdate = 0.1f;
        private void FixedUpdate() // exclusively used for configuring speed
        {
            if (Time.time > nextFupdateTime)
            {
                nextFupdateTime = Time.time + periodFupdate;
                //execute something Here

                //start the configuring speed
                if (currentSeq == (int)sequence.startConfiguringSpeed)
                {
                    print("something");
                    //loop the text
                    //get total char 
                    int i = text_tutor_StartConfiguration.text.Length;
                    //get current char displayed
                    int j = questionDisplay.textDisplay.text.Length;


                    if (i == j)
                    {
                        questionDisplay.textDisplay.text = "";
                    }
                    else
                    {
                        questionDisplay.textDisplay.text += text_tutor_StartConfiguration.text[j];
                    }
                }

            }
        }


        public void TriggerTheOpeningSequence()
        {
            videoOp.SetActive(true);
            //the real second is 34
            StartCoroutine(waitToChangeSequence(34f, sequence.openingText));
        }


        /// <summary>
        /// change the sequence 
        /// </summary>
        /// <param name="_val"></param>
        private void ChangeSequence(int _val)
        {
            switch (_val)
            {
                case (int) sequence.openingText:
                
                    EnableYesNoBall(false);
                    questionDisplay.gameObject.SetActive(true);
                    videoOp.gameObject.SetActive(false);
                    StartCoroutine(questionDisplay.DisplayText_NoAutoTurnOff(text_WelcomeToVIA.text, 0.05f, result =>
                    {
                        //show ball yes and no
                        EnableYesNoBall(true);
                    })); break;


                case (int)sequence.wouldYouLikeToBeReminded:
                    StartCoroutine(questionDisplay.DisplayText_NoAutoTurnOff(text_WouldYouLikeToBeReminded.text, 0.1f, result =>
                    {
                        //show ball yes and no
                        ballYes.gameObject.SetActive(true);
                        ballNo.gameObject.SetActive(true);
                    })); break;


                    
                case (int)sequence.startTutor:
                    StartCoroutine(TutorTalking()); break;

                case (int)sequence.startBallTesting:
                    StartCoroutine(TutorBallTesting()); break;


                case (int)sequence.finish:

                    EnableLanguageBall(false);
                    EnableTutorBalls(false);
                    StartCoroutine(questionDisplay.DisplayText(text_Finish.text, periodFupdate, result =>
                    {
                        //activate the room manager
                        roomManager.SetActive(true);
                    })); break;



                default: break;

            }
        }


        /// <summary>
        /// Just to loop between a given text
        /// </summary>
        /// <returns></returns>
        private IEnumerator TutorTalking()
        {
            /*
            yield return StartCoroutine(questionDisplay.DisplayText(text_tutor_1.text, periodFupdate, result => { }));
            yield return StartCoroutine(questionDisplay.DisplayText(text_tutor_2.text, periodFupdate, result => { }));
            yield return StartCoroutine(questionDisplay.DisplayText(text_tutor_3.text, periodFupdate, result => { }));
            yield return StartCoroutine(questionDisplay.DisplayText(text_tutor_4.text, periodFupdate, result => { }));
            yield return StartCoroutine(questionDisplay.DisplayText(text_tutor_5.text, periodFupdate, result => { }));
            */

            videoTutor.gameObject.SetActive(true);
            questionDisplay.gameObject.SetActive(false);
            yield return StartCoroutine(waitToChangeSequence(32.5f, sequence.donothing));

            videoTutor.gameObject.SetActive(false);
            questionDisplay.gameObject.SetActive(true);

            yield return StartCoroutine(questionDisplay.DisplayText(text_tutor_6.text, periodFupdate, result => { }));

            questionDisplay.textDisplay.text = ""; // reset the text display
            questionDisplay.TurnOnDisplay(true);
            currentSeq = (int)sequence.startConfiguringSpeed;
            ballFaster.gameObject.SetActive(true);
            ballSlower.gameObject.SetActive(true);
            ballYes.gameObject.SetActive(true);
        }


        private IEnumerator TutorBallTesting()
        {
            ballFaster.gameObject.SetActive(false);
            ballSlower.gameObject.SetActive(false);
            yield return StartCoroutine(questionDisplay.DisplayText(text_tutor_8.text, periodFupdate, result => { }));
            yield return StartCoroutine(questionDisplay.DisplayText(text_tutor_9.text, periodFupdate, result => { }));

            EnableTutorBalls(true);

        }


        //enable or disable the yes and no ball selection
        public void EnableYesNoBall(bool _val)
        {
            ballYes.gameObject.SetActive(_val);
            ballNo.gameObject.SetActive(_val);
        }

        //enable or disable the tutors ball
        public void EnableTutorBalls(bool _val)
        {
            foreach(GameObject go  in testBallsList)
            {
                go.SetActive(_val);
            }
        }

        //enable or disable the language ball
        public void EnableLanguageBall(bool _val)
        {
            ballEnglish.SetActive(_val);
            ballGerman.SetActive(_val);
        }

        private IEnumerator waitToChangeSequence(float _wait, sequence _whichSeq)
        {
            yield return new WaitForSeconds(_wait);
            currentSeq = (int)_whichSeq;

            if (_whichSeq == sequence.donothing)
                yield return null ;

            //do something here
            ChangeSequence(currentSeq);
            
        }


        //Just to init the category
        private IEnumerator InitCategoryBall()
        {
            //clear all the child first
            foreach (Transform tr in categoryContainer)
                Destroy(tr.gameObject);



            int x = 0;
            int z = 0;
            //load all the data in the SQL
            yield return StartCoroutine(SQLloader.LoadURL(categoryUrl, returnVal =>
            {
                //clean the url
                string str = SQLloader.stringCleaner_OnlyEnter(returnVal);

                //split the loaded Text to a list
                categoryList = SQLloader.splitGivenString(str, "|");


                foreach (string _s in categoryList)
                {
                    print("the id " + _s);

                    GameObject cateBall = categoryPool.GetObject();
                    CategoryBall cb = cateBall.GetComponent<CategoryBall>();

                    //put it to the main
                    cateBall.transform.SetParent(categoryContainer, false);


                    //set the name
                    cb.SetCategoryText(_s);

                    //spawn the categoryBall
                    cb.transform.localPosition = new Vector3(x * 0.7f, 0, z * 1f);
                    x++;
                    if (x % 3 == 0)
                    {
                        x = 0;
                        z++;
                    }
                }
            }));

        }
    }
}
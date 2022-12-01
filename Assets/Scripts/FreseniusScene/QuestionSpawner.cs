using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Globalization;
using System;

namespace Fresenius
{
    public class QuestionSpawner : MonoBehaviour
    {
        [Header("Question Variable")]
        public string questionUrl = "https://files.lfi.rwth-aachen.de/myscore/MyScoreAssets/Fresenius/Questions_DE.txt";
        public GameObject questionMain;
        public TextMeshProUGUI questionText;
        public QuestionDisplayer questionDisplayer;

        public List<string> questionList = new List<string>();
        public int currentQuestionID = -1;
        public List<int> savedAnsweredQuestList = new List<int>();
        public bool randomlySpawnedQuestion = false;


        [Header("Answer Variable")]
        public bool useRandomAnswerBallEndPos = false;
        public List<Transform> answerObjList;
        public AnswerBall startBall;
        public AnswerBall continueBall;

        public List<int> normalFinalPos = new List<int>();


        [Header("TankVariable")]
        public string tankUrl = "https://files.lfi.rwth-aachen.de/myscore/MyScoreAssets/Fresenius/TanksID_DE.txt";
        public Transform tankMain;
        public List<CollectorTank> tankObjList = new List<CollectorTank>();
        public List<string> tankIDList = new List<string>();

        [Header("Ending Variable")]
        public SkinnedMeshRenderer walls;
        public GameObject waterEffects;

        [Header("Result Variable")]
        public GameObject resultCanvas;
        public TextMeshProUGUI textID;
        public List<TextMeshProUGUI> textStrengthList;
        public GameObject message_Error;
        public GameObject message_Sucess;

        [Header("Script Reference")]
        public SimpleObjectPool simpleObjPool;


        //to move to pref dataList
        string savedUniqueID = "savedUniqueID";
        string savedAnswers = "savedAnswers";
        string savedSpeed = "savedSpeed";
        string savedTankAnswers = "savedTankAnswers";
        string savedHighestAnswers = "savedHighestAnswers";
        string listOfAnswer = "listOfAnswer";
        string savedCategory = "savedCategory";
        public float currentSpeed = 0.1f;
        public float minSpeed = 0.01f;
        public float maxSpeed = 1f;


        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(InitTankObject());
            questionText.text = "";

            int i = 0;
            foreach(Transform tr in answerObjList)
            {
                i++;
                tr.GetComponent<AnswerBall>().answerVal = i;
            }
            startBall.gameObject.SetActive(true);
            continueBall.gameObject.SetActive(true);
            //PlayerPrefs.SetString(savedAnswers, "1|5");

            //set the speed
            currentSpeed = PlayerPrefs.GetFloat(savedSpeed);

            if (currentSpeed <= minSpeed)
                currentSpeed = minSpeed;
            else if (currentSpeed >= 1f)
                currentSpeed = maxSpeed;

            questionDisplayer.gameObject.SetActive(true);
            message_Error.SetActive(false);
            message_Sucess.SetActive(false);

            //testOrder();
        }

        /*
        private void testOrder()
        {
            Dictionary<string, float> something= new Dictionary<string, float>();
            something.Add("a ", 10);
            something.Add("b ", 1);
            something.Add("c ", 7);
            something.Add("d ", 21);
            something.Add("e ", 2);
            something.Add("f ", 39);
            something.Add("g ", 333);

            int totalTank = 7;
            int i = 0;
            int showTank = 3;

            foreach (KeyValuePair<string, float> author in something.OrderByDescending(key => key.Value))
            {
                if(i < totalTank - showTank)
                {
                    print("val " + author.Value);
                }
                i++;
            }
        }
        */
        

        private void Update()
        {
            
        }



        /// <summary>
        /// Spawn the next question
        /// </summary>
        public void SpawnNext()
        {
            //get the new questionID
            //To Make the question randomly spawned.
            if(randomlySpawnedQuestion)
                currentQuestionID  = ConverterFunction.getNewNumberOutsideGivenList(savedAnsweredQuestList, 0, questionList.Count -1);

            else//not to randomly spawned
                currentQuestionID++;
            
            //check if the question hasnt been removed yet
            if (questionList[currentQuestionID] == "")
            {
                print("Question no exist -> find another");
                SpawnNext();
                return;
            }


            //spawn the question -> and eventually the answer
            string[] splited = SQLloader.splitGivenString(questionList[currentQuestionID]).ToArray(); //questionList[currentQuestionID].Split('||');
            string i = SQLloader.stringCleaner(splited[1]);

            SpawnQuestion(splited[0], ConverterFunction.StringToInt(i) - 1); // "-1" because the tank start at 1 but list start at 0



            
            //spawn the question
            /*
            string[] splited = SQLloader.splitGivenString(questionList[currentQuestionID]).ToArray(); //questionList[currentQuestionID].Split('||');
            SpawnQuestion(splited[0], 1);
            

            //clean the string to avoid space and stuff
            //then -> spawn the answer
            string i = SQLloader.stringCleaner(splited[1]);
            SpawnAnswer(ConverterFunction.StringToInt(i) - 1); // "-1" because the tank start at 1 but list start at 0
            //*/
        }

        /*

        /// <summary>
        /// Spawn the next question
        /// </summary>
        public void SpawnNext()
        {
            currentQuestionID++;
            //check if it exist
            if (questionList[currentQuestionID] == "")
            {
                print("Question no exist");
                return;
            }


            //split the question and tank


            //spawn the question
            string[] splited = SQLloader.splitGivenString(questionList[currentQuestionID]).ToArray(); //questionList[currentQuestionID].Split('||');
            SpawnQuestion(splited[0]);

            //clean the string to avoid space and stuff
            //then -> spawn the answer
            string i = SQLloader.stringCleaner(splited[1]);
            SpawnAnswer(ConverterFunction.StringToInt(i) - 1); // "-1" because the tank start at 1 but list start at 0
        }

        */

        /// <summary>
        /// Load all the question from the web
        /// </summary>
        public void LoadAllQuestions()
        {
            StartCoroutine(SQLloader.LoadURL(questionUrl, returnVal =>
            { 
                //clean the url
                string str = SQLloader.stringCleaner_OnlyEnter(returnVal);
                
                //split the loaded Text to a list
                questionList = SQLloader.splitGivenString(str, "~");


                //Advance saving
                string[] aList = PlayerPrefs.GetString(savedAnswers).Split('|');
                
                //iterate through the saved answer list
                for (int i = 0; i < aList.Length -1; i++)
                {
                    //convert to int
                    int a = ConverterFunction.StringToInt(aList[i]);
                    savedAnsweredQuestList.Add(a);
                    //set the question to null -> so it will be excluded from the list
                    questionList[a] = "";
                }

                //to do
                /*
                - dont shuffle them
                    - Question 1
                    - Question 2
                    - Question 3
                    - Question 4
                    - Question 5
                    - Question 6
                    - Question 7
                    - Question 8
                    
                - get the prefDataList of what has been answered(e.g; 3,6,8) -> saved it in the List<int>savedList
                    - foreach of those saved question -> make it as null -> ""

                - create a new var "totalSavedQuestion"
                - the list stay the same

                - everytime spawnNext is hit
                - get random number outside the list
                - check the prefDataList
                - if it is exist -> do nothing and go with the next one
                - 

                */

                //shuffle the list
                //questionList.Shuffle();

                //IF all question has NOT been answered
                if (!allQuestionAnswered())
                {
                    SpawnNext();
                }
                else
                {
                    TriggerEndScene();
                    ShowTheHighestTankValue();
                    ShowIdAndSavedInTheDatabase();
                }
            }));
        }



        /// <summary>
        /// Spawn the question with the given string
        /// </summary>
        /// <param name="_str"></param>
        private void SpawnQuestion(string _str, int _tankID)
        {
            questionDisplayer.TurnOnDisplay(true);
            //you get the question -> put it in the question displayer
            StartCoroutine(questionDisplayer.DisplayText(_str, currentSpeed, returnValue =>
            {
                //after finish spawn the answer
                SpawnAnswer(_tankID);
            }
            ));


            //questionText.text = _str;
        }




        Vector2 rand = new Vector2(-1, 1f);
        List<Vector2> points = new List<Vector2>();
        /// <summary>
        /// To Spawn Answer and assign the tank id
        /// </summary>
        /// <param name="_tankID"></param>
        public void SpawnAnswer(int _tankID)
        {
            points.Clear();
            //using some algorithm therefore the object wont spawn closely/overlap to one another
            points = PoissonDiscSampling.GeneratePoints(0.5f, new Vector2(2.5f, 1f), 30);

            normalFinalPos.Clear();
            int a = 0;
            
            int i = 0;              
            //start spawning if the points is actually more than the child
            if (points.Count >= answerObjList.Count)
            {
                foreach (Transform _obj in answerObjList)
                {
                    //assigning the tankID -> this is for when the user hit the "ball" and it will fill the corresponding tank
                    print("TankID " + _tankID);
                    
                    _obj.GetComponent<AnswerBall>().AssignTankID(tankObjList[_tankID]);


                    //enable the gameObject
                    _obj.gameObject.SetActive(true);


                    if (useRandomAnswerBallEndPos)
                    {
                        //set the start position
                        Vector3 initPos = new Vector3(UnityEngine.Random.Range(rand.x, rand.y), UnityEngine.Random.Range(rand.x, rand.y), 18f);
                        _obj.localPosition = initPos;
                        //get the final pos
                        Vector3 finalPos = new Vector3(points[i].x, points[i].y, 0);


                        _obj.GetComponent<AnswerBall>().smoothness = UnityEngine.Random.Range(3f, 6f);
                        _obj.GetComponent<AnswerBall>().initPos = initPos;
                        _obj.GetComponent<AnswerBall>().finalPos = finalPos;
                    }

                    else
                    {
                        Vector3 initPos = new Vector3(0, 0, 18f);
                        _obj.localPosition = initPos;

                        a = ConverterFunction.getNewNumberOutsideGivenList(normalFinalPos, 0, 4);
                        if(a == -1)
                            a = ConverterFunction.getNewNumberOutsideGivenList(normalFinalPos, 0, 4);

                        print("a " + a);
                        //get final pos
                        Vector3 finalPos = new Vector3(-1.3f + a * 0.65f, 0 , 0);

                        //_obj.GetComponent<AnswerBall>().smoothness = Random.Range(3f, 6f);
                        _obj.GetComponent<AnswerBall>().initPos = initPos;
                        _obj.GetComponent<AnswerBall>().finalPos = finalPos;

                        //Note: -> below to make it move the exact position
                        //start the lerping to the final position.
                        //StartCoroutine(LerpingExtensions.MoveToLocal(_obj, finalPos, 2f));

                    }
                    //randomize the scaling 
                    //_obj.localScale = Vector3.zero;
                    //StartCoroutine(LerpingExtensions.ScaleTo(_obj, Vector3.one, 2f));

                    //iterate
                    normalFinalPos.Add(a);
                    i++;
                }
            }
        }


        /// <summary>
        /// check if all question has been answered
        /// </summary>
        /// <returns></returns>
        public bool allQuestionAnswered()
        {
            if (savedAnsweredQuestList.Count() == questionList.Count())
                return true;
            
            else 
                return false;
        }



        /// <summary>
        /// Trigger the end scene
        /// </summary>
        public void TriggerEndScene()
        {
            //deactivate the question
            questionMain.SetActive(false);

            //deactivate the answer ball
            ActivateAnswerObj(false);

            //show the water tank
            StartCoroutine(LerpingExtensions.LerpBlendshapes(walls, 0, 100, 7));
            StartCoroutine(LerpingExtensions.MoveTo(tankMain, new Vector3(tankMain.position.x, 0, tankMain.position.z), 7));
            StartCoroutine(LerpingExtensions.RotateTo(tankMain, Quaternion.Euler(0f,-125,0f) , 7));

            waterEffects.SetActive(false);

        }

        public void ActivateAnswerObj(bool _val)
        {
            foreach (Transform t in answerObjList)
                t.gameObject.SetActive(_val);

        }


        /// <summary>
        /// To setup all the tank
        /// </summary>
        /// <returns></returns>
        private IEnumerator InitTankObject()
        {
            //clear all the child first
            foreach (Transform tr in tankMain)
                Destroy(tr.gameObject);

            //clear the tanklist
            tankObjList.Clear();

            //START the Instantiate all the tanks.

            //load all the data in the SQL
            yield return StartCoroutine(SQLloader.LoadURL(tankUrl, returnVal =>
            {
                //clean the url
                string str = SQLloader.stringCleaner_OnlyEnter(returnVal);

                //split the loaded Text to a list
                tankIDList = SQLloader.splitGivenString(str, "~");

                //sort the tankID (Natural Sorting)
                var orderedString = tankIDList.OrderBy(g => new Tuple<int, string>(g.ToCharArray().All(char.IsDigit) ? int.Parse(g) : int.MaxValue, g));
                //tankIDList.Sort();
                


                //variable for spawning
                int x = 0;
                int z = 0;
                float rad = 7;
                Vector3 center = tankMain.position;


                foreach (string _s in tankIDList)
                {
                    //print("asdf " + _s);
                    GameObject tank = simpleObjPool.GetObject();
                    CollectorTank ct = tank.GetComponent<CollectorTank>();


                    //assígn the Text
                    ct.AssignTankName(SQLloader.splitGivenString(_s).ElementAt(1));
                    ct.currentValue = 0;

                    //put it to the main
                    tank.transform.SetParent(tankMain, false);




                    //spawn the tank in a CIRCLE/CIRCULAR fashion
                    x = 270 / tankIDList.Count * z;
                    tank.transform.position = SpawnInCircle(center, rad, x);
                    //iterate it
                    z++;


                    //spawn the tank in a RECTANGULAR fashion
                    /*
                    tank.transform.localPosition = new Vector3(x * 1.5f, 0, z * 1f);
                    x++;
                    if (x % 3 == 0)
                    {
                        x = 0;
                        z++;
                    }
                    */


                    //add it to the tankObj list
                    tankObjList.Add(ct);
                }

                //load the saved Value
                LoadSavedTankValue();

            }));
        }


        /// <summary>
        /// to show the highest tank value
        /// </summary>
        public void ShowTheHighestTankValue()
        {
            //key,value
            Dictionary<CollectorTank, float> tankValList = new Dictionary<CollectorTank, float>();
            //add the list  to the dictionary
            foreach(CollectorTank ct in tankMain.GetComponentsInChildren<CollectorTank>())
            {
                tankValList.Add(ct, ct.currentValue);
                ct.activateThisTank(false); 
            }


            //get total tank
            int totalTank = tankValList.Count;

            int i = 0;
            int shownTank = 7;

            string highest = "";
            //order by the key value
            foreach (KeyValuePair<CollectorTank, float> author in tankValList.OrderByDescending(key => key.Value))
            {
                if(i < shownTank)
                {
                    print("bla " + author.Key.currentValue + " a " + author.Key.getTankName());
                    author.Key.activateThisTank(true);

                    highest += author.Key.getTankName() + "|" + author.Key.getAverageValue().ToString("f2") + "|";
                }

                else
                {
                    continue;
                }
                /*
                print("bla " + author.Key.currentValue + " a " + author.Key.getTankName());
                if (i < totalTank - shownTank)
                    author.Key.gameObject.SetActive(false);
                else
                {
                    author.Key.gameObject.SetActive(true);

                    highest += author.Key.getTankName() + "|" + author.Key.getAverageValue().ToString("f2") + "|";
                }
                */
                i++;
            }

            //print("highest " + highest);
            PlayerPrefs.SetString(savedHighestAnswers, highest);

        }



        /// <summary>
        /// to zeroing all tank Value
        /// </summary>
        public void NullifyAllTankValue()
        {
            for (int i = 0; i < tankObjList.Count; i++)
            {
                tankObjList[i].currentValue = 0;
            }
            PlayerPrefs.SetString(savedTankAnswers, "");
        }

        /// <summary>
        /// to load the saved tank Value
        /// </summary>
        private void LoadSavedTankValue()
        {
            //print("tank " + PlayerPrefs.GetString(savedTankAnswers));
            string[] tList = PlayerPrefs.GetString(savedTankAnswers).Split('|');

            if (tList.Length <= 0) return;
            for(int i = 0; i < tList.Length -1; i ++)
            {
                //print("i " + i);
                tankObjList[i].currentValue = ConverterFunction.StringToFloat(tList[i]);
            }
        }


        /// <summary>
        /// to update the saved tank value
        /// </summary>
        public void UpdateSavedTankValue()
        {
            string s = "";
            for (int i = 0; i < tankObjList.Count; i++)
            {
                s += (tankObjList[i].currentValue).ToString() + "|";
            }

            PlayerPrefs.SetString(savedTankAnswers, s);

        }

        /// <summary>
        /// to show the Id entry and show it in the database
        /// </summary>
        public void ShowIdAndSavedInTheDatabase()
        {
            resultCanvas.gameObject.SetActive(true);
            //unique ID

            string uniqueID = PlayerPrefs.GetString(savedUniqueID);
            string category = PlayerPrefs.GetString(savedCategory);
            string answers = PlayerPrefs.GetString(listOfAnswer);
            string highAnswer = PlayerPrefs.GetString(savedHighestAnswers);
            


            //show the Unique ID
            textID.text = uniqueID;

            //Show the highest answer
            string[] a = highAnswer.Split('|');

            for(int i = 0; i < textStrengthList.Count; i++)
            {   textStrengthList[i].text = a[i * 2] 
                                           + ": " 
                                           + a[((i +1) *2 ) -1];
                
            }

            string answerConvert = answers.Replace('|', ';');
            string highConvert = highAnswer.Replace('|', ';');

            //prepare the data to be inserted
            Dictionary<string, string> formDict = new Dictionary<string, string>()
            {
                {"uniqueID_Post", uniqueID},
                {"category_Post", category},
                {"answers_Post", answerConvert},
                {"strengths_Post", highConvert}
            };

            //insert defined data
            string url = "https://fresenius.lfi.rwth-aachen.de/resultInserter.php";
            //string url = "http://localhost/fresenius/resultInserter.php";
            StartCoroutine(SQLModifier.modifierURL(url, formDict, returnValue =>
            {
                print(returnValue); // this will return the "callback"
                if (returnValue.Contains("error"))
                {
                    Debug.Log("Something wrong., perhaps with the internet connection ?");
                    message_Error.SetActive(true);
                }
                else
                {
                    Debug.Log("Success inserting");
                    message_Sucess.SetActive(true);
                }

            }));

            print("Show Id and do save here");
            print("to Send to Database: " + uniqueID + answers + highAnswer);



        }


        /// <summary>
        /// to nullify the question from the list
        /// </summary>
        /// <param name="_id">the index of the question in the list</param>
        public void NullifyQuestion(int _id)
        {
            questionList[_id] = "";
        }

        /// <summary>
        /// to update the saved answered question
        /// </summary>
        /// <param name="_i">to add to the saved data</param>
        public void updateSavedAnsweredQuestion(int _i)
        {
            string s = PlayerPrefs.GetString(savedAnswers);

            PlayerPrefs.SetString(savedAnswers, s + _i.ToString() + "|");
            savedAnsweredQuestList.Add(_i);

        }

        /// <summary>
        /// send the updated answer to here
        /// </summary>
        /// <param name="_val"></param>
        public void UpdateTheListOfAnswer(float _val)
        {
            string s = PlayerPrefs.GetString(listOfAnswer);
            PlayerPrefs.SetString(listOfAnswer, 
                                  s + _val.ToString("f2") +"|");
            print("_val " + _val);
        }


        /// <summary>
        /// To set the uniqueID of the user
        /// </summary>
        public void SetUniqueID()
        {
            string id = UniqueIDGenerator.GenerateID_StringBuilder();
            PlayerPrefs.SetString(savedUniqueID, id);
            print("current id " + id);
        }



        /// <summary>
        /// to spawn object in the circle
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        private Vector3 SpawnInCircle(Vector3 center, float radius, int a)
        {
            float ang = a;
            Vector3 pos;
            pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.y = center.y ;
            pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
            return pos;
        }
    }



}
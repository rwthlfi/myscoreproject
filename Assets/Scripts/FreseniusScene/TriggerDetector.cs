using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Fresenius
{
    public class TriggerDetector : MonoBehaviour
    {
        [Header("Detection Variable")]
        private bool allowHit = true;
        public string detectionTag;
        public GameObject currentCollider;

        [Header("Extra Ball")]
        public GameObject startBall;
        public GameObject continueBall;


        [Header("Particle System")]
        public GameObject particle;
        public AudioSource audioHit;
        public Transform lightbeam;
        public CapsuleCollider lightbeamCollider;

        [Header("Extra Audio")]
        public AudioSource audio_saberOn;
        public AudioSource audio_saberOff;

        [Header("Script Reference")]
        public QuestionSpawner questionerSpawner;
        public OpeningSequence openingSequence;
        public LanguageSelection languageSel;

        string savedUniqueID = "savedUniqueID";
        string savedAnswers = "savedAnswers";
        string savedSpeed = "savedSpeed";
        string listOfAnswer = "listOfAnswer";
        string savedCategory = "savedCategory";
        // this one is needed due to the category appearing at the beginning.
        // if the ball is hit to "continue"., then the saved Category will kick in
        // if the ball is hit to "start from beginning", then the pref of savedCategory will be updated with cacheCategory
        string cacheCategory = ""; 

        float currentSpeed = 0.1f;

        private void Start()
        {
            currentSpeed = PlayerPrefs.GetFloat(savedSpeed);
            if (currentSpeed <= 0.1f)
                currentSpeed = 0.1f;
            coroutine = dummycoroutine();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!allowHit) // dont do anything if it doesnt allow any hit
                return;

            if (other.gameObject.tag == detectionTag && !currentCollider)
            {
                allowHit = false;

                //register it to the system so the OnTriggerExit will be called if the GameObject is disabled.
                ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
                currentCollider = other.gameObject;


                //get the position of the contact
                var collisionPoint = other.ClosestPoint(transform.position);
                particle.transform.position = collisionPoint;
                particle.SetActive(false); // yes this is needed to be disabled first. just for safety
                particle.SetActive(true);

                //play Hit Audio
                playHitAudio(false); // just for safety.
                playHitAudio(true);

                //if it is START ball 
                if (currentCollider == startBall)
                {
                    //create a new uniqueID
                    questionerSpawner.SetUniqueID();

                    startBall.gameObject.SetActive(false);
                    continueBall.gameObject.SetActive(false);

                    //clear all the player Answer
                    PlayerPrefs.SetString(listOfAnswer, "");

                    //clear player Pref -> to start from beginning
                    PlayerPrefs.SetString(savedAnswers, "");
                    
                    //update the category
                    PlayerPrefs.SetString(savedCategory, cacheCategory);
                    questionerSpawner.LoadAllQuestions();

                    //nullify the tank value
                    questionerSpawner.NullifyAllTankValue();

                    

                    // cause the code execute too fast due to optimizazion, I need to put a bit of delay
                    Invoke("AllowHitting", 0.5f);
                }


                //If it hit CONTINUE ball
                else if( currentCollider == continueBall)
                {
                    startBall.gameObject.SetActive(false);
                    continueBall.gameObject.SetActive(false);

                    //if there is no unique ID set one up
                    if(PlayerPrefs.GetString(savedUniqueID) =="")
                        questionerSpawner.SetUniqueID();

                    //if no saved category yet then use the cache
                    if(PlayerPrefs.GetString(savedCategory) == "")
                        PlayerPrefs.SetString(savedCategory, cacheCategory);


                    //do something here
                    questionerSpawner.LoadAllQuestions();

                    // cause the code execute too fast due to optimizazion, I need to put a bit of delay
                    Invoke("AllowHitting", 0.5f);
                }

                else if(currentCollider == openingSequence.ballYes || currentCollider == openingSequence.ballNo)
                {
                    //do something here
                    openingSequence.isHit = true;

                    openingSequence.isYes = false; // reset it first
                    bool a = (currentCollider == openingSequence.ballYes) ? openingSequence.isYes = true : openingSequence.isYes = false;


                    // cause the code execute too fast due to optimizazion, I need to put a bit of delay
                    Invoke("AllowHitting", 0.5f);
                }

                else if(currentCollider == openingSequence.ballFaster || currentCollider == openingSequence.ballSlower)
                {
                    //send to the OpeningSequence
                    _ = (currentCollider == openingSequence.ballFaster) ? currentSpeed -= 0.05f : currentSpeed += 0.05f;

                    if (currentSpeed <= questionerSpawner.minSpeed)
                        currentSpeed = questionerSpawner.minSpeed;
                    else if (currentSpeed >= questionerSpawner.maxSpeed)
                        currentSpeed = questionerSpawner.maxSpeed;


                    //set the opening sequence speed for looping sequence
                    openingSequence.periodFupdate = currentSpeed;
                    print("currentSpeed " + currentSpeed);

                    //saved the speed
                    PlayerPrefs.SetFloat(savedSpeed, openingSequence.periodFupdate);

                    Invoke("AllowHitting", 0.5f);
                }

                else if (currentCollider.GetComponent<CategoryBall>())
                {
                    //save the category
                    Debug.Log("category ");

                    cacheCategory = currentCollider.GetComponent<CategoryBall>().GetCategoryText();

                    openingSequence.TriggerTheOpeningSequence();

                    //disable the category container
                    openingSequence.categoryContainer.gameObject.SetActive(false);
                    Invoke("AllowHitting", 0.5f);
                }

                //for ball testing
                else if (openingSequence.testBallsList.Contains(currentCollider))
                {
                    openingSequence.isHit = true;
                    // cause the code execute too fast due to optimizazion, I need to put a bit of delay
                    Invoke("AllowHitting", 0.5f);
                }
                



                //For Language selection
                else if (currentCollider == openingSequence.ballEnglish || currentCollider == openingSequence.ballGerman)
                {
                    //change language
                    if (currentCollider == openingSequence.ballEnglish)
                    {
                        questionerSpawner.questionUrl = "https://files.lfi.rwth-aachen.de/myscore/MyScoreAssets/Fresenius/Questions_EN.txt";
                        questionerSpawner.tankUrl = "https://files.lfi.rwth-aachen.de/myscore/MyScoreAssets/Fresenius/TanksID_EN.txt";
                        languageSel.SetEnglish();
                        print("EN is selected");
                    }

                    else if (currentCollider == openingSequence.ballGerman)
                    {
                        questionerSpawner.questionUrl = "https://files.lfi.rwth-aachen.de/myscore/MyScoreAssets/Fresenius/Questions_DE.txt";
                        questionerSpawner.tankUrl = "https://files.lfi.rwth-aachen.de/myscore/MyScoreAssets/Fresenius/TanksID_DE.txt";
                        languageSel.SetGerman();
                        print("DE is selected");
                    }
                    

                    Invoke("AllowHitting", 0.5f);
                }


                //but if it is a normal answer
                else
                {
                    //directly disable the gameObject
                    if (currentCollider)
                        currentCollider.gameObject.SetActive(false);

                    //fill the Water
                    other.GetComponent<AnswerBall>().FillTheTank();

                    //update he value
                    questionerSpawner.UpdateTheListOfAnswer(other.GetComponent<AnswerBall>().answerVal);

                    //update the saved list
                    questionerSpawner.updateSavedAnsweredQuestion(questionerSpawner.currentQuestionID);
                    //nullify it
                    questionerSpawner.NullifyQuestion(questionerSpawner.currentQuestionID);

                    //update the tank Value
                    questionerSpawner.UpdateSavedTankValue();


                    //if question hasn't all been answered
                    if (!questionerSpawner.allQuestionAnswered())
                    {
                        //deactivate the answer first;
                        questionerSpawner.ActivateAnswerObj(false);
                        //spawn the next question
                        questionerSpawner.SpawnNext();

                    }

                    //BUT IF ALL THE QUESTION HAS BEEN ANSWERED!!!  
                    else
                    {
                        // then trigger the END scene
                        questionerSpawner.TriggerEndScene();
                        questionerSpawner.ShowTheHighestTankValue();
                        questionerSpawner.ShowIdAndSavedInTheDatabase();
                    }



                    // cause the code execute too fast due to optimizazion, I need to put a bit of delay
                    Invoke("AllowHitting", 0.5f); 
                }
            }

        }

        private void OnTriggerStay(Collider other)
        {
            /*
            if (other.gameObject.tag == detectionTag)
                print("stay");
                */
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == currentCollider)
            {
                ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
                currentCollider = null;
                //print("Exit");
            }
        }


        /// <summary>
        /// enable the hitting again
        /// </summary>
        private void AllowHitting()
        {
            allowHit = true;
        }

        private void playHitAudio(bool _val)
        {
            if (_val)
            {
                audioHit.Play();
            }
            else
            {
                audioHit.Pause();
            }
        }

        private IEnumerator coroutine;
        bool isLightbeamOn = false;
        /// <summary>
        /// to turn on the lightsaber
        /// </summary>
        public void LightSaberOn()
        {
            StopCoroutine(coroutine);
            if (!isLightbeamOn)
            {
                isLightbeamOn = true;
                coroutine = LerpingExtensions.ScaleTo(lightbeam, Vector3.one, 0.25f);
                lightbeamCollider.radius= 0.1f;
                lightbeamCollider.height = 3.61f;
                lightbeamCollider.center = new Vector3(0, 2.786f, 0);
                //audio_saberOn.Pause();
                audio_saberOn.Play();
                StartCoroutine(coroutine);
            }
            else
            {
                isLightbeamOn = false;
                coroutine = LerpingExtensions.ScaleTo(lightbeam, Vector3.zero, 0.25f);
                lightbeamCollider.radius = 0f;
                lightbeamCollider.height = 0;
                lightbeamCollider.center = Vector3.zero;
                //audio_saberOff.Pause();
                audio_saberOff.Play();
                StartCoroutine(coroutine);
            }
        }

        private IEnumerator dummycoroutine()
        {
            yield return null;
        }


    }
}
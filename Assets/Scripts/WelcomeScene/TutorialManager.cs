using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TutorialManager : MonoBehaviour
{
    [Header("Robot Reference")]
    public RobotManager robotManager;
    private bool continueSpeech = true;

    [Header("OVR Reference")]
    public Transform mainOVR;
    public Camera centerEye;

    [Header("Welcome Speech reference")]
    public Image welcomeSpeechBubble;
    public List<TextMeshProUGUI> welcomeSpeechList = new List<TextMeshProUGUI>();
    [Header("Tutorial Move Speech reference")]
    public Image moveSpeechBubble;
    public List<TextMeshProUGUI> movementSpeechList = new List<TextMeshProUGUI>();


    [Header("Position point")]
    public Transform tutorBeginningPoint;
    public Transform tutorForwardDest;
    public Renderer tutorPainting;
    public Transform tutorWalkDest;

    //Typewriter effect
    private TypeWritterEffect typeWritter = new TypeWritterEffect();
    private int currentSpeechList = 0;
    private int currentSpeechID = -1;

    private enum speechList
    {
        welcomeSpeech,
        movementSpeech,
        objectInteractionSpeech,
        UIInteractionSpeech
    }


    private void Awake()
    {
        //disable all the welcome speech first and the bubble 
        welcomeSpeechBubble.gameObject.SetActive(false);
        foreach (TextMeshProUGUI text in welcomeSpeechList)
            text.gameObject.SetActive(false);

        moveSpeechBubble.gameObject.SetActive(false);
        foreach (TextMeshProUGUI text in movementSpeechList)
            text.gameObject.SetActive(false);

    }

    private void Start()
    {
        //show the speechbubble and the first text
        ShowText(welcomeSpeechList, 0);
    }

    private void Update()
    {
        Debug.Log("currentSpeech " + currentSpeechList);
        Debug.Log("currentID " + currentSpeechID);
        //if not finish typing and speech is not allowed
        //dont do anything
        if (!typeWritter.typing_isFinish || !continueSpeech)
        {
            if (!typeWritter.typing_isFinish)
                Debug.Log("is not finish typing");

            if (!continueSpeech)
                Debug.Log("is not finish talking");
            return;
        }
            


        //the welcome speech
        if (currentSpeechList == (int)speechList.welcomeSpeech )
        {
            Debug.Log("Welcome introduction");
            if (currentSpeechID == 1) // waiting for the high Five.
            {
                continueSpeech = false;
                StartCoroutine(Waiting_ForHighFive());
                return;
            }

            ShowText(welcomeSpeechList, ++currentSpeechID);
        }

        //movement
        else if(currentSpeechList == (int)speechList.movementSpeech)
        {
            Debug.Log("movement introduction");
            
            if(currentSpeechID == 0)
                TeleportPlayerAndRobot_ToBeginningTutorial();

            else if(currentSpeechID == 4)
            {
                continueSpeech = false;
                StartCoroutine(Waiting_ToMoveToDestination(tutorForwardDest));
                return;
            }

            else if (currentSpeechID == 6)
            {
                continueSpeech = false;
                StartCoroutine(Waiting_ToLetPlayerSeeThePainting(tutorPainting));
                return;
            }
            else if (currentSpeechID == 8)
            {
                continueSpeech = false;
                StartCoroutine(Waiting_ToMoveToDestination(tutorWalkDest));
                return;
            }

            ShowText(movementSpeechList, ++currentSpeechID);
        }

        //Object Interactable
        else if (currentSpeechList == (int)speechList.objectInteractionSpeech)
        {
            Debug.Log("Object Interaction introduction");
        }

        //UI Interactable
        else if (currentSpeechList == (int)speechList.UIInteractionSpeech)
        {
            Debug.Log("Object Interaction introduction");
        }

        //Speaking Tutorial

    }

    private void ShowSpeechBubble(int _speechList)
    {
        welcomeSpeechBubble.gameObject.SetActive(false);
        moveSpeechBubble.gameObject.SetActive(false);

        switch (_speechList)
        {
            case (int)speechList.welcomeSpeech: welcomeSpeechBubble.gameObject.SetActive(true); break;
            case (int)speechList.movementSpeech: moveSpeechBubble.gameObject.SetActive(true); break;
            default: Debug.Log("should never happened though"); break;
        }
            
    }

    private void ShowText(List<TextMeshProUGUI> _speechList, int _speechID)
    {
        //set the current speech that is being displayed
        currentSpeechID = _speechID;
        foreach (TextMeshProUGUI text in _speechList)
            text.gameObject.SetActive(false);

        //start the speech - enable the speech bubble - enable the corresponding text
        ShowSpeechBubble(currentSpeechList);

        _speechList[_speechID].gameObject.SetActive(true);
        StartCoroutine(typeWritter.DisplayText(_speechList[_speechID]));

        //if the list of the Speech has been finished shown,
        //increment the speech list.
        //and reset the speech ID
        if (currentSpeechID >= _speechList.Count - 1)
        {
            ++currentSpeechList;
            currentSpeechID = 0;
        }
    }

    
    IEnumerator Waiting_ForHighFive()
    {
        Debug.Log("Waiting for HighFive...");
        
        yield return new WaitUntil(() => robotManager.isGettingHighFive);
        ShowText(welcomeSpeechList, ++currentSpeechID);
        continueSpeech = true;
    }

    IEnumerator Waiting_ToMoveToDestination(Transform _destinationPoint)
    {
        Debug.Log("Waiting the player to be at the destination...");

        yield return new WaitUntil(() => player_inPosition(_destinationPoint));
        ShowText(movementSpeechList, ++currentSpeechID);
        Debug.Log("Awesome you are teleported!");
        continueSpeech = true;
    }

    IEnumerator Waiting_ToLetPlayerSeeThePainting(Renderer _tutorObject)
    {
        Debug.Log("Waiting the player to be see the object...");

        yield return new WaitUntil(() => player_seeObject(_tutorObject));
        ShowText(movementSpeechList, ++currentSpeechID);
        Debug.Log("Excellet! player see the painting");
        continueSpeech = true;
    }

    private void TeleportPlayerAndRobot_ToBeginningTutorial()
    {
        //teleport the robot
        Vector3 robotPoint = new Vector3(tutorBeginningPoint.localPosition.x ,
                                         tutorBeginningPoint.localPosition.y + 0.7f,
                                         tutorBeginningPoint.localPosition.z + 1.5f);
        robotManager.transform.localPosition = robotPoint;
        
        //teleport the player
        Vector3 playerPoint = new Vector3(tutorBeginningPoint.localPosition.x,
                                         tutorBeginningPoint.localPosition.y + 3f,
                                         tutorBeginningPoint.localPosition.z);

        mainOVR.transform.localPosition = playerPoint;
        mainOVR.transform.eulerAngles = new Vector3(0f,0f,0f);
    }


    //Check if the player is already in the destination position.
    private bool player_inPosition(Transform _destinationPoint)
    {
        float dist = Vector3.Distance(mainOVR.transform.position, _destinationPoint.position);

        if (dist <= 1f)
            return true;
        else
            return false;
    }

    //check if certain object is present in the camera render
    private bool player_seeObject(Renderer _renderer)
    {
        if (_renderer.IsVisibleFrom(centerEye))
            return true;
        else
            return false;
    }

}

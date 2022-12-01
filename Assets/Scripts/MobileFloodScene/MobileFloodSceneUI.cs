using Mirror;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]

public class MobileFloodSceneUI : NetworkBehaviour
{
    [Header("SyncVar Variable")]
    [SyncVar(hook = nameof(OnTimerChanged))]
    public string timer = "";

    [SyncVar(hook = nameof(OnTutorialStepChanged))]
    public int stepID = -2;

    [Header("UI Refernce")]
    public TextMeshProUGUI timerText;

    [Header("Script References")]
    public Rescaler rescaler;
    public GlassCube glassCube;

    public Component_Stopwatch componentStopwatch;
    public MobileFloodTutorialManager mobileFloodTutorManager;

    [Header("FakeFlood")]
    public GameObject flood;
    private Vector3 floodOriginPos;
    private Quaternion floodOriginRot;

    //Server component
    private GameRoomSetting grs;


    //synchro the timer
    void OnTimerChanged(string _old, string _new)
    {
        //do something here.
        timerText.text = _new;
    }

    void OnTutorialStepChanged(int _Old, int _New)
    {
        //do something here
        mobileFloodTutorManager.currentStep = _New;
    }

    [Command]
    public void CmdTimerChange(string _str) { timer = _str; }

    [Command]
    public void CmdTutorialStepChanged(int _id) { stepID = _id; }



    private void Awake()
    {
        //Deactivated the flood
        flood.SetActive(false);

        //store the transform pos and rot
        floodOriginPos = flood.transform.position;
        floodOriginRot = flood.transform.rotation;
    }

    private float nextActionTime = 0.0f;
    public float period = 1f;
    private void Update()
    {
        
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            //sync the timer
            if (isServer) Server_syncTimer();
        }
    }

    //attach this to the "StartTutorial" UI
    public void Ui_ChangeStep(int _step)
    {
        Cmd_ChangeStep(_step); // only on the server
    }

    
    //to tell the server to start the tutorial.
    [Command(requiresAuthority = false)]
    public void Cmd_ChangeStep(int _step)
    {
        ChangeStep(_step);
    }

    /// <summary>
    /// to start the tutorial. However this function is also needed for Tutorial Synchronization at the beggining.
    /// </summary>
    public void ChangeStep(int _step)
    {
        //dont start the Flood
        StartFlood(false);

        //Start Tutorial
        if (_step != (int)MobileFloodTutorialManager.tutorStep.nothing)
        {
            //start the timer up
            componentStopwatch.StartTimer();
            //open the glass cube
            glassCube.OpenGlassCube();
        }

        //otherwise it means reset
        else
        {
            componentStopwatch.stopTimer();
            glassCube.ResetTransform();
            mobileFloodTutorManager.ResetAllComponentTransform();
        }

        //change the step in the mobileflood
        mobileFloodTutorManager.currentStep = _step;

        //change the tutorial step
        Server_changeStep(_step);
    }



    [Server]
    private void Server_changeStep(int _step)
    {
        //synch the step id
        stepID = _step;
    }

    [Server]
    private void Server_syncTimer()
    {
        timer = componentStopwatch.TimerCalculation();
    }

    /// <summary>
    /// to reset the tutorial
    /// </summary>
    public void Ui_ResetTutorial()
    {
        Cmd_ChangeStep((int)MobileFloodTutorialManager.tutorStep.nothing);
    }


    public void Ui_ReloadScene()
    {
        Cmd_reloadScene();
    }

    [Command(requiresAuthority = false)]
    private void Cmd_reloadScene()
    {
        //your logic here
        NetworkManager.singleton.ServerChangeScene(GlobalSettings.MobileFloodUnitScene);
    }



    //To Start the Flood Networked
    public void TestFloodButton_isClicked()
    {
        Cmd_startFlood();
    }

    [Command(requiresAuthority = false)]
    private void Cmd_startFlood()
    {
        StartFlood(true);
        //synchronize to the client
        Rpc_startFlood();
        //componentStopwatch.stopTimer();
    }

    [ClientRpc]
    private void Rpc_startFlood()
    {
        StartFlood(true);
    }

    private void StartFlood(bool _start)
    {
        flood.SetActive(false);

        //set it to flood to origin position and rotation.
        flood.transform.position = floodOriginPos;
        flood.transform.rotation = floodOriginRot;
        flood.GetComponent<Rigidbody>().velocity = Vector3.zero;
        flood.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        //start the flooooddd!!!
        flood.SetActive(_start);
    }

}

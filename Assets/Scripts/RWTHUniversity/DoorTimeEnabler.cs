using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityStandardAssets.Characters.FirstPerson;
using RoomService;

[System.Serializable]
public class DoorProperties
{
    //Button UI
    public Button buttonDoor;
    public TextMeshProUGUI buttonDoorName;
    public TextMeshProUGUI buttonDoorInfo;
    public TextMeshProUGUI timeText;

    //Target position
    public Transform targetLocation;

    //Times
    public string start = "HH:mm";
    public string end = "HH:mm";

    private TimeSpan startTime, endTime, nowTime;

    /// <summary>
    /// Parsing the String to Time
    /// </summary>
    public void ParseStringToTime()
    {
        startTime = TimeSpan.Parse(start);
        endTime = TimeSpan.Parse(end);
        if (isStartEndTimeDefined()) // if time defined, parse it
            timeText.text = startTime.ToString(@"hh\:mm") + " - " + endTime.ToString(@"hh\:mm");
        else
            timeText.text = "-";
    }


    //to check if the time is defined
    public bool isStartEndTimeDefined()
    {
        if (start == "00:00" && end == "00:00")
            return false;
        else
            return true;
    }

    /// <summary>
    ///To update the time with the current time NOW
    /// </summary>
    /// <returns></returns>
    public bool isTimeOutsideRange()
    {
        //if the start and end is 00:00:00 dont bother, always leave the door open
        if (!isStartEndTimeDefined())
            return true;

        //Otherwise
        else
        {
            nowTime = DateTime.Now.TimeOfDay;
            //nowTime = TimeSpan.Parse(now);
            if ((nowTime > startTime) && (nowTime < endTime))
            {
                //match found
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// To Enable the door interaction
    /// </summary>
    /// <param name="_value"></param>
    public void EnableDoor(bool _value)
    {
        buttonDoor.interactable = _value;
    }


    /// <summary>
    /// Moving the player to target position
    /// </summary>
    /// <param name="_transform">The players </param>
    public IEnumerator MovePlayerOf(Transform _transform)
    {
        yield return new WaitForSeconds(0.1f);

        _transform.position = targetLocation.position;
        _transform.rotation = targetLocation.rotation;
    }
}

public class DoorTimeEnabler : MonoBehaviour
{
    [Header("Script Reference")]
    public TheRoomServices theRoomService;
    private Transform cacheLocalPlayer;

    [Header("Appointments Schedule")]
    public bool useURL = true;
    public string appointmentURLText;
    public TextMeshProUGUI appointmentScheduleText;
    public List<string> cacheAppointmentList;
    enum appoinmentEnum { theName, theInfo, theStartTime, theEndTime };



    [Header("Door's Properties")]
    public List<DoorProperties> doorsList;

    [Header("Go Back Location")]
    public Transform originLoc;

    // FPC Addition
    private FirstPersonController _firstPersonController;


    private void Start()
    {
        foreach (DoorProperties dp in doorsList)
            dp.ParseStringToTime();

        if (useURL)
            ReadURL();
    }



    private float nextActionTime = 0.0f;
    public float period = 3f;
    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
            //Execute code here.

            //check if the door should be enable or not.
            foreach (DoorProperties dp in doorsList)
                dp.EnableDoor(dp.isTimeOutsideRange());
        }
    }


    //Door button to teleport the player to other Room
    public void Ui_DoorButton(int _doorID)
    {
        //first check if the local player has been assigned., if not assign them
        if (!cacheLocalPlayer)
        {
            cacheLocalPlayer = theRoomService.theLocalPlayer.transform;
        }

        //For FPC the FirstPersonController needs to deactivated before the teleport, otherwise the FPC overrides the updated position with the last one
        if (GlobalSettings.DeviceType() == GlobalSettings.Device.WindowsNonVR)
        {
            if (!_firstPersonController)
            {
                GetFirstPersonController();
            }

            StartCoroutine(FirstPersonTeleport());
        }


        //Move the player to the stored position
        StartCoroutine(doorsList[_doorID].MovePlayerOf(cacheLocalPlayer));
    }

    public void Ui_ButtonGoBack()
    {
        StartCoroutine(Ui_GoBack());
    }

    //To Go back to the beginning
    public IEnumerator Ui_GoBack()
    {
        //For FPC the FirstPersonController needs to deactivated before the teleport, otherwise the FPC overrides the updated position with the last one
        if (GlobalSettings.DeviceType() == GlobalSettings.Device.WindowsNonVR)
        {
            if (!_firstPersonController)
            {
                GetFirstPersonController();
            }

            StartCoroutine(FirstPersonTeleport());
        }

        yield return new WaitForSeconds(0.2f);

        cacheLocalPlayer.position = originLoc.position;
        cacheLocalPlayer.rotation = originLoc.rotation;
    }


    private void ReadURL()
    {
        StartCoroutine(SQLloader.LoadURL(appointmentURLText, returnValue =>
        {
            //clean up the string
            var strClean = SQLloader.stringCleaner(returnValue);

            //store the link in the list
            cacheAppointmentList = strClean.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries).ToList();


            //Parse all the info from the list
            ParseURLAppointmentData(cacheAppointmentList);


        }));
    }

    private void ParseURLAppointmentData(List<string> _data)
    {
        string result = "";
        //clear the appointmentScheduleText first
        appointmentScheduleText.text = "";


        for (int i = 0; i < _data.Count; i++)
        {
            //split it with "~"
            string[] a = _data[i].Split('~');


            //parse the button
            doorsList[i].buttonDoorName.text = a[(int)appoinmentEnum.theName];

            //parse the extra info
            doorsList[i].buttonDoorInfo.text = a[(int)appoinmentEnum.theInfo];

            //parse the start time
            doorsList[i].start = a[(int)appoinmentEnum.theStartTime];

            //parse the end time
            doorsList[i].end = a[(int)appoinmentEnum.theEndTime];

            //Conclude everything in the main schedule
            result += doorsList[i].buttonDoorName.text + " ";
            result += doorsList[i].buttonDoorInfo.text + " ";
            result += "\n";
            result += doorsList[i].start + " - ";
            result += doorsList[i].end;
            result += "\n\n";
        }

        appointmentScheduleText.text = result;


        foreach (DoorProperties dp in doorsList)
            dp.ParseStringToTime();
    }


    // Addition for the FPC player
    private void GetFirstPersonController()
    {
        _firstPersonController = theRoomService.theLocalPlayer.GetComponent<FirstPersonController>();
    }
    // Additon for the FPC
    IEnumerator FirstPersonTeleport()
    {
        _firstPersonController.enabled = false;

        yield return new WaitForSeconds(0.2f);

        _firstPersonController.enabled = true;
    }
}

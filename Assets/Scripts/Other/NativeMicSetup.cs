using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Dissonance;
using UnityEngine.Android;

public class NativeMicSetup : MonoBehaviour
{/*
    private DissonanceComms dc;
    void Start()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            Permission.RequestUserPermission(Permission.Microphone);
            Debug.Log("permission ");
        }

        dc = GetComponent<DissonanceComms>();
        SetupMicName();
        //StartRecording();
    }

    void SetupMicName()
    {
        //if there is no saved data available, then take the default one
        string mic = PlayerPrefs.GetString(PrefDataList.savedMicrophone);
        if(mic == "" && Microphone.devices.Length > 0)
        {
            //take the first detected mic and throw the name to the basicMicCapture
            dc.MicrophoneName = Microphone.devices[0];
            //Debug.Log("mic: " + Microphone.devices[0]);
        }

        else
        {
            dc.MicrophoneName = mic;
        }
    */

}

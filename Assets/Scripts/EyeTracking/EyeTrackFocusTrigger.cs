using UnityEngine;
using System.Collections;
/*
using Tobii.G2OM;
using Tobii.XR;

public class EyeTrackFocusTrigger : MonoBehaviour, IGazeFocusable     //Monobehaviour which implements the "IGazeFocusable" interface, meaning it will be called on when the object receives focus
{
    // HOW TO:
    // Attach this script to all gameobjects (e.g. tool or clock or chair) than need to be tracked via Vive Eye Tracking. 
    // Trackig will only save the focus and duration events, when inside "CsvReadWrite.cs" via the Eye Tracking Menu, StartRecording (start recording of focus events) has been started before and also StopRecording (create the text file) has been finished

    private GameObject _csvReadWrite;
    private bool _timerActive;
    private double _counterSeconds = 0.0;
    private CsvReadWrite _csv;

    //The method of the "IGazeFocusable" interface, which will be called when this object receives or loses focus

    private void Start()
    {
        if (GlobalSettings.DeviceType() == GlobalSettings.Device.WindowsVR)
        {
            _csvReadWrite = GameObject.Find("EyeTrackingRecordCanvas");
            StartCoroutine(SecondsCounter());
            if (_csvReadWrite != null)
                _csv = _csvReadWrite.GetComponent<CsvReadWrite>();
        }
    }

    public void GazeFocusChanged(bool hasFocus)
    {
        if (GlobalSettings.DeviceType() == GlobalSettings.Device.WindowsVR)
        {
            //If this object received focus, fade the object's color to highlight color
            if (hasFocus)
            {
                _timerActive = true;
            }
            //If this object lost focus, fade the object's color to it's original color
            else
            {
                if (_csvReadWrite != null)
                {
                    _csv.AddToTracking(gameObject.name, _counterSeconds.ToString());
                }
                _counterSeconds = 0.0;

                _timerActive = false;
            }
        }
        else
            enabled = false;
    }

    private IEnumerator SecondsCounter()
    {
        while (true)
        {
            if (_timerActive)
            {
                _counterSeconds += 0.1;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
*/
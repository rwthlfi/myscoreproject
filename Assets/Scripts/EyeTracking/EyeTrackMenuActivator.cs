using System;
using System.Collections.Generic;
using UnityEngine;
/*
using Tobii.XR.Internal;
using Tobii.XR;
using UnityEngine.SceneManagement;
using System.Collections;

public class EyeTrackMenuActivator : MonoBehaviour
{
    public GameObject eyeTrackMenu;
    private GameObject tobiiXR_Lifecycle;

    // Start is called before the first frame update
    private void Start()
    {
        if (TobiiXR.Internal.Provider != null && TobiiXR.Internal != null)
        {
            if (TobiiXR.Internal.Provider.GetType().Name != "HTCProvider")
            {
                if (SceneManager.GetActiveScene().name == GlobalSettings.WelcomeScene)
                {
                    tobiiXR_Lifecycle = GameObject.Find("TobiiXR Updater");

                    if (tobiiXR_Lifecycle != null)
                    {
                        tobiiXR_Lifecycle.GetComponent<TobiiXR_Lifecycle>().enabled = false;
                    }
                }
            }
        }
        if (GlobalSettings.DeviceType() != GlobalSettings.Device.WindowsVR && gameObject != null)
            gameObject.SetActive(false);
    }

    public void StartTrack()
    {
        if (eyeTrackMenu != null)
            eyeTrackMenu.transform.localPosition = new Vector3(-5f, -3.9f, 0f);
    }

    public void StopTrack()
    {
        if (eyeTrackMenu != null)
            eyeTrackMenu.transform.localPosition = new Vector3(-5f, -13.9f, 0f);
    }
}
*/
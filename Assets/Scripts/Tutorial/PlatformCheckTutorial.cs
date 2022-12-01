using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class PlatformCheckTutorial : MonoBehaviour
{

    public Flowchart flowchart;

    // Start is called before the first frame update
    void Start()
    {
        if (GlobalSettings.DeviceType() == GlobalSettings.Device.Android)
            flowchart.SetBooleanVariable("DisableVive", true);
        else
            flowchart.SetBooleanVariable("DisableVive", false);
    }
}

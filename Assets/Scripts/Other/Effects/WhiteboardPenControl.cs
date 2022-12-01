using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
using Valve.VR;
#endif


public class WhiteboardPenControl : MonoBehaviour
{
    [Header("Controller Variable")]
    public XRController controller;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    public SteamVRBasedController steamVRBasedController;
#endif
    private bool isUsingSteamVR = false;

    private WhiteboardPen whiteboardPen;


    private void Awake()
    {

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        //check if the controller is using a steamVR or not.
        if (steamVRBasedController)
        {
            isUsingSteamVR = true;
        }
#endif
    }

    private void Start()
    {
        whiteboardPen = GetComponent<WhiteboardPen>();
        whiteboardPen.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

         // if it is using steamVR., then only executed the code in this if.
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (isUsingSteamVR)
        {
            if (steamVRBasedController.triggerValue > 0.1f)
            {
                whiteboardPen.enabled = true;
            }
            else
            {
                whiteboardPen.enabled = false;
                whiteboardPen.whiteboard.ToggleTouch(false);
            }
            //stop executing the code.
            return;
        }
#endif

        // For Oculus
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float pointerValue))
        {
            if (pointerValue > 0.1f)
            {
                whiteboardPen.enabled = true;
            }
            else
            {
                whiteboardPen.enabled = false;
                whiteboardPen.whiteboard.ToggleTouch(false);
            }
        }

    }
}

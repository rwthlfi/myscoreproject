using AvatarCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
using Valve.VR;
#endif

public class SteamVRBasedController : XRController
{

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    // SteamVR Tracking
    [Header("SteamVR Tracking")]
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;
    public SteamVR_Action_Pose poseAction = null;

    // SteamVR Input
    public SteamVR_Action_Boolean selectAction = null;
    public SteamVR_Action_Boolean activeAction = null;
    public SteamVR_Action_Boolean interfaceAction = null;
    public SteamVR_Action_Vector2 touchPadAction = null;
    public SteamVR_Action_Single triggerAction = null;
    public SteamVR_Action_Single gripAction = null;
    public SteamVR_Action_Boolean touchButtonAction = null;
    public SteamVR_Action_Boolean teleportAction = null;
    [System.NonSerialized] public Vector2 leftTouchPadValue = Vector2.zero;
    [System.NonSerialized] public Vector2 rightTouchPadValue = Vector2.zero;
    [System.NonSerialized] public float triggerValue = 0f;
    [System.NonSerialized] public float gripValue = 0f;
    [System.NonSerialized] public bool touchButtonValue = false;
    [System.NonSerialized] public bool teleportButtonValue = false;

    private CharacterController charController;
    private XRCameraReference centerEyeCamera;
    private bool showWarning = true;

    public LocomotionListener locomotionListener;

    private void Start()
    {
        // Start SteamVR
        SteamVR.Initialize();

        //get the char controller and the camera

        charController = this.transform.parent.root.GetComponent<CharacterController>();

        /*centerEyeCamera = this.transform.parent.root.GetComponentInChildren<XRCameraReference>();
        locomotionListener = this.transform.parent.root.GetComponentInChildren<LocomotionListener>();
        */


        // Check touchPad Action, and move them according to the Value
        if (Char_isAvailable())
            touchPadAction.AddOnUpdateListener(LocomotionListener, inputSource);

        // Check the Trigger action
        if (triggerAction != null)
            triggerAction.AddOnUpdateListener(TriggerListener, inputSource);

        // Check the Grip action
        if (gripAction != null)
            gripAction.AddOnUpdateListener(GripListener, inputSource);

        // Check the Touch Action
        if (touchButtonAction != null)
            touchButtonAction.AddOnUpdateListener(TouchListener, inputSource);

        //check the teleport
        if (teleportAction != null)
            teleportAction.AddOnUpdateListener(TeleportListener, inputSource);
      
    }




    //Binding for the Controller
    private void TouchListener(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        touchButtonValue = newState;
    }

    private void TriggerListener(SteamVR_Action_Single fromAction, SteamVR_Input_Sources fromSource, float newAxis, float newDelta)
    {
        triggerValue = newAxis;
    }

    private void GripListener(SteamVR_Action_Single fromAction, SteamVR_Input_Sources fromSource, float newAxis, float newDelta)
    {
        gripValue = newAxis;
    }


    private void TeleportListener(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        teleportButtonValue = newState;
    }



    protected override void UpdateTrackingInput(XRControllerState controllerState)
    {
        if (controllerState != null)
        {

            // Get position from pose action
            Vector3 position = poseAction[inputSource].localPosition;
            controllerState.position = position;

            // Get rotation from position action
            Quaternion rotation = poseAction[inputSource].localRotation;
            controllerState.rotation = rotation;
        }
    }

    protected override void UpdateInput(XRControllerState controllerState)
    {
        if (controllerState != null)
        {
            // Reset all of the input values
            controllerState.ResetFrameDependentStates();

            // Check select action, apply to controller
            SetInteractionState(ref controllerState.selectInteractionState, selectAction[inputSource]);

            // Check activate action, apply to controller
            SetInteractionState(ref controllerState.selectInteractionState, activeAction[inputSource]);

            // Check UI action, apply to controller
            SetInteractionState(ref controllerState.uiPressInteractionState, interfaceAction[inputSource]);

        }
    }

    private void LocomotionListener(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        //Debug.Log(fromSource.ToString() + ":  " +  axis);

        if (fromSource == SteamVR_Input_Sources.LeftHand)
        {
            leftTouchPadValue = axis;
        }
        else if (fromSource == SteamVR_Input_Sources.RightHand)
        {
            rightTouchPadValue = axis;
        }

    }



    private void SetInteractionState(ref InteractionState interactionState, SteamVR_Action_Boolean_Source action)
    {
        // Pressed this frame
        interactionState.activatedThisFrame = action.stateDown;

        // Released this frame
        interactionState.deactivatedThisFrame = action.stateUp;

        // Is pressed
        interactionState.active = action.state;
    }

    private bool Char_isAvailable()
    {
        // - - - Just a null reference Checker - - - //
        if (!showWarning)
        {
            return false;
        }

        if (touchPadAction == null)
        {
            Debug.LogWarning("Not Sure if you need the touch pad, cause it is not assign");
            showWarning = false;
            return false;
        }

        if (charController == null)
        {
            Debug.LogWarning("CharController doesn't exist");
            showWarning = false;
            return false;
        }

        return true;

        // - - - End of null checker - - - //
    }


#endif

}

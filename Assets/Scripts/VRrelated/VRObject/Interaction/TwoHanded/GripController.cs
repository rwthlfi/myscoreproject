using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GripController : MonoBehaviour
{
    [System.NonSerialized] public XRController Hand = null;
    private XRDirectInteractor xrDirectInteractor;

    private bool useSteamVR = false;
    private SteamVRBasedController steamVRBasedController = null;
    //public SteamVR_Action_Boolean ToggleGripButton;
    //public SteamVR_Action_Pose position;
    //public SteamVR_Behaviour_Skeleton HandSkeleton;
    //public SteamVR_Behaviour_Skeleton PreviewSkeleton;

    public Grabber grabber;

    private GameObject ConnectedObject;
    private Transform OffsetObject;
    private bool DomanantGrip;

    private void Awake()
    {
        Hand = GetComponent<XRController>();
        xrDirectInteractor = GetComponent<XRDirectInteractor>();

        //check if there is a component called "SteamVRBasedController"
        //if yes, then its 100% likely that the hmd is using a steamVR
        if (Hand.GetComponent<SteamVRBasedController>())
        {
            steamVRBasedController = Hand.GetComponent<SteamVRBasedController>();
            useSteamVR = true;
        }
            
    }

    private void Update()
    {

        if (ConnectedObject != null)
        {
            if (DomanantGrip || !ConnectedObject.GetComponent<Interactable>().SecondGripped)
            {
                if (ConnectedObject.GetComponent<Interactable>().touchCount == 0 && !ConnectedObject.GetComponent<Interactable>().SecondGripped)
                {
                    grabber.FixedJoint.connectedBody = null;
                    grabber.StrongGrip.connectedBody = null;

                    ConnectedObject.transform.position = Vector3.MoveTowards(ConnectedObject.transform.position, transform.position - ConnectedObject.transform.rotation * OffsetObject.GetComponent<GrabPoint>().Offset, .25f);
                    ConnectedObject.transform.rotation = Quaternion.RotateTowards(ConnectedObject.transform.rotation, transform.rotation * Quaternion.Inverse(OffsetObject.GetComponent<GrabPoint>().RotationOffset), 10);
                    grabber.FixedJoint.connectedBody = ConnectedObject.GetComponent<Rigidbody>();
                }
                else if (ConnectedObject.GetComponent<Interactable>().touchCount > 0 || ConnectedObject.GetComponent<Interactable>().SecondGripped)
                {
                    grabber.FixedJoint.connectedBody = null;
                    grabber.StrongGrip.connectedAnchor = OffsetObject.GetComponent<GrabPoint>().Offset;
                    grabber.StrongGrip.connectedBody = ConnectedObject.GetComponent<Rigidbody>();

                }
                else if (ConnectedObject.GetComponent<Interactable>().touchCount < 0)
                {
                    ConnectedObject.GetComponent<Interactable>().touchCount = 0;
                }

            }
            else
            {
                grabber.FixedJoint.connectedBody = null;
                grabber.StrongGrip.connectedBody = null;
                grabber.WeakGrip.connectedBody = ConnectedObject.GetComponent<Rigidbody>();
            }


            if (CurrentGripValue() < 0.2)
                Release();
            /*
            if (Hand.inputDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            {
                if( gripValue < 0.2)
                {
                    Release();
                }
                    
            }
            */

        }
        else
        {
            if (grabber.ClosestGrabbable())
            {
                OffsetObject = grabber.ClosestGrabbable().transform;
                /*
                if (grabber.ClosestGrabbable().GetComponent<SteamVR_Skeleton_Poser>())
                {
                    if (!OffsetObject.GetComponent<GrabPoint>().Gripped)
                    {
                        PreviewSkeleton.transform.SetParent(OffsetObject, false);
                        PreviewSkeleton.BlendToPoser(OffsetObject.GetComponent<SteamVR_Skeleton_Poser>(), 0f);
                    }
                }
                */
            }
            else
            {
                //PreviewSkeleton.transform.gameObject.SetActive(false);
            }


            //if currently there is no object being grabbed then grabbed it
            if (!xrDirectInteractor.selectTarget && CurrentGripValue() > 0.2)
                Grip();
            /*
            if (Hand.inputDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            {
                if (gripValue > 0.2)
                {
                    Grip();
                }
            }
            */
        }
    }
    private void Grip()
    {
        GameObject NewObject = grabber.ClosestGrabbable();
        if (NewObject != null)
        {
            OffsetObject = grabber.ClosestGrabbable().transform;
            ConnectedObject = OffsetObject.GetComponent<GrabPoint>().ParentInteractable.gameObject;//find the Closest Grabbable and set it to the connected object
            ConnectedObject.GetComponent<Rigidbody>().useGravity = false;

            OffsetObject.GetComponent<GrabPoint>().Gripped = true;
            if (ConnectedObject.GetComponent<Interactable>().gripped)
            {
                ConnectedObject.GetComponent<Interactable>().SecondGripped = true;
                if (OffsetObject.GetComponent<GrabPoint>().HelperGrip)
                {
                    DomanantGrip = false;
                    grabber.WeakGrip.connectedBody = ConnectedObject.GetComponent<Rigidbody>();
                    grabber.WeakGrip.connectedAnchor = OffsetObject.GetComponent<GrabPoint>().Offset;
                }
                grabber.WeakGrip.connectedBody = ConnectedObject.GetComponent<Rigidbody>();
                grabber.WeakGrip.connectedAnchor = OffsetObject.GetComponent<GrabPoint>().Offset;
            }
            else
            {
                ConnectedObject.GetComponent<Interactable>().Hand = Hand;
                ConnectedObject.GetComponent<Interactable>().gripped = true;
                if (!OffsetObject.GetComponent<GrabPoint>().HelperGrip)
                {
                    DomanantGrip = true;
                    ConnectedObject.GetComponent<Interactable>().GrippedBy = transform.parent.gameObject;
                }
            }
            /*
            if (OffsetObject.GetComponent<SteamVR_Skeleton_Poser>() && HandSkeleton)
            {
                HandSkeleton.transform.SetParent(OffsetObject, false);
                HandSkeleton.BlendToPoser(OffsetObject.GetComponent<SteamVR_Skeleton_Poser>(), 0f);
            }
            */


        }
    }
    private void Release()
    {
        grabber.FixedJoint.connectedBody = null;
        grabber.StrongGrip.connectedBody = null;
        grabber.WeakGrip.connectedBody = null;
        //ConnectedObject.GetComponent<Rigidbody>().velocity = position.GetVelocity(Hand) + transform.parent.GetComponent<Rigidbody>().velocity;
        //ConnectedObject.GetComponent<Rigidbody>().angularVelocity = position.GetAngularVelocity(Hand) + transform.parent.GetComponent<Rigidbody>().angularVelocity;
        ConnectedObject.GetComponent<Rigidbody>().useGravity = true;
        if (!ConnectedObject.GetComponent<Interactable>().SecondGripped)
        {

            ConnectedObject.GetComponent<Interactable>().gripped = false;

            ConnectedObject.GetComponent<Interactable>().GrippedBy = null;

        }
        else
        {
            ConnectedObject.GetComponent<Interactable>().SecondGripped = false;
        }

        ConnectedObject = null;
        /*
        if (OffsetObject.GetComponent<SteamVR_Skeleton_Poser>() && HandSkeleton)
        {
            HandSkeleton.transform.SetParent(transform, false);
            HandSkeleton.BlendToSkeleton();
        }
        */
        OffsetObject.GetComponent<GrabPoint>().Gripped = false;
        OffsetObject = null;
    }


    private float CurrentGripValue()
    {
        float grip = 0;

        if (!useSteamVR )
        {
            if(Hand.inputDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
                grip = gripValue;
        }

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        else if (useSteamVR)
        {
            grip = steamVRBasedController.gripValue;
        }
#endif

        else
        {
            Debug.LogError("You are using neither SteamVR nor XR. please check");
            grip = 0f;
        }

        return grip;
    }
}
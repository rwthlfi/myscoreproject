using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


#if UNITY_STANDALONE_WIN || UNITY_EDITOR
using Valve.VR;
#endif

namespace AvatarCreation
{
    //Ref; https://answers.unity.com/questions/1441796/what-are-some-of-the-values-of-xrxrdevicemodel.html?page=1&pageSize=5&sort=votes
    public class VRPlatformDetector : MonoBehaviour
    {
        [Header("Component Reference")]
        public GameObject OculusRig;
        public GameObject SteamVRRig;

        [Header("Script Reference")]
        public AvatarController avatarController;
        public AnimationController animationController;




        public bool useSteamVR = false;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        public CVRSystem hmd { get; private set; }

        public string hmd_Type { get { return GetStringProperty(ETrackedDeviceProperty.Prop_ModelNumber_String); } }
#endif



        private void Start()
        {
            // if the application is in android, forget it..., it has to be oculus... 
            // thank goodness no other provider use android
            if (Application.platform == RuntimePlatform.Android)
            {
                SetXRRig(OculusRig);
                return;
            }

            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                //now check which system are they now.

                SetXRRig(OculusRig);
                //SetXRRig(SteamVRRig);
            }



#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            else if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                hmd = OpenVR.System;
                Debug.Log("hmd " + hmd_Type);
                SetXRRig(SteamVRRig);
            }
#endif


        }

        

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        public string GetStringProperty(ETrackedDeviceProperty prop, uint deviceId = OpenVR.k_unTrackedDeviceIndex_Hmd)
        {
            var error = ETrackedPropertyError.TrackedProp_Success;
            var capactiy = hmd.GetStringTrackedDeviceProperty(deviceId, prop, null, 0, ref error);
            if (capactiy > 1)
            {
                var result = new System.Text.StringBuilder((int)capactiy);
                hmd.GetStringTrackedDeviceProperty(deviceId, prop, result, capactiy, ref error);
                return result.ToString();
            }
            return (error != ETrackedPropertyError.TrackedProp_Success) ? error.ToString() : "<unknown>";
        }
#endif


        private void SetXRRig(GameObject _glassType)
        {
            if (_glassType == SteamVRRig)
                useSteamVR = true;

            //activate the corresponding XR Rig
            _glassType.SetActive(true);

            //set the XRRig
            XRRig xrRig = GetComponent<XRRig>();
            xrRig.cameraFloorOffsetObject = _glassType;
            xrRig.cameraGameObject = _glassType.GetComponentInChildren<Camera>().gameObject;


            //setting the XRRig
            avatarController.head.vrTarget = _glassType.GetComponentInChildren<Camera>().transform;
            avatarController.leftHand.vrTarget = _glassType.GetComponentInChildren<LeftHandControllerReference>().transform;
            avatarController.rightHand.vrTarget = _glassType.GetComponentInChildren<RightHandControllerReference>().transform;


            //for saving which controller is which.
            avatarController.mapHandLeft.cacheController = _glassType.GetComponentInChildren<LeftHandControllerReference>().transform;
            avatarController.mapHandRight.cacheController = _glassType.GetComponentInChildren<RightHandControllerReference>().transform;

            //set the Controller value
            animationController.handsAnimatorL = _glassType.GetComponentInChildren<LeftHandControllerReference>().GetComponent<HandsAnimator>();
            animationController.handsAnimatorR = _glassType.GetComponentInChildren<RightHandControllerReference>().GetComponent<HandsAnimator>();
        
        
        }

    }

}

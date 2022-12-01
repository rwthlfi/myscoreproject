using System.Collections.Generic;
using UnityEngine;
using AvatarCreationHandTracking;

namespace AvatarCreation
{
    [System.Serializable]
    public class MapTransform
    {
        public Transform vrTarget;
        public Transform IKTarget;
        public Vector3 trackingPositionOffset;
        public Vector3 trackingRotationOffset;

        public void MapVRAvatar(Vector3 _trackPos, Vector3 _trackRot)
        {
            IKTarget.position = vrTarget.TransformPoint(_trackPos);
            IKTarget.rotation = vrTarget.rotation * Quaternion.Euler(_trackRot);
        }
    }

    [System.Serializable]
    public class MapHandTracker
    {
        public OVRHand ovrHand;
        public Vector3 ovrPos;
        public Vector3 ovrRot;
        [System.NonSerialized] public Transform cacheController; // has been serialized automatically in VrPlatformDetector
        public SkinnedMeshRenderer avatarHand;
        public GestureUIPointer gestureUIPointer;

        //this is to make the controller follow the hands position & rotation
        public void ControllerFollowHand()
        {
            cacheController.transform.position = ovrHand.transform.position;
            cacheController.transform.rotation = ovrHand.transform.rotation;
        }


        /// <summary>
        /// To Show the hand tracking or the controller.
        /// </summary>
        /// <param name="_show"></param>
        public void ShowHandTracking(bool _show)
        {
            if (_show)//hide the Avatar's hands and use the HandTracking
            {
                ovrHand.transform.localScale = Vector3.one;
                avatarHand.enabled = false;
            }

            else // show the Avatars Hand and hide HandTrackingModel
            {
                ovrHand.transform.localScale = Vector3.zero;
                avatarHand.enabled = true;
                gestureUIPointer.ShowRay(false);
            }



            //show or hide the hands accordingly
            //unfortunately it wont work when deactivating the gameobject.
            //threfore hiding it by setting the local scale to zero
            //ovrHandLeft.gameObject.SetActive(_show);
            //ovrHandRight.gameObject.SetActive(_show);
        }
    }

    public class AvatarController : MonoBehaviour
    {
        /*Hint:
        Since this script is used, both in Offline and Online Rig.
        therefore please activate the bool if it is an online rig.
        some variable like the MapTransform will not be use in online rig, but required to be activated in local player

        */

        public bool isLocalPlayer = false;

        [SerializeField] public MapTransform head;
        [SerializeField] public MapTransform leftHand;
        [SerializeField] public MapTransform rightHand;

        [Header("Detailed Variable")]
        [SerializeField] private float turnSmoothness;
        [SerializeField] private Transform IKHead;

        public Vector3 headBodyOffset;


        [Header("Culling Mask Variable")]
        public List<GameObject> cullObjectList = new List<GameObject>();
        public int cullLayer = 2;
        //public bool cullIt = false;
        public Transform target;
        public lockedRotation lockedRot;


        //dont worry about the device that runs outside android, 
        //cause the tracking will be automatically assigned to the default controller
        [Header("Oculus Hand Tracking")]
        public MapHandTracker mapHandLeft;
        public MapHandTracker mapHandRight;



        private void Start()
        {
            if (isLocalPlayer) // offline rig, then dont let the camera render the head, because the camera is inside the head.
                foreach (GameObject go in cullObjectList)
                    go.layer = cullLayer;
        }


        void Update()
        {   
            if (isLocalPlayer)
            {
                transform.position = IKHead.position + headBodyOffset;

                //get the look target, && make body to always face the target looking
                
                var lookPos = target.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);

                if (!head.vrTarget)
                    return;

                if (ConverterFunction.getAngleInspector(head.vrTarget).x >= 89)
                {
                    print("do something");
                    //lockedRot.enabled = false; // i know this code doesnt make sense., but believe me; we need it.
                }


                else
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSmoothness);
                    //lockedRot.enabled = true; // i know this code doesnt make sense., but believe me; we need it.
                }
                    
                

                if (GlobalSettings.handTrackingActive())
                    HandTrackingMapper();

                else
                {
                    //vr controller mapping
                    ControllerMapper();
                    head.MapVRAvatar(head.trackingPositionOffset, head.trackingRotationOffset);
                    leftHand.MapVRAvatar(leftHand.trackingPositionOffset, leftHand.trackingRotationOffset);
                    rightHand.MapVRAvatar(rightHand.trackingPositionOffset, rightHand.trackingRotationOffset);
                }
            }

            else
            {
                //OnlineHeadTracking();
            }
        }



        //public Vector3 debugTestPos = Vector3.zero;
        //public Vector3 debugTestRot = Vector3.zero;
        public void OnlineHeadTracking()
        {
            transform.position = IKHead.position + headBodyOffset;

            //get the look target, && make body to always face the target looking
            var lookPos = target.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSmoothness);

          //  IKHead.position = debugTestPos;
          //  IKHead.eulerAngles = debugTestRot;

        }



        //to change the rig mapping from XR controller to the hand tracking and viceversa
        private void HandTrackingMapper()
        {
            //change the ovr Hand Mapping
            leftHand.vrTarget = mapHandLeft.ovrHand.transform;
            rightHand.vrTarget = mapHandRight.ovrHand.transform;

            //When the hand tracking is active,
            //the Actual controller will follow the hand.
            //because many menu is attached to the actual controller
            mapHandLeft.ControllerFollowHand();
            mapHandRight.ControllerFollowHand();

            //create a new mapping
            head.MapVRAvatar(head.trackingPositionOffset, head.trackingRotationOffset);
            //leftHand.MapVRAvatar(Vector3.zero, mapHandLeft.ovrRot);
            //rightHand.MapVRAvatar(Vector3.zero, mapHandRight.ovrRot);

            leftHand.MapVRAvatar(mapHandLeft.ovrPos, mapHandLeft.ovrRot);
            rightHand.MapVRAvatar(mapHandRight.ovrPos, mapHandRight.ovrRot);


            //hide the Avatar's hands and use the HandTracking
            mapHandLeft.ShowHandTracking(true);
            mapHandRight.ShowHandTracking(true);

        }

        private void ControllerMapper()
        {
            //map back the hand to the controller
            leftHand.vrTarget = mapHandLeft.cacheController; 
            rightHand.vrTarget = mapHandRight.cacheController;
            //hide the Avatar's hands and use the HandTracking
            mapHandLeft.ShowHandTracking(false);
            mapHandRight.ShowHandTracking(false);
        }

    }
}

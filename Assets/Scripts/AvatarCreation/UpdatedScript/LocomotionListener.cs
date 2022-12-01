using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace AvatarCreation
{
    public class LocomotionListener : MonoBehaviour
    {
        //[System.NonSerialized] 
        public float moveSpeed = 1f;
        //[System.NonSerialized]
        public float turnSpeed = 60f;
        private float _moveSpeedOri, _turnSpeedOri;

        // these 3 Variables are only needed in Oculus,
        // because it has different architecture then steamVR
        private VRPlatformDetector vrplatformDetector;
        private DeviceBasedContinuousMoveProvider moveProvider;
        private DeviceBasedContinuousTurnProvider turnProvider;


        // these Variables are needed in SteamVR
        private CharacterController charController;
        private XRCameraReference centerEyeCamera;
        private bool enableMovement = true;
        private IEnumerator ienumerator;
    #if UNITY_STANDALONE_WIN || UNITY_EDITOR
        private SteamVRBasedController leftSteamVRBasedController = null;
        private SteamVRBasedController rightSteamVRBasedController = null;
        [Header("Steam VR Only")]
        public SteamVRBasedController teleporterSteamVRBasedController = null;
#endif
        Vector3 direction;

        private void Awake()
        {
            vrplatformDetector = this.transform.parent.root.GetComponent<VRPlatformDetector>();
            ienumerator = EnableMovement();
        }

        private void Start()
        {
            _moveSpeedOri = moveSpeed;
            _turnSpeedOri = turnSpeed;

            //only needed in Oculus
            if (!vrplatformDetector.useSteamVR)
            {
                moveProvider = GetComponent<DeviceBasedContinuousMoveProvider>();
                turnProvider = GetComponent<DeviceBasedContinuousTurnProvider>();
            }


    #if UNITY_STANDALONE_WIN || UNITY_EDITOR
            //only needed in SteamVR
            if (vrplatformDetector.useSteamVR)
            {
                charController = this.transform.parent.root.GetComponent<CharacterController>();
                centerEyeCamera = this.transform.parent.root.GetComponentInChildren<XRCameraReference>();

                leftSteamVRBasedController = charController.GetComponentInChildren<LeftHandControllerReference>().GetComponent<SteamVRBasedController>();
                rightSteamVRBasedController = charController.GetComponentInChildren<RightHandControllerReference>().GetComponent<SteamVRBasedController>();

            }
    #endif
        }

        private void Update()
        {
            //only needed in Oculus
            if (!vrplatformDetector.useSteamVR)
            {
                moveProvider.moveSpeed = moveSpeed;
                turnProvider.turnSpeed = turnSpeed;
            }


    #if UNITY_STANDALONE_WIN || UNITY_EDITOR
            //Only in SteamVR
            else if (vrplatformDetector.useSteamVR)
            {
                //Debug.Log(leftSteamVRBasedController.leftTouchPadValue);
                //Debug.Log(rightSteamVRBasedController.rightTouchPadValue);

                //if the teleport button is pressed
                if (teleporterSteamVRBasedController.teleportButtonValue)
                {
                    Debug.Log("asdf " + teleporterSteamVRBasedController.teleportButtonValue);
                    enableMovement = false;

                    StopCoroutine(ienumerator);
                    ienumerator = EnableMovement();
                    StartCoroutine(ienumerator);

                }

                //if not pressed; then re-enable the movement again after 3 seconds
                else
                {
                    //enableMovement = true;
                    //move the player
                    direction = centerEyeCamera.transform.TransformDirection(new Vector3(leftSteamVRBasedController.leftTouchPadValue.x,
                                                                                         0,
                                                                                         leftSteamVRBasedController.leftTouchPadValue.y));
                    if (enableMovement)
                        charController.Move(moveSpeed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up) - new Vector3(0, 9.8f, 0) * Time.deltaTime);
                }

                //rotate the player
                charController.transform.Rotate(0, rightSteamVRBasedController.rightTouchPadValue.x * this.turnSpeed * Time.deltaTime, 0);
            }

    #endif

        }

        //disable movement for x seconds
        private IEnumerator EnableMovement()
        {
            enableMovement = false;
            yield return new WaitForSeconds(1f);
            Debug.Log("Enable walking " + enableMovement);
            enableMovement = true;
        }

        public void MovementZero() // Sets movement to zero
        {
            moveSpeed = 0f;
            turnSpeed = 0f;
        }

        public void MovementOri() // Sets movement to original values
        {
            moveSpeed = _moveSpeedOri;
            turnSpeed = _turnSpeedOri;
        }

        public void ChangeMovement(float moveSpeedValue)
        {
            moveSpeed = moveSpeedValue;
        }

        public void ChangeTurn(float turnSpeedValue)
        {
            turnSpeed = turnSpeedValue;
        }
    }
}

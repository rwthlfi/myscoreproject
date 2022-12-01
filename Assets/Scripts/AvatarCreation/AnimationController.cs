using UnityEngine;
using UnityEngine.InputSystem;
using UnityStandardAssets.CrossPlatformInput;

namespace AvatarCreation
{
    public class AnimationController : MonoBehaviour
    {
        [Header("Animator Variable")]
        [SerializeField] private Transform rootAnim;
        [SerializeField] private Animator animator;
        private Transform avatarHead; //-> use to find the camera position to determine the animation of the sitting and crouching
        float currentHeadPos = 0f;

        public float minAllowedStanding = 1f;
        public float smoothness = 5f;
        public float multiplier = 1f;
        public Vector3 pos_SecondsAgo = Vector3.zero;
        public Vector3 rot_SecondsAgo = Vector3.zero;
        public float tolIdle_Pos = 0.01f;
        public float tolIdle_Rot = 3f;

        //Anim Parameter name
        string isMoving = "isMoving";
        string moveForward = "moveForward";
        string moveStrafe = "moveStrafe";
        string isCrouching = "isCrouching";
        string isSitting = "isSitting";
        string animSpeed = "animSpeed";
        bool isWalkingFoward;

        [Header("Script Reference")]
        public AvatarController avatarController;
        public LowerBodyAnimation lowerBodyAnimation;
        public Vector3 cacheAvatarHeadOffset;
        public Vector3 crouchDebug;
        public HandsAnimator handsAnimatorL;
        public HandsAnimator handsAnimatorR;


        private void Start()
        {
            cacheAvatarHeadOffset = avatarController.headBodyOffset;

            //repeat logging the position for x seconds to determine if the heads is moving or not.
            InvokeRepeating("LogPosRot", 0.5f, 0.5f);
        }


        /// <summary>
        /// to log the pos and rot
        /// </summary>
        void LogPosRot()
        {
            pos_SecondsAgo = avatarController.transform.localPosition;
            rot_SecondsAgo = avatarController.transform.localEulerAngles;
        }


        RaycastHit hit;
        private void FixedUpdate()
        {
            if (!avatarHead) //assign the head in case its not yet assign
                avatarHead = avatarController.head.IKTarget.transform;

            //check the head position and rotation
            //currentHeadPos = avatarHead.localPosition.y;

            if (Physics.Raycast(avatarHead.position, Vector3.down, out hit, 5, lowerBodyAnimation.castLayer.value))
            {
                currentHeadPos = hit.distance;
            }


            //if the head local position is lower than minAllowedStanding && head rotation x is more than 45 degree
            //play crouching animation
            if (currentHeadPos <= minAllowedStanding
                &&
                avatarHead.localEulerAngles.x >= 45 && avatarHead.localEulerAngles.x <= 135
               )
            {
                animator.SetBool(isCrouching, true);
                animator.SetBool(isMoving, false);
                //need to change the body offset to
                avatarController.headBodyOffset = crouchDebug;
            }

            else
            {
                animator.SetBool(isCrouching, false);
                animator.SetBool(isSitting, false);

                avatarController.headBodyOffset = cacheAvatarHeadOffset;

            }

            if (GlobalSettings.DeviceType() == GlobalSettings.Device.WindowsNonVR)
            {
                AnimateLegsMovingForward(new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical")), handsAnimatorR.GetJoystickValue()); // for PC movement animation, getting the input of the WASD keys
            }
            else
            {
                AnimateLegsMovingForward(handsAnimatorL.GetJoystickValue(), handsAnimatorR.GetJoystickValue());
            }


            //update the cache according to the scaling
            YOffsetCalculation_Standing();
            YOffsetCalculation_Crouching();

        }

        float tolerance = 0.025f;
        private void AnimateLegsMovingForward(Vector2 _joystickLeft, Vector2 _joystickRight)
        {

            //idling
            if (_joystickLeft.magnitude <= tolerance && _joystickLeft.magnitude >= -tolerance)
            {
                //print("_joystickRight.magnitude " + _joystickRight.magnitude);
                //print("a " + Vector3.Distance(pos_SecondsAgo, avatarController.transform.localPosition));
                //print("a " + Vector3.Distance(rot_SecondsAgo, avatarController.transform.localEulerAngles));

                //check if the head is moving or not
                if (Vector3.Distance(pos_SecondsAgo, avatarController.transform.localPosition) >= tolIdle_Pos
                    || Vector3.Distance(rot_SecondsAgo, avatarController.transform.localEulerAngles) >= tolIdle_Rot)
                {
                    //print("hit");

                    animator.SetBool(isMoving, true);
                    //animator.speed = 0.5f;
                    multiplier = 0.5f;
                    //animator.SetFloat(moveForward, Smoothing(moveForward, Random.Range(1f, 1f)));   //-> Idling
                    //animator.SetFloat(moveStrafe, Smoothing(moveStrafe, 0.25f));   //-> Idling
                }

                else if (_joystickRight.magnitude >= tolerance)
                {
                    animator.SetBool(isMoving, true);
                    multiplier = 1f;
                    AnimateLegsRotating(_joystickRight);
                }

                //else, if not moving, then do idling
                else
                {
                    //print("idle");
                    animator.SetBool(isMoving, false);
                    //animator.speed = 1f;
                    multiplier = 1f;
                    animator.SetFloat(moveForward, Smoothing(moveForward, 0)); //-> Idling
                    animator.SetFloat(moveStrafe, Smoothing(moveStrafe, 0));   //-> Idling

                }

            }

            else // walking and strafing -> Track the y value
            {
                //print("moving");
                animator.SetBool(isMoving, true);
                animator.SetFloat(moveForward, Smoothing(moveForward, _joystickLeft.y));
                animator.SetFloat(moveStrafe, Smoothing(moveStrafe, _joystickLeft.x));
            }

            //Set the speed from the joystick
            var animSpeed = Mathf.Abs(_joystickLeft.magnitude);
            if (animSpeed <= 0.5f)
                animSpeed = 1f;
            animator.speed = animSpeed * multiplier * PlayerPrefs.GetFloat(PrefDataList.playerAccelerationSpeed); // -> it is absolute because the animator cannot play negative value
            //animator.speed = multiplier * PlayerPrefs.GetFloat(PrefDataList.playerAccelerationSpeed); // -> it is absolute because the animator cannot play negative value
        }


        /// <summary>
        /// to animate the strafing with rotating
        /// </summary>
        /// <param name="_joystickValue"></param>
        private void AnimateLegsRotating(Vector2 _joystickValue)
        {
            //the Left controller is the main Controller of the strafing and the forward moving
            //therefore this function only called when the controller of the left is idling.

            print("animate right ");

            animator.SetFloat(moveStrafe, Smoothing(moveStrafe, _joystickValue.x));

            //Set the speed from the joystick
            var animSpeed = Mathf.Abs(_joystickValue.magnitude);
            if (animSpeed <= 0.5f)
                animSpeed = 1f;
            animator.speed = animSpeed * multiplier * PlayerPrefs.GetFloat(PrefDataList.playerAccelerationSpeed); // -> it is absolute because the animator cannot play negative value

        }

        /// <summary>
        /// for smoothing the speed but in general
        /// </summary>
        /// <param name="_targetValue"></param>
        /// <returns></returns>
        private float SmoothingSpeed(float _oriValue, float _targetValue)
        {
            float time = smoothness * Time.unscaledDeltaTime;
            float result;

            result = Mathf.MoveTowards(_oriValue, _targetValue, time);
            return result;
        }

        /// <summary>
        /// for smoothing out the transition
        /// </summary>
        /// <param name="_paramName">the animator parameter</param>
        /// <param name="_targetValue">target value</param>
        /// <returns></returns>
        private float Smoothing(string _paramName, float _targetValue)
        {
            float time = smoothness * Time.unscaledDeltaTime;
            float currentValue, result;
            currentValue = animator.GetFloat(_paramName);

            result = Mathf.MoveTowards(currentValue, _targetValue, time);
            return result;
        }




        private void YOffsetCalculation_Standing()
        {
            //note: taken in the real life measurement
            /*
             scale 0.9 -> offset: -1.35
             scale 1 -> offset: -1.5
             scale 1.15 -> offset: -1.75
            */
            //with linear interpolation finding the coeeficient..,
            //which resulting in -> offset = (-0.25 * scaling + 0.025) / 0.15

            //get the Avatar Scaling (one axis is enough, since every axis will have the same value)
            //set the offset
            //print("Root " + ((-0.25f * rootAnim.localScale.x) + 0.025f) / 0.15f);
            cacheAvatarHeadOffset.y = ((-0.25f * rootAnim.localScale.x) + 0.025f) / 0.15f;

        }


        private void YOffsetCalculation_Crouching()
        {
            //golden rule just add 0.5f to the cacheAvatarHeadOffset
            crouchDebug.y = cacheAvatarHeadOffset.y + 0.5f;
        }
    }
}

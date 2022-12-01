using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace AvatarCreation
{
    public class HandsAnimator : MonoBehaviour
    {
        private bool useSteamVR = false;

        [Header("Controller Variable")]
        public XRController controller = null;
        public float speed = 5.0f;

    #if UNITY_STANDALONE_WIN || UNITY_EDITOR
        private SteamVRBasedController steamVRBasedController = null;
    #endif

        [Header("Animator Variable")]
        public Animator m_animator = null;
        public string handsParam = "L"; // set this to L / R depending on the hand of the controller
        string gripParam = "Grip";
        string triggerParam = "Trigger";
        string thumbParam = "Thumb";


        private void Awake()
        {

    #if UNITY_STANDALONE_WIN || UNITY_EDITOR
            useSteamVR = controller.GetComponentInParent<VRPlatformDetector>().useSteamVR;
            if (useSteamVR)
                steamVRBasedController = controller.GetComponent<SteamVRBasedController>();
    #endif

            //setting proper naming
            gripParam = gripParam + handsParam;
            triggerParam = triggerParam + handsParam;
            thumbParam = thumbParam + handsParam;
        }


        // Update is called once per frame
        void Update()
        {

    #if UNITY_STANDALONE_WIN || UNITY_EDITOR
            //store input - of steam VR
            if (useSteamVR)
            {
                CheckSteamInput();
                return;
            }
    #endif

            CheckOculusInput();
        }

    #if UNITY_STANDALONE_WIN || UNITY_EDITOR
        private void CheckSteamInput()
        {
            HandAnimation(steamVRBasedController.gripValue,
                          steamVRBasedController.triggerValue,
                          true); // this will make the thumbs always goes down. (I havent found a way yet)

            /*
            HandAnimation(steamVRBasedController.gripValue,
                          steamVRBasedController.triggerValue,
                          steamVRBasedController.touchButtonValue);
            */
        }
    #endif

        private void CheckOculusInput()
        {
            float gripInput = 0f, triggerInput = 0f;
            bool thumbsInput = true;

            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
                gripInput = gripValue;

            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float pointerValue))
                triggerInput = pointerValue;
        
            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primaryTouch, out bool pValue)
                && controller.inputDevice.TryGetFeatureValue(CommonUsages.secondaryTouch, out bool sValue))
            {
                thumbsInput = true; // this will make the thumbs always goes down. (I havent found a way yet)
                //the commented version will be able to switch between thumbs up and down.
                //however, no one using that... Lol....
                //hence the commented one.
                /*
                if (pValue || sValue)
                    thumbsInput = true;
                else
                    thumbsInput = false;
                */

            }

            HandAnimation(gripInput, triggerInput, thumbsInput);
        }



        /// <summary>
        /// Will taking care of the hand animation, based on the user input
        /// </summary>
        /// <param name="_grip"> the controller's grip value</param>
        /// <param name="_trigger">the controller's trigger value</param>
        /// <param name="_thumb">the thumb's value</param>
        private void HandAnimation(float _grip, float _trigger, bool _thumb)
        {
            // default pos (0, 0, true)
            // Grab pos (1, 1, true)
            // pinch (0, 1, true)
            // pointing ( 1, 0, true)
            // thumbs up ( -1, -1, false) // however since the value of controller is impossible to be minus, we convert 0 to -1.

            //if thumbs is NOT pressed and the grip //trigger is 0
            //set them to -1
            if (!_thumb)
            {
                if (_grip <= 0.1) _grip = -1;
                if (_trigger <= 0.1) _trigger = -1;
            }

            /*
            //Just for debugging.
            if (handsParam == "R")
            {
                Debug.Log("Thumb " + _thumb);
                Debug.Log("_grip " + _grip);
                Debug.Log("_trigger " + _trigger);
            }

            */
            //otherwise just set them normally
            m_animator.SetBool(thumbParam, _thumb);
            //smooth the grip
            m_animator.SetFloat(gripParam, Smoothing(gripParam, _grip));
            m_animator.SetFloat(triggerParam, Smoothing(triggerParam, _trigger));
        }



        private float Smoothing(string _paramName, float _targetValue)
        {
            float time = speed * Time.unscaledDeltaTime;
            float currentValue, result;
            currentValue = m_animator.GetFloat(_paramName);

            result = Mathf.MoveTowards(currentValue, _targetValue, time);
            return result;
        }

        public Vector2 GetJoystickValue()
        {
            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 gripValue))
                return gripValue;
            else
                return Vector2.zero;
        }

    }
}

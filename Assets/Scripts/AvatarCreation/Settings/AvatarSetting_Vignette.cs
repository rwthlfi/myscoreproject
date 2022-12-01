using UnityEngine;
using UnityEngine.UI;

namespace AvatarCreation
{

    public class AvatarSetting_Vignette : MonoBehaviour
    {
        [Header("Game Object Variable")]
        public Transform vignetteOculus;
        public Transform vignetteSteam;

        [Header("Hands Variable")]
        public HandsAnimator handOculus_L;
        public HandsAnimator handOculus_R;

        public HandsAnimator handSteam_L;
        public HandsAnimator handSteam_R;

        [Header("Script reference")]
        public VRPlatformDetector vrPlatformDetector;

        //cache value
        public Transform currentVignette;
        HandsAnimator currentHandL;
        HandsAnimator currentHandR;
        float vigValue;
        float vanishThreshold = 1f;



        // Start is called before the first frame update
        void Start()
        {
            AssignCurrentVignnete();
            LoadTheSavedVignette();
        }

        private float nextActionTime = 0.0f;
        public float period = 0.2f;
        // Update is called once per frame
        void Update()
        {
            if (Time.time > nextActionTime)
            {
                nextActionTime = Time.time + period;
                //check the joystick every x seconds No need to be per frame
                if (!isCurrentlyUpdating)
                    isJoyStickMoved();
            }
        }


        private void AssignCurrentVignnete()
        {
            //just to setup some variable for easier use.
            if (vrPlatformDetector.useSteamVR)
            {
                currentVignette = vignetteSteam;
                currentHandL = handSteam_L;
                currentHandR = handSteam_R;
            }

            else
            {
                currentVignette = vignetteOculus;
                currentHandL = handOculus_L;
                currentHandR = handOculus_R;
            }
        }

        bool isCurrentlyUpdating = false;
        public void Ui_UpdateVignetteValue(Slider _slider)
        {
            isCurrentlyUpdating = true;
            vigValue = _slider.value;
            //and update it.
            UpdateVignetteScalling(vigValue);
            Invoke("NotUpdating", 3.0f);
        }

        private void NotUpdating()
        {
            isCurrentlyUpdating = false;
        }




        /// <summary>
        /// To load the saved Value
        /// </summary>
        public void LoadTheSavedVignette()
        {
            //get the saved value
            vigValue = PlayerPrefs.GetFloat(PrefDataList.vignette);
            //print("vigValue " + vigValue);
            //and update it
            UpdateVignetteScalling(vigValue);
        }



        /// <summary>
        /// the scaling of the vignette
        /// </summary>
        /// <param name="_trans">the vignette Canvas</param>
        /// <param name="_value">either get the value from the slider or the playerPrefs</param>
        public void UpdateVignetteScalling(float _value)
        {
            //if the vignette is empty, try to assign them again
            if (!currentVignette)
                AssignCurrentVignnete();

            //if 
            if (_value <= vanishThreshold)
                currentVignette.gameObject.SetActive(false);
            else
                currentVignette.gameObject.SetActive(true);

            //linear interpolation it. (you need to recalculate if min and max value of slider changed
            float targetScale = (_value - 5.4f) / -44.4f;
            currentVignette.localScale = new Vector3(targetScale, targetScale, targetScale);
            PlayerPrefs.SetFloat(PrefDataList.vignette, _value);

            /*
            if (_value > maxValue) // or more than the maximum slider
            {
                currentVignette.gameObject.SetActive(false);
               //return;
            }
            else
                currentVignette.gameObject.SetActive(true);

            // linear interpolation a bit
            float targetScale = _value / 100;

            currentVignette.localScale = new Vector3(targetScale, targetScale, targetScale);
            //save the value
            vigValue = _value;
            PlayerPrefs.SetFloat(PrefDataList.vignette, _value);
            //print("Value " + _value + " &targetScale " + targetScale) ;
            */
        }


        /// <summary>
        /// check if joysticks are moving for controlling the  vignette value
        /// </summary>
        /// <returns></returns>
        public bool isJoyStickMoved()
        {
            //get the cache value
            if (vigValue <= vanishThreshold) // dont need to do further things., cause the vignette is deactivated anyway
                return false;

            else
            {
                //get the joystick from the left and right hand.
                //if either one of the move, then spawn the vignette
                if (currentHandL.GetJoystickValue().magnitude >= 0.1f
                    || currentHandR.GetJoystickValue().magnitude >= 0.1f)
                {

                    float targetScale = (vigValue - 5.4f) / -44.4f;
                    currentVignette.localScale = new Vector3(targetScale, targetScale, targetScale);
                    //float targetScale = vigValue / 100;
                    //currentVignette.localScale = new Vector3(vigValue, vigValue, vigValue);
                    return true;
                }
                else
                {
                    currentVignette.localScale = Vector3.zero;
                    return false;
                }

            }
        }
    }
}

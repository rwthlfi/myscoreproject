using UnityEngine;
using UnityEngine.UI;

namespace AvatarCreation
{
    //this class is to set the speed of the avatar
    public class AvatarSetting_Speed : MonoBehaviour
    {
        //set the locomotion listener
        public LocomotionListener locomotionListener_Oculus;
        public LocomotionListener locomotionListener_Steam;


        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }


        /// <summary>
        /// set it for the Acceleration speed in the ui
        /// </summary>
        /// <param name="_speedSlider"></param>
        public void Ui_SetAccelerationSpeed(Slider _speedSlider)
        {
            locomotionListener_Oculus.moveSpeed = _speedSlider.value;
            locomotionListener_Steam.moveSpeed = _speedSlider.value;
            //save the speed
            PlayerPrefs.SetFloat(PrefDataList.playerAccelerationSpeed, _speedSlider.value);
            //extra checking
            LoadSpeed_Acceleration();
        }


        /// <summary>
        /// set the rotation speed in the ui
        /// </summary>
        /// <param name="_rotSlider"></param>
        public void Ui_SetRotationSpeed(Slider _rotSlider)
        {
            locomotionListener_Oculus.turnSpeed = _rotSlider.value;
            locomotionListener_Steam.turnSpeed = _rotSlider.value;
            //save the rotation speed
            PlayerPrefs.SetFloat(PrefDataList.playerRotationSpeed, _rotSlider.value);
            //extra checking
            LoadSpeed_Rotation();

        }

        /// <summary>
        /// to update the acceleration slider just for visualization
        /// </summary>
        /// <param name="_slider"></param>
        public void UpdateSlider_Acc(Slider _slider)
        {
            //set the slider just for visual
            _slider.Set(PlayerPrefs.GetFloat(PrefDataList.playerAccelerationSpeed), false);
        }


        /// <summary>
        /// to update the rotation slider just for visualization
        /// </summary>
        /// <param name="_slider"></param>
        public void UpdateSlider_Turn(Slider _slider)
        {
            //set the slider just for visual
            _slider.Set(PlayerPrefs.GetFloat(PrefDataList.playerRotationSpeed), false);
        }


        /// <summary>
        /// load up the saved speed
        /// </summary>
        public void LoadSpeed_Acceleration()
        {
            if (PlayerPrefs.GetFloat(PrefDataList.playerAccelerationSpeed) <= 0.3f)
            {
                PlayerPrefs.SetFloat(PrefDataList.playerAccelerationSpeed, 1f);
                if (locomotionListener_Oculus != null)
                    locomotionListener_Oculus.moveSpeed = 1f;
                if (locomotionListener_Steam != null)
                    locomotionListener_Steam.moveSpeed = 1f;
            }

            else
            {
                if (locomotionListener_Oculus != null)
                    locomotionListener_Oculus.moveSpeed = PlayerPrefs.GetFloat(PrefDataList.playerAccelerationSpeed);
                if (locomotionListener_Steam != null)
                    locomotionListener_Steam.moveSpeed = PlayerPrefs.GetFloat(PrefDataList.playerAccelerationSpeed);
            }
        }


        /// <summary>
        /// load up the saved rotation
        /// </summary>
        public void LoadSpeed_Rotation()
        {
            if (PlayerPrefs.GetFloat(PrefDataList.playerRotationSpeed) <= 0.2f)
            {
                PlayerPrefs.SetFloat(PrefDataList.playerRotationSpeed, 50f);
                if (locomotionListener_Oculus != null)
                    locomotionListener_Oculus.turnSpeed = 50f;
                if (locomotionListener_Steam != null)
                    locomotionListener_Steam.turnSpeed = 50f;
            }

            else
            {
                if (locomotionListener_Oculus != null)
                    locomotionListener_Oculus.moveSpeed = PlayerPrefs.GetFloat(PrefDataList.playerAccelerationSpeed);
                if (locomotionListener_Steam != null)
                    locomotionListener_Steam.moveSpeed = PlayerPrefs.GetFloat(PrefDataList.playerAccelerationSpeed);
            }

        }


    }
}

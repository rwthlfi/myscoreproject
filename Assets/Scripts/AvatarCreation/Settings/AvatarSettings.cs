using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AvatarCreation
{
    //this class is intended to;
    //- set the height
    //- set and load up the AvatarsSpeed
    //- vignette things
    //- show hide controller
    //- language etc
    //Remarks : Everything is done locally

    public class AvatarSettings : MonoBehaviour
    {
        [Header("Activation Method")]
        public bool initUI = false;
        public bool isNPC = false;


        [Header("Ui Reference")]
        public GameObject menuAvatar;
        public GameObject menuLanguage;
        public GameObject menuSetting;


        [Header("Ui Settings")]
        public Slider sliderHeight;
        public Slider sliderAcc;
        public Slider sliderTurn;
        public Toggle toggleControllerMesh;
        public Toggle toggleControllerInfo;
        public Slider sliderVignette;



        [Header("Script reference")]
        public AvatarSetting_Renderer avatarSetting_Renderer;
        public AvatarSetting_Height avatarSetting_Height;
        public AvatarSetting_Speed avatarSetting_Speed;
        public AvatarSettings_Controller avatarSetting_controller;
        public AvatarSetting_Vignette avatarSetting_vignette;


        // Start is called before the first frame update
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(2f);
            if (!hasPlayed())
            {
                SetPlayerSettingToDefault();
                print("hasn't played");
            }


            if (initUI)
            {
                //Load and Set the menu accordingly
                ActivatesMenu(false);
                //load the first ui
                menuAvatar.SetActive(true);
                //load the avatar settings
                avatarSetting_Renderer.InitFaceSetup();

            }

            if (!isNPC)
            {
                avatarSetting_Renderer.LoadFace();

                SetPlayerValue();
                SetUiSettingValue();

                //sliderVignette.value = sliderVignette.maxValue/2;
            }
        }

        /// <summary>
        /// To Load all the Saved Player Value
        /// </summary>
        public void SetPlayerValue()
        {
            //load the saved custome Avatar
            avatarSetting_Renderer.CheckSavedAvatar();
            if (avatarSetting_Height != null)
                avatarSetting_Height.LoadAvatarHeight();

            //load the rest of the setting.
            avatarSetting_Speed.LoadSpeed_Acceleration();
            avatarSetting_Speed.LoadSpeed_Rotation();
            avatarSetting_vignette.LoadTheSavedVignette();
        }


        /// <summary>
        /// To set the ui Setting according to the User Value
        /// </summary>
        public void SetUiSettingValue()
        {
            //set the slider for setting
            sliderHeight.Set(PlayerPrefs.GetFloat(PrefDataList.avatarHeight), true);
            sliderAcc.Set(PlayerPrefs.GetFloat(PrefDataList.playerAccelerationSpeed), true);
            sliderTurn.Set(PlayerPrefs.GetFloat(PrefDataList.playerRotationSpeed), true);
            //set the toggle setting
            toggleControllerMesh.Set(ConverterFunction.IntToBool(PlayerPrefs.GetInt(PrefDataList.showController)), true);
            toggleControllerInfo.Set(ConverterFunction.IntToBool(PlayerPrefs.GetInt(PrefDataList.InfoController)), true);
            //set the slider for Vignette
            sliderVignette.Set(PlayerPrefs.GetFloat(PrefDataList.vignette), true);
        }

        public void SetUiSettingValuePC()
        {
            PlayerPrefs.SetFloat(PrefDataList.avatarHeight, 1);
        }

        /// <summary>
        /// to check if the player has open the app the first time or not
        /// </summary>
        /// <returns></returns>
        public bool hasPlayed()
        {
            if (PlayerPrefs.GetInt(PrefDataList.hasPlayedBefore) == 0)
            {
                //set the "hasPlayedBefore" to 1
                //which mean that the application is not anymore being launch for the first time.
                PlayerPrefs.SetInt(PrefDataList.hasPlayedBefore, 1);
                return false;// and return true
            }
            else
                return true; // otherwise the player hasnt played before
        }


        /// <summary>
        /// this is to reset every player setting to default value
        /// </summary>
        private void SetPlayerSettingToDefault()
        {
            //assign unique ID
            PlayerPrefs.SetString(PrefDataList.savedCreatorID, UniqueIDGenerator.GenerateID_Guid());

            //to easily set the player setting to default just play arround with the "prefdataList"
            //set the default avatar
            PlayerPrefs.SetInt(PrefDataList.avatarID, 0);

            //set the Avatar height
            PlayerPrefs.SetFloat(PrefDataList.avatarHeight, 1);

            //set the vignette Effect
            PlayerPrefs.SetFloat(PrefDataList.vignette, 0f);

            //set the show controller
            PlayerPrefs.SetInt(PrefDataList.showController, 1);

            //set the show controller's info
            PlayerPrefs.SetInt(PrefDataList.InfoController, 1);

            //set the player acceleration & rotation speed
            PlayerPrefs.SetFloat(PrefDataList.playerAccelerationSpeed, 1f);
            PlayerPrefs.SetFloat(PrefDataList.playerRotationSpeed, 50f);

            //set the haptic and UI audio feedback
            PlayerPrefs.SetFloat(PrefDataList.activateHaptics, 1);
            PlayerPrefs.SetFloat(PrefDataList.activateUIAudio, 1);
        }


        public void Ui_AvatarMenuClicked()
        {
            ActivatesMenu(false);
            menuAvatar.SetActive(true);
        }


        public void Ui_LanguageClicked()
        {
            ActivatesMenu(false);
            menuLanguage.SetActive(true);
        }

        public void Ui_SettingClicked()
        {
            ActivatesMenu(false);
            menuSetting.SetActive(true);
        }

        private void ActivatesMenu(bool _value)
        {
            menuAvatar.SetActive(_value);
            menuLanguage.SetActive(_value);
            menuSetting.SetActive(_value);
        }
    }



}

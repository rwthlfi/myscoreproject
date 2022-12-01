using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarCreation
{
    public class Ui_MainAVatar : MonoBehaviour
    {
        [Header("Ui Menu")]
        public GameObject menuMainSelection; // this is to show the main menu where the user can choose individualize avatar and Generalize avatar

        [Header("SubMenu - Lecturers")]
        public GameObject menuLecturer;
        public GameObject menuCustomizeYourAvatar;


        [Header("SubMenu - Customize Menu")]
        public GameObject customize_menuMain;
        public GameObject customize_faceMenu;
        public GameObject customize_customeMenu;

        [Header("Script Reference")]
        public AvatarSetting_Renderer avatarSetingRenderer;
        public Ui_AvatarCustomeSelection ui_avatarCustomeSelection;

        // Start is called before the first frame update
        void Start()
        {
            ActivateAllMenu(false);
            menuMainSelection.SetActive(true);
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        // - - - - the main menu - - - - //
        // - you can only select the lectuter and customize your avatar

        public void Ui_lecturerMenu_isclicked()
        {
            ActivateAllMenu(false);
            menuLecturer.SetActive(true);

            //load the saved one
            avatarSetingRenderer.Load_SavedCustome(PlayerPrefs.GetString(PrefDataList.savedLecturerCustome));
            //cache it
            PlayerPrefs.SetString(PrefDataList.savedPreviousCustome, PlayerPrefs.GetString(PrefDataList.savedLecturerCustome));
        }

        public void Ui_CustomizeYourAvatarMenu_isclicked()
        {
            ActivateAllMenu(false);
            menuCustomizeYourAvatar.SetActive(true);
            customize_menuMain.SetActive(true);

            //load the saved one
            avatarSetingRenderer.Load_SavedCustome(PlayerPrefs.GetString(PrefDataList.savedAvatarCustome));
            //cache it
            PlayerPrefs.SetString(PrefDataList.savedPreviousCustome, PlayerPrefs.GetString(PrefDataList.savedAvatarCustome));
        }



        // - - - - - the individualize avatar - - - - - - - - - //
        public void Ui_lecturer_GoBackToMainMenu()
        {
            ActivateAllMenu(false);
            menuMainSelection.SetActive(true);

        }



        //- - - - - the Customize your avatar menu - - - - - - //


        //to go back to  the main menu
        public void Ui_Customize_GoBackToMainMenu()
        {
            ActivateAllMenu(false);
            menuCustomizeYourAvatar.SetActive(true);
            customize_menuMain.SetActive(true);
        }



        //To Activate the subMenu
        public void Ui_Customize_ActivateFaceMenu()
        {
            ActivateAllMenu(false);
            menuCustomizeYourAvatar.SetActive(true);
            customize_faceMenu.SetActive(true);
            //activate only the Custome Set
            ui_avatarCustomeSelection.Ui_All_Faces();
        }

        public void Ui_Customize_ActivateSkinHairMenu()
        {
            ActivateAllMenu(false);
            menuCustomizeYourAvatar.SetActive(true);
            customize_customeMenu.SetActive(true);
            //activate only the Custome Set
            ui_avatarCustomeSelection.Ui_All_Faces();
        }


        public void Ui_Customize_ActivateClothesMenu()
        {
            ActivateAllMenu(false);
            menuCustomizeYourAvatar.SetActive(true);
            customize_customeMenu.SetActive(true);
            //activate only the Custome Set
            ui_avatarCustomeSelection.Ui_All_Custome();
        }

        public void Ui_RandomizeAvatar()
        {
            avatarSetingRenderer.RandomizeCustomeValue();
            avatarSetingRenderer.Load_SavedCustome(PlayerPrefs.GetString(PrefDataList.savedPreviousCustome));
        }


        /// <summary>
        /// to disable or activated the menu
        /// </summary>
        /// <param name="_value"></param>
        private void ActivateAllMenu(bool _value)
        {
            menuMainSelection.SetActive(_value);
            menuLecturer.SetActive(_value);
            menuCustomizeYourAvatar.SetActive(_value);
            

            customize_menuMain.SetActive(_value);
            customize_faceMenu.SetActive(_value);
            customize_customeMenu.SetActive(_value);
        }
    }

}

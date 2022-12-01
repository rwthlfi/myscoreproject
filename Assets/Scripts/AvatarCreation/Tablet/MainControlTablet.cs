using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

namespace AvatarTablet
{
    public class MainControlTablet : MonoBehaviour
    {
        [Header("Menu Window")]
        public Toggle adminToggle;
        public Toggle muteToggle;
        public Toggle globalSpeakToggle;
        public Button logoutButton;
        public Toggle shareScreenToggle;

        [Header("Info Message")]
        public GameObject info_youAreKickedOut;
        public GameObject info_teleportedScene;
        public GameObject info_youAreMuted;
        public GameObject info_yourItemIsRemovedByAdmin;
        public GameObject info_yourShareScreenIsTurnOffByAdmin;

        [Header("Script Reference")]
        public PeopleWindow peopleWindow;
        public ItemManagerWindow itemManagerWindow;
        public ShareScreenWindow shareScreenWindow;


        private void Start()
        {
            EnableAdmin(false);
        }

        /// <summary>
        /// To Enable the Admin Functionality
        /// </summary>
        /// <param name="_value"></param>
        public void EnableAdmin(bool _value)
        {
            adminToggle.gameObject.SetActive(_value);
            globalSpeakToggle.gameObject.SetActive(_value);
            peopleWindow.EnableExtraMenu(_value);
            itemManagerWindow.EnableExtraMenu(_value);
        }




        /// <summary>
        /// To Execute the people gettingKicked out
        /// </summary>
        /// <param name="_info"></param>
        public IEnumerator ExecuteKickedOut()
        {
            info_youAreKickedOut.SetActive(true);
            yield return new WaitForSeconds(3f);
            logoutButton.onClick.Invoke();
        }




        /// <summary>
        /// show message that you are muted
        /// </summary>
        public void ShowInfo_MutedByAdmin()
        {
            StartCoroutine(CoroutineExtensions.HideAfterSeconds(info_youAreMuted, 5f));
        }

        /// <summary>
        /// to show message that one of your item is removed
        /// </summary>
        public void ShowInfo_ItemRemoved()
        {
            StartCoroutine(CoroutineExtensions.HideAfterSeconds(info_yourItemIsRemovedByAdmin, 5f));
        }


        /// <summary>
        /// show message that you are muted
        /// </summary>
        public void ShowInfo_ShareScreenTurnOffByAdmin()
        {
            StartCoroutine(CoroutineExtensions.HideAfterSeconds(info_yourShareScreenIsTurnOffByAdmin, 5f));
        }



        //require for the Close button
        public void SetToggle_off(Toggle _toggle)
        {
            _toggle.isOn = false;
        }

        //Require for clode button too
        public void SetButton_click(Button _btn)
        {
            _btn.onClick.Invoke();
        }

    }


}

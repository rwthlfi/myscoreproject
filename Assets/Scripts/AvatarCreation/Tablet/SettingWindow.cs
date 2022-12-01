using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using Mirror;


namespace AvatarTablet
{
    public class SettingWindow : MonoBehaviour
    {
        [Header("Ui Variable")]
        public List<GameObject> windows = new List<GameObject>();

        [Header("Show/Hide Panel Variable")]
        public GameObject mainMenu;
        public GameObject showCanvas;
        public GameObject moveMenu;

        [Header("Recenter Variable")]
        public List<Transform> windowToRecenter = new List<Transform>();
        public Transform pivot;

        [Header("Info Text")]
        public GameObject infoHidePanel;


        [Header("Script Reference")]
        public AvatarCreation.VRPlatformDetector vrPlatformDetector;
        public Transform refOculus;
        public Transform refSteam;


        private void Start()
        {
            ShowWindow(windows[0]);
        }


        /// <summary>
        /// Recenter all the windows according to the pivot.
        /// </summary>
        public void Ui_RecenterWindow()
        {
            foreach(Transform tr in windowToRecenter)
            {
                StartCoroutine(LerpingExtensions.MoveTo(tr, pivot.transform.position, 1f));
                StartCoroutine(LerpingExtensions.RotateTo(tr, pivot.transform.rotation, 1f));
            }
        }


        /// <summary>
        /// to show the main Panel
        /// </summary>
        /// <param name="_value"></param>
        public void Ui_ShowPanel(bool _value)
        {
            //if show -> 
            if (_value)
            {
                mainMenu.SetActive(_value);

                //Do the opposite
                showCanvas.SetActive(!_value);
            }
            else // hide ->
            {
                StartCoroutine(HidePanel());
            }
        }


        /// <summary>
        /// To show the main Panel., but its location depends on where the user is looking.
        /// </summary>
        /// <param name="_value"></param>
        public void Ui_ShowPanel_InFrontOfCamera(bool _value)
        {
            //if show -> 
            if (_value)
            {
                mainMenu.SetActive(_value);
                //Do the opposite
                showCanvas.SetActive(!_value);
            }
            else // hide ->
            {
                StartCoroutine(HidePanel());
            }

            if (vrPlatformDetector.useSteamVR)
            {
                moveMenu.transform.position = refSteam.position;
                moveMenu.transform.rotation = refSteam.rotation;
            }
            else
            {
                moveMenu.transform.position = refOculus.position;
                moveMenu.transform.rotation = refOculus.rotation;

            }

        }



        /// <summary>
        /// Hiding the Main panel
        /// </summary>
        /// <returns></returns>
        private IEnumerator HidePanel()
        {
            //show info for x seconds
            infoHidePanel.gameObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            infoHidePanel.gameObject.SetActive(false);

            //deactivate the panel
            mainMenu.SetActive(false);

            //and do the opposite for the show Canvas
            showCanvas.SetActive(true);

        }



        /// <summary>
        /// For logging Out
        /// </summary>
        public void Ui_Logout()
        {
            NetworkClient.connection.Disconnect();
            SceneManager.LoadScene(GlobalSettings.WelcomeScene);
        }


        public void Ui_OpenWindow(GameObject _window)
        {
            ShowWindow(_window);
        }


        private void ShowWindow(GameObject _window)
        {
            foreach (GameObject _gObj in windows)
                _gObj.SetActive(false);
            _window.SetActive(true);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoomService;
using UnityEngine.UI;

namespace AvatarTablet
{

    public class AdminWindow : MonoBehaviour
    {
        [Header("Ui Variable")]
        public List<GameObject> windows = new List<GameObject>();
        public Button buttonEnvironment;
        public Button buttonIncognito;
        public Button buttonRolePlay;

        //windows
        public Transform dummyWindow;
        private EnvironmentWindow windowEnvironment;
        private RolePlayWindow windowRolePlay; // role Play is kinda a bit different

        private Transform windowIncognito;
        public Transform refPosRot;

        [Header("Script Reference")]
        public TheRoomServices theRoomServices;



        private void Start()
        {
            ShowWindow(windows[0]);
            //disable all button first
            EnableGadgetButtons(false);

            //get the room Services
            if (!theRoomServices)
                theRoomServices = (TheRoomServices)FindObjectOfType(typeof(TheRoomServices));

            //Check the availability of the gadget from the room Services
            windowEnvironment = theRoomServices.GetComponent<EnvironmentWindow>();
            if (windowEnvironment)
                buttonEnvironment.gameObject.SetActive(true);

            windowRolePlay = theRoomServices.GetComponent<RolePlayWindow>();
            if (windowRolePlay)
                buttonRolePlay.gameObject.SetActive(true);

        }


        private void EnableGadgetButtons(bool _value)
        {
            buttonEnvironment.gameObject.SetActive(_value);
            buttonRolePlay.gameObject.SetActive(_value);
            buttonIncognito.gameObject.SetActive(_value);
        }

        /// <summary>
        /// To call the gadget that is located in the scenario
        /// Attach this in the UI
        /// </summary>
        /// <param name="_gadgetID"></param>
        public void CallGadget(int _gadgetID)
        {
            switch (_gadgetID)
            {
                case 0: dummyWindow = windowEnvironment.environment_Ui.transform; break;
                case 1: dummyWindow = windowRolePlay.RolePlayWindowUI.transform; break;
                case 2: dummyWindow = windowIncognito; break;
            }

            //set the parent
            dummyWindow.SetParent(refPosRot, false);

            //set the position and rotation
            dummyWindow.position = refPosRot.position;
            dummyWindow.rotation = refPosRot.rotation;
            dummyWindow.gameObject.SetActive(true);
        }





        /// <summary>
        /// to change the scenario based on the user ID in the GlobalSettings
        /// </summary>
        /// <param name="_scenarioID"></param>
        public void Ui_ChangeScenario(int _scenarioID)
        {
            if (!theRoomServices)
                theRoomServices = (TheRoomServices)FindObjectOfType(typeof(TheRoomServices));

            StartCoroutine(theRoomServices.ChangeScene(_scenarioID));
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
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.SceneManagement;
using AvatarCreation;

public class FPCOnlineChanges : MonoBehaviour
{
    [Header("Misc")]
    public bool isWindowsPlayerNoHmd;
    public bool localPlayer;

    [Header("Deactivated for FPC - Online / Offline Rig")]
    public GameObject cameraOculusVR;
    public GameObject keyboard;
    public GameObject leftHandController;
    public GameObject rightHandController;
    public GameObject uIRayInteractorLeft;
    public GameObject uIRayInteractorRight;

    [Header("Activate for FPC - Online / Offline Rig")]
    public InputActionManager inputActionManager;

    [Header("Deactivated for FPC - Online Rig")]
    public GameObject locomotion;
    public GameObject teleportRayInteractor;
    public GameObject vRKeys;
    public GameObject mainControlPanelButton;
    public GameObject mainControlShowKeyboardButton;
    public GameObject toolButtonMarker, toolButtonKeyboard, toolButtonStickyNote;

    [Header("Activate for FPC - Offline Rig")]
    public GameObject cameraSteamVR;
    public FirstPersonController firstPersonController;

    [Header("Misc FPC - Online Rig")]
    public GameObject mainControl, mainControlMovePanel;
    //public CanvasWebViewPrefab webViewPrefab;
    public GameObject userPanel;

    [Header("More Settings")]
    public AvatarController avatarController;
    public GameObject mainCamera;
    public GameObject uIRayInteractorRightFPC;
    private float _initWalkSpeed;
    private float _initRunSpeed;
    private bool _mainMenuActive;
    private bool _sitting = false;
    private Scene _currentScene;
    public GameObject vignetteCanvas;


    //Start is called before the first frame update
    private IEnumerator Start()
    {
        _currentScene = SceneManager.GetActiveScene();

        if (GlobalSettings.DeviceType() == GlobalSettings.Device.WindowsNonVR && localPlayer)
        {
            isWindowsPlayerNoHmd = true;
        }
        else
        {
            firstPersonController.enabled = false;
            isWindowsPlayerNoHmd = false;

            enabled = false; // disables this script if FPC is not active
        }

        yield return new WaitForSeconds(0.1f);

        if (isWindowsPlayerNoHmd)
        {
            _initWalkSpeed = firstPersonController.m_WalkSpeed;
            _initRunSpeed = firstPersonController.m_RunSpeed;


            cameraOculusVR.SetActive(false);
            cameraSteamVR.SetActive(true);

            if (locomotion != null)
            {
                locomotion.SetActive(false);
            }

            if (teleportRayInteractor != null)
            {
                teleportRayInteractor.SetActive(false);
            }

            if (userPanel != null)
            {
                userPanel.SetActive(true);
            }

            leftHandController.SetActive(false);
            rightHandController.SetActive(false);

            uIRayInteractorLeft.SetActive(false);
            uIRayInteractorRight.SetActive(false);
            uIRayInteractorRightFPC.SetActive(true);
            uIRayInteractorRightFPC.transform.parent = mainCamera.transform;
            uIRayInteractorRightFPC.GetComponent<XRInteractorLineVisual>().lineLength = 0;

            firstPersonController.enabled = true;

            _sitting = false;

            ChangeVRConstraintsFPC();

            yield return new WaitForSeconds(0.2f);

            mainCamera.transform.localPosition = new Vector3(0.0f, 1.6f, 0.1f);
            vignetteCanvas.SetActive(false);
            inputActionManager.enabled = true;

            if (mainControl != null)
            {
                _mainMenuActive = false;
                mainControl.SetActive(false);
                mainControlMovePanel.SetActive(false);
                mainControl.transform.localPosition = new Vector3(0, -1000, 0);
                keyboard.transform.localPosition = new Vector3(0, -1000, 0);

                toolButtonMarker.SetActive(false);
                toolButtonKeyboard.SetActive(false);
                toolButtonStickyNote.SetActive(false);
            }

            keyboard.SetActive(false);
        }
    }

    public void ChangeVRConstraintsFPC()
    {
        if (!_sitting)
        {
            mainCamera.transform.localPosition = new Vector3(0.0f, 1.6f, 0.1f);
            avatarController.head.trackingPositionOffset.y = 1.5f;
            avatarController.leftHand.trackingPositionOffset = new Vector3(-0.25f, 1f, 0.2f);
            avatarController.leftHand.trackingRotationOffset = new Vector3(20f, 160f, 135f);
            avatarController.rightHand.trackingPositionOffset = new Vector3(0.25f, 1f, 0.2f);
            avatarController.rightHand.trackingRotationOffset = new Vector3(20f, -160f, -135f);
        }
        else
        {
            mainCamera.transform.localPosition = new Vector3(0.0f, 1.0f, 0.1f);
            avatarController.head.trackingPositionOffset.y = 1f;
            avatarController.leftHand.trackingPositionOffset = new Vector3(-0.25f, 0.6f, 0.2f);
            avatarController.rightHand.trackingPositionOffset = new Vector3(0.25f, 0.6f, 0.2f);
        }
    }

    public void Update()
    {
        if (isWindowsPlayerNoHmd)
        {
            var CameraRotation = mainCamera.transform.eulerAngles.x;
            avatarController.head.trackingRotationOffset.x = CameraRotation;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_sitting)
            {
                _sitting = false;
                ChangeVRConstraintsFPC();
            }
            else if (!_sitting)
            {
                _sitting = true;
                ChangeVRConstraintsFPC();
            }
        }

        if (Input.GetMouseButtonDown(1)) // UI menu in online scenes
        {
            if (mainControl != null)
            {
                if (_mainMenuActive) // deactivate UI menu
                {
                    mainControl.transform.SetParent(firstPersonController.transform);
                    mainControl.transform.localPosition = new Vector3(0f, 1.37f, 0.64f);
                    mainControl.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    mainControl.SetActive(false);

                    _mainMenuActive = false;
                    mainControlPanelButton.SetActive(false);
                    mainControlShowKeyboardButton.SetActive(false);
                    vRKeys.SetActive(false);
                    keyboard.SetActive(false);

                    if (GlobalSettings.DeviceType() == GlobalSettings.Device.WindowsNonVR)
                    {
                        if (_currentScene.buildIndex == 4) // Check if User is inside 360 sphere in rektorat and prevent walking after menu closing
                        {
                            if (transform.localPosition.y > 500)
                            {
                                firstPersonController.m_WalkSpeed = _initWalkSpeed;
                                firstPersonController.m_RunSpeed = _initRunSpeed;
                            }
                        }
                        else
                        {
                            firstPersonController.m_WalkSpeed = _initWalkSpeed;
                            firstPersonController.m_RunSpeed = _initRunSpeed;
                        }
                    }
                }
                else if (!_mainMenuActive) // activate UI menu
                {
                    mainControl.SetActive(true);
                    mainControl.transform.SetParent(firstPersonController.transform);
                    mainControl.transform.localPosition = new Vector3(0f, 1.37f, 0.64f);
                    mainControl.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    mainControl.transform.SetParent(null);

                    _mainMenuActive = true;
                    mainControlPanelButton.SetActive(false);
                    mainControlShowKeyboardButton.SetActive(false);
                    vRKeys.SetActive(false);
                    keyboard.SetActive(false);

                    if (GlobalSettings.DeviceType() == GlobalSettings.Device.WindowsNonVR)
                    {
                        if (_currentScene.buildIndex == 4) // Check if User is inside 360 sphere in rektorat and prevent walking after menu closing
                        {
                            if (transform.localPosition.y > 500)
                            {
                                firstPersonController.m_WalkSpeed = 0f;
                                firstPersonController.m_RunSpeed = 0f;
                            }
                        }
                        else
                        {
                            firstPersonController.m_WalkSpeed = 0f;
                            firstPersonController.m_RunSpeed = 0f;
                        }
                    }
                }
            }
        }
        /*
        //get the web view prefab and add the keyboard input for the browser
        if (webViewPrefab != null)
        {
            if (webViewPrefab.gameObject.activeInHierarchy)
            {
                if (Input.anyKeyDown)
                {
                    VuplexListener(Input.inputString);
                }
            }
        }*/
    }
    /*
    public void VuplexListener(string _str)
    {
        string value = _str;
        if (!webViewPrefab)
            return;

        if (_str == "Clear")
            webViewPrefab.WebView.SelectAll();

        webViewPrefab.WebView.HandleKeyboardInput(value);
    }*/
}
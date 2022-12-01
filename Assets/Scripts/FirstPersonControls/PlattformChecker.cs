using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.UI;
using AvatarCreation;

public class PlattformChecker : MonoBehaviour
{

    // Script to deactivate all gameobjects that the FPSController don't need in the scenes, will be effective when the game is exported as Windows Standalone
    // GameObject OVR Controller must be active in Welcome Scene / FPS Controller must be inactive on start in the scene to prevent issues, they will be handled here via script

    [Header("Misc")]
    public bool isWindowsPlayerNoHmd;

    [Header("All Scene Objects")]
    public GameObject dissonanceSetup;
    public GameObject eventsystem;
    private AvatarSettings _avatarSettings;

    //Welcome Scene Objects:
    [Header("Welcome Scene Objects")]
    public GameObject vRSettingsButton;
    public GameObject vRTutorialMenu;
    public GameObject vRRobbiFlowchart;
    public GameObject pCRobbiFlowchart;
    public GameObject fPCControlScheme;
    public GameObject panelObject;
    private Scene _currentScene;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        if (GlobalSettings.DeviceType() == GlobalSettings.Device.WindowsNonVR)
        {
            isWindowsPlayerNoHmd = true;

            yield return new WaitForSeconds(1f);
            eventsystem.GetComponent<XRUIInputModule>().enabled = true;
            eventsystem.GetComponent<XRUIInputModule>().enableMouseInput = false;
            _avatarSettings = (AvatarSettings)FindObjectOfType(typeof(AvatarSettings));
        }
        else
        {
            eventsystem.GetComponent<XRUIInputModule>().enabled = true;

            this.gameObject.SetActive(false); // disables this script and its gameobject if FPC is not active
        }

        // Changes for the Welcome Scene

        if (isWindowsPlayerNoHmd)
        {
            _currentScene = SceneManager.GetActiveScene();

            if (_currentScene.name == GlobalSettings.WelcomeScene) // Welcome Scene
            {
                _avatarSettings.SetUiSettingValuePC();

                vRSettingsButton.SetActive(false);
                vRTutorialMenu.SetActive(false);
                vRRobbiFlowchart.SetActive(false);
                panelObject.SetActive(false);

                pCRobbiFlowchart.SetActive(true);
                fPCControlScheme.SetActive(true);
            }
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (isWindowsPlayerNoHmd)
            {
                if (_currentScene.name == GlobalSettings.WelcomeScene) // Welcome scene or first scene after loading
                {
                    Application.Quit();
                }
            }
        }
    }
}
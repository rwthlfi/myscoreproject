using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    [Header("Misc")]
    public GameObject loadingCircle;
    public int scaleTime;
    public Vector3 scaleOne;
    public Vector3 scaleZero;
    public static bool firstStart = true;
    public bool stillLoading;
    public bool stillLoadingAutomatic;
    public static string sceneToLoadString;
    public static int sceneToLoadInt;
    private AsyncOperation _loadingOperation;
    public GameObject beforeStart;
    public GameObject languageMenu;
    private GameObject _mainCamera;
    public GameObject afterStart;
    //private NetworkManager _networkManager;

    //Script References
    public CustomNetworkManager customNetworkManager;

    //Loading progress variable
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Slider slider;

    public Button continueButton;

    [Header("During Scene Load CanvasLoadingObjects")]
    public List<GameObject> loadingObjList;

    [Header("The hint text here.")]
    public TextMeshProUGUI hintTextComplete;
    public List<TextMeshProUGUI> hintList;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            //if (sceneToLoadString == GlobalSettings.WelcomeScene || sceneToLoadString == "Tutorial" || sceneToLoadString == "WelcomeScene_DjamelPleaseCheck") // this is for testing, when the loading scene was started from another scene instead of 0_loading
            //    firstStart = false;

            //if (firstStart)
            sceneToLoadString = GlobalSettings.WelcomeScene;

            TransitionAnimations();

            yield return new WaitForSeconds(5f); // wait a bit so the player will see the loading screen instead of just black in case of using fast machines

            _loadingOperation = SceneManager.LoadSceneAsync(sceneToLoadString);
            _loadingOperation.allowSceneActivation = false;

            //var xRCameraReference = gameObject.GetComponent(typeof(XRCameraReference)) as XRCameraReference;
            //_mainCamera = xRCameraReference.GetComponent<GameObject>();
        }
    }


    // Update is called once per frame
    void Update() // check if scene is loaded 90% (normal unity behaviour, last 10% will be loaded after scene activation)
    {
        if (_loadingOperation != null && SceneManager.GetActiveScene().name == "0_Loading")
        {
            if (stillLoading && _loadingOperation.progress >= 0.89)
            {
                if (firstStart)
                {
                    stillLoading = false;
                    StartCoroutine(LoadingScreenAnimations());
                }
                else
                {
                    if (stillLoadingAutomatic)
                    {
                        stillLoadingAutomatic = false;
                        StartCoroutine(LoadingScreenAnimations());
                    }
                }
            }
        }
    }

    IEnumerator LoadingScreenAnimations() // check if first start or during gameplay and change behaviour
    {
        if (firstStart)
        {
            firstStart = false;
            yield return new WaitForSeconds(1f);

            StartCoroutine(LerpingExtensions.ScaleTo(loadingCircle.transform, scaleZero, 0.5f));
            StartCoroutine(LerpingExtensions.ScaleTo(afterStart.transform, scaleZero, 0.5f));
            yield return new WaitForSeconds(scaleTime);
            afterStart.SetActive(false);
            languageMenu.SetActive(true);
            languageMenu.GetComponent<LocaMenuButtonActivation>().CheckSelection();
            StartCoroutine(LerpingExtensions.ScaleTo(beforeStart.transform, scaleOne, 1f));
        }
        else
        {
            StartCoroutine(LerpingExtensions.ScaleTo(loadingCircle.transform, scaleZero, 1f));
            yield return new WaitForSeconds(.5f);
            StartCoroutine(LerpingExtensions.ScaleTo(afterStart.transform, scaleZero, 1f));
            yield return new WaitForSeconds(scaleTime);
            hintTextComplete.gameObject.SetActive(true);
            print("Complete Loaded");
            StartCoroutine(LerpingExtensions.ScaleTo(hintTextComplete.transform, scaleOne, 1f));
        }
    }

    public void FinishLoad() // can be accessed via UI buttons e.g. first start, after language selection
    {
        stillLoading = false;
        StartCoroutine(LoadFinishedScene());
    }

    IEnumerator LoadFinishedScene() // screen black transition before activating the loaded scene
    {
        //_mainCamera.GetComponent<CameraFade>().FadeCameraIn(0.2f); // needs to be assigned to the rigs for better screen fade effect

        yield return new WaitForSeconds(0.2f);

        _loadingOperation.allowSceneActivation = true;
    }

    private void TransitionAnimations() // check sceneToLoadIndex and activate images and texts dependent of it
    {
        if (firstStart)
        {
            ShowLoadingObj("LoadingGeneral");
            RandomHint(false);
        }

        else
        {
            ShowLoadingObj(sceneToLoadString);
            RandomHint(true);
            return; // dont execute any more.
        }
    }


    private void RandomHint(bool _enable)
    {
        if (hintList.Count == 0)
            return;

        //shuffle between all the text from 0 till the max hint available in the list
        int randint = Random.Range(0, hintList.Count);

        //disable all the text first
        foreach (TextMeshProUGUI textPro in hintList)
            textPro.gameObject.SetActive(false);

        //show the random hint text needed
        hintList[randint].gameObject.SetActive(_enable);
    }

    private void ShowLoadingObj(string _nameContains)
    {
        //deactivate everything at the beginning
        foreach (GameObject go in loadingObjList)
        {
            go.SetActive(false);
            //Since this time we are checking the GameObject via name,
            //make sure that the gameObj name suits with the globalSettingsName
            if (go.name.Contains(_nameContains))
            {
                go.SetActive(true);
            }
        }
    }

    public void EnableContinueButton()
    {
        UpdateProgressUI(100); // reset progress ui

        //disable random hint
        RandomHint(false);

        //show complete text
        hintTextComplete.gameObject.SetActive(true);

        //Since "XRRigOfflineLoadingScene" gameObject doesnt want to register click
        //dunno why though.
        //therefore here is the automatic click.
        //StartCoroutine(AutoClickAfter(3));
    }

    //private IEnumerator AutoClickAfter(int _sec)
    //{
    //    yield return new WaitForSeconds(_sec);
    //    continueButton.onClick.Invoke();
    //}

    //I am not sure how to get the progress of the Async Loading
    //But i know when the scene is finish loading via my CustomNetworkManager.cs
    //Hence, the fakeProgressLoading.

    int pro = 0;

    public void StartFakeProgress(string _sceneName)
    {
        //show the right loading screen
        ShowLoadingObj(_sceneName);

        //Enable random hint
        RandomHint(true);

        //disable complete text
        hintTextComplete.gameObject.SetActive(false);

        //reset the variable
        customNetworkManager.sceneloaded = false;

        pro = 0;
        UpdateProgressUI(pro);
        StartCoroutine(FakeProgress());
    }

    public IEnumerator FakeProgress()
    {
        while (!customNetworkManager.sceneloaded)
        {
            yield return new WaitForSeconds(0.15f);
            if (pro <= 80)
                pro += Random.Range(0, 9);
            UpdateProgressUI(pro);
        }

        //yield return new WaitForSeconds(1f);
        //customNetworkManager.sceneloaded = false; // reset the scene loading
        pro = 100; // tell the progres to be 100%
        UpdateProgressUI(pro);
        print("Pro: " + pro);
    }

    private void UpdateProgressUI(float progress)
    {
        slider.value = progress;
        //progressText.text = (int)(progress * 100f) + "%";
        progressText.text = (int)(progress) + "%";
    }

}
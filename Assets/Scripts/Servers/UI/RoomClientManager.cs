using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using Insight;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Linq;

//public enum PlayerClientGUIState { Login, Main, Game };
public class RoomClientManager : MonoBehaviour
{
    //[HideInInspector] public PlayerClientGUIState playerGuiState;

    [Header("Insight Module")]
    [Tooltip("Attach the Insight module here")]
    public ClientAuthentication authComp;
    public ClientGameManager gameComp;

    [Header("Menu Container")]
    [Tooltip("The main root menu Window")]
    public GameObject LoginPanel;
    public GameObject MainPanel;

    [Header("Login Menu  ")]
    public TMP_InputField playerNameInputField;
    private bool playerIsLogin = false;

    [Header("Create Match - UI element ")]
    [Tooltip("The ui element to help the user to create a new room")]
    public List<Toggle> roomSceneList = new List<Toggle>();

    //variable to create room
    //public TMP_InputField roomID; // only integer is allowed
    public TMP_InputField roomName;
    //public TMP_InputField roomCreator;
    //public TMP_InputField creatorID;
    public TMP_InputField roomPWD;
    public TMP_InputField roomMaxPlayer; // only integer is allowed
    //public TMP_InputField roomExpiredDate; // only string is allowed
    public TMP_InputField roomExpiredDay;
    public TMP_InputField roomExpiredMonth;
    public TMP_InputField roomExpiredHour;
    public TMP_InputField roomExpiredMinute;
    string expireDate;

    private RoomInfoMessage roomInfoMessage;
    public Button createNewGameButton;

    [Header("Room List UI Panels")]
    public GameObject RoomListContent;
    public GameObject RoomListItemPrefab;
    private int filteredSceneID = -1;
    public List<GameContainer> roomList = new List<GameContainer>();

    [Header("Scene Selection")]
    public string sceneName;


    [Header("Custom / LocalServer Variable")]
    public Transform customServerWindow;
    public NetworkManager netManager;
    public InsightClient insightClient;
    public Toggle customIPToggle;
    public Button resetIP_button;
    public TMP_InputField customIP_Inputfield;
    public TextMeshProUGUI customIP_Info;
    private bool isCustomServerShown = false;


    [Header("Debug Room")]
    public Toggle RaymondsLab_Toggle;

    private void Awake()
    {
        //get all the script
        roomInfoMessage = GetComponent<RoomInfoMessage>();
        sceneName = GlobalSettings.KatschofScene;

        //just for debugging purpose.
        RaymondsLab_Toggle.gameObject.SetActive(false);
    }

    private void Start()
    {
        SelectMenu(LoginPanel);
        //load the saved name
        playerNameInputField.text = PlayerPrefs.GetString(PrefDataList.savedUsername);

        //Show the custom IP Window
        ShowCustomIPWindow();

    }

    private void CreatorIDAssigner()
    {
        if (PlayerPrefs.GetString(PrefDataList.savedCreatorID) == "")
        {
            PlayerPrefs.SetString(PrefDataList.savedCreatorID
                                  , UniqueIDGenerator.GenerateID_StringBuilder());
        }
    }
    private float nextActionTime = 0.5f;
    private float period = 3f;
    private void Update()
    {
        //Debug.Log("login info " + authComp.loginResponse);
        //if the login success then changes the menu to main menu
        if (authComp.loginSucessful 
            && !playerIsLogin
            )
        {
            playerIsLogin = true;
            //open the main panel
            SelectMenu(MainPanel);

            roomInfoMessage.error_ConnectText.SetActive(!authComp.loginSucessful);
        }



        //Below is the function to update things every X seconds
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
            // execute block of code here
            //refresh the match every x seconds.
            if (playerIsLogin)
                OnRefreshButton();
        }
    }


    /// <summary>
    /// Function that allow the user to login and play freely.
    /// </summary>
    public void OnLoginAsGuest_isClicked()
    {
        //if the player name is empty
        //show the warning message and do nothing
        if (playerNameInputField.text == "")
        {
            roomInfoMessage.ShowInfo(roomInfoMessage.warning_emptyUsernameText);
            return;
        }

        //assign the creator id, in case it hasnt have yet.
        CreatorIDAssigner();


        if (playerIsLogin) // this is in case that the player want to change the name
        {
            //directly just open the main panel, without the need to "Login" again.
            //Otherwise it will cause double registry in the Server
            SelectMenu(MainPanel);    
        }
        
        
        
        else
        {
            //Send Message to login
            authComp.SendLoginMsg(playerNameInputField.text, "");

        }

        roomInfoMessage.error_ConnectText.SetActive(!authComp.loginSucessful);

        //saved the player name
        PlayerPrefs.SetString(PrefDataList.savedUsername, playerNameInputField.text);

        //just for debugging
        // if the name is RLC -> that means that the RaymondsLab need to be shown.
        if(playerNameInputField.text == "RLC")
            RaymondsLab_Toggle.gameObject.SetActive(true);
    }

    public void OnGoBackToLogin()
    {
        SelectMenu(LoginPanel);
    }

    /// <summary>
    /// Refresh the Game List 
    /// </summary>
    public void OnRefreshButton()
    {
        gameComp.SendGetGameListMsg();
        //Debug.Log("Refreshed");
        CheckGamesList();
        //clean the game list for later use
        gameComp.gamesList.Clear();
    }


    private RoomListEntry cacheRoomList;
    public RoomListEntry currentRoomListSelected;

    private void CheckGamesList()
    {/*
        //cleaning up things first
        DestroyGameContainer();

        if (gameComp.gamesList.Count > 0)
        {
            //combine two lists(our current list and the actual list from the game server)
            List<GameContainer> uList = roomList.Union(gameComp.gamesList).ToList();

            foreach (GameContainer gc in uList)
            {
                //check if the GameServer has it.
                if (gameComp.gamesList.Contains(gc))
                {
                    //check if it already exist in the big window panel
                    //if yes, dont do anything
                    if (GameContainer_isExist(gc.UniqueId))
                        continue;
                    //otherwise create it.
                    else
                        CreateRoomItem(gc);
                }

                //But if the game server DOESNT have it,
                else
                {
                    //destroy it
                }
            }


        }

        //otherwise, just destroy everything
        else
            foreach (Transform tr in RoomListContent.transform)
                Destroy(tr.gameObject);
        */

        //THESE COMMENTED is PREVIOUSLY WORKING FUNCTION. however its not OPTIMAL.

        currentRoomListSelected = null;
        //clear all the content
        foreach (Transform tr in RoomListContent.transform)
        {
            //if the roomListEntry is currently selected, dont destroy it
            cacheRoomList = tr.GetComponent<RoomListEntry>();
            if (cacheRoomList.isCurrentlySelected)
            {
                //assign the room if it is being selected
                currentRoomListSelected = cacheRoomList;
                continue;
            }

            //Destroy it
            Destroy(tr.gameObject);
        }
            
        //clear the room List
        roomList.Clear();


        if (gameComp.gamesList.Count > 0)
        {
            //check if the gameList has the same name and creator id, then skip it.
            roomList.AddRange(gameComp.gamesList);
            gameComp.gamesList.Clear();
            UpdateGameListUI();
        }
        
    }
    
    public void UpdateGameListUI()
    {
        foreach (GameContainer game in roomList)
        {
            //if there is already a room selected, compare the selected unique ID and creator ID
            //dont do anything and just continue to the other list.
            if (currentRoomListSelected != null)
            {
                if (currentRoomListSelected.UniqueID == game.UniqueId
                    && currentRoomListSelected.CreatorID == game.CreatorID)
                {
                    print("The same");
                    continue;
                }
            }


            //get the button prefab
            GameObject instance = Instantiate(RoomListItemPrefab);
            instance.transform.SetParent(RoomListContent.transform, false);

            RoomListEntry comp = instance.GetComponent<RoomListEntry>();

            comp.clientComp = this;
            comp.UniqueID = game.UniqueId;
            comp.CurrentPlayers = game.CurrentPlayers;
            comp.RoomMaxPlayers = game.RoomMaxPlayers;
            comp.SceneName = game.SceneName;
            comp.RoomName = game.RoomName;
            comp.RoomCreator = game.RoomCreator;
            comp.CreatorID = game.CreatorID;
            comp.RoomPassword = game.RoomPassword;
            comp.RoomExpireDate = game.RoomExpireDate;

            //rename the button to the unique id
            instance.name = game.UniqueId;

        }
    }
    


    private bool GameContainer_isExist(string _uniqueId)
    {
        foreach (Transform tr in RoomListContent.transform)
        {
            if (tr.name == _uniqueId)
                return true;
        }

        return false;
    }

    private void DestroyGameContainer()
    {
        foreach (Transform tr in RoomListContent.transform)
        {
            if (isUniqueIdExist(tr.name))
                continue;
            else
                Destroy(tr.gameObject);
        }
    }

    private bool isUniqueIdExist(string _uID)
    {
        foreach(GameContainer gc in gameComp.gamesList)
        {
            if (_uID == gc.UniqueId)
                return true;
        }
        return false;
    }


    private void CreateRoomItem(GameContainer _game)
    {
        //get the button prefab
        GameObject instance = Instantiate(RoomListItemPrefab);
        instance.transform.SetParent(RoomListContent.transform, false);

        RoomListEntry comp = instance.GetComponent<RoomListEntry>();

        comp.clientComp = this;
        comp.UniqueID = _game.UniqueId;
        comp.CurrentPlayers = _game.CurrentPlayers;
        comp.RoomMaxPlayers = _game.RoomMaxPlayers;
        comp.SceneName = _game.SceneName;
        comp.RoomName = _game.RoomName;
        comp.RoomCreator = _game.RoomCreator;
        comp.CreatorID = _game.CreatorID;
        comp.RoomPassword = _game.RoomPassword;
        comp.RoomExpireDate = _game.RoomExpireDate;

        //rename the button to the unique id
        instance.name = _game.UniqueId;
    }



    /// <summary>
    /// To join a game
    /// </summary>
    /// <param name="UniqueID"></param>
    public void HandleJoinGameButton(string UniqueID)
    {
        gameComp.SendJoinGameMsg(UniqueID);
    }


    /// <summary>
    /// To create a room button
    /// </summary>
    public void OnHandleCreateGameButton()
    {
        float tempTimer = 8f;

        //To Future Raymond:
        //there is a bug that occurs when the user doesnt input:
        // the room's name and room password
        //check if the room is valid, if not, dont do anything
        if (!TheRoomCreation_isValid())
            return;

        //Get the requested Scene name
        //sceneName = GlobalSettings.requestedSceneName(0);

        //disable  the interaction to prevent multiple room being created
        StartCoroutine(CoroutineExtensions.InteractableButtonAfterSeconds(createNewGameButton, tempTimer));

        gameComp.SendRequestSpawnStart(new RequestSpawnStartMsg()
        {
            SceneName = sceneName,
            RoomName = roomName.text,
            RoomCreator = PlayerPrefs.GetString(PrefDataList.savedUsername), // get from the saved name
            CreatorID = PlayerPrefs.GetString(PrefDataList.savedCreatorID), // get from the global uniqueID
            RoomPassword = roomPWD.text,
            RoomMaxPlayers = Convert.ToInt32(roomMaxPlayer.text),
            RoomExpireDate = expireDate
        });
        roomInfoMessage.ShowInfo(roomInfoMessage.info_roomCreationIsSent);

        //show message created
        StartCoroutine(ShowMessage_SuccessCreatingRoom(tempTimer));
    }

    //
    private bool TheRoomCreation_isValid()
    {
        if (roomName.text == "")
            roomName.text = "noName";

        if (roomPWD.text == "")
            roomPWD.text = "-";

        //put a converter here.
        if (roomMaxPlayer.text == "")
            roomMaxPlayer.text = "20";

        //the date is not valid dont do anything
        if (!expireDate_isValid())
            return false;

        return true;
    }


    private bool expireDate_isValid()
    {
        //check if the date is today or not
        DateTime currentDate = DateTime.Now;
        //Debug.Log("date " + currentDate);

        //check if the date is not being inputed, then set the current date as default
        if (roomExpiredDay.text == "") roomExpiredDay.text = DateTime.Now.Day.ToString();
        if (roomExpiredMonth.text == "") roomExpiredMonth.text = DateTime.Now.Month.ToString();
        if (roomExpiredHour.text == "") roomExpiredHour.text = DateTime.Now.Hour.ToString();
        if (roomExpiredMinute.text == "") roomExpiredMinute.text = DateTime.Now.Minute.ToString();


        //convert the entered date string to datetime.
        DateTime? inputDate = null;
        try //try getting the time
        {
            inputDate =
            new DateTime(DateTime.Now.Year, Convert.ToInt32(roomExpiredMonth.text), Convert.ToInt32(roomExpiredDay.text),
                         Convert.ToInt32(roomExpiredHour.text), Convert.ToInt32(roomExpiredMinute.text), DateTime.Now.Second);
        }
        catch //print an error message
        {
            roomInfoMessage.ShowInfo(roomInfoMessage.error_ExpireDateIsIncorrect);
        }

        //if the input date doesnt have any value.. dont do anything
        if (!inputDate.HasValue)
            return false;


        //compare between the two date.
        DateTime precautionDate = inputDate.Value.AddSeconds(60);
        int comp = DateTime.Compare(currentDate, precautionDate); // add 60 seconds just for precautions
        //Debug.Log("date is " + precautionDate);
        if (comp == 1) // the input date is in the past. throws error message
        {
            roomInfoMessage.ShowInfo(roomInfoMessage.error_ExpireDateIsinThePast);
            return false;
        }

        //Combine the date Result, 
        expireDate = precautionDate.Day + "/" + precautionDate.Month + "/"
                     + precautionDate.Hour + "/" + precautionDate.Minute;


        //if everything is valid, then return true
        return true;
    }


    /// <summary>
    /// Menu selection
    /// </summary>
    public void SelectMenu(GameObject _panel)
    {
        LoginPanel.SetActive(false);
        MainPanel.SetActive(false);

        _panel.SetActive(true);
    }


    public void ScenarioToggleSelect(Toggle _toggle)
    {
        //Toggle clickedToggle = EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>();
        foreach (Toggle tg in roomSceneList)
            tg.Set(false, false);
        //Debug.Log("asdf " + _toggle.name);
        _toggle.Set(true, false);
        ApplySceneName();
    }


    //attach this to a Scenario selection button
    private void ApplySceneName()
    {
        //find the Activated toggle ID position
        int toggleID;
        for (int i = 0; i < roomSceneList.Count; i++)
        {
            if (roomSceneList[i].isOn)
            {
                toggleID = i;
                sceneName = GlobalSettings.requestedSceneName(toggleID);
                break;
            }
        }

    }

    public void FilterMatchButton(int _sceneID)
    {
        //Update and save the user's filter Id for the match
        filteredSceneID = _sceneID;

        //read the toggle value and convert it to scene name
        string searchedSceneNamed = GlobalSettings.requestedSceneName(_sceneID);


        //Otherwise search them!

        //get all the match button scroll view transform and get all the text.
        RoomListEntry[] buttons = RoomListContent.GetComponentsInChildren<RoomListEntry>(true);

        //if the player want to show all scene..the activated all the button and just return.
        if (searchedSceneNamed == GlobalSettings.AllScene)
        {
            foreach (RoomListEntry RLE_button in buttons)
                RLE_button.gameObject.SetActive(true);
            return;
        }



        //get the buttons in the ScrollView Content
        foreach (RoomListEntry RLE_button in buttons)
        {
            //set everything to active first
            RLE_button.gameObject.SetActive(true);

            //if the label scene match is NOT the same with the requested scene name then disabled them
            if (RLE_button.SceneNameText.text != searchedSceneNamed)
            {
                RLE_button.gameObject.SetActive(false);
            }

        }
        //Debug.Log("Filtered scene " + filteredSceneID);
    }


    private IEnumerator ShowMessage_SuccessCreatingRoom(float _sec)
    {
        yield return new WaitForSeconds(_sec);
        roomInfoMessage.ShowInfo(roomInfoMessage.info_roomIsCreated);
    }

    
    /// <summary>
    /// Depending on the Saved ip address., we will either show or hide the custom window.
    /// </summary>
    private void ShowCustomIPWindow()
    {
        //load the saved ip address
        string str = PlayerPrefs.GetString(PrefDataList.savedCustomIPAddress);
        //if it correspond to the ip address that we have, then Hide the custom window
        if (str == "")
        {
            str = "134.130.88.14";
            PlayerPrefs.SetString(PrefDataList.savedCustomIPAddress, GlobalSettings.serverIP);
        }
            

        if(str == "" || str == "134.130.88.14")
        {
            str = "134.130.88.14";
            customServerWindow.gameObject.SetActive(false);
            customIP_Info.gameObject.SetActive(false);
            isCustomServerShown = false;
            resetIP_button.interactable = false;
        }

        //if the ip is different than our server,
        //then show the window. 
        //so they know that they are using the offline server.
        //also change the ip too.
        else
        {
            customServerWindow.gameObject.SetActive(true);
            customIP_Info.gameObject.SetActive(true);
            isCustomServerShown = true;
            resetIP_button.interactable = true;
        }

        //set the ip in the customip input field
        customIP_Inputfield.text = str;

        //invoke the ip changing
        Ui_SetIPTarget();

        //set which toggle is active
        customIPToggle.isOn = isCustomServerShown;
    }


    public void Ui_OnCustomIPToggleClicked()
    {
        customServerWindow.gameObject.SetActive(customIPToggle.isOn);
    }


    public void Ui_SetIPTarget()
    {
        //get the ip address from the input field
        netManager.networkAddress = customIP_Inputfield.text;
        insightClient.networkAddress = customIP_Inputfield.text;
        //save the ip Data
        PlayerPrefs.SetString(PrefDataList.savedCustomIPAddress, customIP_Inputfield.text);
        //show the message
        if(customIP_Inputfield.text != GlobalSettings.serverIP)
        {
            customIP_Info.gameObject.SetActive(true);
            resetIP_button.interactable = true;
        }
        else
        {
            customIP_Info.gameObject.SetActive(false);
            resetIP_button.interactable = false;
        }
            
    }

    public void Ui_CustomIP_isModified()
    {
        Ui_SetIPTarget();
    }

    public void Ui_ResetIPTarget()
    {
        //set the input field to current ip address
        customIP_Inputfield.text = GlobalSettings.serverIP;
        netManager.networkAddress = GlobalSettings.serverIP;
        insightClient.networkAddress = GlobalSettings.serverIP;

        //set the button not interactable
        resetIP_button.interactable = false;
        //show the message, that it successfully changed
        customIP_Info.gameObject.SetActive(false);
    }

    public void Ui_ReloadScene()
    {
        SceneManager.LoadScene(GlobalSettings.WelcomeScene);
    }
}

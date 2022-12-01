using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class RoomListEntry : MonoBehaviour
{
    public RoomClientManager clientComp;

    [Header("UI References")]
    public TextMeshProUGUI SceneNameText;
    public TextMeshProUGUI  RoomCreatorText;
    public TextMeshProUGUI CreatorIDText;
    public TextMeshProUGUI PlayerCountText;
    public TextMeshProUGUI RoomNameText;
    public TextMeshProUGUI RoomPasswordText;
    //public TextMeshProUGUI RoomExpireDateText;
    public TMP_InputField enterPassInputField;
    public Button joinButton;


    [Header("Button variable")]
    public string UniqueID;
    public string SceneName;
    public int CurrentPlayers;
    public int RoomMaxPlayers;
    public string RoomName;
    public string RoomCreator;
    public string CreatorID;
    public string RoomPassword;
    public string RoomExpireDate;

    private bool Init;

    [Header("Cache Variable")]
    public KeyboardListener keyboardListener;
    public bool isCurrentlySelected  = false;

    [Header("timer variable")]
    public bool timerIsRunning = false;
    public float timeToDeselect = 10f;
    public float timeRemaining = 0f;


    private void Awake()
    {
        //set the currentSeconds to the defined seconds.
        timeRemaining = timeToDeselect;
    }

    private void LateUpdate()
    {
        if (!Init)
        {
            Init = true;

            SceneNameText.text = SceneName;
            PlayerCountText.text = CurrentPlayers + "/" + RoomMaxPlayers;
            
            RoomNameText.text = RoomName;
            RoomCreatorText.text = RoomCreator;

            CreatorIDText.text = CreatorID;
            RoomPasswordText.text = RoomPassword;
            //RoomExpireDateText.text = RoomExpireDate;

            joinButton.interactable = false;
        }
        HideFullRoom();
        HideJoinButton();

        //monitor the deselection
        DeselectCountdown();
    }

    public void HandleSelectButton()
    {
        //register the creatorID of the one who created the room
        PlayerPrefs.SetString(PrefDataList.currentRoomCreatorID, CreatorID);

        //enable the loading screen
        //only works when there is a "loading screen manager" with "customNetworkManager"
        LoadingScreenManager lsm = FindObjectOfType<LoadingScreenManager>(true);
        if (lsm)
        {
            lsm.gameObject.SetActive(true);
            lsm.StartFakeProgress(SceneName);
        }



        clientComp.HandleJoinGameButton(UniqueID);
    }





    private void HideFullRoom()
    {
        if (CurrentPlayers >= RoomMaxPlayers)
            this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Check the password field if it match then enable the join button
    /// </summary>
    private void HideJoinButton()
    {
        //this mean that it doesnt have any password
        if (RoomPasswordText.text == "-")
        {
            enterPassInputField.gameObject.SetActive(false);
            joinButton.interactable = true;
            return;
        }

        //check the enterred password.
        if (enterPassInputField.text == RoomPasswordText.text)
            joinButton.interactable = true;
        else
            joinButton.interactable = false;


    }



    //assign the keyboard Listener to the inputfield
    public void Ui_AssignKeyboardListener()
    {
        //get the keyboardListener
        keyboardListener = (KeyboardListener)FindObjectOfType(typeof(KeyboardListener));

        //if keyboard listener exist -> start typing
        if (keyboardListener)
        {
            //assign the inputField where the key stroke will be recorded.
            keyboardListener.keyboard.displayText = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();
        }

        //set it to true -> so it wont get deleted
        isCurrentlySelected = true;
    }



    //attach to the on deselect.
    public void Ui_OnDeselect()
    {
        timerIsRunning = true;
    }


    private void DeselectCountdown()
    {
        //only count if the timer is runnings
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                
                //set it is no more selected and will be subjected to be destroyed
                isCurrentlySelected = false;

            }
        }
    }


    //when it is deselected
    //start monitoring the progress
    //if inputField is not changes within 7 sec.
    //then start setting selected as false
    //otherwise
    //keep resetting the countdown.
    public void Ui_OnValueChanged()
    {
        timeRemaining = timeToDeselect;
    }

}

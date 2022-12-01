using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Insight.Examples
{
    public enum PlayerClientGUIState { Login, Main, Game };

    public class PlayerClientGUI : MonoBehaviour
    {
        [Header("Root UI Panels")]
        public GameObject RootLoginPanel;
        public GameObject RootMainPanel;
        public GameObject RootGamePanel;

        PlayerClientGUIState playerGuiState;

        [Header("Insight Modules")]
        public ClientAuthentication authComp;
        public ChatClient chatComp;
        public ClientGameManager gameComp;
        public ClientMatchMaking matchComp;

        [Header("Create Room Module")]
        public InputField roomID;
        public InputField roomName;
        public InputField roomCreator;
        public InputField creatorID;
        public InputField roomPWD;
        public InputField roomMaxPlayer;
        public InputField roomExpireDate;

        [Header("UI Buttons")]
        public GameObject StartMatchMakingButton;
        public GameObject StopMatchMakingButton;
        public GameObject GetGameListButton;
        public GameObject CreateGameButton;

        [Header("Game List UI Panels")]
        public GameObject GameListArea;
        public GameObject GameListPanel;

        public GameObject GameListItemPrefab;

        public Text chatTextField;
        public InputField chatInputField;

        public List<GameContainer> gamesList = new List<GameContainer>();

        [Header("Playlist/Game Name")]
        public string GameName = "SuperAwesomeGame";

        private void Start()
        {
            SwitchToLogin();
        }

        void Update()
        {
            switch (playerGuiState)
            {
                case PlayerClientGUIState.Login:
                    SwitchToLogin();

                    if (authComp.loginSucessful)
                    {
                        playerGuiState = PlayerClientGUIState.Main;
                        return;
                    }
                    break;
                case PlayerClientGUIState.Main:
                    SwitchToMain();
                    CheckGamesList();
                    break;
                case PlayerClientGUIState.Game:
                    SwitchToGame();
                    break;
            }

            if (NetworkManager.singleton.isNetworkActive)
            {
                playerGuiState = PlayerClientGUIState.Game;
            }
            else if (authComp.loginSucessful)
            {
                playerGuiState = PlayerClientGUIState.Main;
            }
        }

        public void FixedUpdate()
        {
            //This is gross. Needs a better design that does not introduce coupling.
            chatTextField.text = chatComp.chatLog;
        }

        private void SwitchToLogin()
        {
            RootLoginPanel.SetActive(true);
            RootMainPanel.SetActive(false);
            RootGamePanel.SetActive(false);
        }

        private void SwitchToMain()
        {
            RootLoginPanel.SetActive(false);
            RootMainPanel.SetActive(true);
            RootGamePanel.SetActive(false);
        }

        private void SwitchToGamesList()
        {
            RootLoginPanel.SetActive(false);
            RootMainPanel.SetActive(false);
            RootGamePanel.SetActive(false);
        }

        private void SwitchToGame()
        {
            RootLoginPanel.SetActive(false);
            RootMainPanel.SetActive(false);
            RootGamePanel.SetActive(true);
        }

        public void HandleStartMatchMakingButton()
        {
            StartMatchMakingButton.SetActive(false);
            StopMatchMakingButton.SetActive(true);

            matchComp.SendStartMatchMaking(new StartMatchMakingMsg() { SceneName = GameName });
        }

        public void HandleStopMatchMakingButton()
        {
            StartMatchMakingButton.SetActive(true);
            StopMatchMakingButton.SetActive(false);

            matchComp.SendStopMatchMaking();
        }

        public void HandleGetGameListButton()
        {
            gameComp.SendGetGameListMsg();

            GetGameListButton.SetActive(false);
            StartMatchMakingButton.SetActive(false);
            StopMatchMakingButton.SetActive(false);
            CreateGameButton.SetActive(false);

            GameListArea.SetActive(true);
        }

        public void HandleJoinGameButton(string UniqueID)
        {
            HandleCancelButton();

            gameComp.SendJoinGameMsg(UniqueID);

            playerGuiState = PlayerClientGUIState.Game;
        }

        public void HandleCancelButton()
        {
            foreach (Transform child in GameListPanel.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            GameListArea.SetActive(false);
            GetGameListButton.SetActive(true);
            StartMatchMakingButton.SetActive(true);
            StopMatchMakingButton.SetActive(false);
            CreateGameButton.SetActive(true);
        }

        public void HandleCreateGameButton()
        {
            switch (roomID.text)
            {
                case "1": GameName = "SuperAwesomeGame"; break;
                case "2": GameName = "SuperMarioBross"; break;
            }

            gameComp.SendRequestSpawnStart(new RequestSpawnStartMsg() 
                                            { 
                                                SceneName = GameName,
                                                RoomName = roomName.text,
                                                RoomCreator = roomCreator.text,
                                                CreatorID = creatorID.text,
                                                RoomPassword = roomPWD.text,
                                                RoomMaxPlayers = Convert.ToInt32(roomMaxPlayer.text),
                                                RoomExpireDate = roomExpireDate.text
                                            });
        }

        public void HandleSendChatButton()
        {
            chatComp.SendChatMsg(chatInputField.text);
            chatInputField.text = "";
        }

        public void HandleExitButton()
        {
            NetworkManager.singleton.StopClient();
        }

        private void CheckGamesList()
        {
            gamesList.Clear();

            if (gameComp.gamesList.Count > 0)
            {
                gamesList.AddRange(gameComp.gamesList);
                gameComp.gamesList.Clear();
                UpdateGameListUI();
            }
        }

        public void UpdateGameListUI()
        {
            foreach (GameContainer game in gamesList)
            {
                GameObject instance = Instantiate(GameListItemPrefab);
                instance.transform.parent = GameListPanel.transform;
                GUIGamesListEntry comp = instance.GetComponent<GUIGamesListEntry>();
                comp.clientComp = this;
                comp.UniqueID = game.UniqueId;
                comp.CurrentPlayers = game.CurrentPlayers;
                comp.RoomMaxPlayers = game.RoomMaxPlayers;
                comp.SceneName = game.SceneName;
                comp.RoomName = game.RoomName;
                comp.RoomCreatorText = game.RoomCreator;
                comp.CreatorID = game.CreatorID;
                comp.RoomPassword = game.RoomPassword;
                comp.RoomExpireDate = game.RoomExpireDate;
            }
        }
    }
}

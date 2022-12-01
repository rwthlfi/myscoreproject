using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using Mirror;
using System.Collections.Generic;
using RoomService;
using AvatarCreation;

namespace AvatarTablet
{
    public class ShareScreenWindow : NetworkBehaviour
    {
        [Header("Big Screen")]
        public BigVideoScreenReference[] bigScreensList;
        public List<Renderer> videoRendererList = new List<Renderer>();
        
        [Header("ShareScreen Related")]
        public Camera encodedCamera;
        public MakeTransformTheSame makeTheSame;
        public Toggle shareScreen_toggle;

        [Header("Script Reference")]
        //public GameViewEncoder gameViewEncoder;
        //public GameViewDecoder gameViewDecodeer;
        public TheRoomServices theRoomServices;
        public NetworkPlayerSyncVar networkPlayerSyncVar;
        public MainControlTablet mainControlTablet;


        [Header("Info message")]
        public TextMeshProUGUI info_nameCurrentlySharing;
        public GameObject info_someoneIsSharingScreen;
        public GameObject info_YouAreForcedTurnOffShare;

        private void Start()
        {
            //disable the camera & Audio first
            encodedCamera.enabled = false;

            //store all the big screen reference and its Renderer for later use.
            bigScreensList = FindObjectsOfType<BigVideoScreenReference>();
            foreach (BigVideoScreenReference bvc in bigScreensList)
                videoRendererList.Add(bvc.GetComponentInChildren<Renderer>());
            
            //disable the info message
            info_someoneIsSharingScreen.SetActive(false);

            theRoomServices = FindObjectOfType<TheRoomServices>();
        }



        private void Update()
        {
        }


        //------- VIDEO STREAMING -------//


        //The function to activated the screenshare.
        public void EnableScreenStreaming(bool _activated)
        {
            //gameViewEncoder.gameObject.SetActive(_activated);
        }


        /// <summary>
        /// To activate the share screen
        /// attached this to the toggle OnValueChanged
        /// </summary>
        /// <param name="_toggle"></param>
        public void Ui_ShareScreenButton_isClicked(Transform _window)
        {
            if (shareScreen_toggle.isOn)
            {
                //Move the camera to the _window's Position
                makeTheSame.enabled = true;
                makeTheSame.gameObjectToFollow = _window;

                //Send command from client to serve
                Cmd_streamingAllowed(netIdentity);
            }
            else if (!shareScreen_toggle.isOn)
            {
                //if you just wanna turn off then turn it off directly.
                makeTheSame.enabled = false;
                TurnOffShareScreen();
            }
        }


        //Tell the server that you wanna share and 
        //check the sharing function if it is available in the server
        [Command]
        void Cmd_streamingAllowed(NetworkIdentity _netID)
        {
            //Debug.Log("Check Streaming");
            //get the RoomService
            theRoomServices = FindObjectOfType<TheRoomServices>();
            

            //Debug.Log("allowed " + trs.Stream_isAllowed());
            if (theRoomServices.getStreamerName() == "") // which means no one is streaming
            {
                // change the share screen toggle to On
                Rpc_TurnOnShareScreen(_netID, "", true);
            }
            else // if it is not allowed turn it off
            {
                //and send to the npc that someone is streaming and turn off his/her sharing toggle
                Rpc_TurnOnShareScreen(_netID, theRoomServices.getStreamerName(), false);
            }

        }


        //tell the client that you are allowed to share the screen
        [ClientRpc]
        void Rpc_TurnOnShareScreen(NetworkIdentity _netId, string _streamer, bool _value)
        {
            //check the netid
            if (_netId.netId == netId && _netId.isLocalPlayer)
            {
                //set that you are streaming.
                shareScreen_toggle.Set(_value, false);
                //turn on/off the Share Screen
                EnableScreenStreaming(_value);

                //if on, set the status to streaming
                if (_value)
                {
                    networkPlayerSyncVar.CmdChangeAttribute(networkPlayerSyncVar.attributeChanged 
                                                            + GlobalSettings.att_Streamer);
                    
                }
                else // if it is not on then set the status to not streaming.
                {
                    //display the message that someone is streaming
                    info_nameCurrentlySharing.text = _streamer;
                    StartCoroutine(CoroutineExtensions.HideAfterSeconds(info_someoneIsSharingScreen, 5f));
                    
                    //remove the Streamer Attribute // -> just for safety // -> Should not be needed.
                    string currenAtt = networkPlayerSyncVar.attributeChanged;
                    string convert = currenAtt.Replace(GlobalSettings.att_Streamer, "");
                    networkPlayerSyncVar.CmdChangeAttribute(convert);
                }

            }

        }



        /// <summary>
        /// to turn off the share screen function
        /// </summary>
        void TurnOffShareScreen()
        {
            shareScreen_toggle.Set(false, false);
            //turn on/off the Share Screen
            EnableScreenStreaming(false);

            //remove the Streamer Attribute
            string currenAtt = networkPlayerSyncVar.attributeChanged;
            string convert = currenAtt.Replace(GlobalSettings.att_Streamer, "");
            networkPlayerSyncVar.CmdChangeAttribute(convert);
            
            //reset the screen to white
            ScreenGoesWhite();
        }



        //attach this component to the "FMNetwork->OnDataByteReadyEvent( Byte[] )"
        public void SendStreamingToOther(byte[] _byteData)
        {
            theRoomServices.SendStreamImage(_byteData);
            print("aaa");
            //Cmd_streamImage(_byteData);
        }

        /*
        [Command(requiresAuthority = false)]
        void Cmd_streamImage(byte[] _byteData)
        {
            Rpc_StreamImage(_byteData);
        }

        //send byte data to the other
        [ClientRpc]
        void Rpc_StreamImage(byte[] _byteData)
        {
            print("something");
            StreamImage(_byteData);
        }
        */

        //the logic that needs to be executed
        public void StreamImage(byte[] _a)
        {
            //Decode the received byte array
            //Debug.Log(this.gameObject.GetComponent<NetworkPlayerSyncVar>().playerName + " byte data:  " + _a.Length);

            //gameViewDecodeer.Action_ProcessImageData(_a);

            //Decode the received byte array
            //and show them on the big screen
            foreach (Renderer rendr in videoRendererList)
            {
                //print("received " + gameViewDecodeer.)
                //rendr.material.mainTexture = gameViewDecodeer.ReceivedTexture;
                //rendr.material.mainTexture = Texture2D.blackTexture;
            }

        }


        /// <summary>
        /// set the screen to default
        /// </summary>
        private void ScreenGoesWhite()
        {
            
            foreach (Renderer rendr in videoRendererList)
                rendr.material.mainTexture = Texture2D.whiteTexture;
            
        }






        // - - - Force To Turn off Sharing Screen - - - //
        //Forced the people to turn the screen off.
        public void UI_ForcedTurnOffSharing_isClicked()
        {
            Cmd_ForcedTurnOffButton_isClicked();
        }

        [Command]
        void Cmd_ForcedTurnOffButton_isClicked()
        {
            Rpc_ForcedTurnOff();
        }


        [ClientRpc]
        void Rpc_ForcedTurnOff()
        {
            //check the share screen Toggle if it is on.
            //if it is on, by the time this message comes in.
            //show message that your screen is turn off by the admin
            if (shareScreen_toggle.isOn)
                mainControlTablet.ShowInfo_ShareScreenTurnOffByAdmin();

            TurnOffShareScreen();
        }


    }
}

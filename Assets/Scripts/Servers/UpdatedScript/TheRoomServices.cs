using System.Collections;
using System.Linq;
using UnityEngine;
using Mirror;
using AvatarCreation;
using System;

namespace RoomService
{
    public class TheRoomServices : NetworkBehaviour
    {
        [Header("The Local Player Variable")]
        public NetworkPlayerSyncVar theLocalPlayer;

        [Header("The list of People in this Room")]
        public NetworkPlayerSyncVar[] NetworkPlayerList;
        public string savedCreatorID = "";
        public string roomCreatorID = "";

        [Header("The List of Spawned Object in the Room")]
        public NetObjReference[] netObjList;



        [Header("SyncVar - Properties")]
        [SyncVar(hook = nameof(OnStreamerChanged))]
        public string streamerName;


        //Server Expire date/ timeRemaining Variable
        private GameRoomSetting grs;
        private DateTime inputDate;
        private int expireDay, expireMonth, expireHour, expireMinute = 0;


        //Change Streamer Name
        void OnStreamerChanged(string _Old, string _New)
        {
            streamerName = _New;
        }

        [Command(requiresAuthority =false)]
        public void CmdChangeAttribute(string _newStreamer)
        {
            streamerName = _newStreamer; 
        }



        private void Start()
        {
            //check of it is server, by asking if there is a "GameRoomSetting"(Cause only the Game Server has this component)
            grs = FindObjectOfType<GameRoomSetting>();
            if (grs) //SERVER AUTHORATIVE
            {
                //print("I am Server");
                //get the room expire date
                string expireStr = grs.RoomExpireDate;
                //Split the expireStr (See the RoomClientManager
                expireDay = Convert.ToInt32(expireStr.Split('/').ElementAt(0));
                expireMonth = Convert.ToInt32(expireStr.Split('/').ElementAt(1));
                expireHour = Convert.ToInt32(expireStr.Split('/').ElementAt(2));
                expireMinute = Convert.ToInt32(expireStr.Split('/').ElementAt(3));
                //convert the string to DateTime
                inputDate = new DateTime(DateTime.Now.Year, expireMonth, expireDay,
                                             expireHour, expireMinute, DateTime.Now.Second);

            }


            //store the room Creator ID
            roomCreatorID = PlayerPrefs.GetString(PrefDataList.currentRoomCreatorID);
            savedCreatorID = PlayerPrefs.GetString(PrefDataList.savedCreatorID);

        }


        //Update Time periodically
        private float nextActionTime = 0f;
        private float period = 7f;
        // Update is called once per frame
        void Update()
        {
            if (Time.time > nextActionTime)
            {
                nextActionTime = Time.time + period;
                // execute block of code here

                //Register all the player in the scenario
                NetworkPlayerList = FindObjectsOfType<NetworkPlayerSyncVar>();
                //assign LocalPlayer
                if(!theLocalPlayer)
                    theLocalPlayer = getLocalPlayer();


                //Register all the items in the scenario.
                netObjList = FindObjectsOfType<NetObjReference>();

                AssignAdministrator();
            }

            //check the timeout /-> only in server
            if (grs)
                TimeoutScene();
        }

        
        /// <summary>
        /// To get the local Player network identity
        /// </summary>
        /// <returns></returns>
        public NetworkPlayerSyncVar getLocalPlayer()
        {
            foreach(NetworkPlayerSyncVar np in NetworkPlayerList)
            {
                if (np.isLocalPlayer)
                    return np.GetComponent<NetworkPlayerSyncVar>();
            }

            return null;
        }


        /// <summary>
        /// get the player name from the net sync var
        /// </summary>
        /// <param name="_netsyncVar"></param>
        /// <returns></returns>
        public string getPlayerName(string _playerID)
        {
            foreach (NetworkPlayerSyncVar np in NetworkPlayerList)
            {
                if (np.playerUniqueID == _playerID)
                    return np.playerName;
            }
            return "";
        }


        /// <summary>
        /// get the player Network Identity from the netSyncVar
        /// </summary>
        /// <param name="_playerID"></param>
        /// <returns></returns>
        public NetworkIdentity getPlayerNetIdentity(string _playerID)
        {
            foreach (NetworkPlayerSyncVar np in NetworkPlayerList)
            {
                if (np.playerUniqueID == _playerID)
                    return np.GetComponent<NetworkIdentity>();
            }
            return null;
        }


        /// <summary>
        /// to Get the name of the streamer
        /// </summary>
        /// <returns></returns>
        public string getStreamerName()
        {
            //get the player syncvarhook
            foreach (NetworkPlayerSyncVar np in NetworkPlayerList)
            {
                //check if there is any player that has a streamer Icon.
                if (np.attributeChanged.Contains(GlobalSettings.att_Streamer))
                {
                    //Dont allow the requested player to share the screen
                    //and send the name of the player that streams
                    return np.playerName;
                }
            }
            //sent no one is streaming.
            return "";
        }


        /// <summary>
        /// To Assign the administrator by bypassing the server.
        /// </summary>
        private void AssignAdministrator()
        {
            if (!theLocalPlayer)
                return;
            //if thesavedCreatorID  matches with the roomCreator
            //get the localplayer and activate the Admin button
            if (savedCreatorID == roomCreatorID 
                || theLocalPlayer.adminStatus == (int)GlobalSettings.PlayerRole.admin)
            {
                //assign admin. and changes the 
                theLocalPlayer.mainControlTablet.EnableAdmin(true);
                //set that you are the admin
                theLocalPlayer.CmdAdminStatus((int)GlobalSettings.PlayerRole.admin);
            }
        }




        /// <summary>
        /// to make Admin.
        /// </summary>
        /// <param name="_networkID"></param>
        /// <param name="_makeAdmin"></param>
        public void MakeAdmin(NetworkIdentity _networkID, bool _makeAdmin)
        {
            Cmd_MakeAdmin(_networkID, _makeAdmin);
        }



        [Command(requiresAuthority = false)]
        void Cmd_MakeAdmin(NetworkIdentity _networkID, bool _makeAdmin)
        {
            Rpc_MakeAdmin(_networkID, _makeAdmin);
        }


        //change the value on the client Side
        [ClientRpc]
        public void Rpc_MakeAdmin(NetworkIdentity _networkID, bool _makeAdmin)
        {
            //get the local player; if it exists. usually due to network sync, the variable might be a bit slow to be updated
            if (theLocalPlayer
                && _networkID == theLocalPlayer.GetComponent<NetworkIdentity>())
            {
                //set the status
                if (_makeAdmin)
                {
                    theLocalPlayer.GetComponent<NetworkPlayerSyncVar>().CmdAdminStatus((int)GlobalSettings.PlayerRole.admin);
                    theLocalPlayer.mainControlTablet.EnableAdmin(true);
                }
                    
                else if (!_makeAdmin)
                {
                    theLocalPlayer.GetComponent<NetworkPlayerSyncVar>().CmdAdminStatus((int)GlobalSettings.PlayerRole.normal);
                    theLocalPlayer.mainControlTablet.EnableAdmin(false);
                }
            }
            
        }


        /// <summary>
        /// to send the stream image
        /// </summary>
        /// <param name="_byteData"> the byte of the texture needed to be sent</param>
        public void SendStreamImage(byte[] _byteData)
        {
            //print("bbb");
            Cmd_streamImage(_byteData);
        }

        [Command(requiresAuthority = false)]
        void Cmd_streamImage(byte[] _byteData)
        {
            Rpc_StreamImage(_byteData);
        }


        //change the value on the client Side
        [ClientRpc]
        void Rpc_StreamImage(byte[] _byteData)
        {
            //print("ccc");
            theLocalPlayer.mainControlTablet.shareScreenWindow.StreamImage(_byteData);
        }




        /// <summary>
        /// for Changing the scenario
        /// </summary>
        /// <param name="_sceneID"></param>
        public IEnumerator ChangeScene(int _sceneID)
        {
            //show info for 3 seconds
            yield return StartCoroutine(CoroutineExtensions.HideAfterSeconds(theLocalPlayer.mainControlTablet.info_teleportedScene, 3f));
            //start changing the scene
            Cmd_ChangeScene(_sceneID);
        }


        [Command(requiresAuthority = false)]
        private void Cmd_ChangeScene(int _sceneID)
        {
            string sceneName = "";
            sceneName = GlobalSettings.requestedSceneName(_sceneID);

            //your logic here
            NetworkManager.singleton.ServerChangeScene(sceneName);
        }






        /// <summary>
        /// For Kicking someone
        /// </summary>
        /// <param name="_ni"></param>
        public void KickSomeone(NetworkIdentity _ni)
        {
            print("Kick sent to server");
            Cmd_Kickout(_ni);
        }

        [Command(requiresAuthority = false)]
        void Cmd_Kickout(NetworkIdentity _networkID)
        {
            print("Kick executed in Server");
            Rpc_Kickout(_networkID);
        }


        [ClientRpc]
        void Rpc_Kickout(NetworkIdentity _networkID)
        {
            print("kick people: " + _networkID.netId);

            //Your Logic here.
            if (_networkID == theLocalPlayer.GetComponent<NetworkIdentity>())
            {
                StartCoroutine(theLocalPlayer.mainControlTablet.ExecuteKickedOut());
            }
                
        }






        /// <summary>
        /// For muting everyone
        /// </summary>
        public void MuteEveryone()
        {
            CmdSetMuteEveryone();
        }


        [Command(requiresAuthority = false)]
        private void CmdSetMuteEveryone() 
        {
            //send the command to everyone
            Rpc_MuteEveryone();
        }


        [ClientRpc]
        private void Rpc_MuteEveryone()
        {
            //Exception
            //if you are the admin, dont do anything
            if (theLocalPlayer.adminStatus == (int)GlobalSettings.PlayerRole.admin)
            {
                print("mute yourself?");
                theLocalPlayer.mainControlTablet.muteToggle.isOn = false;
                theLocalPlayer.mainControlTablet.ShowInfo_MutedByAdmin();
                return;
            }

            //otherwise mute yourself.
            else
            {
                theLocalPlayer.mainControlTablet.muteToggle.isOn = false;
                theLocalPlayer.mainControlTablet.ShowInfo_MutedByAdmin();
            }
        }



        /// <summary>
        /// For Recalling everyone to the given pos
        /// </summary>
        public void RecallEveryone(Vector3 _pos)
        {
            CmdRecallEveryone(_pos);
        }


        [Command(requiresAuthority = false)]
        private void CmdRecallEveryone(Vector3 _pos)
        {
            //send the command to everyone
            Rpc_RecallEveryone(_pos);
        }


        [ClientRpc]
        private void Rpc_RecallEveryone(Vector3 _pos)
        {
            //Exception
            theLocalPlayer.transform.position = _pos;
            print("you are being recalled");
        }






        /// <summary>
        /// to despawn a network object
        /// </summary>
        /// <param name="_netId"></param>
        public void Ui_DeSpawnedNetworkObject(NetworkIdentity _netId)
        {
            Cmd_DeSpawnedNetworkObj(_netId);
        }


        [Command(requiresAuthority = false)]
        void Cmd_DeSpawnedNetworkObj(NetworkIdentity _netId)
        {
            NetworkServer.UnSpawn(_netId.gameObject);
        }



        public void Show_MessageItemRemoved(NetworkIdentity _ni)
        {
            Cmd_MessageItemRemoved(_ni);
        }

        [Command(requiresAuthority = false)]
        void Cmd_MessageItemRemoved(NetworkIdentity _networkID)
        {
            Rpc_MessageItemRemoved(_networkID);
        }


        [ClientRpc]
        void Rpc_MessageItemRemoved(NetworkIdentity _networkID)
        {
            //Your Logic here.
            if (_networkID == theLocalPlayer.GetComponent<NetworkIdentity>())
                theLocalPlayer.mainControlTablet.ShowInfo_ItemRemoved();
        }





        //destroy this room if there is no more people for x seconds.
        float timeTillDestroy = 60;
        /// <summary>
        /// To Count the timeout scene in the Server.
        /// The gameServer will automatically close in X seconds
        /// </summary>
        private void TimeoutScene()
        {
            //Get the Server date & Assign date
            //if the assign date is still larger than the server date, do nothing
            if (grs)
            {
                int comp = DateTime.Compare(DateTime.Now, inputDate);
                if (comp != 1)//if it is not in the past anymore, start the 60 seconds countdown.
                {
                    return;
                }
            }


            //if there is still people in there
            //reset the timer and do nothing
            if (NetworkPlayerList.Length > 0)
            {
                //Debug.Log("still 1 people");
                timeTillDestroy = 60;
                return;
            }

            if (timeTillDestroy > 0)
            {
                //Debug.Log("Countingdown");
                timeTillDestroy -= Time.deltaTime;
            }
            else
            {
                //Debug.Log("Quit the app test");
                Application.Quit();
            }
        }
    }
}
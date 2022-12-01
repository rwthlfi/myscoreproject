using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Insight
{
    public class GameRegistration : InsightModule
    {
        InsightClient client;
        Transport networkManagerTransport;

        //Pulled from command line arguments
        public string GameScene;
        public string NetworkAddress;
        public ushort NetworkPort;
        public string UniqueID;
        public string RoomName;
        public string RoomCreator;
        public string CreatorID;
        public string RoomPassword;
        public int RoomMaxPlayers;
        public string RoomExpireDate;

        //These should probably be synced from the NetworkManager
        public int MaxPlayers;
        public int CurrentPlayers;

        public override void Initialize(InsightClient insight, ModuleManager manager)
        {
            client = insight;
            client.transport.OnClientConnected += SendGameRegistrationToGameManager;

            networkManagerTransport = Transport.activeTransport;

            RegisterHandlers();
            GatherCmdArgs();

            InvokeRepeating("SendGameStatusToGameManager", 30f, 30f);
        }

        void RegisterHandlers() { }

        void GatherCmdArgs()
        {
            InsightArgs args = new InsightArgs();
            if (args.IsProvided("-NetworkAddress"))
            {
                Debug.Log("[Args] - NetworkAddress: " + args.NetworkAddress);
                NetworkAddress = args.NetworkAddress;

                NetworkManager.singleton.networkAddress = NetworkAddress;
            }

            if (args.IsProvided("-NetworkPort"))
            {
                Debug.Log("[Args] - NetworkPort: " + args.NetworkPort);
                NetworkPort = (ushort)args.NetworkPort;

                if(networkManagerTransport is MultiplexTransport) {
                    ushort startPort = NetworkPort;
                    foreach(Transport transport in (networkManagerTransport as MultiplexTransport).transports) {
                        SetPort(transport, startPort++);
                    }
                } else {
                    SetPort(networkManagerTransport, NetworkPort);
                }
            }

            if (args.IsProvided("-SceneName"))
            {
                Debug.Log("[Args] - SceneName: " + args.SceneName);
                GameScene = args.SceneName;
                //SceneManager.LoadScene(args.SceneName);

                NetworkManager.singleton.ServerChangeScene(args.SceneName);
            }

            if (args.IsProvided("-UniqueID"))
            {
                Debug.Log("[Args] - UniqueID: " + args.UniqueID);
                UniqueID = args.UniqueID;
            }


            if (args.IsProvided("-RoomName"))
            {
                Debug.Log("[Args] - RoomName: " + args.RoomName);
                RoomName = args.RoomName;
            }

            if (args.IsProvided("-RoomCreator"))
            {
                Debug.Log("[Args] - RoomCreator: " + args.RoomCreator);
                RoomCreator = args.RoomCreator;
            }

            if (args.IsProvided("-CreatorID"))
            {
                Debug.Log("[Args] - CreatorID: " + args.CreatorID);
                CreatorID = args.CreatorID;
            }

            if (args.IsProvided("-RoomPassword"))
            {
                Debug.Log("[Args] - RoomPassword: " + args.RoomPassword);
                RoomPassword = args.RoomPassword;
            }

            if (args.IsProvided("-RoomMaxPlayers"))
            {
                Debug.Log("[Args] - RoomMaxPlayers: " + args.RoomMaxPlayers);
                RoomMaxPlayers = args.RoomMaxPlayers;
            }

            if (args.IsProvided("-RoomExpireDate"))
            {
                Debug.Log("[Args] - RoomExpireDate: " + args.RoomExpireDate);
                RoomExpireDate = args.RoomExpireDate;
            }
            MaxPlayers = NetworkManager.singleton.maxConnections;

            //Save the game setting to be persistent
            SaveGameSetting();

            //Start NetworkManager
            NetworkManager.singleton.StartServer();
        }

        void SetPort(Transport transport, ushort port) {
            if(transport.GetType().GetField("port") != null) {
                transport.GetType().GetField("port").SetValue(transport, port);
            }else if(transport.GetType().GetField("Port") != null) {
                transport.GetType().GetField("Port").SetValue(transport, port);
            }else if(transport.GetType().GetField("CommunicationPort") != null) {//For Ignorance
                transport.GetType().GetField("CommunicationPort").SetValue(transport, port);
            }
        }

        void SendGameRegistrationToGameManager()
        {
            Debug.Log("[GameRegistration] - registering with master");
            client.Send(new RegisterGameMsg()
            {
                NetworkAddress = NetworkAddress,
                NetworkPort = NetworkPort,
                UniqueID = UniqueID,
                SceneName = GameScene,
                CurrentPlayers = CurrentPlayers,
                RoomName = RoomName,
                RoomCreator = RoomCreator,
                CreatorID = CreatorID,
                RoomPassword = RoomPassword,
                RoomMaxPlayers = RoomMaxPlayers,
                RoomExpireDate = RoomExpireDate
            });
        }

        void SendGameStatusToGameManager()
        {
            //Update with current values from NetworkManager:
            CurrentPlayers = NetworkManager.singleton.numPlayers;

            Debug.Log("[GameRegistration] - status update");
            client.Send(new GameStatusMsg()
            {
                UniqueID = UniqueID,
                CurrentPlayers = CurrentPlayers
            });
        }

        //Raymond: addition
        void SaveGameSetting()
        {
            //Get the GameRoomSetting
            GameRoomSetting grs = FindObjectOfType<GameRoomSetting>();
            grs.RoomName = RoomName;
            grs.RoomCreator = RoomCreator;
            grs.CreatorID = CreatorID;
            grs.RoomPassword = RoomPassword;
            grs.RoomMaxPlayers = RoomMaxPlayers;
            grs.RoomExpireDate = RoomExpireDate;
        }
    }
}

using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Insight
{
    public class ClientGameManager : InsightModule
    {
        InsightClient client;
        Transport networkManagerTransport;

        public List<GameContainer> gamesList = new List<GameContainer>();

        public override void Initialize(InsightClient client, ModuleManager manager)
        {
            this.client = client;

            networkManagerTransport = Transport.activeTransport;

            RegisterHandlers();
        }

        void RegisterHandlers()
        {
            client.RegisterHandler<ChangeServerMsg>(HandleChangeServersMsg);
            client.RegisterHandler<GameListMsg>(HandleGameListMsg);
        }

        void HandleChangeServersMsg(InsightNetworkMessage netMsg)
        {
            ChangeServerMsg message = netMsg.ReadMessage<ChangeServerMsg>();

            Debug.Log("[InsightClient] - Connecting to GameServer: " + message.NetworkAddress + ":" + message.NetworkPort + "/" + message.SceneName);

            if(networkManagerTransport is MultiplexTransport) {
                ushort startPort = message.NetworkPort;
                foreach(Transport transport in (networkManagerTransport as MultiplexTransport).transports) {
                    SetPort(transport, startPort++);
                }
            } else {
                SetPort(networkManagerTransport, message.NetworkPort);
            }

            NetworkManager.singleton.networkAddress = message.NetworkAddress;

            NetworkManager.singleton.StartClient();
            SceneManager.LoadScene(message.SceneName);
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

        void HandleGameListMsg(InsightNetworkMessage netMsg)
        {
            GameListMsg message = netMsg.ReadMessage<GameListMsg>();

            Debug.Log("[InsightClient] - Received Games List");

            gamesList.Clear();

            foreach (GameContainer game in message.gamesArray)
            {
                //Debug.Log(game.SceneName);

                gamesList.Add(new GameContainer()
                {
                    UniqueId = game.UniqueId,
                    SceneName = game.SceneName,
                    CurrentPlayers = game.CurrentPlayers,
                    MinPlayers = game.MinPlayers,
                    RoomName = game.RoomName,
                    RoomCreator = game.RoomCreator,
                    CreatorID = game.CreatorID,
                    RoomPassword = game.RoomPassword,
                    RoomMaxPlayers = game.RoomMaxPlayers,
                    RoomExpireDate = game.RoomExpireDate
                });
            }
        }

        #region Message Senders
        public void SendRequestSpawnStart(RequestSpawnStartMsg requestSpawnStartMsg)
        {
            client.Send(requestSpawnStartMsg);
        }

        public void SendJoinGameMsg(string UniqueID)
        {
            client.Send(new JoinGameMsg() { UniqueID = UniqueID });
        }

        public void SendGetGameListMsg()
        {
            client.Send(new GameListMsg());
        }
        #endregion
    }
}

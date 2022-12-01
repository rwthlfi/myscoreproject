using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Insight
{
    public class ServerGameManager : InsightModule
    {
        InsightServer server;
        MasterSpawner masterSpawner;

        public List<GameContainer> registeredGameServers = new List<GameContainer>();

        public void Awake()
        {
            AddDependency<MasterSpawner>();
        }

        public override void Initialize(InsightServer insight, ModuleManager manager)
        {
            server = insight;
            masterSpawner = manager.GetModule<MasterSpawner>();
            RegisterHandlers();

            server.transport.OnServerDisconnected += HandleDisconnect;
        }

        void RegisterHandlers()
        {
            server.RegisterHandler<RegisterGameMsg>(HandleRegisterGameMsg);
            server.RegisterHandler<GameStatusMsg>(HandleGameStatusMsg);
            server.RegisterHandler<JoinGameMsg>(HandleJoinGameMsg);
            server.RegisterHandler<GameListMsg>(HandleGameListMsg);
        }

        void HandleRegisterGameMsg(InsightNetworkMessage netMsg)
        {
            RegisterGameMsg message = netMsg.ReadMessage<RegisterGameMsg>();

            Debug.Log("[GameManager] - Received GameRegistration request");

            registeredGameServers.Add(new GameContainer()
            {
                NetworkAddress = message.NetworkAddress,
                NetworkPort = message.NetworkPort,
                UniqueId = message.UniqueID,
                SceneName = message.SceneName,
                RoomName = message.RoomName,
                RoomCreator = message.RoomCreator,
                CreatorID = message.CreatorID,
                RoomPassword = message.RoomPassword,
                RoomMaxPlayers = message.RoomMaxPlayers,
                CurrentPlayers = message.CurrentPlayers,
                RoomExpireDate = message.RoomExpireDate,

                connectionId = netMsg.connectionId,
            });
        }

        void HandleGameStatusMsg(InsightNetworkMessage netMsg)
        {
            GameStatusMsg message = netMsg.ReadMessage<GameStatusMsg>();

            Debug.Log("[GameManager] - Received Game status update");

            foreach (GameContainer game in registeredGameServers)
            {
                if (game.UniqueId == message.UniqueID)
                {
                    game.CurrentPlayers = message.CurrentPlayers;
                    return;
                }
            };
        }

        //Checks if the connection that dropped is actually a GameServer
        void HandleDisconnect(int connectionId)
        {
            foreach (GameContainer game in registeredGameServers)
            {
                if (game.connectionId == connectionId)
                {
                    registeredGameServers.Remove(game);
                    return;
                }
            }
        }

        void HandleGameListMsg(InsightNetworkMessage netMsg)
        {
            //Debug.Log("[MatchMaking] - Player Requesting Match list");

            GameListMsg gamesListMsg = new GameListMsg();
            gamesListMsg.Load(registeredGameServers);

            netMsg.Reply(gamesListMsg);
        }

        void HandleJoinGameMsg(InsightNetworkMessage netMsg)
        {
            JoinGameMsg message = netMsg.ReadMessage<JoinGameMsg>();

            Debug.Log("[MatchMaking] - Player joining Match.");

            GameContainer game = GetGameByUniqueID(message.UniqueID);

            if (game == null)
            {
                //Something went wrong
                //netMsg.Reply((short)MsgId.ChangeServers, new ChangeServerMsg());
            }
            else
            {
                netMsg.Reply(new ChangeServerMsg()
                {
                    NetworkAddress = game.NetworkAddress,
                    NetworkPort = game.NetworkPort,
                    SceneName = game.SceneName,
                    RoomName = game.RoomName,
                    RoomCreator = game.RoomCreator,
                    CreatorID = game.CreatorID,
                    RoomPassword = game.RoomPassword,
                    RoomMaxPlayers = game.RoomMaxPlayers,
                    RoomExpireDate = game.RoomExpireDate
                });
            }
        }

        //Used by MatchMaker to request a GameServer for a new Match
        public void RequestGameSpawnStart(RequestSpawnStartMsg requestSpawn)
        {
            masterSpawner.InternalSpawnRequest(requestSpawn);
        }

        public GameContainer GetGameByUniqueID(string uniqueID)
        {
            foreach (GameContainer game in registeredGameServers)
            {
                if (game.UniqueId.Equals(uniqueID))
                {
                    return game;
                }
            }
            return null;
        }
    }

    [Serializable]
    public class GameContainer
    {
        public string NetworkAddress;
        public ushort NetworkPort;
        public string UniqueId;
        public int connectionId;

        public string SceneName;
        public int MinPlayers;
        public int CurrentPlayers;
        public string RoomName;
        public string RoomCreator;
        public string CreatorID;
        public string RoomPassword;
        public int RoomMaxPlayers;
        public string RoomExpireDate;
    }
}

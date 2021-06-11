using DontBiteMeBoss.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DontBiteMeBoss.Server
{
    public static class ServerCommand
    {
        enum ClientCommandId
        {
            None,
            Disconnect,
            GetUUID,
            GetLobbies,
            GetLobby,
            CreateLobby,
            JoinLobby,
        }

        private static ClientCommandId GetCommandId(string command)
        {
            string cmd = command.Split('|')[0];
            ClientCommandId result;
            if (System.Enum.TryParse<ClientCommandId>(cmd, true, out result))
                return result;
            return ClientCommandId.None;
        }

        private static string[] GetCommandData(string command)
        {
            return command.Split('|');
        }

        public static void SendClientUUID(Client client)
        {
            client.Send($"SetUUID|{client.UUID}");
            GameServer.Log(null, $"Clients UUID send and set to: {client.UUID}");
        }
        public static void SendLobbies(Client client)
        {
            StringBuilder sb = new StringBuilder($"LobbyList|{client.UUID}|{GameServer.Instance.Lobbies.Count}");
            foreach (Lobby lb in GameServer.Instance.Lobbies)
            {
                sb.Append($"|{lb.UUID}|{lb._name}|{lb._maxPlayers.ToString()}|{lb.CurrentPlayers}|{Enum.GetName(typeof(LobbyStatus), lb.status)}");
            }
            GameServer.Log(null, $"Sent to client#{client.UUID}\n\t{sb.ToString()}");
            client.Send(sb.ToString());
        }
        public static void SendAllCreatedLobby(Lobby lobby)
        {
            string message = $"LobbyCreated|{lobby.UUID}|{lobby._name}|{lobby._maxPlayers}|{lobby.CurrentPlayers}|{lobby._leadersUUID}|{lobby.status.ToString()}";
            foreach (Client cl in GameServer.Instance.Clients)
            {
                cl.Send(message);
            }
        }
        public static void SendClientJoinLobby(Client client, Lobby lobby)
        {
            if (lobby.CurrentPlayers < lobby._maxPlayers)
                client.Send($"JoinLobby|{lobby.UUID}");
        }
        private static void SendLobbyData(Client client, Lobby lobby)
        {
            StringBuilder sb = new StringBuilder($"LobbyData|{lobby._leadersUUID}|{lobby.CurrentPlayers}|{lobby._maxPlayers}|{lobby.status.ToString()}");
            foreach(LobbyClient player in lobby.players)
            {
                sb.Append($"|{player.client.user}");
            }
            client.Send(sb.ToString());
        }
        internal static void ActOnCommand(Client client, string message)
        {
            string[] data = GetCommandData(message);
            ClientCommandId command = GetCommandId(message);
            switch (command)
            {
                case ClientCommandId.Disconnect:
                    if (GameServer.Instance.Clients.Find((cl) => cl.UUID == client.UUID) != null)
                        GameServer.Instance.Clients.Remove(client);
                    GameServer.Log(client.UUID, "Disconnected client");
                    break;
                case ClientCommandId.GetUUID:
                    SendClientUUID(client);
                    break;
                case ClientCommandId.GetLobbies:
                    SendLobbies(client);
                    break;
                case ClientCommandId.CreateLobby:
                    CreateLobby(client, data[2]);
                    break;
                case ClientCommandId.None:
                    GameServer.Log(null, $"Client send unknown command: {data[0]}");
                    break;
                case ClientCommandId.GetLobby:
                    SendLobbyData(client, GameServer.Instance.Lobbies.Find((lb) => lb.UUID == data[2]));
                    break;
                case ClientCommandId.JoinLobby:
                    Lobby lb = GameServer.Instance.Lobbies.Find((lb) => lb.UUID == data[1]);
                    ClientJoinLobby(client, lb);
                    break;
            }
        }
        private static void CreateLobby(Client client, string lobbyName)
        {
            //Create lobby
            Lobby lb = GameServer.Instance.CreateLobby(lobbyName, client.UUID);
            lb._leadersUUID = client.UUID;
            //Send Lobby created
            SendAllCreatedLobby(lb);
            //Send client join mentioned lobby
            lb._leadersUUID = client.UUID;
            ClientJoinLobby(client, lb);
        }

        private static void ClientJoinLobby(Client client, Lobby lb)
        {
            lb.AddPlayer(client);
            SendClientJoinLobby(client, lb);
        }
    }
}

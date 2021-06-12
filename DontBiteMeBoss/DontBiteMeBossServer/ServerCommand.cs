﻿using DontBiteMeBoss.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

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
            Ready, //player set ready status to true
        }

        private static ClientCommandId GetCommandId(string command)
        {
            if (command == string.Empty || command == null)
                return ClientCommandId.Disconnect;
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
            client.Send($"JoinLobby|{lobby.UUID}");
            foreach (Client cl in GameServer.Instance.Clients)
            {
                if (cl.UUID != client.UUID)
                    cl.Send($"SomeneJoinedLobby|{lobby.UUID}|{client.UUID}|{cl.user.Username}|{lobby.CurrentPlayers}");
            }
        }
        private static void SendLobbyData(Client client, Lobby lobby)
        {
            StringBuilder sb = new StringBuilder($"LobbyData|{lobby._leadersUUID}|{lobby.CurrentPlayers}|{lobby._maxPlayers}");
            foreach (LobbyClient player in lobby.players)
            {
                sb.Append($"|{player.client.user.Username}|{player.client.UUID}");
            }
            client.Send(sb.ToString());
        }
        private static void SendToLobbyPlayersPlayerReady(Client client, string lobbyUUID)
        {
            Lobby lobby = GameServer.Instance.Lobbies.Find((lb) => lb.UUID == lobbyUUID);
            lobby.players.ForEach((player) =>
            {
                player.client.Send($"Ready|{client.UUID}");
            });
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
                    SendLobbyData(client, GameServer.Instance.Lobbies.Find((lb) => lb.UUID == data[1]));
                    break;
                case ClientCommandId.JoinLobby:
                    Lobby lb = GameServer.Instance.Lobbies.Find((lb) => lb.UUID == data[1]);
                    ClientJoinLobby(client, lb);
                    break;
                case ClientCommandId.Ready:
                    SendToLobbyPlayersPlayerReady(client, data[1]);
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
            ClientJoinLobby(client, lb);
        }

        private static void ClientJoinLobby(Client client, Lobby lb)
        {
            if (lb.CurrentPlayers < lb._maxPlayers)
            {
                lb.AddPlayer(client);
                SendClientJoinLobby(client, lb);
            }
        }
    }
}

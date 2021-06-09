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
            CreateLobby,
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
            foreach(Lobby lb in GameServer.Instance.Lobbies)
            {
                sb.Append($"|{lb.UUID}|{lb._name}|{lb._maxPlayers.ToString()}|{lb.CurrentPlayers}|{Enum.GetName(typeof(LobbyStatus),lb.status)}");
            }
            GameServer.Log(null, $"Sent to client#{client.UUID}\n\t{sb.ToString()}");
            client.Send(sb.ToString());
        }
        internal static void ActOnCommand(Client client, string message)
        {
            string[] data = GetCommandData(message);
            ClientCommandId command = GetCommandId(message);
            switch (command)
            {
                case ClientCommandId.Disconnect:
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
            }
        }

        private static void CreateLobby(Client client, string lobbyName)
        {
            GameServer.Instance.CreateLobby(lobbyName, client.UUID);
        }
    }
}

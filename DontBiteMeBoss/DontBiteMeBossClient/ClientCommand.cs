using DontBiteMeBoss.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DontBiteMeBoss.ClientSide
{
    public enum ServerCommandId
    {
        None,
        Move,               //UUID, float posx, float posy
        Shoot,              //UUID, float direction
        CollectItem,        //UUID of player, UUID of item
        SetRotation,        //UUID, float direction
        ChangeWeapon,       //UUID, string WeaponType
        ThrowGranade,       //UUID, float direction
        LoginAccepted,      //UUID, User.highscore
        LobbyList,          //UUID, int count, Lobby[Lobby.UUID, string lobby.name, int lobby.maxplayers, int Lobby.currentPlayers string lobby.status]
        LobbyCreated,       //Lobby[Lobby.UUID, string lobby.name, int lobby.maxplayers, int Lobby.currentPlayers string lobby.status]
        SetUUID,            //
        JoinLobby,          //UUID, int playersCount, Player[Player.UUID, string username, bool ready, bool isLeader]
        SomeneJoinedLobby,  //UUID, Player.UUID, string username, bool ready
        SomeoneLeftLobby,   //UUID
        LobbyStart,         //_
        PlayerReadyChanged, //UUID, bool isReady

    }
    public static class ClientCommand
    {
        //basic commands
        public static ServerCommandId GetCommandId(string command)
        {
            string cmd = command.Split('|')[0];
            ServerCommandId result;
            if (System.Enum.TryParse<ServerCommandId>(cmd, true, out result))
                return result;
            return ServerCommandId.None;
        }
        public static string[] GetCommandData(string command)
        {
            return command.Split('|');
        }
        //Commands to send
        public static void AskForLobbyData(Client client)
        {
            StringBuilder sb = new StringBuilder("GetLobbies|");
            sb.Append(client.UUID);
            client.Send(sb.ToString());
        }

        public static void AskForUUID(Client client)
        {
            StringBuilder sb = new StringBuilder("GetUUID");
            client.Send(sb.ToString());
        }
        public static void AskToCreateLobby(Client client, string lobbyName)
        {
            client.Send($"CreateLobby|{client.UUID}|{lobbyName}");
        }

        //Commands on receive
        public static void ActOnResponse(Client client, string message)
        {
            string[] data = GetCommandData(message);
            ServerCommandId command = GetCommandId(message);
            switch (command)
            {
                case ServerCommandId.Move:
                    break;
                case ServerCommandId.Shoot:
                    break;
                case ServerCommandId.CollectItem:
                    break;
                case ServerCommandId.SetRotation:
                    break;
                case ServerCommandId.ChangeWeapon:
                    break;
                case ServerCommandId.ThrowGranade:
                    break;
                case ServerCommandId.LoginAccepted:
                    break;
                case ServerCommandId.LobbyList:
                    ClientSetLobbyList(client, data);
                    break;
                case ServerCommandId.SetUUID:
                    ClientSetUUID(client, data[1]);
                    break;
                case ServerCommandId.JoinLobby:
                    break;
                case ServerCommandId.SomeneJoinedLobby:
                    break;
                case ServerCommandId.SomeoneLeftLobby:
                    break;
                case ServerCommandId.LobbyStart:
                    break;
                case ServerCommandId.PlayerReadyChanged:
                    break;
                case ServerCommandId.None:
                    break;
                case ServerCommandId.LobbyCreated:
                    Lobby lobby = new Lobby(data[1], data[2], int.Parse(data[3]), int.Parse(data[4]), data[5]);
                    CreateLobby(client, lobby);
                    break;
            }
        }

        private static void CreateLobby(Client client, Lobby lobby)
        {
            DontBiteMeBossClient.Get.mMenu.AddLobbyToList(lobby);
        }

        private static void ClientSetLobbyList(Client client, string[] data)
        {
            MainMenuData mmData = DontBiteMeBossClient.Get.mMenu.mmData;
            mmData.RemoveAllLobbies();
            int lobbiesCount = int.Parse(data[2]);
            for (int i = 0; i < lobbiesCount; ++i)
            {
                //UUID, int count, Lobby[Lobby.UUID, string lobby.name, int lobby.maxplayers, int Lobby.currentPlayers string lobby.status
                Lobby lb = new Lobby(data[3 + 5 * i], data[4 + 5 * i], int.Parse(data[5 + 5 * i]), int.Parse(data[6 + 5 * i]), data[7 + 5 * i]);
                mmData.AddLobby(lb);
            }
        }

        private static void ClientSetUUID(Client client, string UUID)
        {
            client.SetUUID(UUID);
        }
    }
}
using System;
using System.Collections.Generic;

namespace DontBiteMeBoss.Core
{
    public enum LobbyStatus
    {
        None,
        WaitingForReady,
        AllReady,
        StartedCountDown,
        InLoading,
        InPlay,
        InFinishing,
        Closing,
    }
    public class Lobby
    {
        public string UUID;
        public string _name;
        public int _maxPlayers;
        public LobbyStatus status;
        private List<Client> players = new List<Client>();
        private string _leadersUUID;
        public int CurrentPlayers;

        public Lobby(string UUID, string name)
        {
            this.UUID = UUID;
            _name = name;
            _maxPlayers = 4;
            CurrentPlayers = 0;
        }
        public Lobby(string UUID, string name, int maxPlayers, int currentPlayers, string leadersUUID)
        {
            this.UUID = UUID;
            _name = name;
            _maxPlayers = maxPlayers;
            this.CurrentPlayers = currentPlayers;
            _leadersUUID = leadersUUID;
        }
        public void SetPlayerReadyStatus(Client player, bool isReady)
        {
            //player.isReady = isReady;
        }

        private void CheckAllPlayersReady()
        {
            bool allReady = true;
            /*foreach (Client player in players)
                if (!player.isReady)
                    allReady = false;*/

            if (allReady)
                SetLobbyStatus(LobbyStatus.AllReady);
            else
                SetLobbyStatus(LobbyStatus.WaitingForReady);
        }

        private void SetLobbyStatus(LobbyStatus status)
        {
            this.status = status;
        }
    }
}
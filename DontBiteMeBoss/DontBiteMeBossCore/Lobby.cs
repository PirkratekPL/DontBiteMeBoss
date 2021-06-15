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
        public List<LobbyClient> players = new List<LobbyClient>();
        public string _leadersUUID;
        public int CurrentPlayers;

        public Lobby(string UUID, string name)
        {
            this.UUID = UUID;
            _name = name;
            _maxPlayers = 4;
            CurrentPlayers = 0;
        }
        public Lobby(string UUID, string name, string leadersUUID, int maxPlayers=4, int currentPlayers=0)
        {
            this.UUID = UUID;
            _name = name;
            _maxPlayers = maxPlayers;
            this.CurrentPlayers = currentPlayers;
            _leadersUUID = leadersUUID;
        }
        public void SetPlayerReadyStatus(LobbyClient player, bool isReady)
        {
            player.isReady = isReady;
        }

        public bool CheckAllPlayersReady()
        {
            bool allReady = true;
            foreach (LobbyClient player in players)
                if (!player.isReady)
                    allReady = false;

            return allReady;
        }

        private void SetLobbyStatus(LobbyStatus status)
        {
            this.status = status;
        }

        public void AddPlayer(Client client)
        {
            ++CurrentPlayers;
            LobbyClient lCl = new LobbyClient(client);
            if (lCl.client.UUID == _leadersUUID)
                lCl.isReady = true;
            players.Add(lCl);
        }
    }
}
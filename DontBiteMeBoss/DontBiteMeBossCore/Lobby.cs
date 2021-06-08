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
        private String UUID;
        private string _name;
        private int _maxPlayers;
        private LobbyStatus status;
        private List<Client> players = new List<Client>();
        private Client _leader;
        public int CurrentPlayers { get { return players.Count; } }
        public Lobby(string UUID, string name, int maxPlayers, Client leader)
        {
            this.UUID = UUID;
            _name = name;
            _maxPlayers = maxPlayers;
            _leader = leader;
            players.Add(leader);
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
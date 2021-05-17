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
        private Guid _guid;
        private int _maxPlayers;
        private LobbyStatus status;
        private List<Player> players;
        private Player _leader;
        public Guid ID { get { return _guid; } }

        public Lobby(Guid guid, int maxPlayers, Player leader)
        {
            _guid = guid;
            _maxPlayers = maxPlayers;
            _leader = leader;
        }
        public void SetPlayerReadyStatus(Player player, bool isReady)
        {
            player.isReady = isReady;
        }

        private void CheckAllPlayersReady()
        {
            bool allReady = true;
            foreach (Player player in players)
                if (!player.isReady)
                    allReady = false;

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
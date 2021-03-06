using System;
using System.Collections.Generic;
using System.Text;
using DontBiteMeBoss.Core;

namespace DontBiteMeBoss.Client
{
    public struct MainMenuData
    {
        private List<Lobby> _lobbies; //list of all lobbies
        private User currentUser; //user info to display
        private bool _showAllLobbies;  //toggle to show hidden and started lobbies

        public List<Lobby> Lobbies { get { return _lobbies; } }

        public void AddLobby(Lobby lobby)
        {
            _lobbies.Add(lobby);
        }

        public void RemoveLobby(Lobby lobby)
        {
            _lobbies.Remove(lobby);
        }

        public void RemoveLobby(Guid ID)
        {
            _lobbies.RemoveAll((lobby) => ID == lobby.ID);
        }
    }
}

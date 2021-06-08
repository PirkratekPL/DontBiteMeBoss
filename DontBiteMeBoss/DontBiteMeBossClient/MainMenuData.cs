using System;
using System.Collections.Generic;
using System.Text;
using DontBiteMeBoss.Core;

namespace DontBiteMeBoss.ClientSide
{
    public class MainMenuData
    {
        private List<Lobby> _lobbies = new List<Lobby>(); //list of all lobbies
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
           // _lobbies.RemoveAll((lobby) => ID == lobby.UUID);
        }
    }
}

namespace DontBiteMeBoss.ClientSide
{
    public enum ServerCommandId
    {
        Move,               //UUID, float posx, float posy
        Shoot,              //UUID, float direction
        CollectItem,        //UUID of player, UUID of item
        SetRotation,        //UUID, float direction
        ChangeWeapon,       //UUID, string WeaponType
        ThrowGranade,       //UUID, float direction
        LoginAccepted,      //UUID, User.highscore
        LobbyList,          //UUID, int count, Lobby[Lobby.UUID, string lobby.name, int lobby.maxplayers, int Lobby.currentPlayers string lobby.status
        UserSent,           //why the fuck i put it here
        JoinLobby,          //UUID, int playersCount, Player[Player.UUID, string username, bool ready, bool isLeader]
        SomeneJoinedLobby,  //UUID, Player.UUID, string username, bool ready
        SomeoneLeftLobby,   //UUID
        LobbyStart,         //_
        PlayerReadyChanged, //UUID, bool isReady
        
    }
    public static class ClientCommand
    {
        public static ServerCommandId GetCommandId(string command)
        {
            string cmd = command.Split('|')[0];
            return (ServerCommandId)System.Enum.Parse(typeof(ServerCommandId), cmd);
        }
        public static object[] GetCommandData(string command)
        {
            return command.Split('|');
        }
    }
}
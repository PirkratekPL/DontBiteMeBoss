using System;

namespace DontBiteMeBoss.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            GameServer gs = new GameServer("127.0.0.1", 34343);
            gs.StartServer();
        }
    }
}

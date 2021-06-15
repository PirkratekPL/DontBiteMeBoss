using System;

namespace DontBiteMeBoss.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            GameServer gs = new GameServer("10.10.10.1", 34343);
            gs.StartServer();
        }
    }
}

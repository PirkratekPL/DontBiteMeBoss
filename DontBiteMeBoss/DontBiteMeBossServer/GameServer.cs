using DontBiteMeBoss.Core;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DontBiteMeBoss.Server
{
    class GameServer
    {
        public List<Client> Clients = new List<Client>();
        public List<Lobby> Lobbies = new List<Lobby>();
        private string serverIp;
        private int serverPort;

        TcpListener tcpListener;
        private bool isRunning = false;
        public static GameServer Instance;
        Dictionary<string, Thread> MatchThreads = new Dictionary<string, Thread>();

        public GameServer(string ip, int port)
        {
            if (Instance == null)
                Instance = this;
            this.serverIp = ip;
            this.serverPort = port;
        }

        public void StartServer()
        {
            Thread listenerThread = new Thread(StartListening);
            isRunning = true;
            Log(null, "Server started!");
            listenerThread.Start();
            while(isRunning)
            {

            }
        }

        private void StartListening()
        {
            Log(null, "Started listening for clients.");
            try
            {
                while (isRunning)
                {
                    tcpListener = new TcpListener(IPAddress.Parse(serverIp), serverPort++);
                    tcpListener.Start();
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    Client client = new Client(Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5), tcpClient.Client);
                    client.user = new User("FunnyName", 0l);
                    Clients.Add(client);
                    Log(null, "Client connected");
                    Thread clientThread = new Thread(ClientConnectionProc);
                    clientThread.Start(client);
                    tcpListener.Stop();
                }
            }
            catch (Exception ex)
            {
                Error(tcpListener, ex);
                tcpListener.Stop();
                StartListening();
            }
        }

        private void ClientConnectionProc(object obj)
        {
            Client client = (Client)obj;
            string command;
            while (client.socket.Connected)
            {
                try
                {
                    if (client != null && (command = client.Read()) != string.Empty)
                    {
                        Log(client.UUID, command);
                        ServerCommand.ActOnCommand(client, command);
                    }
                } catch (Exception ex)
                {
                    client.socket.Disconnect(false);
                    Clients.Remove(client);
                    Error(client.UUID, ex);
                }
            }
        }

        public Lobby CreateLobby(string lobbyName, string clientUUID)
        {
            Lobby lb = new Lobby(Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5), lobbyName, clientUUID);
            Lobbies.Add(lb);
            return lb;
        }

        public void MatchThread()
        {
            GameMatchServer match = new GameMatchServer();
            while(true)
            {
                match.Update();
            }
        }

        public static void Log(object sender, string message)
        {
            StringBuilder sb = new StringBuilder("[Server] ");
            sb.Append(message);
            if (sender != null)
            {
                sb.Append("\n\tby ").Append(sender.ToString());
            }
            Console.WriteLine(sb.ToString());
        }

        public static void Error(object sender, Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            StringBuilder sb = new StringBuilder("[Error] ");
            sb.Append(ex.Message).Append("\n\t\t").Append(ex.StackTrace);
            if (sender != null)
            {
                sb.Append("\n\tby ").Append(sender.ToString());
            }
            Console.WriteLine(sb.ToString());
            Console.ResetColor();
        }

        public static void Error(object sender, string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            StringBuilder sb = new StringBuilder("[Error] ");
            sb.Append(error);
            if (sender != null)
            {
                sb.Append("\n\tby ").Append(sender.ToString());
            }
            Console.WriteLine(sb.ToString());
            Console.ResetColor();
        }
        public static void Error(object sender, Exception ex, int lineNumber)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            StringBuilder sb = new StringBuilder("[Error] ");
            sb.Append(ex.Message);
            if (sender != null)
            {
                sb.Append("\n\tby ").Append(sender.ToString());
            }
            Console.WriteLine(sb.ToString());
            Console.ResetColor();
        }
    }
}

﻿using DontBiteMeBoss.Core;
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

        public GameServer(string ip, int port)
        {
            this.serverIp = ip;
            this.serverPort = port;
        }

        public void StartServer()
        {
            tcpListener = new TcpListener(IPAddress.Parse(serverIp), serverPort);
            Thread listenerThread = new Thread(StartListening);
            isRunning = true;
            listenerThread.Start();
            Log(null, "Server started!");
        }

        private void StartListening()
        {
            tcpListener.Start();

            Log(null, "Started listening for clients.");
            try
            {
                while (isRunning)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    Client client = new Client(Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5), tcpClient.Client);
                    Clients.Add(client);
                    Log(null, "Client connected");
                    Thread clientThread = new Thread(ClientConnectionProc);
                    clientThread.Start(client);
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
            Client cl = (Client)obj;
            while (cl.socket.Poll(1000, SelectMode.SelectWrite))
            {

            }
            Clients.Remove(cl);
            Log(null, "Client disconnected!");
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
            sb.Append(ex.Message);
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
    }
}
using System;
using System.IO;
using System.Net.Sockets;

namespace DontBiteMeBoss.Core
{
    public class Client
    {
        public string UUID;
        public User user;
        public Socket socket;
        public NetworkStream networkStream;
        public StreamWriter streamWriter;
        public StreamReader streamReader;
        public Client() { }
        public Client(string UUID, Socket socket)
        {
            SetUUID(UUID);
            SetSocket(socket);
        }
        public Client(Socket socket)
        {
            SetSocket(socket);
        }
        public void SetUUID(string uuid) { this.UUID = uuid; }
        public void SetSocket(Socket socket)
        {
            this.socket = socket;
            this.networkStream = new NetworkStream(this.socket);
            this.streamWriter = new StreamWriter(this.networkStream);
            this.streamReader = new StreamReader(this.networkStream);
        }

        public void Send(string message)
        {
            streamWriter.WriteLine(message);
            streamWriter.Flush();
        }

        public string Read()
        {
            return streamReader.ReadLine();
        }

        public string ReadTillEnd()
        {
            return streamReader.ReadToEnd();
        }
    }
}
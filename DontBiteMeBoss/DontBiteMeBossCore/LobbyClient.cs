using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontBiteMeBoss.Core
{
    public class LobbyClient
    {
        public Client client;
        public bool isReady = false;

        public LobbyClient(Client client)
        {
            this.client = client;
        }
    }
}

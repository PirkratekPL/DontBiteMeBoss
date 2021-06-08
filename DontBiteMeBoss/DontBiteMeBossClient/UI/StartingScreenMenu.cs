using DontBiteMeBoss.Core;
using Microsoft.Xna.Framework;
using MonoGame.UI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DontBiteMeBoss.ClientSide
{
    public class StartingScreenMenu : ControlManager
    {
        Game game;
        Rectangle windowRect;
        private Button InfoLabel;
        private bool isConnected = false;
        public StartingScreenMenu(Game game) : base(game)
        {
            this.game = game;
            this.windowRect = game.Window.ClientBounds;
        }
        public void SetScreenText(string text)
        {
            if (InfoLabel != null)
                InfoLabel.Text = text;
        }
        public override void InitializeComponent()
        {
            FilledRectangle bckg = new FilledRectangle(0, 0, windowRect.Width, windowRect.Height)
            {
                BackgroundColor = Color.Azure,
            };
            Controls.Add(bckg);
            InfoLabel = new Button()
            {
                Text = "Click button to connect with server...",
                TextColor = Color.Black,
                Size = new Vector2(windowRect.Width, windowRect.Height),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            
            Controls.Add(InfoLabel);

            Button ConnectButton = new Button()
            {
                Text = "Connect to game server",
                Size = new Vector2(200f, 80f),
                Location = new Vector2(windowRect.Width / 2 - 100f, windowRect.Height / 2 + 40f),
            };
            ConnectButton.Clicked += ConnectButton_Clicked;
            this.Controls.Add(ConnectButton);
        }

        private void ConnectButton_Clicked(object sender, EventArgs e)
        {
            TcpClient tcpClient;
            if (!isConnected)
            {
                try
                {
                    tcpClient = new TcpClient("127.0.0.1", 34343);
                    isConnected = true;
                    InfoLabel.Text = "Connected";
                    (game as DontBiteMeBossClient).thisClient = new Client(tcpClient.Client);
                    (game as DontBiteMeBossClient).ServerConnectionThread.Start(new object[] { (game as DontBiteMeBossClient).thisClient, game });

                    game.Components.Remove((game as DontBiteMeBossClient).ssMenu);
                    game.Components.Add((game as DontBiteMeBossClient).mMenu);
                }
                catch (Exception ex)
                {
                    InfoLabel.Text = "Cant connect to server! Try again later";
                }
            }
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
    }
}

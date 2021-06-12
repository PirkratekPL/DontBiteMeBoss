using DontBiteMeBoss.Core;
using Microsoft.Xna.Framework;
using MonoGame.UI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DontBiteMeBoss.ClientSide
{
    public class LobbyMenu : ControlManager
    {
        public Lobby thisLobby;
        DontBiteMeBossClient game;
        Rectangle windowRect;
        public bool allReady = false;
        private List<FilledRectangle> playerSlots = new List<FilledRectangle>();
        private List<Label> occupiedSlots = new List<Label>();
        private Button readyButton;
        public bool imReady = false;
        public LobbyMenu(DontBiteMeBossClient game, string lobbyUUID) : base(game)
        {
            this.game = game;
            thisLobby = new Lobby(lobbyUUID, "New Lobby");
            windowRect = new Rectangle(0, 0, this.game.windowWidth, this.game.windowHeight);
        }
        public override void Initialize()
        {
            game.thisClient.Send($"GetLobby|{thisLobby.UUID}");
            base.Initialize();
        }
        public void AddUser(Client client)
        {
            if (thisLobby.players.FindIndex((player) => player.client.UUID == client.UUID) == -1)
            {
                playerSlots[thisLobby.players.Count].BackgroundColor = Color.DarkGray;
                occupiedSlots.Add(new Label()
                {
                    Text = client.user.Username,
                    Location = new Vector2(windowRect.Width * 0.25f, windowRect.Height * 0.435f + windowRect.Height * 0.15f * (occupiedSlots.Count)),
                });
                Controls.Add(occupiedSlots[occupiedSlots.Count - 1]);
            }
            thisLobby.AddPlayer(client);
        }

        public override void InitializeComponent()
        {
            FilledRectangle bckground = new FilledRectangle(0, 0, 0, 0)
            {
                BackgroundColor = Color.Coral,
            };
            Controls.Add(bckground);
            readyButton = new Button()
            {
                Text = "Set Ready!",
                Size = new Vector2(windowRect.Width * 0.25f, windowRect.Height * 0.15f),
                Location = new Vector2(windowRect.Width * 0.55f, windowRect.Height * 0.1f),
                BackgroundColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
            };

            readyButton.Clicked += ClickedReadyButton;
            Controls.Add(readyButton);
        }

        private void ClickedReadyButton(object sender, EventArgs e)
        {
            if (thisLobby._leadersUUID == DontBiteMeBossClient.Get.thisClient.UUID)
            {
                //Start game
            }
            else
            {

                readyButton.BackgroundColor = Color.LimeGreen;
                if (!imReady)
                    ClientCommand.SendReady(thisLobby.UUID, DontBiteMeBossClient.Get.thisClient);
                imReady = true;
                SetPlayerReady(DontBiteMeBossClient.Get.thisClient.UUID);
                readyButton.Text = "Ready!";
            }
        }

        public void SetPlayerReady(string UUID)
        {
            thisLobby.players.Find((player) => player.client.UUID == UUID).isReady = true;
            allReady = thisLobby.CheckAllPlayersReady();
            if (allReady)
                readyButton.BackgroundColor = Color.LimeGreen;
        }

        public void SetPlayersList(string[] data)
        {
            thisLobby._leadersUUID = data[1];
            int slots = int.Parse(data[3]);
            for (int i = 0; i < slots; ++i)
            {
                playerSlots.Add(new FilledRectangle((int)(windowRect.Width * 0.2f), (int)(windowRect.Height * 0.4f + windowRect.Height * 0.15f * i), (int)(windowRect.Width * 0.6f), (int)(windowRect.Height * 0.14f))
                {
                    BackgroundColor = Color.LightGray,
                });
                Controls.Add(playerSlots[i]);
            }
            for (int i = 4; i < data.Length; i += 2)
            {
                occupiedSlots.Add(new Label()
                {
                    Text = data[i],
                    Location = new Vector2(windowRect.Width * 0.25f, windowRect.Height * 0.435f + windowRect.Height * 0.15f * (i - 4) / 2),
                });
                Controls.Add(occupiedSlots[(i - 4)/2]);
                playerSlots[(i - 4)/2].BackgroundColor = Color.DarkGray;
                Client player = new Client();
                player.UUID = data[i + 1];
                player.user = new User(data[i], 0L);
                thisLobby.AddPlayer(player);
                if (thisLobby._leadersUUID == DontBiteMeBossClient.Get.thisClient.UUID)
                    readyButton.Text = "Start Game!";
            }
        }
    }
}

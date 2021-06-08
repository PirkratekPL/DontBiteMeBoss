using DontBiteMeBoss.Core;
using Microsoft.Xna.Framework;
using MonoGame.UI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontBiteMeBoss.ClientSide
{
    public class MainMenu : ControlManager
    {
        User user;
        MainMenuData mmData;
        Rectangle windowRect;
        Game game;
        List<Button> BtnsLobbies = new List<Button>();

        public MainMenu(Game game) : base(game)
        {
            this.game = game;
            windowRect = game.Window.ClientBounds;
            //TODO: ask server for lobby data
            //temp
            /*
            mmData = new MainMenuData();
            Player pl = new Player();
            pl.user = new User(Guid.NewGuid(), "Drag", 123L);
            Lobby lb = new Lobby(Guid.NewGuid(), "test lobby", 8, pl);
            mmData.AddLobby(lb);
            mmData.AddLobby(lb);
            mmData.AddLobby(lb);
            mmData.AddLobby(lb);
            mmData.AddLobby(lb);*/
        }

        public override void InitializeComponent()
        {
            FilledRectangle bckg = new FilledRectangle(0, 0, windowRect.Width, windowRect.Height)
            {
                BackgroundColor = Color.Azure
            };
            Controls.Add(bckg);

            Line LnLeft = new Line(new Vector2(windowRect.Width * 0.1f, windowRect.Height * 0.2f), new Vector2(windowRect.Width * 0.1f, windowRect.Height * 0.9f));
            LnLeft.BackgroundColor = Color.Black;
            Controls.Add(LnLeft);
            Line LnRight = new Line(new Vector2(windowRect.Width * 0.7f, windowRect.Height * 0.2f), new Vector2(windowRect.Width * 0.7f, windowRect.Height * 0.9f));
            LnRight.BackgroundColor = Color.Black;
            Controls.Add(LnRight);

            Button BtnCreateLobby = new Button()
            {
                Text = "Create Lobby",
                Size = new Vector2(windowRect.Width * 0.2f, windowRect.Height * 0.10f),
                Location = new Vector2(windowRect.Width * 0.5f, windowRect.Height * 0.09f),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            Controls.Add(BtnCreateLobby);
            BtnCreateLobby.Clicked += BtnCreateLobby_Clicked;

            //Lobbies
            if (mmData != null)
            {
                for (int i = 0; i < mmData.Lobbies.Count; ++i)
                {
                    Button BtnLobby = new Button()
                    {
                        Text = "",
                        Size = new Vector2(windowRect.Width * 0.58f, windowRect.Height * 0.1f),
                        Location = new Vector2(windowRect.Width * 0.11f, windowRect.Height * 0.2f + windowRect.Height * 0.1f * i),
                        BackgroundColor = Color.LightGray,
                    };
                    Controls.Add(BtnLobby);
                }
                for (int i = 1; i < mmData.Lobbies.Count; ++i)
                {
                    Line LnSeparator = new Line(new Vector2(windowRect.Width * 0.11f, windowRect.Height * 0.2f + windowRect.Height * 0.1f * i), new Vector2(windowRect.Width * 0.69f, windowRect.Height * 0.2f + windowRect.Height * 0.1f * i));
                    Controls.Add(LnSeparator);
                }
            }
            if(mmData == null || mmData.Lobbies.Count == 0)
            {
                Label lbl = new Label()
                {
                    Text = "No lobbies found. Consider creating a new one.",
                    Size = new Vector2(windowRect.Width * 0.58f, windowRect.Height * 0.1f),
                    Location = new Vector2(windowRect.Width * 0.11f, windowRect.Height * 0.2f),
                    BackgroundColor = Color.LightGray,
                };
            }
        }

        private void BtnCreateLobby_Clicked(object sender, EventArgs e)
        {
            //TODO: Ask server to create lobby and join it
        }
    }
}

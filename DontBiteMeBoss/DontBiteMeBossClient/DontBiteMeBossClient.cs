﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DontBiteMeBoss.Core;
using System.Threading;
using System.Collections.Generic;
using System;

namespace DontBiteMeBoss.ClientSide
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class DontBiteMeBossClient : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public StartingScreenMenu ssMenu;
        public MainMenu mMenu;
        public LobbyMenu lbMenu;
        public Client thisClient;
        public bool isConnectedToServer = false;
        public Thread ServerConnectionThread = new Thread(CommunicationProc);

        public int windowWidth = 1280;
        public int windowHeight = 720;
        private static DontBiteMeBossClient instance;
        public static DontBiteMeBossClient Get { get { return instance; } }

        public DontBiteMeBossClient()
        {
            if (instance == null)
                instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            SetWindowSize(windowWidth, windowHeight);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //Content.Load<Texture2D>("Content/assets/sprites/cursor");
            this.IsMouseVisible = true;
            ssMenu = new StartingScreenMenu(this);
            mMenu = new MainMenu(this);

            this.Components.Add(ssMenu);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void SetWindowSize(int width, int height)
        {
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            ServerConnectionThread.Abort();
            thisClient?.Send("Disconnect");
            base.OnExiting(sender, args);
        }

        public static void CommunicationProc(object arg0)
        {
            object[] parameters = (object[])arg0;
            Client client = (Client)parameters[0];
            DontBiteMeBossClient game = (DontBiteMeBossClient)parameters[1];
            while (client.socket.Connected)
            {
                try
                {
                    string message = client.streamReader.ReadLine();
                    if (message != string.Empty)
                    {
                        ClientCommand.ActOnResponse(client, message);
                    }
                }
                catch(System.IO.IOException ex)
                {
                    //client?.Send("Disconnect|IOException");
                }
                catch(Exception ex)
                {
                    //game.Exit();
                }
            }
        }
    }
}

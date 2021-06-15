using DontBiteMeBoss.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontBiteMeBoss.ClientSide
{
    public class GameMatchClient : GameComponent
    {
        public CollisionManager colMgr = new CollisionManager();
        public GameManager gameMgr = new GameManager();
        private DontBiteMeBossClient game;
        Player thisPlayer;
        WaveManager waveMgr;
        public bool started = false;
        Texture2D bgrTexture;
        public GameMatchClient() : base(DontBiteMeBossClient.Get)
        {
            game = DontBiteMeBossClient.Get;
            waveMgr = new WaveManager(game.Content.Load<Texture2D>("assets/sprites/Zombie"));
            bgrTexture = game.Content.Load<Texture2D>("assets/sprites/Background");
            Player player = new ClientPlayer(true);
            gameMgr.AddObject(player);
        }
        public override void Update(GameTime gameTime)
        {
            gameMgr.Update(gameTime.ElapsedGameTime.TotalSeconds);
            colMgr.CheckAllCollisions();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (started)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(bgrTexture, Vector2.Zero, null, bgrTexture.Bounds);
                gameMgr.Draw(spriteBatch);
                spriteBatch.End();
            }
        }
    }
}

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
        public WaveManager waveMgr = new WaveManager();
        private DontBiteMeBossClient game;
        Player thisPlayer;
        public bool started = false;
        public bool isClient;
        Texture2D bgrTexture;
        Texture2D zombieTexture;
        Texture2D bulletTexture;
        Texture2D playerTexture;
        Texture2D playerDeadTexture;
        public string leadersUUID;

        public GameMatchClient(bool isClient) : base(DontBiteMeBossClient.Get)
        {
            this.isClient = isClient;
            if(isClient)
            {
                game = DontBiteMeBossClient.Get;
                bgrTexture = game.Content.Load<Texture2D>("assets/sprites/Background");
                zombieTexture = game.Content.Load<Texture2D>("assets/sprites/Zombie");
                bulletTexture = game.Content.Load<Texture2D>("assets/sprites/BulletSprite");
                playerTexture = game.Content.Load<Texture2D>("assets/sprites/playerSprite");
                playerDeadTexture = game.Content.Load<Texture2D>("assets/sprites/playerDeadSprite");
                thisPlayer = new ClientPlayer(true);
                thisPlayer.texture = playerTexture;
                (thisPlayer as ClientPlayer).OnShoot += Shoot;
                thisPlayer.UUID = game.thisClient?.UUID;
                thisPlayer.Position = new Vector2(640f, 360f);
                gameMgr.AddObject(thisPlayer);
            }
        }

        public void Start()
        {
            started = true;
            if (thisPlayer.UUID == leadersUUID)
            {
                waveMgr = new WaveManager(true, game.thisClient, zombieTexture);
                gameMgr.AddObject(waveMgr);
            }
        }
        public override void Update(GameTime gameTime)
        {
            if(started)
            {
                gameMgr.Update(gameTime.ElapsedGameTime.TotalSeconds);
                colMgr.CheckAllCollisions();
            }
        }

        public void PlayerDied(string playerUUID)
        {
            Player player = gameMgr.gameObjects.Find((obj) => obj.UUID == playerUUID) as Player;
            player.CurrentHP = 0;
            player.IsAlive = false;
            player.texture = playerDeadTexture;
            List<GameObject> zombies = gameMgr.gameObjects.FindAll((obj) => obj.GetType().Name == "Zombie");
            foreach(Zombie zombie in zombies)
            {
                if(zombie.target.UUID == playerUUID)
                {
                    zombie.GetTarget();
                }
            }
        }

        public void ShootClient(float posX, float posY, float rotation)
        {
            Bullet newBullet = new Bullet(new Vector2(posX, posY), rotation, bulletTexture);
            gameMgr.AddObject(newBullet);
        }
        public void AddPlayer(string UUID, float posX, float posY)
        {
            if(UUID != thisPlayer.UUID)
            {
                Player otherPlayer = new Player(playerTexture);
                otherPlayer.Position = new Vector2(posX, posY);
                otherPlayer.UUID = UUID;
                gameMgr.AddObject(otherPlayer);
            }
        }
        public void ZombieMoved(string UUID, float posX, float posY, float rotation)
        {
            Zombie zombie = gameMgr.gameObjects.Find((obj) => obj.UUID == UUID) as Zombie;
            zombie.Position = new Vector2(posX, posY);
            zombie.rotation = rotation;
        }

        public void PlayerMove(string UUID, float posX, float posY, float rotation)
        {
            Player player = gameMgr.gameObjects.Find((obj) => obj.UUID == UUID) as Player;
            player.Position = new Vector2(posX, posY);
            player.rotation = rotation;
        }
        public void SpawnZombie(string zombieUUID, Vector2 position, float maxHP, float moveSpeed, float damage)
        {
            if(leadersUUID != thisPlayer.UUID)
            {
                Zombie zombie = new Zombie(position, maxHP, moveSpeed, damage, zombieTexture);
                zombie.UUID = zombieUUID;
                gameMgr.AddObject(zombie);
            }
        }

        public void Shoot()
        {
            ClientCommand.SendShoot(thisPlayer.Position, thisPlayer.rotation);
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

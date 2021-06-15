using DontBiteMeBoss.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontBiteMeBoss.ClientSide
{
    public class ClientPlayer : Player
    {
        public bool isMainPlayer;
        public Texture2D texture;
        GunComponent gunC;
        private bool shotButtonLastPressed = false;
        Texture2D bulletTexture;
        public ClientPlayer(bool isMainPlayer)
        {
            this.isMainPlayer = isMainPlayer;
            texture = DontBiteMeBossClient.Get.Content.Load<Texture2D>("assets/sprites/playerSprite");
            bulletTexture = DontBiteMeBossClient.Get.Content.Load<Texture2D>("assets/sprites/BulletSprite");
            gunC = new GunComponent();
            AddComponent(gunC);
        }
        public override void Update(double deltaTime)
        {
            if(isMainPlayer)
            {
                Vector2 moveDir = Vector2.Zero;
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                    moveDir.Y -= (float)(moveSpeed * deltaTime);
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    moveDir.X -= (float)(moveSpeed * deltaTime);
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                    moveDir.Y += (float)(moveSpeed * deltaTime);
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    moveDir.X += (float)(moveSpeed * deltaTime);
                Point mousePos = Mouse.GetState().Position;
                rotation = (float)Math.Atan2(mousePos.Y - Position.Y, mousePos.X - Position.X);
                base.Move(moveDir);
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    if (!shotButtonLastPressed)
                    {
                        Bullet newBullet = new Bullet(Position, rotation, bulletTexture);
                        gunC.Shoot(newBullet);
                        shotButtonLastPressed = true;
                    }
                }
                else if (Mouse.GetState().LeftButton == ButtonState.Released)
                {
                    shotButtonLastPressed = false;
                }

            }
            base.Update(deltaTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(spriteBatch != null)
            {
                spriteBatch.Draw(texture, Position, null, texture.Bounds, new Vector2(texture.Bounds.Width / 2, texture.Bounds.Height / 2), rotation);
                base.Draw(spriteBatch);
            }
        }
    }
}

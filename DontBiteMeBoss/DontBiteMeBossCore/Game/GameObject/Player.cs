using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontBiteMeBoss.Core
{
    public class Player : GameObject
    {
        public string playerUUID;
        public float moveSpeed = 150;
        public float MaxHP = 500;
        public float CurrentHP;
        public float LifeRegen = 10;
        public float MaxLyingTime = 20;
        public float MaxRespawnTime = 5;
        public float lyingTime = 0;
        public float respingTime = 0;
        public bool IsOnFloor = false;
        List<Player> otherPlayers = new List<Player>();
        Texture2D texture;
        public Player()
        {
            this.CurrentHP = MaxHP;
        }
        public Player(Texture2D texture)
        {
            this.CurrentHP = MaxHP;
            this.texture = texture;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (spriteBatch != null)
            {
                if (texture != null)
                    spriteBatch.Draw(texture, Position, null, texture.Bounds, new Vector2(texture.Bounds.Width / 2, texture.Bounds.Height / 2), rotation);
                base.Draw(spriteBatch);
            }
        }
        public override void Update(double deltaTime)
        {
            if (IsOnFloor)
            {
                lyingTime += (float)deltaTime;

            }
            base.Update(deltaTime);
        }

        public void Move(Vector2 movement)
        {
            this.Position += movement;
        }

        public void SetPosition(Vector2 position)
        {
            this.Position = position;
        }

        public void TakeDamage(float damage)
        {
            CurrentHP -= damage;
            if (CurrentHP <= 0)
            {
                LieOnGround();
            }
        }

        private void OtherPlayersInHealingRange()
        {

        }

        public virtual void LieOnGround()
        {
            lyingTime = MaxLyingTime;
            IsOnFloor = true;
        }
    }
}

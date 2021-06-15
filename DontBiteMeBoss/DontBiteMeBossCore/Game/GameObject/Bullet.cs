using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontBiteMeBoss.Core
{
    public class Bullet : GameObject
    {
        public Vector2 directionNormalised;
        public float speed = 350;
        public float maxLifetime = 10, totalLifeTime = 0;
        public Texture2D texture;
        public Bullet(Vector2 position, float rotation)
        {
            Position = position;
            this.rotation = rotation;
            Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            direction.Normalize();
            directionNormalised = direction;
        }

        public Bullet(Vector2 position, float rotation, Texture2D texture)
        {
            Position = position;
            this.rotation = rotation;
            Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            direction.Normalize();
            directionNormalised = direction;
            this.texture = texture;
        }

        public override void Update(double deltaTime)
        {
            Position += directionNormalised * speed * (float)deltaTime;
            totalLifeTime += (float)deltaTime;
            if (totalLifeTime >= maxLifetime)
                GameManager.Get.RemoveGameObject(this);
            base.Update(deltaTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(spriteBatch != null)
            {
                spriteBatch.Draw(texture, Position, null, texture.Bounds, new Vector2(texture.Bounds.X / 2, texture.Bounds.Y / 2), rotation);
                base.Draw(spriteBatch);
            }
        }
    }
}

using Microsoft.Xna.Framework;
using System;

namespace DontBiteMeBoss.Core
{
    public class Collider : Component
    {
        public string Layer;
        public bool isSolid;
        public Vector2 Position;
        public Vector2 Size;
        public Vector2 Offset;
        public event EventHandler OnCollision;
        public Collider(string LayerName,bool isSolid, Vector2 Size, Vector2 Offset)
        {
            this.Layer = LayerName;
            this.isSolid = isSolid;
            this.Position = parent.Position;
            this.Size = Size;
            this.Offset = Offset;
        }
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(new Point((int)(Position.X + Size.X * Offset.X),(int)(Position.Y + Size.Y * Offset.Y)), new Point((int)Size.X, (int)Size.Y));
            }
        }

        public override void Update(double deltaTime)
        {
            this.Position = parent.Position;
        }
    }
}
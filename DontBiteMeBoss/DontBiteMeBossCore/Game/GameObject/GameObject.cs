using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontBiteMeBoss.Core
{
    public class GameObject
    {
        public string UUID;
        public Vector2 Position = Vector2.Zero;
        public Vector2 Size = Vector2.One;
        public float rotation = 0f;
        public List<Component> Components = new List<Component>();
        public virtual void Update(double deltaTime)
        {
            foreach(Component comp in Components)
            {
                comp.Update(deltaTime);
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch) { }
        public T AddComponent<T>(T component) where T : Component
        {
            component.parent = this;
            Components.Add(component);
            return component;
        }
        public void RemoveComponent<T>(T component) where T : Component
        {
            Components.Remove(component);
        }
        public void Destroy()
        {
            GameManager.Get.RemoveGameObject(this);
        }
    }
}

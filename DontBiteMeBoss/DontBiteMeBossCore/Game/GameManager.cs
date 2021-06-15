using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontBiteMeBoss.Core
{
    public class GameManager
    {
        public List<GameObject> gameObjects = new List<GameObject>();

        public static GameManager Get;
        public GameManager()
        {
            if (Get == null)
                Get = this;
        }

        public void Update(double deltaTime)
        {
            for(int i = 0; i < gameObjects.Count; ++i)
            {
                gameObjects[i].Update(deltaTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObject obj in gameObjects)
            {
                obj.Draw(spriteBatch);
            }
        }

        public T AddObject<T>(T gameObject) where T : GameObject
        {
            gameObjects.Add(gameObject);
            return gameObject;
        }
        public void RemoveGameObject<T>(T gameObject) where T : GameObject
        {
            gameObjects.Remove(gameObject);
        }
    }
}

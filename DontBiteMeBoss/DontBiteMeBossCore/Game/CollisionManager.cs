using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontBiteMeBoss.Core
{
    public class CollisionManager
    {
        Dictionary<string, List<Collider>> CollisionLayers;

        public static CollisionManager Instance;

        public CollisionManager()
        {
            if (Instance == null)
                Instance = this;
        }
        string[] Layers
        {
            get { return CollisionLayers.Keys.ToArray(); }
        }

        List<(string, string)> CollisionRules = new List<(string, string)>();

        public void AddLayer(string LayerName)
        {
            if (!CollisionLayers.ContainsKey(LayerName))
                CollisionLayers.Add(LayerName, new List<Collider>());
        }

        public void RemoveLayer(string LayerName)
        {
            if (CollisionLayers.ContainsKey(LayerName))
                CollisionLayers.Remove(LayerName);
        }

        public Collider CreateCollider(GameObject parent, string LayerName, bool isSolid, Vector2 Position, Vector2 Size, Vector2 Offset)
        {

            Collider col = new Collider(LayerName, isSolid, Size, Offset);
            parent.AddComponent(col);
            if (CollisionLayers.ContainsKey(LayerName))
                CollisionLayers[LayerName].Add(col);
            else
                throw new Exception($"Couldn't add collider to layer {LayerName}, layer does not exist.");
            return col;
        }

        public bool CheckCollision(Collider col1, Collider col2)
        {
            Rectangle rect1 = col1.Bounds;
            Rectangle rect2 = col2.Bounds;
            return rect1.X < rect2.X + rect2.Width
                && rect1.X + rect1.Width > rect2.X
                && rect1.Y < rect2.Y + rect2.Height
                && rect1.Y + rect1.Height > rect2.Y;
        }

        public void CheckAllCollisions()
        {
            foreach((string,string) Rule in CollisionRules)
            {
                foreach(Collider col1 in CollisionLayers[Rule.Item1])
                {
                    foreach(Collider col2 in CollisionLayers[Rule.Item2])
                    {
                        if (CheckCollision(col1, col2))
                        {
                            //col1.OnCollision(col2,null);
                        }
                    }
                }
            }
        }

        public void AddRule(string Layer1, string Layer2)
        {
            CollisionRules.Add((Layer1, Layer2));
        }

        public void RemoveRule(string Layer1, string Layer2)
        {
            CollisionRules.Remove((Layer1, Layer2));
        }
    }
}

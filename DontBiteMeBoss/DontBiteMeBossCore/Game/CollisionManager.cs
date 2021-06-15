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
        Dictionary<string, List<Collider>> CollisionLayers = new Dictionary<string, List<Collider>>();

        public static CollisionManager Instance;
        public delegate void OnCollisionDelegate(string uuid1, string uuid2);
        public event OnCollisionDelegate OnCollision = delegate { };
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
                for(int i = 0; i < CollisionLayers[Rule.Item1].Count; ++i)
                {
                    Collider col1 = CollisionLayers[Rule.Item1][i];
                    for (int j = 0; j < CollisionLayers[Rule.Item2].Count; ++j)
                    {
                        Collider col2 = CollisionLayers[Rule.Item2][j];
                        if (CheckCollision(col1, col2))
                        {
                            OnCollision.Invoke(col1.parent.UUID, col2.parent.UUID);
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

        public void RemoveColliderFromLayer(string layer, Collider col)
        {
            CollisionLayers[layer].Remove(col);
        }
    }
}

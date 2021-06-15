using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontBiteMeBoss.Core
{
    public abstract class Component
    {
        public GameObject parent;
        public Component()
        {
            Start();
        }
        public virtual void Update(double deltaTime) { }
        public virtual void Start() { }
    }
}

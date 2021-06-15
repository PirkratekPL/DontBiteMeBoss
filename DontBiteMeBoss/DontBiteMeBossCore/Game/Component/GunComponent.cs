using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontBiteMeBoss.Core
{
    public class GunComponent : Component
    {
        public GunComponent()
        {
        }
        public void Shoot(Bullet newBullet)
        {
            newBullet.totalLifeTime = 0;
            newBullet.Position = parent.Position;
            GameManager.Get.AddObject(newBullet);
        }
    }
}

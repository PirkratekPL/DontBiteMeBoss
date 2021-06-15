using DontBiteMeBoss.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DontBiteMeBoss.Server
{
    public class GameMatchServer
    {
        private DateTime time = DateTime.Now;
        GameManager gameMgr;
        CollisionManager colMgr;
        WaveManager waveMgr;

        public GameMatchServer()
        {
            gameMgr = new GameManager();
            colMgr = new CollisionManager();
            colMgr.AddLayer("Zombie");
            colMgr.AddLayer("Bullet");
            colMgr.AddRule("Zombie", "Bullet");
            waveMgr = new WaveManager(true);
        }
        public void Update()
        {
            double deltaTime = (DateTime.Now - time).TotalSeconds;
            gameMgr.Update(deltaTime);
            colMgr.CheckAllCollisions();
            waveMgr.Update(deltaTime);
        }
    }
}

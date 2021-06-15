using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontBiteMeBoss.Core
{
    public class WaveManager : GameObject
    {
        public int CurrentWave;
        public int ZombiesKilled;

        private float baseZombieHP = 100;
        private float baseZombieSpeed = 100;
        private float baseZombieDamage = 100;
        private int baseZombieSpawnLimit = 5;
        private int CurrentWaveMaxZombies = 0;
        private bool waveStarted = false;
        private Random random = new Random();
        public Texture2D zombieTexture;
        public Client client;

        private float delayBetweenWaves = 5;
        private float timeBetweenWaves = 0;

        public static WaveManager Get;
        public bool isServer = false;
        public WaveManager()
        {
            if (Get == null)
                Get = this;
            CurrentWave = 1;
            ZombiesKilled = 0;
        }
        public WaveManager(bool isServer)
        {
            if (Get == null)
                Get = this;
            CurrentWave = 1;
            ZombiesKilled = 0;
            this.isServer = isServer;
        }
        public WaveManager(bool isServer, Client client, Texture2D zombieTexture)
        {
            if (Get == null)
                Get = this;
            CurrentWave = 1;
            ZombiesKilled = 0;
            this.client = client;
            this.zombieTexture = zombieTexture;
            this.isServer = isServer;
        }

        public void SpawnZombie()
        {
            float multiplier = GetZombiesStatsMultiplier();
            Zombie zombie = new Zombie(GetRandomisedSpawnPosition(), baseZombieHP * multiplier, baseZombieSpeed * multiplier, baseZombieDamage * multiplier, zombieTexture);
            zombie.UUID = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);

            client.Send($"SpawnZombie|{client.UUID}|{zombie.UUID}|{zombie.Position.X}|{zombie.Position.Y}|{zombie.MaxHP}|{zombie.MoveSpeed}|{zombie.Damage}");
        }


        public override void Update(double deltaTime)
        {
            if (isServer)
            {
                if (waveStarted)
                {
                    if (ZombiesKilled == CurrentWaveMaxZombies)
                        EndWave();
                }
                else
                {
                    if (timeBetweenWaves < delayBetweenWaves)
                        timeBetweenWaves += (float)deltaTime;
                    else
                        StartWave();
                }
                base.Update(deltaTime);
            }
        }

        private Vector2 GetRandomisedSpawnPosition()
        {
            Vector2 position = Vector2.Zero;
            int choice = random.Next(0, 100) % 2;
            if (choice == 0)
                position.X = (float)random.NextDouble() * 200 - 220;
            else
                position.X = (float)random.NextDouble() * 200 + 1300;
            position.Y = (float)random.NextDouble() * 720;

            return position;
        }

        public int GetMaxZombiesSpawn()
        {
            return baseZombieSpawnLimit + CurrentWave/2;
        }

        private float GetZombiesStatsMultiplier()
        {
            return 1f + CurrentWave * CurrentWave / 80f;
        }

        private void StartWave()
        {
            waveStarted = true;
            ZombiesKilled = 0;
            CurrentWaveMaxZombies = GetMaxZombiesSpawn();
            for (int i = 0; i < CurrentWaveMaxZombies; ++i)
            {
                SpawnZombie();
            }
        }
        private void EndWave()
        {
            waveStarted = false;
            ++CurrentWave;
            timeBetweenWaves = 0;
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontBiteMeBoss.Core
{
    public class Zombie : GameObject
    {
        public float MoveSpeed;
        public float MaxHP;
        public float CurrentHP;
        public float Damage;

        public Player target;
        public Texture2D texture;
        public Collider col;

        private float minimalAttackDistance = 16;
        private float attackDelay = 2;
        private float attackDelayTimer = 2;

        public delegate void ZombieDelegate(Zombie zombie);
        public delegate void ZombieAttackDelegate(Zombie zombie, string playerUUID, float damage);
        public delegate void ZombieMoveDelegate(Zombie zombie, Vector2 Position, float rotation);
        public event ZombieDelegate ZombieDeath = delegate { };
        public event ZombieMoveDelegate ZombieMove = delegate { };
        public event ZombieAttackDelegate ZombieAttack = delegate { };
        public Zombie(Vector2 position, float MaxHP, float moveSpeed, float damage)
        {
            Position = position;
            this.MaxHP = MaxHP;
            this.MoveSpeed = moveSpeed;
            this.Damage = damage;
        }
        public Zombie(Vector2 position, float MaxHP, float moveSpeed, float damage, Texture2D texture)
        {
            Position = position;
            this.MaxHP = MaxHP;
            this.MoveSpeed = moveSpeed;
            this.Damage = damage;
            this.texture = texture;
        }

        public void GetTarget()
        {
            List<GameObject> players = GameManager.Get.gameObjects.FindAll((obj) => obj.GetType().Name == "ClientPlayer" || obj.GetType().Name == "Player");
            players = players.FindAll((player) => ((Player)player).CurrentHP > 0);
            if (players != null)
                target = (Player)players[GameRandom.NextInt(0, players.Count)];
        }

        public void Move(double deltaTime)
        {
            if (target != null)
            {
                Vector2 direction = target.Position - Position;
                direction.Normalize();
                this.ZombieMove.Invoke(this, Position + direction * MoveSpeed * (float)deltaTime, (float)Math.Atan2(direction.Y, direction.X));
            }
        }

        private bool IsCloseEnoughtToAttack()
        {
            if (target != null)
            {
                float distance = Vector2.Distance(Position, target.Position);
                return distance <= minimalAttackDistance;
            }
            return false;
        }

        public void TakeDamage(float damage)
        {
            CurrentHP -= damage;
            if (CurrentHP <= 0)
                Die();
        }

        private void Die()
        {
            ZombieDeath.Invoke(this);
            Destroy();
        }

        private void Attack()
        {
            if (Vector2.DistanceSquared(Position, target.Position) < minimalAttackDistance)
            {
                target.TakeDamage(Damage);
                ZombieAttack.Invoke(this, target.UUID, Damage);
            }
        }

        public override void Update(double deltaTime)
        {
            Move(deltaTime);
            if (target == null)
            {
                GetTarget();
            }
            if (IsCloseEnoughtToAttack())
            {
                if (attackDelayTimer >= attackDelay)
                {
                    attackDelayTimer = 0;
                    Attack();
                }
            }
            attackDelayTimer += (float)deltaTime;
            base.Update(deltaTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (spriteBatch != null)
            {
                spriteBatch.Draw(texture, Position, null, texture.Bounds, new Vector2(texture.Width / 2f, texture.Height / 2f), rotation);
                base.Draw(spriteBatch);
            }
        }
    }
}

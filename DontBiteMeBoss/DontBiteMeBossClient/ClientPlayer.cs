using DontBiteMeBoss.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontBiteMeBoss.ClientSide
{
    public class ClientPlayer : Player
    {
        public bool isMainPlayer;
        GunComponent gunC;
        private bool shotButtonLastPressed = false;
        public ClientPlayer(bool isMainPlayer)
        {
            this.isMainPlayer = isMainPlayer;
            gunC = new GunComponent();
            AddComponent(gunC);
        }
        public delegate void OnShootDelegate();
        public event OnShootDelegate OnShoot = delegate { };
        public override void Update(double deltaTime)
        {
            if (isMainPlayer && IsAlive)
            {
                Vector2 moveDir = Vector2.Zero;
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                    moveDir.Y -= (float)(moveSpeed * deltaTime);
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    moveDir.X -= (float)(moveSpeed * deltaTime);
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                    moveDir.Y += (float)(moveSpeed * deltaTime);
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    moveDir.X += (float)(moveSpeed * deltaTime);
                Point mousePos = Mouse.GetState().Position;
                float lastRotation = rotation;
                rotation = (float)Math.Atan2(mousePos.Y - Position.Y, mousePos.X - Position.X);
                bool PositionRotationChanged = moveDir != Vector2.Zero || lastRotation != rotation;
                base.Move(moveDir);
                if (PositionRotationChanged)
                    ClientCommand.thisClient.Send($"Move|{ClientCommand.thisClient.UUID}|{Position.X}|{Position.Y}|{rotation}");
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    if (!shotButtonLastPressed && DontBiteMeBossClient.Get.IsActive)
                    {
                        OnShoot.Invoke();
                        shotButtonLastPressed = true;
                    }
                }
                else if (Mouse.GetState().LeftButton == ButtonState.Released)
                {
                    shotButtonLastPressed = false;
                }
            }
            base.Update(deltaTime);
        }
    }
}

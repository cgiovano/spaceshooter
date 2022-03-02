using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.GameObjects
{
    class Weakpoint : GameObject
    {
        public Weakpoint(Texture2D texture, int energy)
        {
            spriteTexture = texture;
            frameHeight = texture.Height;
            frameWidth = texture.Width;
            Energy = energy;
        }

        public Weakpoint()
        {
        }

        public virtual void Update(Vector2 targetPosition)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);

            CheckLaserDamage();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTexture, position, defaultColor);
        }

        protected void CheckLaserDamage()
        {
            foreach (var l in Components.Lasers)
            {
                var laser = l as Laser;

                if (rectangle.Intersects(laser.Rectangle) && laser.LaserType == LaserType.Player)
                {
                    Energy -= laser.Energy;

                    if (laser.Mode != LaserMode.Disruptor)
                        laser.IsRemoved = true;

                    isHitted = true;
                }
            }
        }
    }
}

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
    public class Shield : GameObject
    {
        private int _hurtColorDuration;

        public Shield() : base()
        {
            spriteTexture = Textures.Shield;
            Energy = 100;
        }

        public override void Update(GameTime gameTime)
        {
            position = new Vector2(Components.Player.Origin.X - spriteTexture.Width / 2, Components.Player.Position.Y - spriteTexture.Height);
            rectangle = new Rectangle((int)position.X, (int)position.Y, spriteTexture.Width, spriteTexture.Height);

            _hurtColorDuration += gameTime.ElapsedGameTime.Milliseconds;

            CheckEnemyLaserDamage();

            if (Energy <= 0 || Components.Player.IsDestroyed)
            {
                IsDestroyed = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (isHitted == true && _hurtColorDuration < 150)
            {
                spriteBatch.Draw(spriteTexture, position, damageColor);
            }
            else
            {
                spriteBatch.Draw(spriteTexture, position, defaultColor);
                isHitted = false;
                _hurtColorDuration = 0;
            }
            
        }

        public void CheckEnemyLaserDamage()
        {
            foreach (var l in Components.Lasers)
            {
                var laser = l as Laser;

                if (rectangle.Intersects(laser.Rectangle) && laser.LaserType == LaserType.Enemy)
                {
                    if (laser.SpriteTexture == Textures.Boss_Laser_Texture)
                    {
                        if (_hurtColorDuration > 150)
                            Energy -= laser.Energy;

                        laser.IsRemoved = false;
                    }
                    else
                    {
                        Energy -= laser.Energy;
                        laser.IsRemoved = true;
                    }

                    isHitted = true;
                }
            }
        }

    }
}

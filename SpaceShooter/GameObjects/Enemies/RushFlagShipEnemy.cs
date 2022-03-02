using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using System.Collections.Generic;

namespace SpaceShooter.GameObjects.Enemies
{
    class RushFlagShipEnemy : RushEnemyBase
    {
        private Color _laserColor = new Color(0, 255, 100);
        private int _shootInterval;
        private int _shootCounter;
        private int _shootTimer = 0;
        private int _hurtColorDuration = 0;

        public RushFlagShipEnemy() : base()
        {
            spriteTexture = Textures.Rush_Enemy_B_Texture;
            frameWidth = 22;
            frameHeight = 22;
            Energy = 25;
            position = new Vector2(RandomMod.GetRandom(0, (int)(Screen.GetWidth - frameWidth)), -(int)(frameHeight));
            origin = new Vector2(frameWidth / 2, frameHeight / 2);
            Speed = .05f;
            _shootInterval = 30;
        }

        public override void Update(GameTime gameTime)
        {
            _hurtColorDuration += gameTime.ElapsedGameTime.Milliseconds;
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);

            _shootTimer += (int)gameTime.ElapsedGameTime.Milliseconds;

            _shootCounter++;

            //Shoot();

            if (_shootTimer > 2000 && Components.Player.IsDestroyed == false)
            {
                Shoot();
            }
            
            if (_shootTimer > 3000)
            {
                _shootTimer = 0;
            }

            position.Y += Speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (rectangle.Top > Screen.GetHeight)
                IsRemoved = true;

            CheckLaserDamage();

            if (Energy <= 0)
                IsDestroyed = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isHitted == true && _hurtColorDuration < 150)
                spriteBatch.Draw(spriteTexture, position, damageColor);
            else
            {
                isHitted = false;
                _hurtColorDuration = 0;
                spriteBatch.Draw(spriteTexture, position, defaultColor);
            }
        }

        private void Shoot()
        {
            if (_shootCounter > _shootInterval)
            {
                Components.Lasers.Add(new EnemyLaser_B(new Vector2(Position.X + frameWidth / 2, Position.Y + frameHeight), Color.Yellow, 10, Direction.Right));
                Components.Lasers.Add(new EnemyLaser_B(new Vector2(Position.X + frameWidth / 2, Position.Y + frameHeight), Color.Yellow, 10, Direction.Center));
                Components.Lasers.Add(new EnemyLaser_B(new Vector2(Position.X + frameWidth / 2, Position.Y + frameHeight), Color.Yellow, 10, Direction.Left));
                Sounds.EnemyShot.Play();
                _shootCounter = 0;
            }
        }
    }
}

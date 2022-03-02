using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using SpaceShooter.Library.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.GameObjects.Enemies
{
    class RushDragonflyEnemy : RushEnemyBase
    {
        private Direction _direction;
        private Color _laserColor = new Color(0, 255, 100);
        private int _shootInterval;
        private int _shootCounter;
        private float _attackCounter = 0;
        private float _friction = 5f;
        private Animation _animation;
        private int _hurtColorDuration = 0;
        private Direction _targetSide;

        public RushDragonflyEnemy() : base()
        {
            spriteTexture = Textures.Rush_Enemy_C_Texture;
            frameWidth = 30;
            frameHeight = 24;
            Energy = 20;
            
            Speed = 1f;
            _shootInterval = 20;
            _animation = new Animation(5, frameWidth, frameHeight, 2, 0.1f);
            _direction = Direction.Center;

            if (Components.Player.Position.X < Screen.GetWidth / 2)
            {
                position = new Vector2(RandomMod.GetRandom(Screen.GetWidth / 2 - frameWidth, Screen.GetWidth - frameWidth), -frameHeight);
                _targetSide = Direction.Left;
            }
            else
            {
                position = new Vector2(RandomMod.GetRandom(0, Screen.GetWidth / 2 - frameWidth), -frameHeight);
                _targetSide = Direction.Right;
            }       
        }

        public override void Update(GameTime gameTime)
        {
            _hurtColorDuration += gameTime.ElapsedGameTime.Milliseconds;
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);

            _attackCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;

            _shootCounter++;

            if (position.Y > Screen.GetHeight / 5)
            {
                if (Components.Player.IsDestroyed == false)
                {
                    Shoot();
                }

                if (_targetSide == Direction.Right)
                {
                    position.X += 1.5f * _attackCounter;
                    _direction = Direction.Right;
                }
                else if (_targetSide == Direction.Left)
                {
                    position.X -= 1.5f * _attackCounter;
                    _direction = Direction.Left;
                }
                else
                {
                    _direction = Direction.Center;
                }
            }

            position.Y += Speed * 2;

            _animation.Update(gameTime, _direction);

            if (rectangle.Top > Screen.GetHeight || rectangle.Right < 0 || rectangle.Left > Screen.GetWidth)
                IsRemoved = true;

            CheckLaserDamage();

            if (Energy <= 0)
                IsDestroyed = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, Position, source, defaultColor, 0.0f, origin, Scale, SpriteEffects.None, 0.0f);
            if (isHitted == true && _hurtColorDuration < 150)
                _animation.Draw(spriteBatch, spriteTexture, position, damageColor, scale);
            else
            {
                isHitted = false;
                _hurtColorDuration = 0;
                _animation.Draw(spriteBatch, spriteTexture, position, defaultColor, scale);
            }
        }

        private void Shoot()
        {
            if (_shootCounter > _shootInterval)
            {
                Components.Lasers.Add(new EnemyLaser_A(new Vector2(Position.X + frameWidth / 2, Position.Y + frameHeight), Color.CadetBlue, 10));
                Sounds.EnemyShot.Play();
                _shootCounter = 0;
            }
        }
    }
}

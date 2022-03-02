using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using SpaceShooter.Library.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.GameObjects.Enemies
{
    class FinalBoss_Stage3 : GameObject
    {
        private int _hurtColorDuration;
        private int _shootTimer;
        private int _shootCounter;
        private int _shootInterval = 8;
        private System.Random random = new System.Random();

        private Vector2 _direction;
        private Vector2 _targetPosition;
        private int _rotateDirection = 0;
        private float _rotationAngle;
        private double contador2;
        int i = 0;

        private int _directionX = 1;
        private int _directionY = 1;
        private bool _isOnScreen = false;
        private Animation animation;

        public FinalBoss_Stage3() : base()
        {
            spriteTexture = Textures.FinalBossStage3_Texture;
            frameWidth = spriteTexture.Width / 3;
            frameHeight = spriteTexture.Height;
            position = new Vector2(random.Next(0, Screen.GetWidth - frameWidth), -frameHeight);
            Speed = 3f;
            Energy = 500;

            animation = new Animation(2, frameWidth, frameHeight);

            position = new Vector2(Library.RandomMod.GetRandom(0, Screen.GetWidth - frameWidth), -frameHeight);

            if (Components.Player.Position.X <= Screen.GetWidth / 2)
            {
                _directionX = -_directionX;
            }
        }

        private Vector2 NewRandomPosition()
        {
            return (new Vector2(random.Next(0, Screen.GetWidth - frameWidth), random.Next(0, Screen.GetHeight - frameHeight)));
        }


        public override void Update(GameTime gameTime)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
            _hurtColorDuration += gameTime.ElapsedGameTime.Milliseconds;
            contador2 += gameTime.ElapsedGameTime.Seconds;
            _shootTimer += (int)gameTime.ElapsedGameTime.Milliseconds;
            _shootCounter++;

            animation.Update(gameTime);

            if (_shootTimer > 500 && Components.Player.IsDestroyed == false)
            {
                Shoot();
            }

            if (_shootTimer > 1500)
            {
                _shootTimer = 0;
            }

            if (position.Y > 0 && position.Y < Screen.GetHeight && position.X > 0 && position.X < Screen.GetWidth && _isOnScreen == false)
            {
                _isOnScreen = true;
            }

            if (_isOnScreen)
            {
                if (rectangle.Right >= Screen.GetWidth)
                {
                    _directionX = -_directionX;
                }
                if (rectangle.Left <= 0)
                {
                    _directionX = -_directionX;
                }
                if (rectangle.Top <= 0)
                {
                    _directionY = -_directionY;
                }
                if (rectangle.Bottom >= Screen.GetHeight)
                {
                    _directionY = -_directionY;
                }
            }

            position.X += Speed * _directionX;
            position.Y += Speed * _directionY;

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

            if (Energy <= 0)
                IsDestroyed = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            origin = new Vector2(spriteTexture.Width / 2, spriteTexture.Width / 2);
            //origin = new Vector2(position.X + frameWidth / 2, position.Y + frameHeight / 2);

            if (isHitted == true && _hurtColorDuration < 150)
                animation.Draw(spriteBatch, spriteTexture, position, damageColor, scale);
            else
            {
                isHitted = false;
                _hurtColorDuration = 0;
                //spriteBatch.Draw(spriteTexture, position, null, defaultColor, rotation, origin, scale, SpriteEffects.None, 0f);
                //spriteBatch.Draw(spriteTexture, position, defaultColor);
                animation.Draw(spriteBatch, spriteTexture, position, defaultColor, scale);
            }

            spriteBatch.Draw(Textures.Pointer, _targetPosition, defaultColor);
        }

        private void Shoot()
        {
            if (_shootCounter > _shootInterval)
            {
                Components.Lasers.Add(new EnemyLaser_B(new Vector2(Position.X + 4, Position.Y + frameHeight), Color.Cyan, 10, Direction.Right));
                Components.Lasers.Add(new EnemyLaser_B(new Vector2(Position.X + frameWidth / 2, Position.Y + frameHeight), Color.Cyan, 10, Direction.Center));
                Components.Lasers.Add(new EnemyLaser_B(new Vector2(Position.X + frameWidth - 15, Position.Y + frameHeight), Color.Cyan, 10, Direction.Left));
                Sounds.EnemyShot.Play();
                _shootCounter = 0;
            }
        }

        private float CalculateAngleDirection(Vector2 currPos, Vector2 tgtPos)
        {
            float angle;
            Vector2 difference = currPos - tgtPos;
            difference.Normalize();
            angle = (float)Math.Atan2(difference.Y, -difference.X);

            return (angle);
        }

        private void Idle()
        {
            position.X = (float)(Math.Cos(Math.PI * contador2) * 0.15 + (position.X));
            position.Y = (float)(Math.Sin(Math.PI * contador2) * 0.15 + (position.Y));

            if (_rotateDirection == 1)
            {
                if (rotation > 0.2)
                {
                    _rotateDirection = 0;
                    return;
                }
                rotation += 0.01f;
            }
            else if (_rotateDirection == 0)
            {
                if (rotation < -0.2)
                {
                    _rotateDirection = 1;
                    return;
                }
                rotation -= 0.01f;
            }
        }
    }
}

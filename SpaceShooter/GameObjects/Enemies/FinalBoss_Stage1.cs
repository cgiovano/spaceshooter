using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using SpaceShooter.Library.Utils;
using System;

namespace SpaceShooter.GameObjects.Enemies
{
    class FinalBoss_Stage1 : GameObject
    {
        private int _hurtColorDuration;
        private int _shootTimer;
        private int _shootCounter;
        private int _shootInterval = 2;
        private System.Random random = new System.Random();

        private Vector2 _direction;
        private Vector2 _targetPosition;
        private int _rotateDirection = 0;
        private float _rotationAngle;
        private double contador2;
        int i = 0;

        private Animation animation;

        public FinalBoss_Stage1() : base()
        {
            spriteTexture = Textures.FinalBossStage2_Texture;
            frameWidth = spriteTexture.Width / 8;
            frameHeight = spriteTexture.Height;
            position = new Vector2(random.Next(0, Screen.GetWidth - frameWidth), -frameHeight);
            Speed = 1f;
            Energy = 800;
            _targetPosition = NewRandomPosition();

            animation = new Animation(7, frameWidth, frameHeight, 0.1f);
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

            position = Vector2.Lerp(position, _targetPosition, 0.05f);

            int rotationAngle = 10;

            Idle();


            if (_shootTimer > 1000 && Components.Player.IsDestroyed == false)
            {
                if (i < 36)
                {
                    Shoot(i, rotationAngle);

                    i++;
                }
                else
                {
                    i = 0;
                }
            }

            if (_shootTimer > 3000)
            {
                _shootTimer = 0;
                _targetPosition = NewRandomPosition();
            }

            position.Y += Speed;

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
            {
                //spriteBatch.Draw(spriteTexture, position, null, defaultColor, rotation, origin, scale, SpriteEffects.None, 0f);
                animation.Draw(spriteBatch, spriteTexture, position, damageColor, scale);
            }
            else
            {
                isHitted = false;
                _hurtColorDuration = 0;
                //spriteBatch.Draw(spriteTexture, position, null, defaultColor, rotation, origin, scale, SpriteEffects.None, 0f);
                animation.Draw(spriteBatch, spriteTexture, position, defaultColor, scale);
                //spriteBatch.Draw(spriteTexture, position, defaultColor);
            }

            //aspriteBatch.Draw(Textures.Pointer, _targetPosition, defaultColor);
        }

        private void Shoot(int interator, int rotationAngle)
        {
            if (_shootCounter > _shootInterval)
            {
                float rotation = interator * rotationAngle;
                Components.Lasers.Add(new EnemyLaser_D(Origin, Color.White, 10, rotation, Textures.EnemyLaser_E_Texture, true));
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

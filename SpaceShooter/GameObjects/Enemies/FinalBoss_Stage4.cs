using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.GameObjects.Enemies
{
    class FinalBoss_Stage4 : GameObject
    {
        private int _hurtColorDuration;
        private int _shootTimer;
        private int _shootCounter;
        private int _shootInterval = 4;
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

        Boss_4_eyeWeakpoint _eyeWeakpoint;
        Boss_4_gunWeakpoint _gunWeakpoint_A;
        Boss_4_gunWeakpoint _gunWeakpoint_B;
        private bool _gunsDestroyed;

        public FinalBoss_Stage4() : base()
        {
            spriteTexture = Textures.FinalBossStage4_Texture;
            frameWidth = spriteTexture.Width;
            frameHeight = spriteTexture.Height;
            position = new Vector2(random.Next(0, Screen.GetWidth - frameWidth), -frameHeight);
            Speed = 3f;
            Energy = 500;

            _eyeWeakpoint = new Boss_4_eyeWeakpoint(Textures.FinalBossStage4_EyeWeakpoint_Texture, 500);
            _gunWeakpoint_A = new Boss_4_gunWeakpoint(Textures.FinalBossStage4_GunWeakpoint_Texture, 500, "right", 4);
            _gunWeakpoint_B = new Boss_4_gunWeakpoint(Textures.FinalBossStage4_GunWeakpoint_Texture, 500, "left", 4);

            AddComponents();

            position = new Vector2(Library.RandomMod.GetRandom(0, Screen.GetWidth - frameWidth), -frameHeight);

            if (Components.Player.Position.X <= Screen.GetWidth / 2)
            {
                _directionX = -_directionX;
            }
        }

        public void AddComponents()
        {
            Components.Boss.Add(_gunWeakpoint_A);
            Components.Boss.Add(_gunWeakpoint_B);
            Components.Boss.Add(_eyeWeakpoint);
        }

        private Vector2 NewRandomPosition()
        {
            return (new Vector2(random.Next(frameWidth, Screen.GetWidth - frameWidth), random.Next(0, Screen.GetHeight - frameHeight)));
        }

        public override void Update(GameTime gameTime)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth + 20, frameHeight);
            _hurtColorDuration += gameTime.ElapsedGameTime.Milliseconds;
            contador2 += gameTime.ElapsedGameTime.Seconds;
            _shootCounter++;

            if (_gunWeakpoint_A == null && _gunWeakpoint_B == null)
                _gunsDestroyed = true;

            if (_eyeWeakpoint != null)
            {
                _eyeWeakpoint.Update(gameTime, Origin, _gunsDestroyed);
                //_gunWeakpoint_B.Update(gameTime);

                if (_eyeWeakpoint.IsDestroyed)
                    _eyeWeakpoint = null;
            }
                

            if (_gunWeakpoint_A != null)
            {
                _gunWeakpoint_A.Update(gameTime, new Vector2(rectangle.Right, rectangle.Top));
                //_gunWeakpoint_A.Update(gameTime);

                if (_gunWeakpoint_A.IsDestroyed)
                    _gunWeakpoint_A = null;
            }

            if (_gunWeakpoint_B != null)
            {
                _gunWeakpoint_B.Update(gameTime, new Vector2(rectangle.Left, rectangle.Top));
                //_gunWeakpoint_B.Update(gameTime);


                if (_gunWeakpoint_B.IsDestroyed)
                    _gunWeakpoint_B = null;
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

            position.X += (int)Speed * _directionX;
            position.Y += (int)Speed * _directionY;

            if (_gunWeakpoint_A == null && _gunWeakpoint_B == null && _eyeWeakpoint == null)
                IsDestroyed = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            origin = new Vector2(spriteTexture.Width / 2, spriteTexture.Width / 2);
            //origin = new Vector2(position.X + frameWidth / 2, position.Y + frameHeight / 2);

            if (isHitted == true && _hurtColorDuration < 150)
                spriteBatch.Draw(spriteTexture, position, damageColor);
            else
            {
                isHitted = false;
                _hurtColorDuration = 0;
                //spriteBatch.Draw(spriteTexture, position, null, defaultColor, rotation, origin, scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(spriteTexture, position, defaultColor);
            }

            spriteBatch.Draw(Textures.Pointer, _targetPosition, defaultColor);


             if (_eyeWeakpoint != null)
            {
                _eyeWeakpoint.Draw(spriteBatch);
            }

            /*
            if (_gunWeakpoint_A != null)
            {
                _gunWeakpoint_A.Draw(spriteBatch);
            }

            if (_gunWeakpoint_B != null)
            {
                _gunWeakpoint_B.Draw(spriteBatch);
            }*/
        }
    }
    
    class Boss_4_eyeWeakpoint : Weakpoint
    {
        private int _hurtColorDuration;
        private int _shootCounter;
        private int _shootTimer;

        public Boss_4_eyeWeakpoint(Texture2D texture, int energy) : base()
        {
            spriteTexture = texture;
            frameHeight = texture.Height;
            frameWidth = texture.Width;
            Energy = energy;
        }

        public void Update(GameTime gameTime, Vector2 origin, bool gunsDestroyed)
        {
            _hurtColorDuration += gameTime.ElapsedGameTime.Milliseconds;
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);

            CheckLaserDamage();
            position = new Vector2(origin.X - spriteTexture.Width / 2, origin.Y - spriteTexture.Height / 2);

            _shootTimer += (int)gameTime.ElapsedGameTime.Milliseconds;
            _shootCounter++;

            if (_shootTimer > 500 && gunsDestroyed == true)
            {
                Shoot();
            }

            if (_shootTimer > 1500)
            {
                _shootTimer = 0;
            }

            if (Energy <= 0)
            {
                IsDestroyed = true;
                Explosion.Create(Origin);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isHitted == true && _hurtColorDuration < 150)
                spriteBatch.Draw(spriteTexture, position, damageColor);
            else
            {
                isHitted = false;
                _hurtColorDuration = 0;
                //spriteBatch.Draw(spriteTexture, position, null, defaultColor, rotation, origin, scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(spriteTexture, position, defaultColor);
            }
        }

        public void Shoot()
        {
            if (_shootCounter > 5)
            {
                Components.Lasers.Add(new EnemyLaser_A(new Vector2(Position.X + frameWidth / 2, Position.Y + frameHeight), Color.OrangeRed, 10));
                Sounds.EnemyShot.Play();
                _shootCounter = 0;

                //_direction = -_direction;
            }
        }
    }

    class Boss_4_gunWeakpoint : Weakpoint
    {
        string _side;
        private int _shootCounter = 0;
        private int _shootInterval;
        private int _shootTimer;
        int _direction = 1;
        private int _hurtColorDuration;

        public Boss_4_gunWeakpoint(Texture2D texture, int energy, string side, int shootInterval) : base()
        {
            spriteTexture = texture;
            frameHeight = texture.Height;
            frameWidth = texture.Width;
            Energy = energy;
            _shootInterval = shootInterval;
            _side = side;
        }

        public void Update(GameTime gameTime, Vector2 pos)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
            _hurtColorDuration += gameTime.ElapsedGameTime.Milliseconds;
            CheckLaserDamage();

            _shootTimer += (int)gameTime.ElapsedGameTime.Milliseconds;
            _shootCounter++;

            if (_side == "right")
                position = new Vector2(pos.X - 15, pos.Y + spriteTexture.Height / 4);
            else if (_side == "left")
                position = new Vector2(pos.X - (frameWidth + 5), pos.Y + spriteTexture.Height / 4);

            if (_shootTimer > 500 && Components.Player.IsDestroyed == false)
            {
                Shoot();
            }

            if (_shootTimer > 1500)
            {
                _shootTimer = 0;
            }

            if (Energy <= 0)
            {
                IsDestroyed = true;
            }

            position.Y += 3 * _direction;

            CheckLaserDamage();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isHitted == true && _hurtColorDuration < 150)
                spriteBatch.Draw(spriteTexture, position, damageColor);
            else
            {
                isHitted = false;
                _hurtColorDuration = 0;
                //spriteBatch.Draw(spriteTexture, position, null, defaultColor, rotation, origin, scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(spriteTexture, position, defaultColor);
            }
        }

        public void Shoot()
        {
            if (_shootCounter > 10)
            {
                Components.Lasers.Add(new EnemyLaser_B(new Vector2(Position.X + frameWidth / 2, Position.Y + frameHeight), Color.LightYellow, 10, Direction.Center));
                Sounds.EnemyShot.Play();
                _shootCounter = 0;

                _direction = -_direction;
            }
        }
    }
}

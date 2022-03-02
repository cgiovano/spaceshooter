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
    class FinalBoss_Stage5 : GameObject
    {
        private int _hurtColorDuration;
        private int _shootTimer;
        private int _shootCounter;
        private int _shootInterval = 10;
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

        Boss_5_brainWeakpoint _brainWeakpoint;
        Boss_5_handWeakpoint _handWeakpoint_A;
        Boss_5_handWeakpoint _handWeakpoint_B;
        Boss_5_mouthWeakpoint _mouthWeakpoint;
        bool _canBrainReceiveDamage = false;

        private bool _gunsDestroyed;

        public FinalBoss_Stage5() : base()
        {
            spriteTexture = Textures.FinalBossStage5_Texture;
            frameWidth = spriteTexture.Width;
            frameHeight = spriteTexture.Height;
            position = new Vector2(random.Next(frameWidth, Screen.GetWidth - frameWidth), -frameHeight);
            Speed = 3f;
            Energy = 500;

            _brainWeakpoint = new Boss_5_brainWeakpoint(Textures.FinalBossStage5_BrainWeakpoint_Texture, 1000);
            _handWeakpoint_A = new Boss_5_handWeakpoint(Textures.FinalBossStage5_HandWeakpoint_Texture, 700, "right", 4);
            _handWeakpoint_B = new Boss_5_handWeakpoint(Textures.FinalBossStage5_HandWeakpoint_Texture, 700, "left", 4);
            _mouthWeakpoint = new Boss_5_mouthWeakpoint(Textures.FinalBossStage5_MouthWeakpoint_Texture, 1000);

            AddComponents();

            position = new Vector2(Library.RandomMod.GetRandom(0, Screen.GetWidth - frameWidth), -frameHeight);

            if (Components.Player.Position.X <= Screen.GetWidth / 2)
            {
                _directionX = -_directionX;
            }
        }

        public void AddComponents()
        {
            Components.Boss.Add(_handWeakpoint_A);
            Components.Boss.Add(_handWeakpoint_B);
            Components.Boss.Add(_brainWeakpoint);
            Components.Boss.Add(_mouthWeakpoint);
        }

        private Vector2 NewRandomPosition()
        {
            return (new Vector2(random.Next(frameWidth + Textures.FinalBossStage5_HandWeakpoint_Texture.Width, Screen.GetWidth - (frameWidth + Textures.FinalBossStage5_HandWeakpoint_Texture.Width)), random.Next(0, Screen.GetHeight - frameHeight - 20)));
        }

        public override void Update(GameTime gameTime)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
            _hurtColorDuration += gameTime.ElapsedGameTime.Milliseconds;
            contador2 += gameTime.ElapsedGameTime.Seconds;
            _shootCounter++;

            _mouthWeakpoint.Update(gameTime, Origin);

            if (_brainWeakpoint != null)
            {
                _brainWeakpoint.Update(gameTime, Origin, _canBrainReceiveDamage);
                //_gunWeakpoint_B.Update(gameTime);

                if (_brainWeakpoint.IsDestroyed)
                    _brainWeakpoint = null;
            }


            if (_handWeakpoint_A != null)
            {
                _handWeakpoint_A.Update(gameTime, new Vector2(rectangle.Right, rectangle.Top));
                //_gunWeakpoint_A.Update(gameTime);

                if (_handWeakpoint_A.IsDestroyed)
                    _handWeakpoint_A = null;
            }

            if (_handWeakpoint_B != null)
            {
                _handWeakpoint_B.Update(gameTime, new Vector2(rectangle.Left, rectangle.Top));
                //_gunWeakpoint_B.Update(gameTime);


                if (_handWeakpoint_B.IsDestroyed)
                    _handWeakpoint_B = null;
            }

            if (_mouthWeakpoint.IsDestroyed || _mouthWeakpoint == null)
            {
                _canBrainReceiveDamage = true;
            }

            _shootTimer += (int)gameTime.ElapsedGameTime.Milliseconds;

            position = Vector2.Lerp(position, _targetPosition, 0.05f);

            int rotationAngle = 36;

            //Idle();


            if (_shootTimer > 1000 && Components.Player.IsDestroyed == false)
            {
                Shoot(rotationAngle);
            }
            else if (Components.Player.IsDestroyed == false)
            {
                Shoot();
            }

            if (_shootTimer > 2000)
            {
                _shootTimer = 0;
                _targetPosition = NewRandomPosition();
            }

            // position.Y += Speed;

            if (_handWeakpoint_A == null && _handWeakpoint_B == null && _brainWeakpoint == null)
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

            //spriteBatch.Draw(Textures.Pointer, _targetPosition, defaultColor);
            //spriteBatch.Draw(Textures.Pointer, position, defaultColor);

            if (_brainWeakpoint != null)
            {
                _brainWeakpoint.Draw(spriteBatch);
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

        private void Shoot(int rotationAngle)
        {
            if (_shootCounter > 30)
            {
                for (int i = 0; i < 10; i++)
                {
                    float rotation = i * rotationAngle;
                    Components.Lasers.Add(new EnemyLaser_D(Origin, Color.White, 10, rotation, 2));
                    Sounds.EnemyShot.Play();
                    _shootCounter = 0;
                }
            }
        }

        private void Shoot()
        {
            if (_shootCounter > _shootInterval)
            {
                Components.Lasers.Add(new EnemyLaser_A(new Vector2(rectangle.Left + 15, rectangle.Bottom - 12), Color.OrangeRed, 10));
                Components.Lasers.Add(new EnemyLaser_A(new Vector2(rectangle.Right - 15, rectangle.Bottom - 12), Color.OrangeRed, 10));
                Sounds.EnemyShot.Play();
                _shootCounter = 0;
            }
        }
    }

    class Boss_5_brainWeakpoint : Weakpoint
    {
        private int _hurtColorDuration;
        private int _shootCounter;
        private int _shootTimer;

        public Boss_5_brainWeakpoint(Texture2D texture, int energy) : base()
        {
            spriteTexture = texture;
            frameHeight = texture.Height;
            frameWidth = texture.Width;
            Energy = energy;
        }

        public void Update(GameTime gameTime, Vector2 origin, bool canReceiveDamage)
        {
            _hurtColorDuration += gameTime.ElapsedGameTime.Milliseconds;
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);

            if (canReceiveDamage)
                CheckLaserDamage();
            
            position = new Vector2(origin.X - spriteTexture.Width / 2 - 1, origin.Y - SpriteTexture.Height / 2 - 10);

            _shootTimer += (int)gameTime.ElapsedGameTime.Milliseconds;
            _shootCounter++;

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
    }

    class Boss_5_handWeakpoint : Weakpoint
    {
        string _side;
        private int _shootCounter = 0;
        private int _shootInterval;
        private int _handMovement;
        int _direction = 1;
        private int _hurtColorDuration;

        public Boss_5_handWeakpoint(Texture2D texture, int energy, string side, int shootInterval) : base()
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

            _handMovement += (int)gameTime.ElapsedGameTime.Milliseconds;
            _shootCounter++;

            if (_side == "right")
                position = new Vector2(pos.X + frameWidth / 2 - 10, pos.Y + spriteTexture.Height + 20);
            else if (_side == "left")
                position = new Vector2(pos.X - (frameWidth + 10), pos.Y + spriteTexture.Height + 20);

            if (_handMovement > 500)
            {
                if (_shootCounter > 10)
                {
                    _direction = -_direction;
                    _shootCounter = 0;
                }
            }

            /*if (_handMovement > 1500)
            {
                _handMovement = 0;
            }*/

            if (Energy <= 0)
            {
                IsDestroyed = true;
            }

            position.Y += (_shootCounter + 5) * _direction;

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
    }

    class Boss_5_mouthWeakpoint : Weakpoint
    {
        string _side;
        private int _shootCounter = 0;
        private int _shootInterval;
        private int _mouthMovement;
        int _direction = 1;
        private int _hurtColorDuration;

        public Boss_5_mouthWeakpoint(Texture2D texture, int energy) : base()
        {
            spriteTexture = texture;
            frameHeight = texture.Height;
            frameWidth = texture.Width;
            Energy = energy;
        }

        public void Update(GameTime gameTime, Vector2 origin)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
            _hurtColorDuration += gameTime.ElapsedGameTime.Milliseconds;
            CheckLaserDamage();

            _mouthMovement += (int)gameTime.ElapsedGameTime.Milliseconds;
            _shootCounter++;
            position = new Vector2(origin.X - frameWidth / 2, origin.Y + spriteTexture.Height + 15);

            if (_mouthMovement > 300)
            {
                if (_shootCounter > 10)
                {
                    _direction = -_direction;
                    _shootCounter = 0;
                }
            }

            /*if (_handMovement > 1500)
            {
                _handMovement = 0;
            }*/

            if (Energy <= 0)
            {
                IsDestroyed = true;
            }

            position.Y += 5 * _direction;

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
    }
}
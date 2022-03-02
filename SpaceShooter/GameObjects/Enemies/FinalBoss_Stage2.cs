using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using SpaceShooter.Library.Utils;
using System;
using System.Windows;

namespace SpaceShooter.GameObjects.Enemies
{
    class FinalBoss_Stage2 : GameObject
    {
        private int _hurtColorDuration;
        private int _shootTimer;
        private int _shootCounter;
        private int _shootInterval = 10;
        private System.Random random = new System.Random();

        private Animation animation;

        private Boss_2_eyeWeakpoint boss_1_EyeWeakpoint;

        public FinalBoss_Stage2() : base()
        {
            spriteTexture = Textures.FinalBossStage1_Texture;
            frameWidth = spriteTexture.Width / 4;
            frameHeight = spriteTexture.Height;
            position = new Vector2(random.Next(0, Screen.GetWidth - frameWidth), -frameHeight);
            Speed = 2f;
            Energy = 1000;

            animation = new Animation(3, frameWidth, frameHeight);

            boss_1_EyeWeakpoint = new Boss_2_eyeWeakpoint(Textures.FinalBossStage1_EyeWeakpoint_Texture, 500);

            Components.Boss.Add(boss_1_EyeWeakpoint);
        }

        public override void Update(GameTime gameTime)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
            _hurtColorDuration += gameTime.ElapsedGameTime.Milliseconds;
            _shootTimer += (int)gameTime.ElapsedGameTime.Milliseconds;
            _shootCounter++;

            boss_1_EyeWeakpoint.Update(gameTime, Origin);

            animation.Update(gameTime);

            if (_shootTimer > 1000 && Components.Player.IsDestroyed == false)
            {
                if (Components.Player.Position.Y < rectangle.Bottom)
                    Shoot();
            }

            if (_shootTimer > 3000)
            {
                _shootTimer = 0;
            }

            if (rectangle.Top > Screen.GetHeight)
            {
                position = new Vector2(random.Next(0, Screen.GetWidth - frameWidth), -frameHeight);
            }

            position.Y += Speed;

            if (boss_1_EyeWeakpoint.IsDestroyed)
            {
                IsDestroyed = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isHitted == true && _hurtColorDuration < 150)
            {
                //spriteBatch.Draw(spriteTexture, position, damageColor);
                animation.Draw(spriteBatch, spriteTexture, position, damageColor, scale);
            }   
            else
            {
                isHitted = false;
                _hurtColorDuration = 0;
                //spriteBatch.Draw(spriteTexture, position, defaultColor);
                animation.Draw(spriteBatch, spriteTexture, position, defaultColor, scale);
            }

            boss_1_EyeWeakpoint.Draw(spriteBatch);
        }

        private void Shoot()
        {
            if (_shootCounter > _shootInterval)
            {
                Components.Lasers.Add(new EnemyLaser_B(new Vector2(Position.X, Position.Y + 10), Color.White, 10, Direction.Left, Textures.EnemyLaser_F_Texture, true));
                Components.Lasers.Add(new EnemyLaser_B(new Vector2(Position.X + 13, Position.Y), Color.White, 10, Direction.Center, Textures.EnemyLaser_F_Texture, true));
                Components.Lasers.Add(new EnemyLaser_B(new Vector2(Position.X + frameWidth - 16, Position.Y), Color.White, 10, Direction.Center, Textures.EnemyLaser_F_Texture, true));
                Components.Lasers.Add(new EnemyLaser_B(new Vector2(Position.X + frameWidth - 3, Position.Y + 10), Color.White, 10, Direction.Right, Textures.EnemyLaser_F_Texture, true));
                Sounds.EnemyShot.Play();
                _shootCounter = 0;
            }
        }
    }

    class Boss_2_eyeWeakpoint : Weakpoint
    {
        private int _hurtColorDuration;
        private int _shootCounter;
        private int _shootTimer;

        public Boss_2_eyeWeakpoint(Texture2D texture, int energy) : base()
        {
            spriteTexture = texture;
            frameHeight = texture.Height;
            frameWidth = texture.Width;
            Energy = energy;
        }

        public void Update(GameTime gameTime, Vector2 origin)
        {
            _hurtColorDuration += gameTime.ElapsedGameTime.Milliseconds;
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);

            CheckLaserDamage();
            position = new Vector2(origin.X - spriteTexture.Width / 2, origin.Y);

            _shootTimer += (int)gameTime.ElapsedGameTime.Milliseconds;
            _shootCounter++;

            if (_shootTimer > 500)
            {
                if (Components.Player.Position.Y > rectangle.Bottom)
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
            float angle = CalculateAngle(position, Components.Player.Position);

            //Vector a = new Vector(position.X, position.Y);
            //Vector b = new Vector(Components.Player.Position.X, Components.Player.Position.Y);

            //float angle = MathHelper.ToDegrees((float)Vector.AngleBetween(b, a));

            if (_shootCounter > 5)
            {
                Components.Lasers.Add(new EnemyLaser_D(new Vector2(Position.X + frameWidth / 2, Position.Y + frameHeight), Color.White, 10, angle, Textures.EnemyLaser_D_Texture, false));
                Sounds.EnemyShot.Play();
                _shootCounter = 0;

                //_direction = -_direction;
            }
        }

        private float CalculateAngle(Vector2 currPos, Vector2 tgtPos)
        {
            float angle;
            Vector2 difference = currPos - tgtPos;
            difference.Normalize();
            angle = (float)Math.Atan2(difference.Y, -difference.X);
            return (MathHelper.ToDegrees(angle));
        }
    }
}

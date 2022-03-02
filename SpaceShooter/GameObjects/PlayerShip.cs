using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using SpaceShooter.Library.Utils;
using System.Runtime.CompilerServices;
using SpaceShooter.Shared;

namespace SpaceShooter.GameObjects
{
    public class PlayerShip : GameObject
    {
        public bool IsShooting { get; private set; }
        public bool IsBlinking { get; private set; } = true;
        public int SelectedGun = 1;
        public int Lives { get; set; }

        private Vector2 _primaryLaserPosition;
        private Vector2 _secondaryLaserPosition;
        private Vector2 _centeredLaserPosition;
        private Color _hurtColor = new Color(255, 130, 60);
        private Color _laserColor = new Color(118, 218, 138);
        private int _shootInterval;
        private int _laserEnergy = 10;
        private float _friction = .15f;
        private int _hurtColorDuration;
        private bool _canPlay = true;
        private Animation _animation;
        private float objectScale;
        private Animation _flameAnimation;
        private Texture2D _flameTexture = Textures.PlayerShipFlames;
        private Direction dir;
        private double _blinkTimer = 0;

        private int _fireTimer;

        public PlayerShip(Texture2D texture, bool createColorMap, int lives) : base(texture, createColorMap)
        {
            //input = new Input();
            frameWidth = 32;
            frameHeight = 32;
            position = new Vector2(Screen.GetWidth / 2 - frameWidth / 2, Screen.GetHeight - frameHeight);
            Energy = 100;
            Information.Life = Energy;
            _animation = new Animation(5, frameWidth, frameHeight, 2, 0.1f);
            _flameAnimation = new Animation(1, 10, 10);
            this.Speed = 0.8f;
            IsDestroyed = false;
            Lives = lives;
        }

        public void Update(GameTime gameTime, bool canShoot)
        {
            var state = Keyboard.GetState();

            if (_blinkTimer > 3)
                IsBlinking = false;
            else
                _blinkTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (IsBlinking)
                defaultColor.A -= 10;
            else
                defaultColor.A = 255;

            if (state.IsKeyDown(Input.Shoot) && canShoot == true)
            {
                if (SelectedGun == 1)
                    PrimaryGun();
                else if (SelectedGun == 2)
                    SecondaryGun();
                else if (SelectedGun == 3)
                    TernaryGun();
            }

            _animation.Update(gameTime, dir);
            //_flameAnimation.Update(gameTime);
            _shootInterval += gameTime.ElapsedGameTime.Milliseconds;
            _hurtColorDuration += gameTime.ElapsedGameTime.Milliseconds;
            _fireTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (state.IsKeyDown(Input.Up) || state.IsKeyDown(Input.Left) || state.IsKeyDown(Input.Right))
            {
                Components.FireParticles.Add(new Particle(Textures.ExplosionParticleTexture, false, new Vector2(Origin.X - 3, rectangle.Bottom - 2), true, 1));
                Components.FireParticles.Add(new Particle(Textures.ExplosionParticleTexture, false, new Vector2(Origin.X, rectangle.Bottom - 2), true, 1));
            }
            else
            {
                if (_fireTimer > 100)
                {
                    Components.FireParticles.Add(new Particle(Textures.ExplosionParticleTexture, false, new Vector2(Origin.X - 3, rectangle.Bottom - 2), true, 0));
                    Components.FireParticles.Add(new Particle(Textures.ExplosionParticleTexture, false, new Vector2(Origin.X, rectangle.Bottom - 2), true, 0));
                    _fireTimer = 0;
                }
            }
            

            CheckInput(state);

            position.X = MathHelper.Clamp(position.X, 0, Screen.GetWidth - frameWidth);
            position.Y = MathHelper.Clamp(position.Y, (Screen.GetWidth / 4), Screen.GetHeight - frameHeight - _flameTexture.Height);

            position += velocity;

            _primaryLaserPosition = new Vector2(position.X, position.Y + 15);
            _secondaryLaserPosition = new Vector2(position.X + frameWidth - 3, position.Y + 15);
            _centeredLaserPosition = new Vector2(position.X + frameWidth / 2 - 3, position.Y);

            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)(frameWidth), (int)(frameHeight));

            if (velocity != Vector2.Zero)
            {
                var i = velocity;
                velocity = i -= i * _friction;
            }
            

            if (Energy <= 0)
            {
                IsDestroyed = true;
                Explosion.Create(position);
            }

            if (isHitted == true && _canPlay == true)
            {
                Sounds.PlayerHurt.Play();
                _canPlay = false;
            }

            if (Energy > 100)
                Energy = 100;
            else if (Energy < 0)
                Energy = 0;

            Information.Life = Energy;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //_flameAnimation.Draw(spriteBatch, _flameTexture, new Vector2(position.X + 11, position.Y + spriteTexture.Height - 5), defaultColor, scale);

            //spriteBatch.Draw(spriteTexture, position, rectangle, defaultColor, rotation, origin, 20f, SpriteEffects.None, 0f);
            if (isHitted == true && _hurtColorDuration < 150)
            {
                //spriteBatch.Draw(Texture, Position, source, _hurtColor, 0.0f, origin, Scale, SpriteEffects.None, 0.0f);
                _animation.Draw(spriteBatch, spriteTexture, position, damageColor, scale);
                
            }
            else
            {
                _animation.Draw(spriteBatch, spriteTexture, position, defaultColor, scale);
                _canPlay = true;
                isHitted = false;
                _hurtColorDuration = 0;
            }
        }

        public void CreateShield()
        {
            Components.ShieldArmature = new Shield();
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

        public void CheckInput(KeyboardState state)
        {
            dir = Direction.Center;

            if (state.IsKeyDown(Input.Up))
                velocity.Y -= Speed;
            if (state.IsKeyDown(Input.Down))
                velocity.Y += Speed;
            if (state.IsKeyDown(Input.Left))
            {
                velocity.X -= Speed;
                dir = Direction.Left;
            }
            if (state.IsKeyDown(Input.Right))
            {
                velocity.X += Speed;
                dir = Direction.Right;
            }

            if (state.IsKeyDown(Keys.D1))
            {
                SelectedGun = 1;
            }
            if (state.IsKeyDown(Keys.D2))
            {
                SelectedGun = 2;
            }
            if (state.IsKeyDown(Keys.D3))
            {
                SelectedGun = 3;
            }
        }

        public Vector2 GetPos()
        {
            return (new Vector2(position.X, position.Y));
        }

        private void PrimaryGun()
        {
            if (_shootInterval > 200)
            {
                Components.Lasers.Add(new PlayerLaser_A(_primaryLaserPosition, Color.White, _laserEnergy, 0.4f));
                Components.Lasers.Add(new PlayerLaser_A(_secondaryLaserPosition, Color.White, _laserEnergy, 0.4f));
                Sounds.PlayerShot.Play();

                _shootInterval = 0;
            }

        }

        private void SecondaryGun()
        {
            if (_shootInterval > 300)
            {
                Components.Lasers.Add(new PlayerLaser_B(_primaryLaserPosition, Color.White, _laserEnergy, Direction.Left));
                Components.Lasers.Add(new PlayerLaser_B(_centeredLaserPosition, Color.White, _laserEnergy, Direction.Center));
                Components.Lasers.Add(new PlayerLaser_B(_secondaryLaserPosition, Color.White, _laserEnergy, Direction.Right));
                Sounds.PlayerShot.Play();

                _shootInterval = 0;
            }
        }

        private void TernaryGun()
        {
            if (_shootInterval > 400)
            {
                Components.Lasers.Add(new PlayerLaser_C(_centeredLaserPosition, Color.White, _laserEnergy));
                Sounds.PlayerShot.Play();

                _shootInterval = 0;
            }
        }

        public void UpdateObstacleDamage()
        {
            foreach (var obstacle in Components.Obstacles)
            {
                if (rectangle.Intersects(obstacle.Rectangle))
                {
                    /*Energy -= obstacle.Energy;
                    obstacle.Energy -= obstacle.Energy;
                    isHitted = true;*/

                    if (Collision.IntersectsPixel(rectangle, colorMap, obstacle.Rectangle, obstacle.ColorMap))
                    {
                        Energy -= obstacle.Energy;
                        obstacle.Energy -= obstacle.Energy;
                        isHitted = true;
                    }
                }
            }
        }

        public void UpdateGridEnemyDamage()
        {
            foreach (var enemy in Components.GridEnemies)
            {
                if (rectangle.Intersects(enemy.Rectangle))
                {
                    var playerlifeTemp = Energy;
                    Energy -= enemy.Energy;
                    enemy.Energy -= Energy;
                    isHitted = true;

                    /*if (Collision.IntersectsPixel(this.Rectangle, colorMap, enemy.Rectangle, enemy.ColorMap))
                    {
                        var playerlifeTemp = Energy;
                        Energy -= enemy.Energy;
                        enemy.Energy -= Energy;
                        isHitted = true;
                    }*/
                }
            }
        }

        public void UpdateBonusEnemyDamage()
        {
            foreach (var enemy in Components.BonusEnemies)
            {
                if (rectangle.Intersects(enemy.Rectangle))
                {
                    var playerlifeTemp = Energy;
                    Energy -= enemy.Energy;
                    enemy.Energy -= Energy;
                    isHitted = true;

                    /*if (Collision.IntersectsPixel(this.Rectangle, colorMap, enemy.Rectangle, enemy.ColorMap))
                    {
                        var playerlifeTemp = Energy;
                        Energy -= enemy.Energy;
                        enemy.Energy -= Energy;
                        isHitted = true;
                    }*/
                }
            }
        }

        public void UpdateRushEnemyDamage()
        {
            foreach (var enemy in Components.Enemies)
            {
                if (rectangle.Intersects(enemy.Rectangle))
                {
                    var playerlifeTemp = Energy;
                    Energy -= enemy.Energy;
                    enemy.Energy -= Energy;
                    isHitted = true;

                    /*if (Collision.IntersectsPixel(this.Rectangle, colorMap, enemy.Rectangle, enemy.ColorMap))
                    {
                        var playerlifeTemp = Energy;
                        Energy -= enemy.Energy;
                        enemy.Energy -= Energy;
                        isHitted = true;
                    }*/
                }
            }
        }

        public void UpdateBossEnemyDamage()
        {
            foreach (var enemy in Components.Boss)
            {
                if (rectangle.Intersects(enemy.Rectangle))
                {
                    var playerlifeTemp = Energy;
                    Energy -= enemy.Energy;
                    enemy.Energy -= Energy;
                    isHitted = true;

                    /*if (Collision.IntersectsPixel(this.Rectangle, colorMap, enemy.Rectangle, enemy.ColorMap))
                    {
                        var playerlifeTemp = Energy;
                        Energy -= enemy.Energy;
                        enemy.Energy -= Energy;
                        isHitted = true;
                    }*/
                }
            }
        }
    }
}

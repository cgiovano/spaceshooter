using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using SpaceShooter.Library.Utils;

namespace SpaceShooter.GameObjects
{
    public class PlayerShip : GameObject
    {
        public bool IsShooting { get; private set; }

        private Input input;
        private Vector2 _primaryLaserPosition;
        private Vector2 _secondaryLaserPosition;
        private Color _hurtColor = new Color(255, 130, 60);
        private Color _laserColor = new Color(118, 218, 138);
        private int _shootInterval;
        private int _laserEnergy = 10;
        private float _friction = .15f;
        private int _hurtColorDuration;
        private bool _canPlay = true;
        private Animation _animation;
        private float objectScale;

        public PlayerShip(Texture2D texture, Texture2D textureCollision, bool criarMapaDeCor) : base(texture, criarMapaDeCor)
        {
            input = new Input();
            frameWidth = 39;
            frameHeight = 39;
            position = new Vector2(Screen.GetWidth / 2, Screen.GetHeight - frameHeight);
            Energy = 100;
            Information.Life = Energy;
            _animation = new Animation(0, frameWidth, frameHeight);
            this.Speed = 0.8f;
    }

        public override void Update(GameTime gameTime)
        {
            _animation.Update(gameTime);
            _shootInterval += gameTime.ElapsedGameTime.Milliseconds;
            _hurtColorDuration += gameTime.ElapsedGameTime.Milliseconds;

            CheckInput();

            position.X = MathHelper.Clamp(position.X, 0, Screen.GetWidth - frameWidth);
            position.Y = MathHelper.Clamp(position.Y, (Screen.GetWidth / 4), Screen.GetHeight - frameHeight);

            position += velocity;

            _primaryLaserPosition = new Vector2(position.X, position.Y + 15);
            _secondaryLaserPosition = new Vector2(position.X + spriteTexture.Width - 3, position.Y + 15);

            Rectangle = new Rectangle((int)position.X, (int)position.Y, (int)(frameWidth), (int)(frameHeight));

            if (velocity != Vector2.Zero)
            {
                var i = velocity;
                velocity = i -= i * _friction;
            }

            foreach (var l in Components.Lasers)
            {
                var laser = l as Laser;

                if (this.Rectangle.Intersects(laser.Rectangle) && laser.LaserType == LaserType.Enemy)
                {
                    Energy -= laser.Energy;
                    laser.IsRemoved = true;
                    isHitted = true;
                }
            }

            foreach (var obstacle in Components.Obstacles)
            {
                if (this.Rectangle.Intersects(obstacle.Rectangle))
                {
                    Energy -= obstacle.Energy;
                    obstacle.Energy -= obstacle.Energy;
                    isHitted = true;

                    /*if (Collision.IntersectsPixel(this.Rectangle, colorMap, obstacle.Rectangle, obstacle.ColorMap))
                    {
                        Energy -= obstacle.Energy;
                        obstacle.Energy -= obstacle.Energy;
                        isHitted = true;
                    }*/
                }
            }

            foreach (var enemy in Components.Enemies)
            {
                if (this.Rectangle.Intersects(enemy.Rectangle))
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

            if (Energy <= 0)
                IsDestroyed = true;

            if (isHitted == true && _canPlay == true)
            {
                Sounds.PlayerHurt.Play();
                _canPlay = false;
            }

            Information.Life = Energy;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
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

        public void CheckInput()
        {
            var state = Keyboard.GetState();

            if (state.IsKeyDown(input.Up))
                velocity.Y -= Speed;
            else if (state.IsKeyDown(input.Down))
                velocity.Y += Speed;
            else if (state.IsKeyDown(input.Left))
                velocity.X -= Speed;     
            else if (state.IsKeyDown(input.Right))
                velocity.X += Speed;
            
            if (state.IsKeyDown(input.Shoot) && _shootInterval > 200)
            {
                Shoot();
                IsShooting = true;
                _shootInterval = 0;
            }
            else
            {
                IsShooting = false;
            }
        }

        private void Shoot()
        {
            Components.Lasers.Add(new Laser(Textures.Laser, false, _laserEnergy, LaserType.Player, Color.White, _primaryLaserPosition));
            Components.Lasers.Add(new Laser(Textures.Laser, false, _laserEnergy, LaserType.Player, Color.White, _secondaryLaserPosition));
            Sounds.PlayerShot.Play();
        }
    }
}

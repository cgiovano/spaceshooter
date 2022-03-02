using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpaceShooter.Library;
using SpaceShooter.GameUtils;
using SpaceShooter.Library.Utils;

namespace SpaceShooter.GameObjects.Enemies
{
    public enum Direction
    {
        Right,
        Left
    }

    public class Enemy : GameObject
    {
        private Direction _direction;
        private Color _laserColor = new Color(255, 110, 110);
        private int _shootInterval;
        private int contador;
        private int contador2;
        private float _friction = 5f;
        private Animation _animation;
        private float objectScale;
        private int _hurtColorDuration = 0;

        public Enemy(Texture2D texture, bool createColorMap) : base(texture, createColorMap)
        {
            ScaleFactor = 3.5f;
            frameWidth = 20;
            frameHeight = 20;
            spriteTexture = texture;
            Energy = 50;
            position = new Vector2(Random.GetRandom(0, (int)(Screen.GetWidth - frameWidth)), -(int)(frameHeight));
            Speed = .25f;
            objectScale = scale * ScaleFactor;
            _shootInterval = 50;
            _animation = new Animation(1, 20, 20);

            var directionHelper = Random.GetRandom(1, 2);

            switch (directionHelper)
            {
                case (1):
                    _direction = Direction.Right;
                    break;
                case (2):
                    _direction = Direction.Left;
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            var player = Components.Player as PlayerShip;
            _animation.Update(gameTime);
            _hurtColorDuration += gameTime.ElapsedGameTime.Milliseconds;
            Rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);

            contador++;
            contador2++;
            Shoot(Components.Lasers);

            if (Position.X < Components.Player.Position.X)
            {
                if (velocity.X < 3)
                    velocity.X += Speed;
            }
            else if (Position.X > Components.Player.Position.X)
            {
                if (velocity.X > -3)
                    velocity.X -= Speed;
            }
            else
            {
                if (velocity != Vector2.Zero)
                {
                    var i = velocity;
                    velocity = i -= i * _friction;
                }
            }

            position += velocity;
            position.Y += Speed * 5;

            if (rectangle.Top > Screen.GetHeight)
                IsRemoved = true;

            foreach (var l in Components.Lasers)
            {
                var laser = l as Laser;

                if (this.Rectangle.Intersects(laser.Rectangle) && laser.LaserType == LaserType.Player)
                {
                    Energy -= laser.Energy;
                    laser.IsRemoved = true;
                    isHitted = true;
                }

                if (this.Rectangle.Top > Screen.GetHeight)
                {
                    this.IsRemoved = true;
                }
            }

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

        private void Shoot(List<GameObject> Lasers)
        {
            if (contador > _shootInterval)
            {
                Lasers.Add(new Laser(Textures.Laser, false, 10, LaserType.Enemy, _laserColor, new Vector2(Position.X + spriteTexture.Width / 2, Position.Y + (spriteTexture.Height - 5))));
                Sounds.EnemyShot.Play();
                contador = 0;
            }
        }
    }
}

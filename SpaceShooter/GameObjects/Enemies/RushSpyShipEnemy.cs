using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using SpaceShooter.Library.Utils;
using System.Collections.Generic;

namespace SpaceShooter.GameObjects.Enemies
{
    class RushSpyShipEnemy : RushEnemyBase
    {
        private Direction _direction;
        private Color _laserColor = new Color(0, 255, 100);
        private int _shootInterval;
        private int contador;
        private int contador2;
        private float _friction = 5f;
        private Animation _animation;
        private int _hurtColorDuration = 0;

        public RushSpyShipEnemy() : base()
        {
            spriteTexture = Textures.Rush_Enemy_A_Texture;
            frameWidth = 22;
            frameHeight = 24;
            Energy = 50;
            position = new Vector2(RandomMod.GetRandom(0, (int)(Screen.GetWidth - frameWidth)), -(int)(frameHeight));
            Speed = .25f;
            _shootInterval = 50;
            _animation = new Animation(5, frameWidth, frameHeight, 0.1f);

            var directionHelper = RandomMod.GetRandom(1, 2);

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
            _animation.Update(gameTime, _direction);
            _hurtColorDuration += gameTime.ElapsedGameTime.Milliseconds;
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);

            contador++;
            contador2++;

            if (Components.Player.IsDestroyed == false)
            {
                Shoot(Components.Lasers);
            }

            if (Position.X < Components.Player.Position.X)
            {
                if (velocity.X < 3)
                    velocity.X += Speed;

                _direction = Direction.Right;
            }
            else if (Position.X > Components.Player.Position.X)
            {
                if (velocity.X > -3)
                    velocity.X -= Speed;

                _direction = Direction.Left;
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

            CheckLaserDamage();

            if (Energy <= 0)
                IsDestroyed = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, Position, source, defaultColor, 0.0f, origin, Scale, SpriteEffects.None, 0.0f);
            if (isHitted == true && _hurtColorDuration < 150)
                _animation.Draw(spriteBatch, spriteTexture, position, damageColor, scale, SpriteEffects.FlipVertically);
            else
            {
                isHitted = false;
                _hurtColorDuration = 0;
                _animation.Draw(spriteBatch, spriteTexture, position, defaultColor, scale, SpriteEffects.FlipVertically);
            }
        }

        private void Shoot(List<GameObject> Lasers)
        {
            if (contador > _shootInterval)
            {
                Lasers.Add(new EnemyLaser_A(new Vector2(Position.X + frameWidth / 2, Position.Y + frameHeight), Color.LightGreen, 10));
                Sounds.EnemyShot.Play();
                contador = 0;
            }
        }
    }
}

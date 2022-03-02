using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using SpaceShooter.Library.Utils;

namespace SpaceShooter.GameObjects.Enemies
{
    class RushSatelliteEnemy : RushEnemyBase
    {
        private int _shootInterval;
        private int _shootCounter;
        private Animation _animation;
        private int _hurtColorDuration = 0;
        private int _directionX = 1;
        private int _directionY = 1;
        private bool _isOnScreen = false;

        public RushSatelliteEnemy() : base()
        {
            spriteTexture = Textures.Rush_Enemy_D_Texture;
            frameWidth = 32;
            frameHeight = 30;
            Energy = 50;

            Speed = 2f;
            _shootInterval = 20;
            _animation = new Animation(2, frameWidth, frameHeight, 0.1f);

            position = new Vector2(RandomMod.GetRandom(0, Screen.GetWidth - frameWidth), -frameHeight);

            if (Components.Player.Position.X < Screen.GetWidth / 2)
            {  
                _directionX = -_directionX;
            }
        }

        public override void Update(GameTime gameTime)
        {
            _hurtColorDuration += gameTime.ElapsedGameTime.Milliseconds;
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);

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

            _animation.Update(gameTime);

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
    }
}

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpaceShooter.Library;
using SpaceShooter.GameUtils;
using SpaceShooter.Library.Utils;

namespace SpaceShooter.GameObjects
{
    public enum ObstacleSize
    {
        Small,
        Medium,
        Big,
    }

    public class Obstacle : GameObject
    {
        public ObstacleSize Size { get; private set; }

        private int _counter = 0;
        private Animation _animation;
        private new float scale = 1f;

        public Obstacle(Texture2D texture, bool createColorMap) : base(texture, createColorMap)
        {
            frameWidth = spriteTexture.Width;
            frameHeight = spriteTexture.Height;
            Energy = frameWidth - 20;
            Speed = RandomMod.GetRandom(20, 40) / 10f;
            _animation = new Animation(0, frameWidth, frameHeight);

            position = new Vector2(RandomMod.GetRandom((int)(Screen.GetWidth - frameWidth)), -frameHeight);

            if (texture == Textures.AsteroidSmallSizeTexture)
                Size = ObstacleSize.Small;
            else if (texture == Textures.AsteroidMedSizeTexture)
                Size = ObstacleSize.Medium;
            else if (texture == Textures.AsteroidBigSizeTexture)
                Size = ObstacleSize.Big;
        }

        public override void Update(GameTime gameTime)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
            _animation.Update(gameTime);
            _counter += gameTime.ElapsedGameTime.Milliseconds;

            foreach (var l in Components.Lasers)
            {
                
                var laser = l as Laser;

                if (rectangle.Intersects(laser.Rectangle) && laser.LaserType == LaserType.Player)
                {
                    Energy -= laser.Energy;
                    laser.IsRemoved = true;
                    isHitted = true;
                }
            }

            if (Energy <= 0)
                IsDestroyed = true;

            if (this.Rectangle.Top > Screen.GetHeight)
                this.IsRemoved = true;

            _counter += 1;
            position.Y += Speed;
            rotation += 0.01f;
        }

        
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 localOrigin = new Vector2(spriteTexture.Width / 2, spriteTexture.Height / 2);

            if (isHitted == true && _counter < 10)
            {
                _animation.Draw(spriteBatch, spriteTexture, position, damageColor, scale, rotation, localOrigin);
                return;
            }
            else
            {
                _animation.Draw(spriteBatch, spriteTexture, position, defaultColor, scale, rotation, localOrigin);
                isHitted = false;
                _counter = 0;
            }
        }
    }
}

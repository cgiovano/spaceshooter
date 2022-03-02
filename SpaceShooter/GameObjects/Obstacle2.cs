using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpaceShooter.Library;
using SpaceShooter.GameUtils;
using SpaceShooter.Library.Utils;

namespace SpaceShooter.GameObjects
{
    public class Obstacle : GameObject
    {
        private int _counter = 0;
        private Animation _animation;
        private new float scale;

        public Obstacle(Texture2D texture, bool createColorMap) : base(texture, createColorMap)
        {
            frameWidth = 40;
            frameHeight = 40;
            Energy = 30;
            position = new Vector2(Random.GetRandom((int)(Screen.GetWidth - 40)), -frameHeight);
            Speed = Random.GetRandom(20, 40) / 10f;
            //_animation = new Animation(4, 8, 20);
            _animation = new Animation(0, 40, 40);

            scale = (2 / Speed) * 2;
            //Rectangle = new Rectangle((int)position.X, (int)position.Y, (int)(frameWidth * scale), (int)(frameHeight * scale));
        }

        public override void Update(GameTime gameTime)
        {
            Rectangle = new Rectangle((int)position.X, (int)position.Y, (int)(frameWidth * scale), (int)(frameHeight * scale));
            _animation.Update(gameTime);
            _counter += gameTime.ElapsedGameTime.Milliseconds;

            foreach (var l in Components.Lasers)
            {
                
                var laser = l as Laser;

                if (this.Rectangle.Intersects(laser.Rectangle) && laser.LaserType == LaserType.Player)
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
            //var newPos = new Vector2(Position.X + (Texture.Width / 2), Position.Y + (Texture.Height / 2));
            /*
             * In Draw method you need to use new Vector2(Position.X + (Texture.Width / 2), Position.Y + (Texture.Height / 2)) in place of Position.
             * For some that i don't know, when you create a incremental rotation, the origin is always the star point of the object.
             */

            //origin = new Vector2(spriteTexture.Width / 2, spriteTexture.Height / 2);
            origin = new Vector2(spriteTexture.Width / 2, spriteTexture.Height / 2);
            //Rectangle = new Rectangle((int)position.X, (int)position.Y, spriteTexture.Width, spriteTexture.Height);

            if (isHitted == true && _counter < 10)
            {
                _animation.Draw(spriteBatch, spriteTexture, position, damageColor, (scale / Speed * 3), rotation, origin);
                return;
            }
            else
            {
                _animation.Draw(spriteBatch, spriteTexture, position, defaultColor, (scale / Speed * 3), rotation, origin);
                isHitted = false;
                _counter = 0;
            }
        }
    }
}

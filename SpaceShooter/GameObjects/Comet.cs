using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpaceShooter.Library;
using SpaceShooter.GameUtils;
using SpaceShooter.Library.Utils;

namespace SpaceShooter.GameObjects
{
    public class Comet : GameObject
    {
        private int _counter = 0;
        private Animation _animation;
        private new float scale;

        public Comet(Texture2D texture, bool createColorMap) : base(texture, createColorMap)
        {
            frameWidth = 40;
            frameHeight = 40;
            Energy = 30;
            position = new Vector2(RandomMod.GetRandom((int)(Screen.GetWidth - 8)), -frameHeight);
            Speed = RandomMod.GetRandom(20, 40) / 10f;
            //_animation = new Animation(4, 8, 20);
            _animation = new Animation(0, 40, 40);

            scale = (2 / Speed) * 2;
            //Rectangle = new Rectangle((int)position.X, (int)position.Y, (int)(frameWidth * scale), (int)(frameHeight * scale));
        }

        public override void Update(GameTime gameTime)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y + spriteTexture.Height - frameHeight, (int)(frameWidth * scale), (int)(frameHeight * scale));
            _animation.Update(gameTime);
            //_counter += gameTime.ElapsedGameTime.Milliseconds

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
        }

        
        public override void Draw(SpriteBatch spriteBatch)
        {
            //var newPos = new Vector2(Position.X + (Texture.Width / 2), Position.Y + (Texture.Height / 2));
            /*
             * In Draw method you need to use new Vector2(Position.X + (Texture.Width / 2), Position.Y + (Texture.Height / 2)) in place of Position.
             * For some that i don't know, when you create a incremental rotation, the origin is always the star point of the object.
             */

            if (isHitted == true && _counter < 10)
            {

                _animation.Draw(spriteBatch, spriteTexture, position, damageColor, scale);
                return;
                //spriteBatch.Draw(texture, Position, source, Color.Red, rotation, origin, (Scale / Speed * 13), SpriteEffects.None, 0);
            }
            else
            {
                //spriteBatch.Draw(texture, Position, source, defaultColor, rotation, origin, (Scale / Speed * 13), SpriteEffects.None, 0);
                isHitted = false;
                _animation.Draw(spriteBatch, spriteTexture, position, defaultColor, scale);
                _counter = 0;
            }
        }
    }
}

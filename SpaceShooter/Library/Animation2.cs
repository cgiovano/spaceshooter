using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Library.Utils
{
    public class Animation
    {
        Vector2 origin = Vector2.Zero;
        Rectangle source;
        double time;
        float frameTime = 0.1f;
        int frameIndex = 0;
        int totalFrames;
        int frameWidth;
        int frameHeight;

        public Animation(int totalFrames, int frameWidth, int frameHeight)
        {
            this.totalFrames = totalFrames;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
        }

        public void Update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.TotalSeconds;

            while (time > frameTime)
            {
                frameIndex++;
                time = 0f;
            }

            //Loop the animation
            if (frameIndex > totalFrames)
                frameIndex = 0;

            source = new Rectangle(frameIndex * frameWidth, 0, frameWidth, frameHeight);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color color, float scale)
        {
            spriteBatch.Draw(texture, position, source, color, 0.0f, origin, scale, SpriteEffects.None, 0.0f);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color color, float scale, float rotation, Vector2 origin)
        {
            //source = new Rectangle((int)(source.X - origin.X), (int)(source.Y - origin.Y), source.Width, source.Height);
            spriteBatch.Draw(texture, (position + origin), source, color, rotation, origin, scale, SpriteEffects.None, 0.0f);
        }
    }
}

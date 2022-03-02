using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.GameUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Library.Utils
{
    public class Animation
    {
        public Rectangle GetSourceRectangle { get { return(source); } }

        Vector2 origin = Vector2.Zero;
        Rectangle source;
        double time;
        float frameTime = 0.4f;
        int frameIndex = 0;
        int totalFrames;
        int frameWidth;
        int frameHeight;
        int _mainFrameIndex;

        public Animation(int totalFrames, int frameWidth, int frameHeight)
        {
            this.totalFrames = totalFrames;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
        }

        public Animation(int totalFrames, int frameWidth, int frameHeight, float frameTime)
        {
            this.totalFrames = totalFrames;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.frameTime = frameTime;
        }

        public Animation(int totalFrames, int frameWidth, int frameHeight, int mainFrameIndex, float frameTime)
        {
            this.totalFrames = totalFrames;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.frameTime = frameTime;
            _mainFrameIndex = mainFrameIndex;
        }

        public void SetRectangle(Rectangle rect)
        {
            source = rect;
        }

        public void SetRectangleHeight(int height)
        {
            frameHeight = height;
        }

        public void Update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.TotalSeconds;

            if (time > frameTime)
            {
                frameIndex++;
                time = 0f;
            }

            //Loop the animation
            if (frameIndex > totalFrames)
                frameIndex = 0;

            source = new Rectangle(frameIndex * frameWidth, 0, frameWidth, frameHeight);
        }

        public void Update(GameTime gameTime, Direction direction)
        {
            time += gameTime.ElapsedGameTime.TotalSeconds;

            frameIndex = MathHelper.Clamp(frameIndex, 0, totalFrames);

            if (direction == Direction.Left)
            {
                if (time > frameTime && frameIndex > 0)
                {
                    frameIndex--;
                    time = 0f;
                }
            }
            else
            {
                if (time > frameTime && frameIndex < _mainFrameIndex)
                {
                    frameIndex++;
                    time = 0f;
                }
            }
            
            
            if (direction == Direction.Right)
            {
                if (time > frameTime && frameIndex < (totalFrames - 1))
                {
                    frameIndex++;
                    time = 0f;
                }
            }
            else
            {
                if (time > frameTime && frameIndex > _mainFrameIndex)
                {
                    frameIndex--;
                    time = 0f;
                }
            }

            source = new Rectangle(frameIndex * frameWidth, 0, frameWidth, frameHeight);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color color, float scale)
        {
            spriteBatch.Draw(texture, position, source, color, 0.0f, origin, scale, SpriteEffects.None, 0.0f);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color color, float scale, float rotation, Vector2 origin)
        {
            spriteBatch.Draw(texture, (position + origin), source, color, rotation, origin, scale, SpriteEffects.None, 0.0f);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color color, float scale, SpriteEffects flip)
        {
            spriteBatch.Draw(texture, position, source, color, 0.0f, origin, scale, flip, 0.0f);
        }
    }
}

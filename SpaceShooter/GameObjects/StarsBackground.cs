using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;

namespace SpaceShooter
{
    public enum StarBackgroundType
    {
        Small, 
        Big
    }

	public class StarsBackground : GameObject
	{
        public StarBackgroundType StarType { get; private set; }

        private Color _starColor;
        int blindtime;
        
        int offTimer = 0;
        int onTimer = 0;
        int off = -25;
        int counter = 0;

		public StarsBackground(StarBackgroundType starType, Vector2 startPosition) : base()
		{
            if (starType == StarBackgroundType.Small)
            {
                Speed = 2f;
                spriteTexture = Textures.StarsSmallTexture;
            }  
            else
            {
                Speed = 3f;
                spriteTexture = Textures.StarsBigTexture;
            }

            StarType = starType;

            position = startPosition;
		}

        public override void Update(GameTime gameTime)
		{
            position.Y += Speed;

            if (position.Y >= Screen.GetHeight)
                IsRemoved = true;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
            spriteBatch.Draw(spriteTexture, position, Color.White);
		}
	}
}

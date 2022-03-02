using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Library;

namespace SpaceShooter
{
	public class Star : GameObject
	{
        private Color _starColor;

		public Star(Texture2D texture, bool createColorMap, bool firstTime) : base(texture, createColorMap)
		{
			switch (firstTime)
			{
				case (true):
					position = new Vector2(Random.GetRandom(Screen.GetWidth), Random.GetRandom(Screen.GetHeight));
					break;
				case (false):
                    position = new Vector2(Random.GetRandom(Screen.GetWidth), -texture.Height);
					break;
			}

            Speed = Random.GetRandom(4, 10) / 10f;

            // Random color Selection for the star
            var randomOption = Random.GetRandom(1, 3);

            switch(randomOption)
            {
                case (1):
                    _starColor = new Color(240, 100, 255);
                    break;
                case (2):
                    _starColor = new Color(100, 220, 255);
                    break;
                case (3):
                    _starColor = Color.White;
                    break;
                default:
                    _starColor = Color.White;
                    break;
            }
		}

        public override void Update()
		{
            position.Y += Speed;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
            float objectScale = scale * (Speed + 0.3f) / 3;
            spriteBatch.Draw(spriteTexture, Position, null, _starColor, rotation, origin, objectScale, SpriteEffects.None, 0f);
		}
	}
}

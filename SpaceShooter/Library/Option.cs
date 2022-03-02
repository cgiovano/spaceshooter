using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.GameUtils;

namespace SpaceShooter.Library
{
    class Option
    {
        public OptionType Type { get; private set; }
        public Vector2 Position { get; private set; }

        private Texture2D _texture;

        public Option(Texture2D texture, Vector2 position , OptionType type)
        {
            _texture = texture;
            Position = position;
            Type = type;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
        }

        public void ChangeTextures(Texture2D newTexture)
        {
            _texture = newTexture;
        }
    }
}
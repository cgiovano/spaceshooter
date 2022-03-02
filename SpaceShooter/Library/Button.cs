using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.GameUtils;

namespace SpaceShooter.Library
{
    class Button
    {
        public bool Clicked;
        public string Text;

        private Vector2 _position;
        private Vector2 _size;
        private Texture2D _texture;
        private Rectangle _rectangle;
        private Rectangle _mouseRectangle;
        private Color _buttonColor = new Color(150, 200, 250, 0);

        public Button(Texture2D texture, Vector2 position , string text)
        {
            _texture = texture;
            _size = new Vector2(texture.Width, texture.Height);
            _position = position;
            _rectangle = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
            Text = text;
        }

        public void Update(MouseState mouse)
        {
            _mouseRectangle = new Rectangle(mouse.X, mouse.Y, 10, 10);

            if (_mouseRectangle.Intersects(_rectangle))
            {
                if (_buttonColor.R < 255)
                {
                    _buttonColor.R += 5;
                }
                
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    Clicked = true;
                }
            }
            else if (_buttonColor.R > 155)
            {
                _buttonColor.R -= 5;
                Clicked = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, _buttonColor);
            spriteBatch.DrawString(Fonts.Joystix, Text, new Vector2(_position.X + 5, _position.Y + 5), Color.White);
        }
    }
}
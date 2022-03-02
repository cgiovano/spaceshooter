using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Scenes
{
    public class Credits : IScene
    {
        private Texture2D _brackground;
        private Texture2D _text;

        private int _timer = 0;

        private Vector2 _textPosition;

        private BaseGame _game;

        public Credits(BaseGame game)
        {
            _brackground = Textures.EndGameBackground;
            _text = Textures.Credits;

            _game = game;

            _textPosition = new Vector2(0, 0);
        }

        public void Update(GameTime gameTime)
        {
            _timer += gameTime.ElapsedGameTime.Milliseconds;

            _textPosition.Y -= 0.5f;

            if (_textPosition.Y + _text.Height <= 0)
                _game.ResetGame();
        }

        public void Update(BaseGame game)
        {
            throw new NotImplementedException();
        }

        public void Update(BaseGame game, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_brackground, Vector2.Zero, Color.White);
            spriteBatch.Draw(_text, _textPosition, Color.White);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpaceShooter.Scenes
{
    class Help : IScene
    {
        private BaseGame _game;
        private List<Option> _options = new List<Option>();
        private int _selectedIndex;
        private KeyboardState _ks1;
        private KeyboardState _ks2;
        private Vector2 _pointerPosition;
        private int _intervalTimer;

        public Help(BaseGame game)
        {
            _game = game;
            _options.Add(new Option(Textures.Back, new Vector2(560, 440), OptionType.Menu));
        }

        public void Update(GameTime gameTime)
        {
            _intervalTimer += gameTime.ElapsedGameTime.Milliseconds;
            _ks1 = Keyboard.GetState();

            if (_intervalTimer > 500)
            {
                if (_ks1.IsKeyDown(Keys.Enter) && _ks2.IsKeyUp(Keys.Enter))
                {
                    if (_options[_selectedIndex].Type == OptionType.Menu)
                        _game.CreateMenu();
                }
            }

            _pointerPosition = new Vector2(_options[_selectedIndex].Position.X - (Textures.Pointer.Width + 5), _options[_selectedIndex].Position.Y + Textures.Pointer.Height / 2);
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
            spriteBatch.Draw(Textures.HelpBackground, Vector2.Zero, Color.White);
            spriteBatch.Draw(Textures.HelpScreen, Vector2.Zero, Color.White);

            foreach (var option in _options)
                option.Draw(spriteBatch);

            spriteBatch.Draw(Textures.Pointer, _pointerPosition, Color.White);
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using SpaceShooter.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Scenes
{
    class Transition : IScene
    {
        public bool IsEnded { get; private set; } = false;

        private Texture2D _spriteText;
        private int _timer;

        BaseGame _game;

        public Transition(BaseGame game)
        {
            _game = game;

            switch (GameInformation.CurrentLevel)
            {
                case (Level.Level_1):
                    _spriteText = Textures.Stage1;
                    break;
                case (Level.Level_2):
                    _spriteText = Textures.Stage2;
                    break;
                case (Level.Level_3):
                    _spriteText = Textures.Stage3;
                    break;
                case (Level.Level_4):
                    _spriteText = Textures.Stage4;
                    break;
                case (Level.Level_5):
                    _spriteText = Textures.Stage5;
                    break;
            }
        }

        public void Update(BaseGame game)
        {
            throw new NotImplementedException();
        }

        public void Update(BaseGame game, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            _timer += gameTime.ElapsedGameTime.Milliseconds;

            if (_timer > 2000)
            {
                _game.Start();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
             spriteBatch.Draw(_spriteText, new Vector2(Screen.GetWidth / 2 - _spriteText.Width / 2, Screen.GetHeight / 2 - _spriteText.Height / 2), Color.White);
        }
    }
}

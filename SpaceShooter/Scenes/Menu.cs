using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using SpaceShooter.Scenes;
using System.Collections.Generic;

namespace SpaceShooter
{
	public class Menu : IScene
	{
        private KeyboardState _ks1;
        private KeyboardState _ks2;
        private int _selectedIndex = 0;

        private Option _continue;
        private Option _start;
        private Option _help;
        private Option _settings;
        private Option _quit;

        private Vector2 _pointerPosition;

        List<Option> _options = new List<Option>();

        BaseGame _game;
        private int _intervalTimer;
        private OptionType _eventOption;
        private bool isOnConfirmDialog;

        public Menu(BaseGame game)
		{
            _game = game;
            game.IsMouseVisible = true;

            CreateOptionsMenu();

            /*
            _start = new Option(Textures.Start, new Vector2(Screen.GetWidth / 2 - Textures.Start.Width / 2, Screen.GetHeight / 2), OptionType.NewGame);
            _help = new Option(Textures.Help, new Vector2(Screen.GetWidth / 2 - Textures.Help.Width / 2, Screen.GetHeight / 2 + Textures.Help.Height * 2), OptionType.Help);
            _settings = new Option(Textures.Settings, new Vector2(Screen.GetWidth / 2 - Textures.Settings.Width / 2, Screen.GetHeight / 2 + Textures.Settings.Height * 4), OptionType.Settings);
            _quit = new Option(Textures.Quit, new Vector2(Screen.GetWidth / 2 - Textures.Quit.Width / 2, Screen.GetHeight / 2 + Textures.Quit.Height * 6), OptionType.Quit);

            if (BaseGame.SaveFileExists())
            {
                _continue = new Option(Textures.Continue, new Vector2(Screen.GetWidth / 2 - Textures.Continue.Width / 2, Screen.GetHeight / 2 - Textures.Continue.Height * 2), OptionType.Continue);
                _options.Add(_continue);
            }

            _options.Add(_start);
            _options.Add(_help);
            _options.Add(_settings);
            _options.Add(_quit);

            _pointerPosition = new Vector2(_continue.Position.X - 10, (_continue.Position.Y + Textures.Continue.Height/2) - Textures.Pointer.Height / 2);*/
        }

        public void Update(BaseGame gameTime) { }

        public void Update(BaseGame game, GameTime gameTime) { }

        public void Update(GameTime gameTime)
        {
            /*
             * The Times is necessary to prevent the State of the keyboard trigger one of the options when you back from other Scene.
             * example: when you confirm to back to the main menu in the pause manu in the game.
             */

            _intervalTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (_intervalTimer > 500)
            {
                _ks1 = Keyboard.GetState();

                if (_ks1.IsKeyDown(Keys.W) && _ks2.IsKeyUp(Keys.W))
                {
                    if (_selectedIndex > 0)
                        _selectedIndex--;
                }
                else if (_ks1.IsKeyDown(Keys.S) && _ks2.IsKeyUp(Keys.S))
                {
                    if (_selectedIndex < _options.Count - 1)
                        _selectedIndex++;
                }


                if (!isOnConfirmDialog)
                {
                    if (_ks1.IsKeyDown(Keys.Enter) && _ks2.IsKeyUp(Keys.Enter))
                    {
                        if (_options[_selectedIndex].Type == OptionType.Continue)
                            _game.ContinueGame();
                        if (_options[_selectedIndex].Type == OptionType.NewGame)
                        {
                            if (BaseGame.SaveFileExists())
                                CreateConfirmDialog(_options[_selectedIndex].Type);
                            else
                                _game.CreateNewGame();

                        }
                        if (_options[_selectedIndex].Type == OptionType.Help)
                            _game.CreateHelp();
                        if (_options[_selectedIndex].Type == OptionType.Settings)
                            _game.Settings();
                        if (_options[_selectedIndex].Type == OptionType.Quit)
                            CreateConfirmDialog(_options[_selectedIndex].Type);
                    }
                }
                else
                {
                    if (_ks1.IsKeyDown(Keys.Enter) && _ks2.IsKeyUp(Keys.Enter))
                    {
                        if (_options[_selectedIndex].Type == OptionType.Yes)
                            ConfirmAction();
                        if (_options[_selectedIndex].Type == OptionType.No)
                            CreateOptionsMenu();
                    }
                }
                
                _pointerPosition = new Vector2(_options[_selectedIndex].Position.X - (Textures.Pointer.Width + 5), _options[_selectedIndex].Position.Y + Textures.Pointer.Height / 2);

                _ks2 = _ks1;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.StartScreenBackground, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(Textures.StartScreenLogo, new Vector2(Screen.GetWidth / 2 - Textures.StartScreenLogo.Width / 2, 20), Color.White);

            if (isOnConfirmDialog)
                spriteBatch.Draw(Textures.Confirmation, new Vector2(Screen.GetWidth / 2 - Textures.Confirmation.Width / 2, Screen.GetHeight / 2 - Textures.Confirmation.Height / 2), Color.White);

            foreach (var option in _options)
            {
                option.Draw(spriteBatch);
            }

            spriteBatch.Draw(Textures.Pointer, _pointerPosition, Color.White);
        }

        private void CreateOptionsMenu()
        {
            isOnConfirmDialog = false;
            _selectedIndex = 0;
            _options.Clear();

            if (BaseGame.SaveFileExists())
                _options.Add(new Option(Textures.Continue, new Vector2(Screen.GetWidth / 2 - Textures.Continue.Width / 2, Screen.GetHeight / 2 - Textures.Continue.Height * 2), OptionType.Continue));

            _options.Add(new Option(Textures.Start, new Vector2(Screen.GetWidth / 2 - Textures.Start.Width / 2, Screen.GetHeight / 2), OptionType.NewGame));
            _options.Add(new Option(Textures.Help, new Vector2(Screen.GetWidth / 2 - Textures.Help.Width / 2, Screen.GetHeight / 2 + Textures.Help.Height * 2), OptionType.Help));
            _options.Add(new Option(Textures.Settings, new Vector2(Screen.GetWidth / 2 - Textures.Settings.Width / 2, Screen.GetHeight / 2 + Textures.Settings.Height * 4), OptionType.Settings));
            _options.Add(new Option(Textures.Quit, new Vector2(Screen.GetWidth / 2 - Textures.Quit.Width / 2, Screen.GetHeight / 2 + Textures.Quit.Height * 6), OptionType.Quit));
        }

        private void CreateConfirmDialog(OptionType selectedOption)
        {
            _eventOption = selectedOption;
            isOnConfirmDialog = true;
            _selectedIndex = 0;
            _options.Clear();
            _options.Add(new Option(Textures.Yes, new Vector2(Screen.GetWidth / 2 - Textures.Yes.Width / 2, Screen.GetHeight / 2 + Textures.Yes.Height * 4), OptionType.Yes));
            _options.Add(new Option(Textures.No, new Vector2(Screen.GetWidth / 2 - Textures.No.Width / 2, Screen.GetHeight / 2 + Textures.No.Height * 6), OptionType.No));
        }

        private void ConfirmAction()
        {
            if (_eventOption == OptionType.NewGame)
                _game.CreateNewGame();
            if (_eventOption == OptionType.Quit)
                _game.Exit();
        }
    }
}

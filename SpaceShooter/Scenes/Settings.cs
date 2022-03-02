using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using SpaceShooter.Scenes;
using SpaceShooter.Shared;
using System;
using System.Collections.Generic;

namespace SpaceShooter.Scenes
{
    class Settings : IScene
    {
        BaseGame _game;

        Option _saveAndBack;
        Option _effectsVolume;
        Option _musicVolume;
        Option _fullScreen;
        Option _back;

        List<Option> _options = new List<Option>();

        private int _intervalTimer;
        private KeyboardState _ks1;
        private Vector2 _pointerPosition;
        private KeyboardState _ks2;
        private int _selectedIndex = 0;

        private int _effectsVolumeValue = 0;
        private int _musicVolumeValue = 0;
        private bool _fullScreenValue = false;

        private bool _valuesChanged = false;

        private Vector2 _effectsVolumeValuePosition = new Vector2(300, 80);
        private Vector2 _musicVolumeValuePosition = new Vector2(300, 120);
        private Vector2 _sliderPosition = new Vector2(300, 160);

        public Settings(BaseGame game)
        {
            //game.IsMouseVisible = true;

            _effectsVolumeValue = GameInformation.EffectsVolume;
            _musicVolumeValue = GameInformation.MusicVolume;
            _fullScreenValue = GameInformation.FullScreen;

            _game = game;

            _effectsVolume = new Option(Textures.EffectsVolume, new Vector2(40, 80), OptionType.EffectsVolume);
            _musicVolume = new Option(Textures.MusicVolume, new Vector2(40, 120), OptionType.MusicVolume);
            //_fullScreen = new Option(Textures.FullScreen, new Vector2(40, 160), OptionType.FullScreen);

            _saveAndBack = new Option(Textures.Back, new Vector2(40, 400), OptionType.Menu);
            
            _options.Add(_effectsVolume);
            _options.Add(_musicVolume);
            _options.Add(_saveAndBack);
        }

        public void Update(BaseGame game, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Update(BaseGame game)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
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

                if(_options[_selectedIndex].Type == OptionType.EffectsVolume)
                {
                    if (_ks1.IsKeyDown(Keys.D) && _ks2.IsKeyUp(Keys.D) && _effectsVolumeValue < 100)
                        _effectsVolumeValue += 2;
                    if (_ks1.IsKeyDown(Keys.A) && _ks2.IsKeyUp(Keys.A) && _effectsVolumeValue > 0)
                        _effectsVolumeValue -= 2;
                }
                if (_options[_selectedIndex].Type == OptionType.MusicVolume)
                {
                    if (_ks1.IsKeyDown(Keys.D) && _ks2.IsKeyUp(Keys.D) && _musicVolumeValue < 100)
                        _musicVolumeValue += 2;
                    if (_ks1.IsKeyDown(Keys.A) && _ks2.IsKeyUp(Keys.A) && _musicVolumeValue > 0)
                        _musicVolumeValue -= 2;
                }

                if (_ks1.IsKeyDown(Keys.Enter) && _ks2.IsKeyUp(Keys.Enter))
                {
                    if (_options[_selectedIndex].Type == OptionType.Menu)
                    {
                        if (_valuesChanged)
                        {
                            _game.SaveGameSettings(_effectsVolumeValue, _musicVolumeValue, _fullScreenValue);
                            _game.ApplyChanges();
                        }
                        
                        _game.CreateMenu();
                    }
                }

                if (_effectsVolumeValue != GameInformation.EffectsVolume ||
                    _musicVolumeValue != GameInformation.MusicVolume ||
                    _fullScreenValue != GameInformation.FullScreen)
                {
                    _valuesChanged = true;
                    _saveAndBack.ChangeTextures(Textures.SaveAndBack);
                }

                _pointerPosition = new Vector2(_options[_selectedIndex].Position.X - (Textures.Pointer.Width + 5), _options[_selectedIndex].Position.Y + Textures.Pointer.Height / 2);

                _ks2 = _ks1;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.ConfigurationsBackground, Vector2.Zero, Color.White);

            foreach (var option in _options)
                option.Draw(spriteBatch);

            spriteBatch.DrawString(Fonts.Joystix, _effectsVolumeValue.ToString(),_effectsVolumeValuePosition, Color.White);
            spriteBatch.DrawString(Fonts.Joystix, _musicVolumeValue.ToString(), _musicVolumeValuePosition, Color.White);
            //spriteBatch.DrawString(Fonts.Joystix, _fullScreenValue ? "On" : "Off", new Vector2(_sliderPosition.X + 40, _sliderPosition.Y - 10), Color.White);

            spriteBatch.Draw(Textures.Pointer, _pointerPosition, Color.White);
        }
    }
}

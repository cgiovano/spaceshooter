using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Library;
using SpaceShooter.GameObjects;
using SpaceShooter.GameObjects.Enemies;
using SpaceShooter.Shared;
using System.Linq;
using SpaceShooter.Scenes;
using System;
using System.Collections.Generic;

namespace SpaceShooter
{
    public enum BattleType
    {
        Boss,
        Default
    }

    public class MainGame : IScene
    {
        private int _enemiesDestroyed = 0;
        private int _obstaclesDestroyed = 0;
        private int _obstaclesTimer;
        private int _starsTimer;
        private int _EnemiesTimer;
        private double _intervalTimer;

        private Sequence wave;

        private bool _primeiraVez = true;

        private BattleType _battleType = BattleType.Default;

        private int _spawnTimer = 0;

        //DieScreen configuration values
        private Color _colorUI = new Color(255, 255, 255, 0);
        private Vector2 _playAgainPosition = new Vector2(Screen.GetWidth / 2 - Textures.PlayAgain.Width / 2, Screen.GetHeight - (Textures.PlayAgain.Height + 20));
        private Vector2 _spaceKeyPosition = new Vector2((Screen.GetWidth / 2 - Textures.PlayAgain.Width / 2) + 60, Screen.GetHeight - (Textures.PlayAgain.Height + 20));
        private int _spaceKeyPositionCounter = 0;
        private Vector2 vel = new Vector2(5);
        ///

        private double attackCounter = 0;

        SequenceType[,] waves = new SequenceType[,] { { SequenceType.StartStage, SequenceType.Rush, SequenceType.Interval, SequenceType.Bonus, SequenceType.Interval, SequenceType.Grid, SequenceType.Interval, SequenceType.Asteroid, SequenceType.Interval, SequenceType.Bonus, SequenceType.Interval, SequenceType.Boss, SequenceType.EndStage },
                                                      { SequenceType.StartStage, SequenceType.Asteroid, SequenceType.Interval, SequenceType.Rush, SequenceType.Interval, SequenceType.Grid, SequenceType.Interval, SequenceType.Asteroid, SequenceType.Interval, SequenceType.Bonus, SequenceType.Interval, SequenceType.Boss, SequenceType.EndStage },
                                                      { SequenceType.StartStage, SequenceType.Grid, SequenceType.Interval, SequenceType.Bonus, SequenceType.Interval, SequenceType.Asteroid, SequenceType.Interval, SequenceType.Rush, SequenceType.Interval, SequenceType.Bonus, SequenceType.Interval, SequenceType.Boss, SequenceType.EndStage },
                                                      { SequenceType.StartStage, SequenceType.Asteroid, SequenceType.Interval, SequenceType.Grid, SequenceType.Interval, SequenceType.Rush, SequenceType.Interval, SequenceType.Asteroid, SequenceType.Interval, SequenceType.Rush, SequenceType.Interval, SequenceType.Boss, SequenceType.EndStage },
                                                      { SequenceType.StartStage, SequenceType.Rush, SequenceType.Interval, SequenceType.Bonus, SequenceType.Interval, SequenceType.Grid, SequenceType.Interval, SequenceType.Asteroid, SequenceType.Interval, SequenceType.Bonus, SequenceType.Interval, SequenceType.Boss, SequenceType.EndStage } };

        //SequenceType.StartStage, SequenceType.Rush, SequenceType.Interval, SequenceType.Bonus, SequenceType.Interval, SequenceType.Grid, SequenceType.Interval, SequenceType.Asteroid, SequenceType.Interval, SequenceType.Bonus, SequenceType.Interval, SequenceType.Boss, SequenceType.EndStage

        int counter = 0;

        private int _gridMoveTimer;

        SequenceType _currentWaveType;
        private float _alphaReadyAnimation = 0;
        private Color _readyTextureColor = new Color(255, 255, 255, 255);
        private Vector2 _lifeBarPosition = new Vector2(Screen.GetWidth - Textures.LifeBar.Width - 10, 10);
        private int _respawnTimer = 0;
        private Texture2D[] _weaponsTextures = { Textures.PrimaryWeaponSelection, Textures.SecondaryWeaponSelection, Textures.TerciaryWeaponSelection };

        #region PauseScreenElements
        private KeyboardState _ks1;
        private KeyboardState _ks2;
        private List<Option> _pauseOptions = new List<Option>();
        private Vector2 _pointerPosition;
        private int _selectedIndex = 0;
        private bool _isOnConfirmDialog = false;
        private OptionType _eventOption;
        #endregion

        private BaseGame _game;

        public bool OneUpActivated { get; private set; } = false;

        public static int InGameLives { get; set; }

        public MainGame(BaseGame game, Level level)
        {
            _game = game;
            GameInformation.CurrentLevel = level;
            InGameLives = GameInformation.Lives;
            LoadComponents();
            GameState.currentState = GameState.State.Playing;
            Sounds.PlaySoundTrack();
        }

        private void LoadComponents()
        {
            //General
            Components.CreateComponents();
            GameInformation.Lives = 3;
            LevelConfiguration.CreateLevel(CurrentLevel.GetLevel);

            //Initialize background stars
            Components.Stars.Add(new StarsBackground(StarBackgroundType.Small, new Vector2(0, 0)));
            Components.Stars.Add(new StarsBackground(StarBackgroundType.Small, new Vector2(0, -Textures.StarsSmallTexture.Height)));
            Components.Stars.Add(new StarsBackground(StarBackgroundType.Big, new Vector2(0, 0)));
            Components.Stars.Add(new StarsBackground(StarBackgroundType.Big, new Vector2(0, -Textures.StarsSmallTexture.Height)));

            //Initialize the pause screen elements
            CreateOptionsMenu();
            //_pointerPosition = new Vector2(_pauseOptions[0].Position.X - 10, (_pauseOptions[0].Position.Y + Textures.Continue.Height / 2) - Textures.Pointer.Height / 2);
        }

        private void CreateOptionsMenu()
        {
            _isOnConfirmDialog = false;
            _selectedIndex = 0;
            _pauseOptions.Clear();
            _pauseOptions.Add(new Option(Textures.MainMenu, new Vector2(Screen.GetWidth / 2 - Textures.MainMenu.Width / 2, Screen.GetHeight / 2 + Textures.MainMenu.Height * 4), OptionType.Menu));
            _pauseOptions.Add(new Option(Textures.Quit, new Vector2(Screen.GetWidth / 2 - Textures.Quit.Width / 2, Screen.GetHeight / 2 + Textures.Quit.Height * 6), OptionType.Quit));
        }

        private void CreateConfirmDialog(OptionType selectedOption)
        {
            _eventOption = selectedOption;
            _isOnConfirmDialog = true;
            _selectedIndex = 0;
            _pauseOptions.Clear();
            _pauseOptions.Add(new Option(Textures.Yes, new Vector2(Screen.GetWidth / 2 - Textures.Yes.Width / 2, Screen.GetHeight / 2 + Textures.Yes.Height * 4), OptionType.Yes));
            _pauseOptions.Add(new Option(Textures.No, new Vector2(Screen.GetWidth / 2 - Textures.No.Width / 2, Screen.GetHeight / 2 + Textures.No.Height * 6), OptionType.No));
        }

        private void ConfirmAction()
        {
            if (_eventOption == OptionType.Quit)
                _game.Exit();
            if (_eventOption == OptionType.Menu)
                _game.CreateMenu();
        }

        private void UpdateGameComponents(GameTime gameTime)
        {
            _obstaclesTimer += gameTime.ElapsedGameTime.Milliseconds;
            _starsTimer += gameTime.ElapsedGameTime.Milliseconds;
            _EnemiesTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (wave == null)
            {
                CreateWave();
            }
            else
            {
                switch (wave.Type)
                {
                    case (SequenceType.Grid):
                        GridUpdate(gameTime);
                        break;
                    case (SequenceType.Rush):
                        RushUpdate(gameTime);
                        break;
                    case (SequenceType.Bonus):
                        BonusUpdate(gameTime);
                        break;
                    case (SequenceType.Asteroid):
                        ObstaclesUpdate(gameTime);
                        break;
                    case (SequenceType.Interval):
                        IntervalUpdate(gameTime);
                        break;
                    case (SequenceType.StartStage):
                        IntervalUpdate(gameTime);
                        break;
                    case (SequenceType.EndStage):
                        IntervalUpdate(gameTime);
                        break;
                    case (SequenceType.Boss):
                        BossUpdate(gameTime);
                        break;
                }
            }

            StarsUpdate(gameTime);
            PlayerUpdate(gameTime);
            LaserUpdate(gameTime);
            ExplosionUpdate(gameTime);

            if (Components.ShieldArmature != null)
            {
                if (Components.ShieldArmature.IsDestroyed)
                {
                    Explosion.Create(Components.ShieldArmature.Origin);
                    Components.ShieldArmature = null;
                }
                else
                {
                    Components.ShieldArmature.Update(gameTime);
                }
            }

            if (Components.OneUpBonus != null)
            {
                if (Components.OneUpBonus.IsRemoved)
                    Components.OneUpBonus = null;
                else
                    Components.OneUpBonus.Update(gameTime);
            }

            if (Components.FireParticles != null)
            {
                for(int i = 0; i < Components.FireParticles.Count; i++)
                {
                    Components.FireParticles[i].Update(gameTime);

                    if (Components.FireParticles[i].IsRemoved)
                        Components.FireParticles.RemoveAt(i);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            switch (GameState.currentState)
            {
                case (GameState.State.Playing):
                    UpdateGameComponents(gameTime);
                    break;
                case (GameState.State.GameOver):
                    UpdateGameOverScreen(gameTime);
                    break;
                case (GameState.State.Paused):
                    PauseUpdate(gameTime);
                    break;
            }
        }

        private void UpdateGameOverScreen(GameTime gameTime)
        {
            UpdateGameComponents(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                _game.ContinueGame();
        }

        internal void PauseUpdate(GameTime gameTime)
        {
            _ks1 = Keyboard.GetState();

            if (_ks1.IsKeyDown(Keys.W) && _ks2.IsKeyUp(Keys.W))
            {
                if (_selectedIndex > 0)
                    _selectedIndex--;
            }
            else if (_ks1.IsKeyDown(Keys.S) && _ks2.IsKeyUp(Keys.S))
            {
                if (_selectedIndex < _pauseOptions.Count - 1)
                    _selectedIndex++;
            }

            if (!_isOnConfirmDialog)
            {
                if (_ks1.IsKeyDown(Keys.Enter) && _ks2.IsKeyUp(Keys.Enter))
                {
                    if (_pauseOptions[_selectedIndex].Type == OptionType.Menu)
                        CreateConfirmDialog(_pauseOptions[_selectedIndex].Type);
                    if (_pauseOptions[_selectedIndex].Type == OptionType.Quit)
                        CreateConfirmDialog(_pauseOptions[_selectedIndex].Type);
                }
            }
            else
            {
                if (_ks1.IsKeyDown(Keys.Enter) && _ks2.IsKeyUp(Keys.Enter))
                {
                    if (_pauseOptions[_selectedIndex].Type == OptionType.Yes)
                        ConfirmAction();
                    if (_pauseOptions[_selectedIndex].Type == OptionType.No)
                        CreateOptionsMenu();
                }
            }

            _pointerPosition = new Vector2(_pauseOptions[_selectedIndex].Position.X - (Textures.Pointer.Width + 5), _pauseOptions[_selectedIndex].Position.Y + Textures.Pointer.Height / 2);

            _ks2 = _ks1;
        }

        internal void DrawPauseScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.Paused, new Vector2(Screen.GetWidth / 2 - Textures.Paused.Width / 2, Screen.GetHeight / 4 - Textures.Paused.Height / 2), Color.White);

            if (_isOnConfirmDialog)
                spriteBatch.Draw(Textures.Confirmation, new Vector2(Screen.GetWidth / 2 - Textures.Confirmation.Width / 2, Screen.GetHeight / 2 - Textures.Confirmation.Height / 2), Color.White);

            foreach (var option in _pauseOptions)
            {
                option.Draw(spriteBatch);
            }

            spriteBatch.Draw(Textures.Pointer, _pointerPosition, Color.White);
        }

        void CreateWave()
        {
            if (counter < waves.GetLength(1))
            {
                _currentWaveType = waves[(int)GameInformation.CurrentLevel, counter];
                OneUpActivated = false;
            }
            else
            {
                //if reaches the end of game, creates a credits scene
                if ((int)GameInformation.CurrentLevel == waves.GetLength(0) - 1)
                    _game.Credits();
                else
                {
                    GameInformation.CurrentLevel++;
                    GameInformation.Lives = InGameLives;
                    GameInformation.Life = Components.Player.Energy;
                    wave = null;
                    counter = 0;
                    _game.Save();
                    _game.CreateTransition();
                }
            }

            //var waveType = WaveType.Bonus;
            counter++;

            switch (_currentWaveType)
            {
                case (SequenceType.Grid):
                    wave = new Sequence(SequenceType.Grid);
                    break;
                case (SequenceType.Rush):
                    wave = new Sequence(SequenceType.Rush);
                    break;
                case (SequenceType.Bonus):
                    wave = new Sequence(SequenceType.Bonus);
                    break;
                case (SequenceType.Asteroid):
                    wave = new Sequence(SequenceType.Asteroid);
                    break;
                case (SequenceType.Interval):
                    wave = new Sequence(SequenceType.Interval);
                    break;
                case (SequenceType.StartStage):
                    wave = new Sequence(SequenceType.StartStage);
                    break;
                case (SequenceType.EndStage):
                    wave = new Sequence(SequenceType.EndStage);
                    break;
                case (SequenceType.Boss):
                    wave = new Sequence(SequenceType.Boss);
                    break;
            }

        }

        private void DrawGameComponents(SpriteBatch spriteBatch)
        {
            if (Components.Stars != null && Components.Explosions != null && Components.Obstacles != null && Components.Lasers != null)
            {
                foreach (var star in Components.Stars)
                    star.Draw(spriteBatch);

                if (Components.FireParticles != null)
                {
                    foreach (var particle in Components.FireParticles)
                        particle.Draw(spriteBatch);
                }

                if (Components.Player != null && Components.Player.IsDestroyed == false)
                    Components.Player.Draw(spriteBatch);

                foreach (var laser in Components.Lasers)
                    laser.Draw(spriteBatch);

                foreach (var obstacle in Components.Obstacles)
                    obstacle.Draw(spriteBatch);

                foreach (var inimigo in Components.Enemies)
                    inimigo.Draw(spriteBatch);

                foreach (var gridEnemy in Components.GridEnemies)
                    gridEnemy.Draw(spriteBatch);

                foreach (var bonusEnemy in Components.BonusEnemies)
                    bonusEnemy.Draw(spriteBatch);

                foreach (var boss in Components.Boss)
                {
                    if (boss != null)
                        boss.Draw(spriteBatch);
                }

                if (Components.ShieldArmature != null)
                    Components.ShieldArmature.Draw(spriteBatch);

                if (Components.OneUpBonus != null)
                    Components.OneUpBonus.Draw(spriteBatch);

                Explosion.Draw(spriteBatch);
            }
        }

        private void DrawGameOverScreen(SpriteBatch spriteBatch)
        {
            if (_colorUI.A < 255)
                _colorUI.A += 3;

            if ((int)_spaceKeyPosition.Y > (int)Screen.GetHeight - 40)
                vel = -vel;
            if ((int)_spaceKeyPosition.Y < (int)_playAgainPosition.Y - 5)
                vel = -vel;

            if (_spaceKeyPositionCounter > 16)
            {
                _spaceKeyPosition.Y += vel.Y;
                _spaceKeyPositionCounter = 0;
            }

            _spaceKeyPositionCounter++;

            //spriteBatch.DrawString(Fonts.Joystix, "Game Over", new Vector2(Screen.GetWidth / 2 - 50, Screen.GetHeight / 2), Color.Tomato, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
            spriteBatch.Draw(Textures.GameOver, new Vector2(Screen.GetWidth / 2 - Textures.GameOver.Width / 2, Screen.GetHeight / 2 - Textures.GameOver.Height / 2), _colorUI);
            spriteBatch.Draw(Textures.PlayAgain, _playAgainPosition, _colorUI);
            spriteBatch.Draw(Textures.SpaceKey, _spaceKeyPosition, _colorUI);
            //spriteBatch.DrawString(Fonts.Joystix, "\n\nPress \"space\" to start again", new Vector2(Screen.GetWidth / 2 - 100, Screen.GetHeight / 2), Color.White);
        }

        private void DrawUiComponents(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.LifeBar, _lifeBarPosition, new Rectangle((int)_lifeBarPosition.X, (int)Information.Life, Information.Life, 10), Color.White);
            spriteBatch.Draw(Textures.LifeBarContour, new Vector2(_lifeBarPosition.X - 2, _lifeBarPosition.Y - 2), Color.White);
            spriteBatch.Draw(Textures.Heart, new Vector2(_lifeBarPosition.X - 70, 10), Color.White);
            spriteBatch.DrawString(Fonts.Joystix, " x " + Components.Player.Lives.ToString(), new Vector2(_lifeBarPosition.X - 55, 7), Color.White, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
            spriteBatch.Draw(_weaponsTextures[Components.Player.SelectedGun - 1], new Vector2(10, 10), Color.White);
            //spriteBatch.DrawString(Fonts.Joystix, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString(), new Vector2(10, 30), Color.White);
            spriteBatch.DrawString(Fonts.Joystix, "Score: " + GameInformation.Points.ToString(), new Vector2(Screen.GetWidth / 2 - 100, 10), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            //spriteBatch.DrawString(Fonts.Joystix, "Level: " + GameInformation.CurrentLevel.ToString(), new Vector2(Screen.GetWidth / 2, 30), Color.LightSeaGreen, 0, Vector2.Zero, 1f, SpriteEffects.None, 0); ;

            /*
            if (wave != null)
            {
                
                spriteBatch.DrawString(Fonts.Joystix, "Obstacles Destroyed: " + wave.ObstaclesDestroyed, new Vector2(Screen.GetWidth / 2, 50), Color.LightSeaGreen, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(Fonts.Joystix, "Rush Enemies Destroyed: " + wave.RushEnemiesDestroyed, new Vector2(Screen.GetWidth / 2, 70), Color.LightSeaGreen, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(Fonts.Joystix, "Grid Enemies Destroyed: " + wave.GridEnemiesDestroyed, new Vector2(Screen.GetWidth / 2, 90), Color.LightSeaGreen, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(Fonts.Joystix, "Bonus Enemies Destroyed: " + wave.BonusEnemiesDestroyed, new Vector2(Screen.GetWidth / 2, 110), Color.LightSeaGreen, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(Fonts.Joystix, "OneUp Trigger: " + wave.OneUpTrigger, new Vector2(Screen.GetWidth / 2, 130), Color.LightSeaGreen, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(Fonts.Joystix, "OneUp activated: " + OneUpActivated.ToString(), new Vector2(Screen.GetWidth / 2, 150), Color.LightSeaGreen, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
            }*/

            if (_currentWaveType == SequenceType.Interval || _currentWaveType == SequenceType.StartStage || _currentWaveType == SequenceType.EndStage)
            {
                foreach (var element in Components.UserInterfaceElements)
                {
                    element.Draw(spriteBatch);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawGameComponents(spriteBatch);

            switch (GameState.currentState)
            {
                case (GameState.State.Playing):
                    DrawUiComponents(spriteBatch);
                    break;
                case (GameState.State.GameOver):
                    DrawGameOverScreen(spriteBatch);
                    break;
                case (GameState.State.Paused):
                    DrawPauseScreen(spriteBatch);
                    break;
            }
        }

        private void IntervalUpdate(GameTime gameTime)
        {
            _intervalTimer += gameTime.ElapsedGameTime.TotalSeconds;

            wave.Update(gameTime);

            if (wave.IsEnded == true)
            {
                wave = null;

                _intervalTimer = 0;
            }
        }

        private void ObstaclesUpdate(GameTime gameTime)
        {
            if (Components.Obstacles != null)
            {
                if (!wave.IsEnded)
                    wave.Update(gameTime);

                foreach (var obstacle in Components.Obstacles)
                    obstacle.Update(gameTime);

                for (int i = 0; i < Components.Obstacles.Count; i++)
                {
                    var obstacle = Components.Obstacles[i];

                    if (obstacle.IsRemoved == true)
                        Components.Obstacles.RemoveAt(i);

                    if (obstacle.IsDestroyed == true)
                    {
                        if (wave.OneUpTrigger == wave.ObstaclesDestroyed && OneUpActivated == false)
                        {
                            Components.OneUpBonus = new OneUp(obstacle.Origin);
                            OneUpActivated = true;
                        }

                        Explosion.Create(obstacle.Origin);

                        Components.Obstacles.RemoveAt(i);
                        wave.ObstaclesDestroyed += 1;
                        GameInformation.Points += 10;    // Preciso mudar isso
                    }
                }

                if (wave.IsEnded && Components.Obstacles.Count < 1)
                {
                    wave = null;
                }
            }
        }

        private void StarsUpdate(GameTime gameTime)
        {
            foreach (var starBackground in Components.Stars)
                starBackground.Update(gameTime);

            for (int i = 0; i < Components.Stars.Count; i++)
            {
                var starBackground = Components.Stars[i];

                if (starBackground.IsRemoved == true)
                {
                    if (starBackground.StarType == StarBackgroundType.Small)
                        Components.Stars.Add(new StarsBackground(StarBackgroundType.Small, new Vector2(0, -Textures.StarsSmallTexture.Height)));
                    else
                        Components.Stars.Add(new StarsBackground(StarBackgroundType.Big, new Vector2(0, -Textures.StarsSmallTexture.Height)));

                    Components.Stars.RemoveAt(i);
                }
            }
        }

        private void RushUpdate(GameTime gameTime)
        {
            if (Components.Enemies != null)
            {
                if (!wave.IsEnded)
                    wave.Update(gameTime);

                foreach (var enemy in Components.Enemies)
                    enemy.Update(gameTime);


                for (int i = 0; i < Components.Enemies.Count; i++)
                {
                    var enemy = Components.Enemies[i];

                    if (enemy.IsRemoved == true)
                        Components.Enemies.RemoveAt(i);

                    if (enemy.IsDestroyed == true)
                    {
                        if (wave.OneUpTrigger == wave.RushEnemiesDestroyed && OneUpActivated == false)
                        {
                            Components.OneUpBonus = new OneUp(enemy.Origin);
                            OneUpActivated = true;
                        }

                        //Vector2 origin = new Vector2(enemy.Position.X + enemy.Rectangle.Width / 2, enemy.Position.Y + enemy.Rectangle.Height / 2);

                        Explosion.Create(enemy.Origin);

                        Components.Enemies.RemoveAt(i);
                        wave.RushEnemiesDestroyed += 1;
                        GameInformation.Points += 10;    // Preciso mudar isso
                    }
                }

                if (wave.IsEnded && Components.Enemies.Count < 1)
                {
                    wave = null;
                }
            }
        }

        private void BonusUpdate(GameTime gameTime)
        {
            if (Components.BonusEnemies != null)
            {
                if (!wave.IsEnded)
                    wave.Update(gameTime);

                foreach (var enemy in Components.BonusEnemies)
                {
                    enemy.Update(gameTime);
                }

                for (int i = 0; i < Components.BonusEnemies.Count; i++)
                {
                    var enemy = Components.BonusEnemies[i];

                    if (enemy.IsRemoved == true)
                        Components.BonusEnemies.RemoveAt(i);

                    if (enemy.IsDestroyed == true)
                    {
                        //Vector2 origin = new Vector2(enemy.Position.X + Textures.Enemy_A.Width / 2, enemy.Position.Y + Textures.Enemy_A.Height / 2);

                        Explosion.Create(enemy.Origin);

                        Components.BonusEnemies.RemoveAt(i);
                        wave.BonusEnemiesDestroyed += 1;
                        GameInformation.Points += 10;    // Preciso mudar isso
                    }
                }
            }

            if (wave.IsEnded)
            {
                Components.BonusEnemies.Clear();
                wave = null;
                _enemiesDestroyed = 0;
            }
        }

        private void GridUpdate(GameTime gameTime)
        {
            int counter = 0;
            attackCounter += gameTime.ElapsedGameTime.TotalSeconds;
            _gridMoveTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (_gridMoveTimer > 800)
            {
                Grid.UpdateGridPosition();

                _gridMoveTimer = 0;
            }
            if (Components.GridEnemies != null)
            {
                if (!wave.IsEnded)
                    wave.Update(gameTime);

                foreach (var enemy in Components.GridEnemies)
                {
                    //enemy.UpdateSpline();
                    enemy.Update(gameTime);

                    if (GameState.currentState == GameState.State.GameOver)
                        enemy.CanAttack = false;
                }

                for (int i = 0; i < Components.GridEnemies.Count; i++)
                {
                    var enemy = Components.GridEnemies[i];

                    if (enemy.IsRemoved == true)
                        Components.GridEnemies.RemoveAt(i);

                    if (enemy.IsDestroyed == true)
                    {
                        if (wave.OneUpTrigger == wave.GridEnemiesDestroyed && OneUpActivated == false)
                        {
                            Components.OneUpBonus = new OneUp(enemy.Origin);
                            OneUpActivated = true;
                        }

                        //Vector2 origin = new Vector2(enemy.Position.X + Textures.Enemy_A.Width / 2, enemy.Position.Y + Textures.Enemy_A.Height / 2);

                        Explosion.Create(enemy.Origin);

                        Components.GridEnemies.RemoveAt(i);
                        wave.GridEnemiesDestroyed += 1;
                        GameInformation.Points += 10;    // Preciso mudar isso
                    }
                }


                if (wave.IsEnded && Components.GridEnemies.Count > 0)
                {
                    if (attackCounter > 5)
                    {
                        if (Components.GridEnemies.All(enemy => enemy.IsPositioned) && !Components.Player.IsDestroyed)
                        {
                            GridEnemyAttackHelper();
                            attackCounter = 0;
                        }
                    }
                }
            }

            if (wave.IsEnded && Components.GridEnemies.Count <= 0)
            {
                wave = null;
                _enemiesDestroyed = 0;
            }
        }

        private void PlayerUpdate(GameTime gameTime)
        {
            //if (GameState.currentState != GameState.State.GameOver && GameInformation.Lives <= 0 && Components.Player.IsDestroyed == true)

            if (Components.Player != null)
            {
                if (Components.Player.IsDestroyed == true)
                {
                    if (InGameLives > 1)
                    {
                        _respawnTimer += gameTime.ElapsedGameTime.Milliseconds;

                        if (_respawnTimer > 2000)
                        {
                            InGameLives--;
                            Components.Player = new PlayerShip(Textures.PlayerShipTextureCombined, true, InGameLives);
                            _respawnTimer = 0;
                        }
                    }
                    else
                    {
                        GameState.currentState = GameState.State.GameOver;
                    }
                }
                else
                {
                    if (_currentWaveType == SequenceType.Interval)
                        Components.Player.Update(gameTime, false);
                    else
                        Components.Player.Update(gameTime, true);

                    if (Components.Player.IsBlinking == false)
                    {
                        switch (_currentWaveType)
                        {
                            case (SequenceType.Asteroid):
                                Components.Player.UpdateObstacleDamage();
                                break;
                            case (SequenceType.Bonus):
                                Components.Player.UpdateBonusEnemyDamage();
                                break;
                            case (SequenceType.Grid):
                                Components.Player.UpdateGridEnemyDamage();
                                break;
                            case (SequenceType.Rush):
                                Components.Player.UpdateRushEnemyDamage();
                                break;
                            case (SequenceType.Boss):
                                Components.Player.UpdateBossEnemyDamage();
                                break;
                        }

                        Components.Player.CheckEnemyLaserDamage();
                    }
                }
            }
        }

        private void LaserUpdate(GameTime gameTime)
        {
            if (Components.Lasers != null)
            {
                foreach (var laser in Components.Lasers)
                    laser.Update(gameTime);

                for (int i = 0; i < Components.Lasers.Count; i++)
                {
                    var laser = Components.Lasers[i];

                    if (laser.IsRemoved == true || laser.IsEnded == true)
                        Components.Lasers.RemoveAt(i);
                }
            }
        }

        private void ExplosionUpdate(GameTime gameTime)
        {
            Explosion.Update(gameTime);
        }

        private void OneUpUpdate(GameTime gameTime)
        {

        }

        private void BossUpdate(GameTime gameTime)
        {
            if (!wave.IsEnded)
                wave.Update(gameTime);


            for (int i = 0; i < Components.Boss.Count; i++)
            {
                var boss = Components.Boss[i];

                if (boss.IsDestroyed)
                {
                    Components.Boss.RemoveAt(i);
                    Explosion.Create(boss.Origin);
                }
            }

            if (wave.IsEnded)
            {
                wave = null;
            }
        }

        private void GridEnemyAttackHelper()
        {
            int rndAttackPattern = Library.RandomMod.GetRandom(0, 2);
            Point2D[] attackPattern = Patterns.AttackPattern_A.ToArray();

            if (rndAttackPattern == 0)
                attackPattern = Patterns.AttackPattern_A.ToArray();
            else if (rndAttackPattern == 1)
                attackPattern = Patterns.AttackPattern_B.ToArray();

            if (GameInformation.CurrentLevel < Level.Level_3)
            {
                int index = Library.RandomMod.GetRandom(0, Components.GridEnemies.Count - 1);

                if (Components.Player.Position.X > Screen.GetWidth / 2)
                    Components.GridEnemies[index].CreateAttackPatternSpline(attackPattern, true);
                else
                    Components.GridEnemies[index].CreateAttackPatternSpline(attackPattern, false);

                Components.GridEnemies[index].CanAttack = true;
                Components.GridEnemies[index].IsPositioned = false;
            }
            else if (GameInformation.CurrentLevel >= Level.Level_3 && GameInformation.CurrentLevel <= Level.Level_5)
            {
                if (Components.GridEnemies.Count >= 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        int index = Library.RandomMod.GetRandom(0, Components.GridEnemies.Count - 1);

                        if (Components.GridEnemies[index].CanAttack == false)
                        {
                            if (Components.Player.Position.X > Screen.GetWidth / 2)
                                Components.GridEnemies[index].CreateAttackPatternSpline(attackPattern, true);
                            else
                                Components.GridEnemies[index].CreateAttackPatternSpline(attackPattern, false);

                            Components.GridEnemies[index].CanAttack = true;
                            Components.GridEnemies[index].IsPositioned = false;
                        }
                        else
                        {
                            index = Library.RandomMod.GetRandom(0, Components.GridEnemies.Count - 1);
                        }
                    }
                }
                else if (Components.GridEnemies.Count < 4 && Components.GridEnemies.Count > 0)
                {
                    int index = Components.GridEnemies.Count - 1;

                    if (Components.GridEnemies[index].CanAttack == false && Components.GridEnemies[index].IsPositioned == true)
                    {
                        if (Components.Player.Position.X > Screen.GetWidth / 2)
                            Components.GridEnemies[index].CreateAttackPatternSpline(attackPattern, true);
                        else
                            Components.GridEnemies[index].CreateAttackPatternSpline(attackPattern, false);

                        Components.GridEnemies[index].CanAttack = true;
                        Components.GridEnemies[index].IsPositioned = false;
                    }
                }
            }

        }

        public void Update(BaseGame game)
        {
            throw new System.NotImplementedException();
        }

        public void Update(BaseGame game, GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }
    }
}

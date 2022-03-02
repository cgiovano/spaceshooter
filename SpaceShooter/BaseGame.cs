using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SpaceShooter.FX;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using SpaceShooter.Scenes;
using SpaceShooter.Shared;
using System;
using System.IO;
using System.Xml;

namespace SpaceShooter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class BaseGame : Game
    {
        public static bool NewGame = false;

        private int _width = 640;
        private int _height = 480;
        private int _virtualWidth = 960;
        private int _virtualHeight = 720;
        private bool _halfRes = false;
        private float _avgFps = 0;
        private bool _isActiveWindow = true;
        private BloomFilter _bloomFilter;
        private float _scaleFactorHeight = 1;
        private float _scaleFactorWidth = 1;

        private Vector2 _renderTargetPosition = new Vector2(0, 0);

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Color backgroundColor = new Color(23, 17, 32);

        Menu _menu;
        Settings _settings;
        MainGame _game;
        Transition _transition;
        Screen screen;

        GraphicsDevice graphicsDevice;
        RenderTarget2D renderTarget;

        KeyboardState _ks1, _ks2;

        Textures textures;
        Sounds sounds;
        Fonts fonts;

        public IScene CurrentScene;

        private string _pathFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SpaceHunter";
        private string _saveFileName = "save.xml";
        private string _settingsFileName = "save.xml";

        public BaseGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true;
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            graphics.IsFullScreen = false;

            graphics.PreferredBackBufferWidth = _virtualWidth;
            graphics.PreferredBackBufferHeight = _virtualHeight;

            //_renderTargetPosition = new Vector2(graphics.PreferredBackBufferWidth / 2 - _width / 2, graphics.PreferredBackBufferHeight / 2 - _height / 2);
            _renderTargetPosition = new Vector2(Window.ClientBounds.Width / 2 - _width / 2, Window.ClientBounds.Height / 2 - _height / 2);
            //_scaleFactor = (float)graphics.PreferredBackBufferWidth / (float)_width;

            graphics.ApplyChanges();

            //Console.WriteLine(Window.ClientBounds.Width);

            //Frametime not limited to 16.66 Hz / 60 FPS
            IsFixedTimeStep = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.GraphicsProfile = GraphicsProfile.Reach;
            IsMouseVisible = false;

            //Active window?
            Activated += IsActivated;
            Deactivated += IsDeactivated;

            GameState.currentState = GameState.State.Menu;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            graphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.HiDef, new PresentationParameters());
            renderTarget = new RenderTarget2D(graphicsDevice, _width, _height);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textures = new Textures(Content);
            sounds = new Sounds(Content);
            fonts = new Fonts(Content);

            // TODO: use this.Content to load your game content here
            screen = new Screen(graphics, renderTarget.Width, renderTarget.Height);

            if(SaveGameSeetingsFileExists())
            {
                LoadGameSettings();
            }
            else
            {
                CreateGameSettings();
            }

            ApplyChanges();

            // Initialize bloom filter
            _bloomFilter = new BloomFilter();
            _bloomFilter.Load(GraphicsDevice, Content, _width, _height);
            _bloomFilter.BloomPreset = BloomFilter.BloomPresets.Focussed;
            _bloomFilter.BloomThreshold = 0.02f;
            _bloomFilter.BloomStrengthMultiplier = 1.16f;

            //creates the menu
            CreateMenu();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            _bloomFilter.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _ks1 = Keyboard.GetState();
            MouseState mstate = Mouse.GetState();

            if (GameState.currentState != GameState.State.Paused)
            {
                if (_ks1.IsKeyDown(Keys.P) && _ks2.IsKeyUp(Keys.P) && GameState.currentState != GameState.State.GameOver)
                    GameState.currentState = GameState.State.Paused;
            }
            else if (GameState.currentState == GameState.State.Paused)
            {
                if (_ks1.IsKeyDown(Keys.P) && _ks2.IsKeyUp(Keys.P))
                    GameState.currentState = GameState.State.Playing;
            }

            _ks2 = _ks1;

            CurrentScene.Update(gameTime);

            /*
            if (_ks1.IsKeyDown(Keys.F1)) _bloomFilter.BloomPreset = BloomFilter.BloomPresets.Wide;
            if (_ks1.IsKeyDown(Keys.F2)) _bloomFilter.BloomPreset = BloomFilter.BloomPresets.SuperWide;
            if (_ks1.IsKeyDown(Keys.F3)) _bloomFilter.BloomPreset = BloomFilter.BloomPresets.Focussed;
            if (_ks1.IsKeyDown(Keys.F4)) _bloomFilter.BloomPreset = BloomFilter.BloomPresets.Small;
            if (_ks1.IsKeyDown(Keys.F5)) _bloomFilter.BloomPreset = BloomFilter.BloomPresets.Cheap;

            if (_ks1.IsKeyDown(Keys.F9)) _bloomFilter.BloomStreakLength = 1;
            if (_ks1.IsKeyDown(Keys.F10)) _bloomFilter.BloomStreakLength = 2;

            if (_ks1.IsKeyDown(Keys.F7)) _halfRes = true;
            if (_ks1.IsKeyDown(Keys.F8)) _halfRes = false;

            if (mstate.LeftButton == ButtonState.Pressed)
            {
                float x = (float)mstate.X / Window.ClientBounds.Width;
                _bloomFilter.BloomThreshold = x;

                float y = (float)mstate.Y / Window.ClientBounds.Height * 2;

                //clamp

                _bloomFilter.BloomThreshold = x;
                _bloomFilter.BloomStrengthMultiplier = y;
            }*/

            float fps = 0;
            if (gameTime.ElapsedGameTime.TotalMilliseconds > 0)
                fps = (float)Math.Round(1000 / (gameTime.ElapsedGameTime.TotalMilliseconds), 1);

            //Set _avgFPS to the first fps value when started.
            if (_avgFps < 0.01f) _avgFps = fps;

            //Average over 20 frames
            _avgFps = _avgFps * 0.95f + fps * 0.05f;

            Window.Title = "Space Hunter" + "  |   FPS:   " + _avgFps + "  |  Width: " + Window.ClientBounds.Width + "  |  Height: " + Window.ClientBounds.Height + "  |  Scale Factor: " + _scaleFactorHeight.ToString(); /* "BloomFilter Preset: " + _bloomFilter.BloomPreset +
                           " with " + _bloomFilter.BloomDownsamplePasses +
                           " Passes | Threshold: " + Math.Round(_bloomFilter.BloomThreshold, 2) +
                           "Strength: " + Math.Round(_bloomFilter.BloomStrengthMultiplier, 2) +
                           " | Half res: " + _halfRes +
                           " | Streaks: " + _bloomFilter.BloomStreakLength +
                           " | FPS : " + _avgFps;*/

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            int w = (int)(_width * (float)_scaleFactorWidth);
            int h = (int)(_height * (float)_scaleFactorHeight);

            //For performance reasons we might want to calculate the blur in half resolution (or lower!)
            if (_halfRes)
            {
                w /= 2;
                h /= 2;
            }

            //Default 
            _bloomFilter.BloomUseLuminance = false;

            Texture2D bloom = _bloomFilter.Draw(renderTarget, w, h);
            
            DrawSceneToRenderTarget(renderTarget);
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(1.5f, 1.5f, 0));
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, null, null, null, Matrix.CreateScale((float)_scaleFactorWidth, (float)_scaleFactorHeight, 0));
            spriteBatch.Draw(renderTarget, _renderTargetPosition, Color.White);
            spriteBatch.Draw(bloom, new Rectangle((int)_renderTargetPosition.X, (int)_renderTargetPosition.Y, _width, _height), Color.White);
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        void DrawSceneToRenderTarget(RenderTarget2D target)
        {
            GraphicsDevice.SetRenderTarget(target);
            //graphicsDevice.Clear(backgroundColor);
            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Matrix.CreateScale(1f));

            CurrentScene.Draw(spriteBatch);

            spriteBatch.End();
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            float outputAspect = (float)Window.ClientBounds.Width / (float)Window.ClientBounds.Height;
            float preferredAspect = 640 / 480;

            //Window.ClientSizeChanged -= Window_ClientSizeChanged;

            /*
            graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            _scaleFactorHeight = (float)Window.ClientBounds.Height / (float)_height;
            graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            _scaleFactorWidth = (float)Window.ClientBounds.Width / (float)_width;*/

            //_virtualWidth = Window.ClientBounds.Width;
            //_virtualHeight = Window.ClientBounds.Height;

            //graphics.ApplyChanges();

            //_scaleFactorHeight = (float)Window.ClientBounds.Height / (float)_height;
            //_scaleFactorWidth = (float)Window.ClientBounds.Width / (float)_width;

            
            if ((float)Window.ClientBounds.Height / 480 <= 4 / 3)
            {
                _scaleFactorHeight = (float)Window.ClientBounds.Height / (float)_height;
            }
            else if ((float) Window.ClientBounds.Width / 640 >= 4 / 3)
            {
                _scaleFactorWidth = (float)Window.ClientBounds.Width / (float)_width;
            }

            graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            _renderTargetPosition = new Vector2(Window.ClientBounds.Width / 2 - (_width * _scaleFactorWidth) / 2, Window.ClientBounds.Height / 2 - (_height * _scaleFactorHeight) / 2);

            //_bloomFilter.UpdateResolution(_width, _height);

            //scaleFactor = rs > ri ? (wi * hs / hi, hs) : (ws, hi * ws / wi);

            //if (_virtualWidth)
        }

        public void Start() => CurrentScene = new MainGame(this, GameInformation.CurrentLevel);

        public void CreateTransition() => CurrentScene = new Transition(this);

        public void CreateMenu() => CurrentScene = new Menu(this);

        public void CreateHelp() => CurrentScene = new Help(this);

        public void Quit() => Exit();

        public void Credits() => CurrentScene = new Credits(this);

        public void ResetGame()
        {
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SpaceHunter\\save.xml");
            CreateMenu();
        }

        public void CreateNewGame()
        {
            GameUtils.Components.CreateComponents();

            Create();

            CreateTransition();
        }

        public void ContinueGame()
        {
            // Load data from last checkpoint or save point/file;
            Load();

            GameUtils.Components.ResetComponents();

            CreateTransition();
        }

        public void ApplyChanges()
        {
            SoundEffect.MasterVolume = (float)GameInformation.EffectsVolume / 100f;
            MediaPlayer.Volume = (float)GameInformation.MusicVolume / 100f;
        }

        public void SaveGameSettings(int effectsVolume, int musicVolume, bool isFullScreen)
        {
            GameInformation.EffectsVolume = effectsVolume;
            GameInformation.MusicVolume = musicVolume;
            GameInformation.FullScreen = isFullScreen;

            XmlDocument saveFile = new XmlDocument();
            XmlNode gameSettings = saveFile.CreateElement("GameSettings");
            saveFile.AppendChild(gameSettings);

            XmlNode effectsVolumeNode = saveFile.CreateElement("EffectsVolume");
            effectsVolumeNode.InnerText = GameInformation.MusicVolume.ToString();
            gameSettings.AppendChild(effectsVolumeNode);

            XmlNode musicVolumeNode = saveFile.CreateElement("MusicVolume");
            musicVolumeNode.InnerText = GameInformation.MusicVolume.ToString();
            gameSettings.AppendChild(musicVolumeNode);

            XmlNode fullScreenNode = saveFile.CreateElement("FullScreen");
            fullScreenNode.InnerText = GameInformation.FullScreen.ToString();
            gameSettings.AppendChild(fullScreenNode);

            if (!Directory.Exists(_pathFolder))
                Directory.CreateDirectory(_pathFolder);

            saveFile.Save(_pathFolder + "\\settings.xml");
        }

        public void CreateGameSettings()
        {
            XmlDocument saveFile = new XmlDocument();
            XmlNode gameSettings = saveFile.CreateElement("GameSettings");
            saveFile.AppendChild(gameSettings);

            XmlNode effectsVolumeNode = saveFile.CreateElement("EffectsVolume");
            effectsVolumeNode.InnerText = "100";
            gameSettings.AppendChild(effectsVolumeNode);

            XmlNode musicVolumeNode = saveFile.CreateElement("MusicVolume");
            musicVolumeNode.InnerText = "100";
            gameSettings.AppendChild(musicVolumeNode);

            XmlNode fullScreenNode = saveFile.CreateElement("FullScreen");
            fullScreenNode.InnerText = "false";
            gameSettings.AppendChild(fullScreenNode);


            if (!Directory.Exists(_pathFolder))
                Directory.CreateDirectory(_pathFolder);

            saveFile.Save(_pathFolder + "\\settings.xml");
        }

        public void LoadGameSettings()
        {
            XmlDocument saveFile = new XmlDocument();

            if (!Directory.Exists(_pathFolder))
                Directory.CreateDirectory(_pathFolder);

            saveFile.Load(_pathFolder + "\\settings.xml");

            XmlNodeList nodeList = saveFile.SelectNodes("/GameSettings");

            foreach (XmlNode node in nodeList)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name == "EffectsVolume")
                        GameInformation.EffectsVolume = int.Parse(child.InnerText);
                    if (child.Name == "MusicVolume")
                        GameInformation.MusicVolume = int.Parse(child.InnerText);
                    if (child.Name == "FullScreen")
                        GameInformation.FullScreen = bool.Parse(child.InnerText);
                }
            }
        }

        public void Settings() => CurrentScene = new Settings(this);

        private void IsDeactivated(object sender, EventArgs e) => _isActiveWindow = false;

        private void IsActivated(object sender, EventArgs e) => _isActiveWindow = true;

        public static bool SaveFileExists()
        {
            return (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SpaceHunter\\save.xml"));
        }

        public static bool SaveGameSeetingsFileExists()
        {
            return (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SpaceHunter\\settings.xml"));
        }

        public void Create()
        {
            XmlDocument saveFile = new XmlDocument();
            XmlNode gameInformation = saveFile.CreateElement("GameInformation");
            saveFile.AppendChild(gameInformation);

            XmlNode life = saveFile.CreateElement("Life");
            life.InnerText = "100";
            gameInformation.AppendChild(life);

            XmlNode lives = saveFile.CreateElement("Lives");
            lives.InnerText = "3";
            gameInformation.AppendChild(lives);

            XmlNode pointNode = saveFile.CreateElement("Points");
            pointNode.InnerText = "0";
            gameInformation.AppendChild(pointNode);

            XmlNode stageNode = saveFile.CreateElement("Stage");
            stageNode.InnerText = "Level_1";
            gameInformation.AppendChild(stageNode);

            if (!Directory.Exists(_pathFolder))
                Directory.CreateDirectory(_pathFolder);

            saveFile.Save(_pathFolder + "\\save.xml");

            Load();
        }

        public void Save()
        {
            XmlDocument saveFile = new XmlDocument();
            XmlNode gameInformation = saveFile.CreateElement("GameInformation");
            saveFile.AppendChild(gameInformation);

            XmlNode life = saveFile.CreateElement("Life");
            life.InnerText = GameInformation.Life.ToString();
            gameInformation.AppendChild(life);

            XmlNode lives = saveFile.CreateElement("Lives");
            lives.InnerText = GameInformation.Lives.ToString();
            gameInformation.AppendChild(lives);

            XmlNode pointNode = saveFile.CreateElement("Points");
            pointNode.InnerText = GameInformation.Points.ToString();
            gameInformation.AppendChild(pointNode);

            XmlNode stageNode = saveFile.CreateElement("Stage");
            stageNode.InnerText = GameInformation.CurrentLevel.ToString();
            gameInformation.AppendChild(stageNode);


            if (!Directory.Exists(_pathFolder))
                Directory.CreateDirectory(_pathFolder);

            saveFile.Save(_pathFolder + "\\save.xml");
        }

        public void Load()
        {
            XmlDocument saveFile = new XmlDocument();
            
            if (!Directory.Exists(_pathFolder))
                Directory.CreateDirectory(_pathFolder);

            saveFile.Load(_pathFolder + "\\save.xml");

            XmlNodeList nodeList = saveFile.SelectNodes("/GameInformation");

            foreach (XmlNode node in nodeList)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name == "Life")
                        GameInformation.Life = int.Parse(child.InnerText);
                    if (child.Name == "Lives")
                        GameInformation.Lives = int.Parse(child.InnerText);
                    if (child.Name == "Points")
                        GameInformation.Points = int.Parse(child.InnerText);
                    if (child.Name == "Stage")
                    {
                        switch (child.InnerText)
                        {
                            case ("Level_1"):
                                GameInformation.CurrentLevel = Level.Level_1;
                                break;
                            case ("Level_2"):
                                GameInformation.CurrentLevel = Level.Level_2;
                                break;
                            case ("Level_3"):
                                GameInformation.CurrentLevel = Level.Level_3;
                                break;
                            case ("Level_4"):
                                GameInformation.CurrentLevel = Level.Level_4;
                                break;
                            case ("Level_5"):
                                GameInformation.CurrentLevel = Level.Level_5;
                                break;
                        }
                    }
                }
            }
        }
    }
}

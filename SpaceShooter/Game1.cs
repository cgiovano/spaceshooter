using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using SpaceShooter.Scenes;

namespace SpaceShooter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static bool NewGame = true;
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Color backgroundColor = new Color(23, 17, 32);

        Menu _menu;
        Settings _settings;
        MainGame _game;
        Screen screen;

        GraphicsDevice graphicsDevice;
        RenderTarget2D renderTarget;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;
            Window.AllowUserResizing = true;

            GameState.currentState = GameState.State.Playing;
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
            renderTarget = new RenderTarget2D(graphicsDevice, 400, 240);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            var textures = new Textures(Content);
            var sounds = new Sounds(Content);
            var fonts = new Fonts(Content);

            // TODO: use this.Content to load your game content here
            screen = new Screen(graphics, renderTarget.Width, renderTarget.Height);

            //_menu = new Menu(this);
            CreateNewGame();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (GameState.currentState != GameState.State.Paused && state.IsKeyDown(Keys.P))
                GameState.currentState = GameState.State.Paused;
            else if (GameState.currentState == GameState.State.Paused && state.IsKeyDown(Keys.P))
                GameState.currentState = GameState.State.Playing;

            switch (GameState.currentState)
            {
                case (GameState.State.Menu):
                    if (_menu == null)
                    {
                        _settings = null;
                        _menu = new Menu(this);
                    }
                    else
                        _menu.Update();
                    break;
                case (GameState.State.Settings):
                    if (_settings == null)
                    {
                        _menu = null;
                        _settings = new Settings(this);
                    }
                    _settings.Update();
                    break;
                case (GameState.State.Playing):
                    if (NewGame == true)
                    {
                        _game = null;
                        _settings = null;
                        _menu = null;
                        GameUtils.Components.ResetComponents();
                        CreateNewGame();
                        NewGame = false;
                    }
                    else if (GameState.currentState != GameState.State.Paused)
                    {
                        _game.UpdateComponents(gameTime);
                    }
                    break;
                default:
                    break;
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            DrawSceneToRenderTarget(renderTarget);
            base.Draw(gameTime);

            GraphicsDevice.Clear(Color.Red);
            //spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(2f));
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(3.5f, 3.5f, 0));
            spriteBatch.Draw(renderTarget, new Vector2(0, 0), Color.White);
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        void DrawSceneToRenderTarget(RenderTarget2D target)
        {
            GraphicsDevice.SetRenderTarget(target);
            // TODO: Add your drawing code here
            graphicsDevice.Clear(backgroundColor);
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(1f));

            if (GameState.currentState == GameState.State.Menu)
            {
                if (_menu != null)
                    _menu.Draw(spriteBatch);
            }

            if (GameState.currentState == GameState.State.Settings)
            {
                if (_settings != null)
                    _settings.Draw(spriteBatch);
            }

            if (GameState.currentState == GameState.State.Playing || GameState.currentState == GameState.State.Paused)
            {
                if (_game != null)
                    _game.DrawComponents(spriteBatch);
            }
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
        }

        public void CreateNewGame()
        {
            GameUtils.Components.CreateComponents();
            _game = new MainGame(Level.Level_1);
        }
    }
}

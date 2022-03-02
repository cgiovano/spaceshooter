using Microsoft.Xna.Framework;

namespace SpaceShooter.Library
{
    class Screen
    {
        public static float Scale { get; set; } = 1f;

        public static int GetWidth { get { return (_width); } }
        public static int GetHeight { get { return (_height); } }

        static int _width;
        static int _height;

        GraphicsDeviceManager graphicsManager;

        public Vector2 RealScreenSize()
        {
            _width = graphicsManager.PreferredBackBufferWidth;
            _height = graphicsManager.PreferredBackBufferHeight;

            return (new Vector2(_width, _height));
        }

        public Screen(GraphicsDeviceManager graphicsManager, int targetFrameWidth, int targetFrameHeight)
        {
            this.graphicsManager = graphicsManager;
            _width = targetFrameWidth;
            _height = targetFrameHeight;
        }
    }
}

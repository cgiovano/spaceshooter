using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Library
{
    public class GameObject
    {
        public Vector2 Position { get { return (new Vector2(position.X, position.Y)); } }
        public Vector2 Origin { get { return (new Vector2(position.X + frameWidth / 2, position.Y + frameHeight / 2)); } }
        public Rectangle Rectangle { get { return (rectangle); } }
        public Color[] ColorMap { get { return (colorMap); } }
        public Texture2D SpriteTexture { get { return (spriteTexture); } }
        public int Energy { get; set; }
        public float Speed { get; set; } = .5f;
		public bool IsRemoved { get; set; } = false;
        public bool IsDestroyed { get; set; } = false;
        public float ScaleFactor { get; set; } = 1f;
        public bool IsEnded { get; internal set; }
        public int Points { get; }

        protected int points;
        protected Vector2 position;
        protected Vector2 velocity;
        protected Vector2 origin;
        protected Rectangle rectangle;
        protected Color[] colorMap;
        protected Texture2D spriteTexture;
        protected int frameWidth;
        protected int frameHeight;
        protected int numberOfFrames;
        protected bool isHitted = false;
        protected float rotation;
        protected Color defaultColor = Color.White;
        protected readonly Color damageColor = Color.Orange;
        protected static float scale;

        public GameObject()
        {
            scale = 1.0f;
        }

        public GameObject(Texture2D texture, bool createColorMap)
        {
            scale = 1.0f;
            spriteTexture = texture;

            if (createColorMap)
            {
                colorMap = new Color[spriteTexture.Width * spriteTexture.Height];
                texture.GetData<Color>(colorMap);
            }
        }

        public GameObject(Texture2D texture, Texture2D textureCollision, bool createColorMap)
        {
            scale = 1.0f;
            spriteTexture = texture;

            if (createColorMap)
            {
                colorMap = new Color[spriteTexture.Width * spriteTexture.Height];
                texture.GetData<Color>(colorMap);
            }
        }
        public GameObject(bool createColorMap)
        {
            if (createColorMap)
            {
                colorMap = new Color[spriteTexture.Width * spriteTexture.Height];
                spriteTexture.GetData<Color>(colorMap);
            }
        }

        public virtual void Update() { }

        public virtual void Update(GameTime gameTime) { }

        
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTexture, position, rectangle, defaultColor, rotation, origin, scale, SpriteEffects.None, 0);
        }
    }
}

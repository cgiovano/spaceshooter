using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Library
{
	public class GameObject
    {
        public Vector2 Position { get { return (position); } }
        public Rectangle Rectangle { get; set; }
        public Color[] ColorMap { get { return (colorMap); } }
        public int Energy { get; set; }
        public float Speed { get; set; } = .5f;
		public bool IsRemoved { get; set; } = false;
        public bool IsDestroyed { get; set; } = false;
        public float ScaleFactor { get; set; } = 1f;

        protected Vector2 position;
        protected Vector2 velocity;
        protected Vector2 origin;
        protected Rectangle rectangle;
        protected Color[] colorMap;
        protected Texture2D spriteTexture;
        protected Texture2D spriteTextureCollision;
        protected int frameWidth;
        protected int frameHeight;
        protected bool isHitted = false;
        protected float rotation;
        protected readonly Color defaultColor = Color.White;
        protected readonly Color damageColor = Color.Orange;
        protected static float scale;

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
            scale = Screen.Scale * ScaleFactor;
            spriteTexture = texture;
            spriteTextureCollision = textureCollision;

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

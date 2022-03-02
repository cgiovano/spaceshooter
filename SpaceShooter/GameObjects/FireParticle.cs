using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Library;
using System;

namespace SpaceShooter.GameObjects
{
    public class FireParticle: GameObject
    {
        private Vector2 _direction;
        private Color _particleColor = Color.Orange;    //new Color(255, 130, 60);
        private int _lifeTime = 350;
        private int _timer;
        private float _rotation;
        private float _rotationAngle;

        public FireParticle(Texture2D texture, bool createColorMap, Vector2 position, float rotationAngle) : base (texture, createColorMap)
        {
            this.position = position;
            Speed = Library.Random.GetRandom(1, 3);
            _rotationAngle = rotationAngle;
        }

        public override void Update(GameTime gameTime)
        {
            _timer += gameTime.ElapsedGameTime.Milliseconds;
            _rotation = MathHelper.ToRadians(_rotationAngle);
            _direction = new Vector2((float)Math.Cos(_rotation), -(float)Math.Sin(_rotation));
            position += _direction * Speed;

            if (_timer > _lifeTime)
                IsRemoved = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(spriteTexture, Position, rectangle, _particleColor, _rotation, origin, Vector2.One, SpriteEffects.None, 0);
            spriteBatch.Draw(spriteTexture, Position, _particleColor);
        }
    }
}
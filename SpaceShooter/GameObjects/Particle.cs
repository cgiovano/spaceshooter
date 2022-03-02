using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using System;

namespace SpaceShooter.GameObjects
{
    public class Particle : GameObject
    {
        private Vector2 _direction;
        private Color _particleColor;    //new Color(255, 130, 60);
        private int _lifeTime = 400;
        private int _timer;
        private float _rotation;
        private float _rotationAngle;

        bool _isFire;

        public Particle(Texture2D texture, bool createColorMap, Vector2 _position, float rotationAngle) : base (texture, createColorMap)
        {
            position = _position;
            Speed = Library.RandomMod.GetRandom(2, 4);
            _lifeTime = RandomMod.GetRandom(300, 350);

            _rotationAngle = rotationAngle;

            if (texture == Textures.ExplosionParticleTexture)
                _particleColor = new Color(255, 255, 0, 255); // Color.Orange;
        }

        public Particle(Texture2D texture, bool createColorMap, Vector2 _position, bool isFire, int lifeTime) : base(texture, createColorMap)
        {
            Speed = RandomMod.RandomFloat(1, 6);
            position = _position;
            if (lifeTime != 0)
                _lifeTime = RandomMod.GetRandom(50, 100);
            else
                _lifeTime = RandomMod.GetRandom(200, 600);
            /*
            if (RandomMod.RandomBool())
                position = _position;
            else
            {
                if (RandomMod.RandomBool())
                    position.X -= Speed;
                else
                    position.X += Speed;
            }*/

            _isFire = isFire;

            if (texture == Textures.ExplosionParticleTexture)
                _particleColor = new Color(255, 255, 0, 255); // Color.Orange;
        }

        public override void Update(GameTime gameTime)
        {
            if (!_isFire)
            {
                _rotation = MathHelper.ToRadians(_rotationAngle);
                _direction = new Vector2((float)Math.Cos(_rotation), -(float)Math.Sin(_rotation));
                position += _direction * Speed;
                _particleColor.A -= 4;
                _particleColor.G -= 7;
            }
            else
            {
                position.Y -= 1 * Speed;
                _particleColor.A -= 7;
                _particleColor.G -= 7;
            }
            
            _timer += gameTime.ElapsedGameTime.Milliseconds;

            if (_timer > _lifeTime)
                IsRemoved = true;

            

            Speed -= 0.03f;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {   
            //spriteBatch.Draw(spriteTexture, Position, rectangle, _particleColor, _rotation, origin, Vector2.One, SpriteEffects.None, 0);
            spriteBatch.Draw(spriteTexture, Position, _particleColor);
        }
    }
}
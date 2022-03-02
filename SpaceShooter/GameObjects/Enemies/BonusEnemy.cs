using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using SpaceShooter.Library.Utils;
using System;
using System.Collections.Generic;

namespace SpaceShooter.GameObjects.Enemies
{
    public class BonusEnemy : GameObject
    {
        private Direction _direction;
        private Color _laserColor = new Color(255, 110, 110);
        private Color _color = Color.White;
        private int _shootInterval;
        private int contador;
        private int contador2;
        private float _friction = 5f;
        private Animation _animation;
        private int _hurtColorDuration = 0;
        private Vector2 _dir;
        private Vector2 _targetPosition;

        public bool IsPositioned { get; private set; }
        private Spline _spline;
        private Vector2[] _splinePoints;
        private List<Point2D> _splineControlPoints = new List<Point2D>();
        private Point2D[] _controlPoints;
        private int _splineIndex = 1;
        private int _rotateDirection;
        private float _rotationAngle;

        int i = 0;
        int timer = 0;

        public BonusEnemy(Texture2D texture, List<Point2D> splinePoints, bool createColorMap) : base(texture, createColorMap)
        {
            for (int i = 0; i < splinePoints.Count; i++)
            {
                _splineControlPoints.Add(splinePoints[i]);
            }


            _spline = new Spline(_splineControlPoints.ToArray());
            _splinePoints = _spline.GetCatmullRomSpline();
            position = _splinePoints[_splineIndex];

            spriteTexture = texture;

            numberOfFrames = 2;
            frameWidth = spriteTexture.Width / numberOfFrames;
            frameHeight = spriteTexture.Height;
            Energy = 20;
            Speed = .25f;
            _shootInterval = 50;
            _animation = new Animation(1, frameWidth, frameHeight);
            origin = new Vector2(frameWidth / 2, frameHeight / 2);
        }

        public override void Update(GameTime gameTime)
        {
            var player = Components.Player as PlayerShip;
            _animation.Update(gameTime);
            _hurtColorDuration += gameTime.ElapsedGameTime.Milliseconds;
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
            timer += gameTime.ElapsedGameTime.Milliseconds;

            contador++;
            //contador2 += gameTime.ElapsedGameTime.TotalSeconds;

            if (timer > 12)
            {
                if (_splineIndex < _splinePoints.Length)
                {
                    position = _splinePoints[_splineIndex];
                    _splineIndex++;
                }
                else
                {
                    IsRemoved = true;
                }

                timer = 0;
            }

            if (!IsPositioned)
            {
                if (_splineIndex < _splinePoints.Length)
                    _rotationAngle = (float)Math.Atan2((position.Y - _splinePoints[_splineIndex].Y), (position.X - _splinePoints[_splineIndex].X));

                _rotationAngle = (float)(_rotationAngle - (90 * (Math.PI / 180)));
            }
            else
            {
                _rotationAngle = 0;
            }

            CheckLaserDamage();

            if (Energy <= 0)
                IsDestroyed = true;
        }

        private void CheckLaserDamage()
        {
            foreach (var l in Components.Lasers)
            {
                var laser = l as Laser;

                if (rectangle.Intersects(laser.Rectangle) && laser.LaserType == LaserType.Player)
                {
                    Energy -= laser.Energy;

                    if (laser.Mode != LaserMode.Disruptor)
                        laser.IsRemoved = true;

                    isHitted = true;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(_spriteTexture, _position, _rec, Color.White, (float)_angle, _origin, 1, SpriteEffects.None, 1);

            string info = "X:" + Math.Floor(position.X) + " Y:" + Math.Floor(position.Y);
            if (isHitted == true && _hurtColorDuration < 150)
            {
                _animation.Draw(spriteBatch, spriteTexture, position, damageColor, scale, _rotationAngle, origin);
            }
            else
            {
                isHitted = false;
                _hurtColorDuration = 0;
                _animation.Draw(spriteBatch, spriteTexture, position, _color, scale, _rotationAngle, origin);
                //spriteBatch.DrawString(Fonts.Joystix, info, new Vector2(position.X, position.Y + frameHeight), Color.White, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);
            }

            if (IsPositioned)
            {
                //spriteBatch.DrawString(Fonts.Joystix, info, new Vector2(position.X, position.Y + frameHeight), Color.Green, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);
            }

            /*for (int i = 0; i < _splinePoints.Length; i++)
            {
                spriteBatch.Draw(Textures.StarTexture, _splinePoints[i], Color.Yellow);
            }*/
        }

        private float CalculateAngleDirection(Vector2 currPos, Vector2 tgtPos)
        {
            float angle;
            Vector2 difference = Vector2.Normalize(currPos - tgtPos);  // CurrentPosition minus target position
            //difference.Normalize();
            angle = (float)System.Math.Atan2(difference.Y, -difference.X);
            return (angle);
        }

        private void Idle()
        {
            if (_rotateDirection == 1)
            {
                if (_rotationAngle > 0.2)
                {
                    _rotateDirection = 0;
                    return;
                }
                _rotationAngle += 0.01f;
            }
            else if (_rotateDirection == 0)
            {
                if (_rotationAngle < -0.2)
                {
                    _rotateDirection = 1;
                    return;
                }
                _rotationAngle -= 0.01f;
            }
        }
    }
}

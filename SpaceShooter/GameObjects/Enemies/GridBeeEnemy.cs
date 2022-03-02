using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using System;
using SpaceShooter.Library.Utils;

namespace SpaceShooter.GameObjects.Enemies
{   
    public class GridBeeEnemy : GridEnemyBase
    {
        public override bool CanAttack { get; set; } = false;
        public override bool IsPositioned { get; set; } = false;

        public GridBeeEnemy(EnemyType enemyType, List<Point2D> splinePoints, int targetPositionIndex, bool canAttack) : base()
        {
            _targetPosition = Grid.GridPositionList[targetPositionIndex];
            _enemyType = enemyType;
            CanAttack = canAttack;
            i = targetPositionIndex;

            for (int i = 0; i < splinePoints.Count; i++)
            {
                _splineControlPoints.Add(splinePoints[i]);
            }
            
            _splineControlPoints[_splineControlPoints.Count - 2] = (new Point2D((int)_targetPosition.X, (int)_targetPosition.Y));
            _splineControlPoints[_splineControlPoints.Count - 1] = (new Point2D((int)_targetPosition.X, (int)_targetPosition.Y));
            

            _spline = new Spline(_splineControlPoints.ToArray());
            _splinePoints = _spline.GetCatmullRomSpline();
            position = _splinePoints[_splineIndex];
            
            spriteTexture = Textures.Enemy_A_Texture;

            numberOfFrames = 2;
            frameWidth = spriteTexture.Width / numberOfFrames ;
            frameHeight = spriteTexture.Height;
            Energy = 50;
            Speed = .25f;
            _laserColor = new Color(255, 225, 111);
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
            contador2 += gameTime.ElapsedGameTime.TotalSeconds;
            
            if (timer > 12)
            {
                if (_splineIndex < _splinePoints.Length)
                {
                    position = _splinePoints[_splineIndex];
                    _splineIndex++;
                }
                else
                {
                    CanAttack = false;
                    position = _targetPosition;
                    IsPositioned = true;
                }
                    

                timer = 0;
            }

            if (!IsPositioned)
            {
                base.UpdateSpline();

                if (_splineIndex < _splinePoints.Length)
                    _rotationAngle = (float)Math.Atan2((position.Y - _splinePoints[_splineIndex].Y), (position.X - _splinePoints[_splineIndex].X));

                _rotationAngle = (float)(_rotationAngle - (90 * (Math.PI / 180)));
            }
            else
            {
                position = Grid.GridPositionList[i];
                _rotationAngle = 0;
            }

            if (CanAttack == true)
            {
                Attack();
            }

            CheckLaserDamage();

            if (Energy <= 0)
                IsDestroyed = true;
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

        private void Shoot(List<GameObject> Lasers)
        {
            if (contador > _shootInterval)
            {
                Lasers.Add(new EnemyLaser_A(new Vector2(Position.X + frameWidth / 2, Position.Y + (frameHeight - 10)), Color.Yellow, 10));
                Sounds.EnemyShot.Play();
                contador = 0;
            }
        }

        private void Attack()
        {
            Shoot(Components.Lasers);
        }

        private void Idle()
        {
            if (IsPositioned)
            {
                position.X = (float)(Math.Cos(Math.PI * contador2) * 0.15 + (position.X));
                position.Y = (float)(Math.Sin(Math.PI * contador2) * 0.15 + (position.Y));
            }
                
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

        public override void CreateAttackPatternSpline(Point2D[] attackPattern, bool mirror)
        {
            NewAttackPattern(attackPattern);
        }
    }
}

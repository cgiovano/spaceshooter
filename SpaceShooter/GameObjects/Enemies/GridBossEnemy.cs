using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using System;
using SpaceShooter.Library.Utils;
using System.Runtime.InteropServices;

namespace SpaceShooter.GameObjects.Enemies
{   
    public class GridBossEnemy : GridEnemyBase
    {
        public override bool CanAttack { get; set; } = false;
        public override bool IsPositioned { get; set; } = false;

        public Rectangle LaserRectangle { get { return (_laserSourceRectangle); } }

        private Vector2 _attackTargetPosition;
        private bool _onAttackPosition = false;

        private Texture2D _laserTexture = Textures.Boss_Laser_Texture;
        private double _laserTimer;
        private float _laserFrameTime = 0.4f;
        private int _laserFrameIndex = 0;
        private int _laserTotalFrames = 3;
        private Rectangle _laserSourceRectangle;
        private int _laserFrameWidth;
        private int _laserFrameHeight;
        private int _laserCounter = 1;
        private int _laserFrameTimer;
        private int _laserIndex;

        private Laser _bossLaser;

        public GridBossEnemy(EnemyType enemyType, List<Point2D> splinePoints, int targetPositionIndex, bool canAttack) : base()
        {
            _laserFrameWidth = _laserTexture.Width / 3;
            _laserFrameHeight = 0;


            _targetPosition = Grid.GridPositionList[targetPositionIndex];
            _enemyType = enemyType;
            CanAttack = false;
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
            
            spriteTexture = Textures.Enemy_C_Texture;

            //CanAttack = false;

            numberOfFrames = 2;
            frameWidth = spriteTexture.Width / numberOfFrames ;
            frameHeight = spriteTexture.Height;
            Energy = 50;
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
            contador2 += gameTime.ElapsedGameTime.TotalSeconds;
            _laserIndex = Components.Lasers.IndexOf(_bossLaser);

            if (!CanAttack)
            {
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
            }
            
            if (CanAttack)
            {
                if (_onAttackPosition)
                {
                    if (_bossLaser.IsEnded == true)
                    {
                        _onAttackPosition = false;
                        CanAttack = false;
                        NewAttackPattern(_targetPosition);
                    }
                }
                else
                {
                    if (timer > 12)
                    {
                        if (_splineIndex < _splinePoints.Length)
                        {
                            position = _splinePoints[_splineIndex];
                            _splineIndex++;
                        }
                        else
                        {
                            position = _attackTargetPosition;
                            _onAttackPosition = true;
                            _bossLaser = new EnemyLaser_C(new Vector2((position.X - _laserFrameWidth / 2) + (frameWidth / 2), position.Y + spriteTexture.Height), Color.White, 5);
                            Components.Lasers.Add(_bossLaser);
                        }

                        timer = 0;
                    }
                }
            }

            if (!IsPositioned)
            {
                if (!CanAttack)
                    base.UpdateSpline();

                if (!_onAttackPosition)
                {
                    if (_splineIndex < _splinePoints.Length)
                        _rotationAngle = (float)Math.Atan2((position.Y - _splinePoints[_splineIndex].Y), (position.X - _splinePoints[_splineIndex].X));

                    _rotationAngle = (float)(_rotationAngle - (90 * (Math.PI / 180)));
                }
                else
                {
                    _rotationAngle = MathHelper.ToRadians(180);
                }
            }
            else
            {
                position = Grid.GridPositionList[i];
                _rotationAngle = 0;
            }

            CheckLaserDamage();

            if (Energy <= 0)
            {
                IsDestroyed = true;

                if (_bossLaser != null)
                    _bossLaser.IsRemoved = true;
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

            if (CanAttack)
            {
                //spriteBatch.DrawString(Fonts.Joystix, _splineControlPoints.Count.ToString(), new Vector2(20, 500), Color.Green, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);
                //spriteBatch.Draw(_laserTexture, new Vector2((position.X - _laserFrameWidth / 2) + (frameWidth / 2), position.Y + spriteTexture.Height), _laserSourceRectangle, _color, 0, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
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
                //Lasers.Add(new Laser(Textures.EnemyLaser_C_Texture, false, 10, LaserType.Enemy, _laserColor, new Vector2(Position.X + frameWidth / 2, Position.Y + (frameHeight - 10))));
                Sounds.EnemyShot.Play();
                contador = 0;
            }
        }

        private void Attack(GameTime gameTime)
        {
            _laserTimer += gameTime.ElapsedGameTime.TotalSeconds;
            _laserFrameTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (_laserFrameHeight > Textures.Boss_Laser_Texture.Height)
                _laserCounter = -_laserCounter;

            if (_laserFrameTimer > 200)
            {
                _laserFrameHeight += _laserCounter;
            }

            if (_laserTimer > _laserFrameTime)
            {
                _laserFrameIndex++;
                _laserTimer = 0f;
            }

            //Loop the animation
            if (_laserFrameIndex > _laserTotalFrames - 1)
                _laserFrameIndex = 0;

            _laserSourceRectangle = new Rectangle(_laserFrameIndex * _laserFrameWidth, 0, _laserFrameWidth, _laserFrameHeight);

            if (_laserFrameHeight < 0)
            {
                NewAttackPattern(_targetPosition);
                CanAttack = false;
                _onAttackPosition = false;
                _laserTimer = 0;
                _laserFrameTimer = 0;
                _laserCounter = 1;
                _laserFrameHeight = 0;
            }
            //Shoot(Components.Lasers);
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
            CanAttack = true;

            _attackTargetPosition = Components.Player.GetPos();
            _attackTargetPosition.Y -= 160;

            NewAttackPattern(_attackTargetPosition);
        }
    }
}

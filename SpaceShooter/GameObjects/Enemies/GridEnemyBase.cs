using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using System;
using SpaceShooter.Library.Utils;

namespace SpaceShooter.GameObjects.Enemies
{   
    public enum EnemyType
    {
        Bee,
        Butterfly,
        Boss,
        Kamikaze
    }

    public abstract class GridEnemyBase : GameObject
    {
        public virtual bool CanAttack { get; set; } = false;
        public virtual bool IsPositioned { get; set; } = false;

        protected EnemyType _enemyType;
        protected bool CanMove { get; set; }
        protected Color _laserColor = new Color(255, 110, 110);
        protected int _shootInterval;
        protected int contador;
        protected double contador2;
        protected float _friction = 5f;
        protected Animation _animation;
        protected int _hurtColorDuration = 0;
        protected Vector2 _targetPosition;
        protected Vector2 _dir;
        protected float _rotationAngle = 0;
        protected int _rotateDirection = 1;
        protected Color _color = Color.White;
        protected Vector2[] _splinePoints;
        protected int timer;
        protected int i;
        protected int _splineIndex = 0;
        protected Spline _spline;
        protected List<Point2D> _splineControlPoints = new List<Point2D>();

        public GridEnemyBase()
        {
            // implemented in child classes
        }

        protected void NewAttackPattern(Point2D[] attackPattern)
        {
            Point2D[] attackRightDown = { new Point2D((int)position.X + 15, (int)position.Y - 15),
                                          new Point2D((int)position.X + 30, (int)position.Y - 15),
                                          new Point2D((int)position.X + 45, (int)position.Y),
                                          new Point2D((int)position.X + 45, (int)position.Y + 15),
                                          new Point2D((int)position.X + 30, (int)position.Y + 30)};

            Point2D[] attackLeftDown = { new Point2D((int)position.X - 15, (int)position.Y - 15),
                                         new Point2D((int)position.X - 30, (int)position.Y - 15),
                                         new Point2D((int)position.X - 45, (int)position.Y),
                                         new Point2D((int)position.X - 45, (int)position.Y + 15),
                                         new Point2D((int)position.X - 30, (int)position.Y + 30)};

            Point2D[] attackRightUp = { new Point2D((int)position.X + 15, (int)position.Y + 15),
                                        new Point2D((int)position.X + 30, (int)position.Y + 15),
                                        new Point2D((int)position.X + 45, (int)position.Y),
                                        new Point2D((int)position.X + 45, (int)position.Y - 15),
                                        new Point2D((int)position.X + 30, (int)position.Y - 30)};

            Point2D[] attackLeftUp = { new Point2D((int)position.X - 15, (int)position.Y + 15),
                                       new Point2D((int)position.X - 30, (int)position.Y + 15),
                                       new Point2D((int)position.X - 45, (int)position.Y),
                                       new Point2D((int)position.X - 45, (int)position.Y - 15),
                                       new Point2D((int)position.X - 30, (int)position.Y - 30)};

            Point2D[] attackStart;

            _splineIndex = 0;
            _splinePoints = null;
            _splineControlPoints.Clear();

            if (position.X >= Screen.GetWidth / 2)
            {
                if (position.Y >= Screen.GetHeight / 2)
                    attackStart = attackRightUp;
                else
                    attackStart = attackRightDown;
            }
            else
            {
                if (position.Y < Screen.GetHeight / 2)
                    attackStart = attackLeftUp;
                else
                    attackStart = attackLeftDown;
            }

            _splineControlPoints.Add(new Point2D((int)position.X, (int)position.Y));
            _splineControlPoints.Add(new Point2D((int)position.X, (int)position.Y));

            for (int i = 0; i < attackStart.Length; i++)
            {
                _splineControlPoints.Add(attackStart[i]);
            }

            for (int i = 1; i < attackPattern.Length; i++)
            {
                if (position.X >= Screen.GetWidth / 2)
                {
                    Point2D mirrorPointPosition = new Point2D();
                    mirrorPointPosition = attackPattern[i];
                    mirrorPointPosition.x = Screen.GetWidth - attackPattern[i].x;
                    _splineControlPoints.Add(mirrorPointPosition);
                }
                else
                {
                    _splineControlPoints.Add(attackPattern[i]);
                }
            }

            _splineControlPoints[_splineControlPoints.Count - 2] = (new Point2D((int)_targetPosition.X, (int)_targetPosition.Y));
            _splineControlPoints[_splineControlPoints.Count - 1] = (new Point2D((int)_targetPosition.X, (int)_targetPosition.Y));

            _splinePoints = _spline.UpdateRomSpline(_splineControlPoints.ToArray());
        }

        protected void NewAttackPattern(Vector2 targetPosition)
        {
            Point2D[] attackRightDown = { new Point2D((int)position.X + 15, (int)position.Y - 15),
                                          new Point2D((int)position.X + 30, (int)position.Y - 15),
                                          new Point2D((int)position.X + 45, (int)position.Y),
                                          new Point2D((int)position.X + 45, (int)position.Y + 15),
                                          new Point2D((int)position.X + 30, (int)position.Y + 30)};

            Point2D[] attackLeftDown = { new Point2D((int)position.X - 15, (int)position.Y - 15),
                                         new Point2D((int)position.X - 30, (int)position.Y - 15),
                                         new Point2D((int)position.X - 45, (int)position.Y),
                                         new Point2D((int)position.X - 45, (int)position.Y + 15),
                                         new Point2D((int)position.X - 30, (int)position.Y + 30)};

            Point2D[] attackRightUp = { new Point2D((int)position.X + 15, (int)position.Y + 15),
                                        new Point2D((int)position.X + 30, (int)position.Y + 15),
                                        new Point2D((int)position.X + 45, (int)position.Y),
                                        new Point2D((int)position.X + 45, (int)position.Y - 15),
                                        new Point2D((int)position.X + 30, (int)position.Y - 30)};

            Point2D[] attackLeftUp = { new Point2D((int)position.X - 15, (int)position.Y + 15),
                                       new Point2D((int)position.X - 30, (int)position.Y + 15),
                                       new Point2D((int)position.X - 45, (int)position.Y),
                                       new Point2D((int)position.X - 45, (int)position.Y - 15),
                                       new Point2D((int)position.X - 30, (int)position.Y - 30)};

            Point2D[] attackStart;

            _splineIndex = 0;
            _splinePoints = null;
            _splineControlPoints.Clear();

            if (position.X >= Screen.GetWidth / 2)
            {
                if (position.Y >= Screen.GetHeight / 2)
                    attackStart = attackRightUp;
                else
                    attackStart = attackRightDown;
            }
            else
            {
                if (position.Y < Screen.GetHeight / 2)
                    attackStart = attackLeftUp;
                else
                    attackStart = attackLeftDown;
            }

            _splineControlPoints.Add(new Point2D((int)position.X, (int)position.Y));
            _splineControlPoints.Add(new Point2D((int)position.X, (int)position.Y));

            for (int i = 0; i < attackStart.Length; i++)
            {
                _splineControlPoints.Add(attackStart[i]);
            }

            /*for (int i = 1; i < attackPattern.Length; i++)
            {
                if (position.X >= Screen.GetWidth / 2)
                {
                    Point2D mirrorPointPosition = new Point2D();
                    mirrorPointPosition = attackPattern[i];
                    mirrorPointPosition.x = Screen.GetWidth - attackPattern[i].x;
                    _splineControlPoints.Add(mirrorPointPosition);
                }
                else
                {
                    _splineControlPoints.Add(attackPattern[i]);
                }
            }*/

            _splineControlPoints.Add(new Point2D((int)targetPosition.X, (int)targetPosition.Y));
            _splineControlPoints.Add(new Point2D((int)targetPosition.X, (int)targetPosition.Y));

            _splinePoints = _spline.UpdateRomSpline(_splineControlPoints.ToArray());
        }

        protected void UpdateSpline()
        {
            if (_enemyType != EnemyType.Kamikaze)
                _targetPosition = Grid.GridPositionList[i];

            _splineControlPoints[_splineControlPoints.Count - 2] = (new Point2D((int)_targetPosition.X, (int)_targetPosition.Y));
            _splineControlPoints[_splineControlPoints.Count - 1] = (new Point2D((int)_targetPosition.X, (int)_targetPosition.Y));

            _splinePoints = _spline.UpdateRomSpline(_splineControlPoints.ToArray());
        }

        protected void UpdateSpline(Vector2 targetPosition)
        {
            if (_enemyType != EnemyType.Kamikaze)
                _targetPosition = Grid.GridPositionList[i];

            _splineControlPoints[_splineControlPoints.Count - 2] = (new Point2D((int)targetPosition.X, (int)targetPosition.Y));
            _splineControlPoints[_splineControlPoints.Count - 1] = (new Point2D((int)targetPosition.X, (int)targetPosition.Y));

            _splinePoints = _spline.UpdateRomSpline(_splineControlPoints.ToArray());
        }

        private void Idle()
        {
            /*if (IsPositioned)
            {
                position.X = (float)(Math.Cos(Math.PI * contador2) * 0.15 + (position.X));
                position.Y = (float)(Math.Sin(Math.PI * contador2) * 0.15 + (position.Y));
            }*/
                
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

        protected void CheckLaserDamage()
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

        public virtual void CreateAttackPatternSpline(Point2D[] attackPattern, bool mirror)
        {
            // implemented in child classes
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using SpaceShooter.Library.Utils;
using System.Collections.Generic;

namespace SpaceShooter.GameObjects.Enemies
{
    public abstract class RushEnemyBase : GameObject
    {
        private Direction _direction;
        private Color _laserColor = new Color(0, 255, 100);
        private int _shootInterval;
        private int contador;
        private int contador2;
        private float _friction = 5f;
        private Animation _animation;
        private int _hurtColorDuration = 0;

        public RushEnemyBase() : base()
        {

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
    }
}

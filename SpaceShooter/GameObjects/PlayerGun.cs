using Microsoft.Xna.Framework;
using SpaceShooter.GameUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.GameObjects
{
    enum GunLevel
    {
        NoGun,
        Level_1,
        Level_2,
        Level_3,
        Level_4,
        Level_5,
    }

    enum GunType
    {
        MachineGun,
        PlasmaLaser,
        DisruptorLaser
    }

    class PlayerGun
    {
        public GunLevel GunLevel { get { return (_gunLevel); } }

        private GunType _gunType;
        private GunLevel _gunLevel;
        private int _shootTimer;
        private int _shootInterval;

        public PlayerGun(GunType gunType, GunLevel gunLevel)
        {
            _gunType = gunType;
            _gunLevel = gunLevel;
        }

        public void Update(GameTime gameTime)
        {
            _shootTimer += (int)gameTime.ElapsedGameTime.Milliseconds;

            if (_shootTimer > 200)
            {
                //Components.Lasers.Add(new PlayerLaser_A(_primaryLaserPosition, Color.White, _laserEnergy, 0.4f));
                //Components.Lasers.Add(new PlayerLaser_A(_secondaryLaserPosition, Color.White, _laserEnergy, 0.4f));
                Sounds.PlayerShot.Play();

                _shootInterval = 0;
            }
        }
    }
}

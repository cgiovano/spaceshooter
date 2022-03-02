using Microsoft.Xna.Framework;
using SpaceShooter.GameObjects.Enemies;
using System;
using System.Collections.Generic;

namespace SpaceShooter.GameUtils
{
    class GridEnemyHelper
    {
        public int Index { get { return (_index); } }
        public EnemyType EnType { get { return (_enemyType); } }

        private int _index;
        private EnemyType _enemyType;
        private Random _rng = new Random();

        public GridEnemyHelper(int index, EnemyType enemyType)
        {
            _index = index;
            _enemyType = enemyType;
        }

        public GridEnemyHelper(EnemyType enemyType)
        {
            _enemyType = enemyType;
        }
    }
}

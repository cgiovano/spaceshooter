using System.Collections.Generic;
using SpaceShooter.GameObjects;
using SpaceShooter.Library;
using SpaceShooter.GameObjects.Enemies;
using SpaceShooter.Shared;

namespace SpaceShooter.GameUtils
{
    public class Components
    {
        public static PlayerShip Player;
        public static Shield ShieldArmature;
        public static OneUp OneUpBonus;
        public static List<GameObject> Boss;
        public static List<GameObject> Obstacles;
        public static List<GameObject> Lasers;
        public static List<GameObject> Enemies;
        public static List<GridEnemyBase> GridEnemies;
        public static List<BonusEnemy> BonusEnemies;
        public static List<StarsBackground> Stars;
        public static List<GameObject> Explosions;
        public static List<GameObject> FireParticles;
        public static List<GameObject> Weaknesses;
        public static List<GameObject> UserInterfaceElements;

        public static void CreateComponents()
        {
            Player = new PlayerShip(Textures.PlayerShipTextureCombined, true, GameInformation.Lives);
            Obstacles = new List<GameObject>();
            Enemies = new List<GameObject>();
            GridEnemies = new List<GridEnemyBase>();
            BonusEnemies = new List<BonusEnemy>();
            Stars = new List<StarsBackground>();
            Lasers = new List<GameObject>();
            Explosions = new List<GameObject>();
            FireParticles = new List<GameObject>();
            Weaknesses = new List<GameObject>();
            UserInterfaceElements = new List<GameObject>();
            Boss = new List<GameObject>();
        }

        public static void ResetComponents()
        {
            Player = null;
            ShieldArmature = null;
            Weaknesses = null;
            Boss = null;
            Obstacles = null;
            Enemies = null;
            GridEnemies = null;
            BonusEnemies = null;
            Stars = null;
            Lasers = null;
            Explosions = null;
            FireParticles = null;
            UserInterfaceElements = null;
        }
    }
}

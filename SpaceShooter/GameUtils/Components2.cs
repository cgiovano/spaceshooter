using System.Collections.Generic;
using SpaceShooter.GameObjects;
using SpaceShooter.Library;
using SpaceShooter.GameObjects.Enemies;

namespace SpaceShooter.GameUtils
{
    public class Components
    {
        public static GameObject Player;
        public static GameObject Boss;
        public static List<GameObject> Obstacles;
        public static List<GameObject> Lasers;
        public static List<GameObject> Enemies;
        public static List<GameObject> Stars;
        public static List<GameObject> Explosions;
        public static List<GameObject> Weaknesses;

        public static void CreateComponents()
        {
            Player = new PlayerShip(Textures.PlayerShip, Textures.SpaceShipTextureCollision, true);
            Obstacles = new List<GameObject>();
            Enemies = new List<GameObject>();
            Stars = new List<GameObject>();
            Lasers = new List<GameObject>();
            Explosions = new List<GameObject>();
            Weaknesses = new List<GameObject>();
        }

        public static void ResetComponents()
        {
            Player = null;
            Weaknesses = null;
            Boss = null;
            Obstacles = null;
            Enemies = null;
            Stars = null;
            Lasers = null;
            Explosions = null;
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.GameUtils
{
	public class LevelConfiguration
	{
        public static List<Texture2D> ObstacleTextures;
        public static List<Texture2D> EnemyTextures;

        public static int EnemiesToDestroy { get; private set;}
        public static int ObstaclesToDestroy { get; private set; }
        public static int NumberOfWaves { get; private set; }
        public static int EnemySpawnInterval { get; private set; }
        public static int Enemy2SpawnInterval { get; private set; }
        public static int ObstacleSpawnInterval { get; private set; }

        public static void CreateLevel(Level level)
        {
            EnemyTextures = new List<Texture2D>();
            ObstacleTextures = new List<Texture2D>();

            switch (level)
            {
                case (Level.Level_1):
                    Level_1();
                    break;
                case (Level.Level_2):
                    Level_2();
                    break;
                case (Level.Level_3):
                    Level_3();
                    break;
                case (Level.Level_4):
                    Level_4();
                    break;
                case (Level.Level_5):
                    Level_5();
                    break;
            }
        }

        private static void Level_1()
        {
            EnemiesToDestroy = 10;
            ObstaclesToDestroy = 10;

            EnemySpawnInterval = 5000;  // Em milisegundos
            Enemy2SpawnInterval = 1000;
            ObstacleSpawnInterval = 500;

            NumberOfWaves = 3;

            ObstacleTextures.Add(Textures.Asteroid);
        }

        private static void Level_2()
        {
            foreach (var texture in ObstacleTextures)
            {
                ObstacleTextures.Remove(texture);
            }

            EnemiesToDestroy = 100;
            ObstaclesToDestroy = 15;

            EnemySpawnInterval = 4000;  // Em milisegundos
            ObstacleSpawnInterval = 2500;

            ObstacleTextures.Add(Textures.Asteroid);
            ObstacleTextures.Add(Textures.Satelite);
        }

        private static void Level_3()
        {
            foreach (var textura in ObstacleTextures)
            {
                ObstacleTextures.Remove(textura);
            }

            EnemiesToDestroy = 150;
            ObstaclesToDestroy = 200;

            EnemySpawnInterval = 3000;  // Em milisegundos
            ObstacleSpawnInterval = 2000;

            ObstacleTextures.Add(Textures.Asteroid);
            ObstacleTextures.Add(Textures.Satelite);
        }

        private static void Level_4()
        {
            foreach (var textura in ObstacleTextures)
            {
                ObstacleTextures.Remove(textura);
            }

            EnemiesToDestroy = 200;
            ObstaclesToDestroy = 250;

            EnemySpawnInterval = 2500;  // Em milisegundos
            ObstacleSpawnInterval = 1500;

            ObstacleTextures.Add(Textures.Asteroid);
            ObstacleTextures.Add(Textures.Satelite);
        }

        private static void Level_5()
        {
            foreach (var textura in ObstacleTextures)
            {
                ObstacleTextures.Remove(textura);
            }

            EnemiesToDestroy = 250;
            ObstaclesToDestroy = 300;

            EnemySpawnInterval = 2000;  // Em milisegundos
            ObstacleSpawnInterval = 1000;

            ObstacleTextures.Add(Textures.Asteroid);
            ObstacleTextures.Add(Textures.Satelite);
        }
    }
}

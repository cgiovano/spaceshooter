using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Library;
using SpaceShooter.Shared;

namespace SpaceShooter.GameUtils
{
	public class LevelConfiguration
	{
        public static int RushEnemiesToDestroy { get; private set;}
        public static int ObstaclesToDestroy { get; private set; }
        public static int RushEnemySpawnInterval { get; private set; }
        public static int ObstacleSpawnInterval { get; private set; }

        public static void CreateLevel(Level level)
        {
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
            RushEnemiesToDestroy = 10;
            ObstaclesToDestroy = 10;
            RushEnemySpawnInterval = 2000;  // miliseconds
            ObstacleSpawnInterval = 1000;
        }

        private static void Level_2()
        {
            RushEnemiesToDestroy = 15;
            ObstaclesToDestroy = 15;
            RushEnemySpawnInterval = 1800;
            ObstacleSpawnInterval = 800;
        }

        private static void Level_3()
        {
            RushEnemiesToDestroy = 40;
            ObstaclesToDestroy = 40;
            RushEnemySpawnInterval = 1600;
            ObstacleSpawnInterval = 600;
        }

        private static void Level_4()
        {
            RushEnemiesToDestroy = 45;
            ObstaclesToDestroy = 45;
            RushEnemySpawnInterval = 1200;
            ObstacleSpawnInterval = 400;
        }

        private static void Level_5()
        {
            RushEnemiesToDestroy = 50;
            ObstaclesToDestroy = 50;
            RushEnemySpawnInterval = 800;
            ObstacleSpawnInterval = 200;
        }

        public static List<bool> GetCanAttackPattern(Level level)
        {
            List<bool> toReturn = Patterns._stage1CanAttackList;

            switch (level)
            {
                case (Level.Level_1):
                    toReturn = Patterns._stage1CanAttackList;
                    break;
                case (Level.Level_2):
                    toReturn = Patterns._stage2CanAttackList;
                    break;
                case (Level.Level_3):
                    toReturn = Patterns._stage3CanAttackList;
                    break;
                case (Level.Level_4):
                    toReturn = Patterns._stage4CanAttackList;
                    break;
                case (Level.Level_5):
                    toReturn = Patterns._stage5CanAttackList;
                    break;
            }

            return (toReturn);
        }

        public static Point2D[][] GetPattern()
        {
            Point2D[][] bonusPattern = new Point2D[6][];

            switch (GameInformation.CurrentLevel)
            {
                case (Level.Level_1):
                    bonusPattern = new Point2D[6][] { Patterns.BonusStage1_A, Patterns.BonusStage1_A, Patterns.BonusStage1_B, Patterns.BonusStage1_B, Patterns.BonusStage1_A, Patterns.BonusStage1_A };
                    break;
                case (Level.Level_2):
                    bonusPattern = new Point2D[6][] { Patterns.BonusStage2_A, Patterns.BonusStage2_A, Patterns.BonusStage2_B, Patterns.BonusStage2_B, Patterns.BonusStage2_A, Patterns.BonusStage2_A };
                    break;
                case (Level.Level_3):
                    bonusPattern = new Point2D[6][] { Patterns.BonusStage3_A, Patterns.BonusStage3_A, Patterns.BonusStage3_B, Patterns.BonusStage3_B, Patterns.BonusStage3_A, Patterns.BonusStage3_A };
                    break;
                case (Level.Level_4):
                    bonusPattern = new Point2D[6][] { Patterns.BonusStage4_A, Patterns.BonusStage4_A, Patterns.BonusStage4_B, Patterns.BonusStage4_B, Patterns.BonusStage4_A, Patterns.BonusStage4_A };
                    break;
                case (Level.Level_5):
                    bonusPattern = new Point2D[6][] { Patterns.BonusStage5_A, Patterns.BonusStage5_A, Patterns.BonusStage5_B, Patterns.BonusStage5_B, Patterns.BonusStage5_A, Patterns.BonusStage5_A };
                    break;
            }

            return (bonusPattern);
        }
    }
}

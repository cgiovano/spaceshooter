using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameObjects.Enemies;
using SpaceShooter.Library;
using SpaceShooter.Shared;

namespace SpaceShooter.GameUtils
{
    static class Grid
    {
        public static List<Vector2> GridPositionList;
        
        private static int counter = 4;
        private static int movementAmount = 10;

        public static List<Vector2> CreateGrid()
        {
            int containerSize = 40;

            List<Vector2> gridPositionList = new List<Vector2>();
            
            for (int i = 0; i < (Screen.GetHeight / containerSize); i++)
            {
                for (int j = 0; j < (Screen.GetWidth / containerSize); j++)
                {
                    gridPositionList.Add(new Vector2(j * containerSize, i * containerSize));
                    
                    // Usar abaixo, caso queira cenralizado
                    //gridPositionList.Add(new Vector2((j * containerSize) + (containerSize / 2), (i * containerSize) + (containerSize / 2)));
                }
            }

            GridPositionList = gridPositionList;

            return (gridPositionList);
        }

        public static void UpdateGridPosition()
        {

            if (counter == 0)
            {
                counter = 8;

                movementAmount = movementAmount * (-1);

                for (int i = 0; i < Grid.GridPositionList.Count; i++)
                {
                    Grid.GridPositionList[i] = new Vector2(Grid.GridPositionList[i].X, Grid.GridPositionList[i].Y + 10);
                }
            }
            else
            {
                for (int i = 0; i < Grid.GridPositionList.Count; i++)
                {
                    Grid.GridPositionList[i] = new Vector2(Grid.GridPositionList[i].X + movementAmount, Grid.GridPositionList[i].Y);
                }

                counter--;
            }
        }

        public static List<GridEnemyHelper> GridEnemyDisposition()
        {
            //int[] dispositionRaw = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] dispositionRaw = Patterns.GridPattern(GameInformation.CurrentLevel);

            List<GridEnemyHelper> disposition = new List<GridEnemyHelper>();

            for (int i = 0; i < GridPositionList.Count; i++)
            {
                if (dispositionRaw[i] == 1)
                {
                    disposition.Add(new GridEnemyHelper(i, EnemyType.Bee));
                }
                if (dispositionRaw[i] == 2)
                {
                    disposition.Add(new GridEnemyHelper(i, EnemyType.Butterfly));
                }
                if (dispositionRaw[i] == 3)
                {
                    disposition.Add(new GridEnemyHelper(i, EnemyType.Boss));
                }
            }

            return (disposition);
        }
    }
}
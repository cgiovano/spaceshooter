using System;
using Microsoft.Xna.Framework;
using SpaceShooter.Library;

namespace SpaceShooter.GameUtils
{
    static class Grid
    {
        public static Vector2[,] CreateGrid(int columns, int lines)
        {
            Vector2 position;
            Vector2[,] gridArray = new Vector2[lines, columns];

            int containerWidth = (int)(Screen.GetWidth / (columns + 1));
            int containerHeight = (int)(Math.Floor((double)(Screen.GetHeight - Screen.GetHeight / 3) / (lines * 2)));

            for (int l = 0; l < lines; l++)
            {
                for (int c = 0; c < columns; c++)
                {
                    position = new Vector2((c + 1) * containerWidth, (l + 1) * containerHeight);
                    gridArray[l,c] = position;
                }
            }

            return (gridArray);
        }
    }
}
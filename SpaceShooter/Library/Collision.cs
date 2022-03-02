using System;
using Microsoft.Xna.Framework;

namespace SpaceShooter.Library
{
	public class Collision
	{
        /// <summary>
        /// Checa se existe colisão entre doi objetos complexos (sprites com muitos detalhes).
        /// </summary>
        /// <param name="retangulo_objA"></param>
        /// <param name="mapaDeCor_objA"></param>
        /// <param name="retangulo_objB"></param>
        /// <param name="mapaDeCor_objB"></param>
        /// <returns></returns>
		public static bool IntersectsPixel(Rectangle rectangleA, Color[] colorMapA, Rectangle rectangleB, Color[] colorMapB)
		{
			int Top = Math.Max(rectangleA.Top, rectangleB.Top);
			int Bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
			int Left =  Math.Max(rectangleA.Left, rectangleB.Left);
			int Right = Math.Min(rectangleA.Right, rectangleB.Right);

			for (int i = Top; i < Bottom; i++)
			{
				for (int j = Left; j <Right; j++)
				{
					Color ColorA = colorMapA[(j - rectangleA.Left) + (i - rectangleA.Top) * rectangleA.Width];
					Color ColorB = colorMapB[(j - rectangleB.Left) + (i - rectangleB.Top) * rectangleB.Width];

					if (ColorA.A != 0 && ColorB.A != 0)
					{
						return (true);
					}
				}
			}

			return (false);
		}
	}
}

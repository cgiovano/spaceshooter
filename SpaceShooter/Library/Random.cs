using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using System.Collections.Generic;

namespace SpaceShooter.Library
{
    class Random
	{
		private static System.Random random;

		public Random()
		{
            random = new System.Random();
		}

		public static int GetRandom(int min, int max)
		{
			return (random.Next(min, max));
		}

		public static int GetRandom(int numeroMaximo)
		{
			return (random.Next(0, numeroMaximo));
		}

        public static Texture2D GetRandomTexture(List<Texture2D> textures, Level level)
        {
            int  index = GetRandom(0, textures.Count);

            return (textures[0]);
        }
    }
}

using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using System;
using System.Collections.Generic;

namespace SpaceShooter.Library
{
    class RandomMod
	{
		private static Random random;

		public RandomMod()
		{
            random = new System.Random();
		}

		public static int GetRandom(int min, int max)
		{
			random = new Random();

			int rnd;

			if (min == 0 && max == 0)
				rnd = 0;
			else if (min == 0 && max == 1)
				rnd = 1;
			else
				rnd = new Random().Next(min, max);
			
			return (rnd);
		}

		public static bool RandomBool()
        {
			var i = random.Next(0, 10);

			if (i < 5)
				return (true);
			else
				return (false);

        }

		public static float RandomFloat(int min, int max)
		{
			var i = new Random().Next(min, max);

			return (i / 100f);

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

        public static implicit operator RandomMod(System.Random v)
        {
            throw new NotImplementedException();
        }
    }
}

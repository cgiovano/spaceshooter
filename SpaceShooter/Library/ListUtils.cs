using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpaceShooter.Library
{
	public static class ThreadSafeRandom
	{
		[ThreadStatic] private static RandomMod Local;

		public static System.Random ThisThreadsRandom
		{
			get 
			{ 
				return new System.Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId)); 
			}
		}
	}

	public static class ListUtils
    {
		public static void Shuffle<T>(this IList<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public static void ShuffleCrypto<T>(this IList<T> list)
		{
			RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
			int n = list.Count;
			while (n > 1)
			{
				byte[] box = new byte[1];
				do provider.GetBytes(box);
				while (!(box[0] < n * (Byte.MaxValue / n)));
				int k = (box[0] % n);
				n--;
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public static List<Point2D> Mirror(this List<Point2D> list)
		{
			List<Point2D> newList = new List<Point2D>();

			for (int i = 0; i < list.Count; i++)
			{
				Point2D mirrorPointPosition = new Point2D();
				mirrorPointPosition = list[i];
				mirrorPointPosition.x = Screen.GetWidth - list[i].x;
				newList.Add(mirrorPointPosition);
			}

			return (newList);
		}
	}
}

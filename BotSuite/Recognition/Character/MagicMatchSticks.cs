// -----------------------------------------------------------------------
//  <copyright file="MagicMatchSticks.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite.Recognition.Character
{
	using System;
	using System.Collections.Generic;

	using BotSuite.Imaging;

	/// <summary>
	///     receptors for optical character recognition
	/// </summary>
	[Serializable]
	public class MagicMatchSticks
	{
		/// <summary>
		///     intern instance of PRNG
		/// </summary>
		protected Random Rand = new Random();

		/// <summary>
		///     intern list of receptors
		/// </summary>
		protected List<MagicMatchStick> MagicStickList;

		/// <summary>
		///     handle the list of receptors
		/// </summary>
		/// <param name="index">
		///     index of receptor
		/// </param>
		/// <returns>
		///     receptor from list
		/// </returns>
		public MagicMatchStick this[int index]
		{
			get
			{
				return this.MagicStickList[index];
			}
		}

		/// <summary>
		///     get the number of magic match sticks
		/// </summary>
		/// <returns>num of magic sticks</returns>
		public int Num()
		{
			return this.MagicStickList.Count;
		}

		/// <summary>
		///     add a magic stick to list
		/// </summary>
		/// <param name="stick">
		///     magic stick
		/// </param>
		public void Add(MagicMatchStick stick)
		{
			this.MagicStickList.Add(stick);
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="MagicMatchSticks" /> class.
		///     Create a Container of Magic match Sticks
		/// </summary>
		/// <returns>
		/// </returns>
		public MagicMatchSticks()
		{
			this.MagicStickList = new List<MagicMatchStick>();
		}

		/// <summary>
		///     generate some magic sticks in [0,1]^2
		/// </summary>
		/// <param name="count">
		///     number of magic sticks
		/// </param>
		public void Generate(int count)
		{
			for (int i = 0; i < count; i++)
			{
				int xa = this.Rand.Next(100);
				int ya = this.Rand.Next(100);
				int xb = this.Rand.Next(100);
				int yb = this.Rand.Next(100);

				int dx = xa - xb;
				int dy = ya - yb;
				int length = (int)Math.Sqrt(dx * dx + dy * dy);

				// if their length is to short, then take another
				if ((length < 9) || (length > 54))
				{
					i--;
					continue;
				}

				this.MagicStickList.Add(new MagicMatchStick(xa, ya, xb, yb));
			}
		}

		/// <summary>
		///     Get the pattern of the magics match sticks
		/// </summary>
		/// <param name="image">
		///     image of character
		/// </param>
		/// <returns>
		///     pattern
		/// </returns>
		public float[] GetMagicMatchSticksState(ImageData image)
		{
			int width = image.Width; // width of image
			int height = image.Height; // height of image
			int n = this.Num(); // num of magicsticks
			float[] pattern = new float[n]; // state

			int lastx = -1, lasty = -1;

			for (int i = 0; i < n; i++)
			{
				pattern[i] = 0.0f;
			}

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					// is the pixel black?
					if (image.GetPixel(x, y).R == 0)
					{
						int tmpx = Convert.ToInt16(x * 100 / width);
						int tmpy = Convert.ToInt16(y * 100 / height);

						if ((tmpx != lastx) || (tmpy != lasty))
						{
							lastx = tmpx;
							lasty = tmpy;
							for (int i = 0; i < n; i++)
							{
								// skip already activated receptors
								if (Math.Abs(pattern[i] - 1.0f) < 0)
								{
									continue;
								}

								if (this.MagicStickList[i].GetMagicMatchStickState(tmpx, tmpy))
								{
									pattern[i] = 1.0f;
								}
							}
						}
					}
				}
			}

			return pattern;
		}
	}
}
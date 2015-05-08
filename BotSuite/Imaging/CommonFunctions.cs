// -----------------------------------------------------------------------
//  <copyright file="CommonFunctions.cs" company="Binary Overdrive">
//      Copyright (c) Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>Framework for creating automation applications.</purpose>
//  <homepage>https://bitbucket.org/KarillEndusa/botsuite.net</homepage>
//  <license>https://bitbucket.org/KarillEndusa/botsuite.net/wiki/license</license>
// -----------------------------------------------------------------------

namespace BotSuite.Imaging
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;

	/// <summary>
	///     collection of common functions
	/// </summary>
	public static class CommonFunctions
	{
		/// <summary>
		///     test if the colors seems to be similar.
		/// </summary>
		/// <param name="one">
		///     Color A
		/// </param>
		/// <param name="two">
		///     Color B
		/// </param>
		/// <param name="tolerance">
		///     tolerance (0,...,255)
		/// </param>
		/// <returns>
		///     similar or not
		/// </returns>
		public static bool ColorsSimilar(Color one, Color two, uint tolerance)
		{
			if(Math.Abs(one.R - two.R) > tolerance)
			{
				return false;
			}
			if(Math.Abs(one.G - two.G) > tolerance)
			{
				return false;
			}
			if(Math.Abs(one.B - two.B) > tolerance)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		///     Get the average rgb-values from an image in a given rectangle
		///     By default the function calculate the average rgb-values from the whole image
		/// </summary>
		/// <param name="img">
		///     Image
		/// </param>
		/// <param name="left">
		///     left of rectangle (default=0)
		/// </param>
		/// <param name="top">
		///     top of rectangle (default=0)
		/// </param>
		/// <param name="width">
		///     width of rectangle (default=full width)
		/// </param>
		/// <param name="height">
		///     height of rectangle (default=full height)
		/// </param>
		/// <returns>
		///     averag rgb-values
		/// </returns>
		public static double[] AverageRgbValues(ImageData img, int left = 0, int top = 0, int width = -1, int height = -1)
		{
			long[] totals = { 0, 0, 0 };

			if(width == -1)
			{
				width = img.Width;
			}
			if(height == -1)
			{
				height = img.Height;
			}

			for(int x = left; x < left + width; x++)
			{
				for(int y = top; y < top + height; y++)
				{
					Color currentColor = img.GetPixel(x, y);
					totals[0] += currentColor.R;
					totals[1] += currentColor.G;
					totals[2] += currentColor.B;
				}
			}

			int count = width * height;
			double[] retvar = { totals[0] / (double)count, totals[1] / (double)count, totals[2] / (double)count };
			return retvar;
		}

		/// <summary>
		///     Get the average color from an image in a given rectangle
		///     By default the function calculate the average color from the whole image
		/// </summary>
		/// <param name="img">
		///     Image
		/// </param>
		/// <param name="left">
		///     left of rectangle (default=0)
		/// </param>
		/// <param name="top">
		///     top of rectangle (default=0)
		/// </param>
		/// <param name="width">
		///     width of rectangle (default=full width)
		/// </param>
		/// <param name="height">
		///     height of rectangle (default=full height)
		/// </param>
		/// <returns>
		///     average color
		/// </returns>
		public static Color AverageColor(ImageData img, int left = 0, int top = 0, int width = -1, int height = -1)
		{
			double[] retvar = AverageRgbValues(img, left, top, width, height);
			return Color.FromArgb(Convert.ToInt32(retvar[0]), Convert.ToInt32(retvar[1]), Convert.ToInt32(retvar[2]));
		}

		/// <summary>
		///     calculate the similarity of image A in a given rectangle and a reference image B
		/// </summary>
		/// <param name="img">
		///     image A
		/// </param>
		/// <param name="reference">
		///     image B
		/// </param>
		/// <param name="left">
		///     offset from left of image A
		/// </param>
		/// <param name="top">
		///     offset from top of image A
		/// </param>
		/// <param name="width">
		///     width of rectangle (default: full width)
		/// </param>
		/// <param name="height">
		///     height of rectangle (default: full height)
		/// </param>
		/// <param name="offsetLeft">
		///     The Offset Left.
		/// </param>
		/// <param name="offsetTop">
		///     The Offset Top.
		/// </param>
		/// <returns>
		///     similarity (1=exact,0=none)
		/// </returns>
		public static double Similarity(
			ImageData img,
			ImageData reference,
			int left = 0,
			int top = 0,
			int width = -1,
			int height = -1,
			int offsetLeft = 0,
			int offsetTop = 0)
		{
			double sim = 0.0;
			width = (width == -1) ? img.Width - left : width;
			height = (height == -1) ? img.Height - top : height;

			if((img.Width == reference.Width) && (img.Height == reference.Height))
			{
				for(int column = left; column < left + width; column++)
				{
					for(int row = top; row < top + height; row++)
					{
						Color a = img.GetPixel(offsetLeft + column, offsetTop + row);
						Color b = reference.GetPixel(column, row);

						int cr = Math.Abs(a.R - b.R);
						int cg = Math.Abs(a.G - b.G);
						int cb = Math.Abs(a.B - b.B);

						sim += (cr + cg + cb) / 3.0;
					}
				}

				sim /= 255.0;
				sim /= img.Height * img.Width;
			}

			return 1 - sim;
		}

		/// <summary>
		///     identify the color of a part(rectangle) from an image by a given list of reference colors
		/// </summary>
		/// <param name="img">
		///     image to look in
		/// </param>
		/// <param name="statReference">
		///     list of possible colors
		/// </param>
		/// <param name="left">
		///     left of rectangle (default: 0)
		/// </param>
		/// <param name="top">
		///     top of rectangle (default: 0)
		/// </param>
		/// <param name="width">
		///     width of rectangle (default: full width)
		/// </param>
		/// <param name="height">
		///     height of rectangle (default: full height)
		/// </param>
		/// <returns>
		///     Color
		/// </returns>
		public static Color IdentifyColor(
			ImageData img,
			Dictionary<Color, List<double>> statReference,
			int left = 0,
			int top = 0,
			int width = -1,
			int height = -1)
		{
			double[] av = AverageRgbValues(img, left, top, width, height);

			double bestScore = 255;

			Color foo = Color.White;

			foreach(KeyValuePair<Color, List<double>> item in statReference)
			{
				double currentScore = Math.Pow((item.Value[0] / 255.0) - (av[0] / 255.0), 2)
									+ Math.Pow((item.Value[1] / 255.0) - (av[1] / 255.0), 2)
									+ Math.Pow((item.Value[2] / 255.0) - (av[2] / 255.0), 2);
				if(currentScore < bestScore)
				{
					foo = item.Key;
					bestScore = currentScore;
				}
			}

			return foo;
		}

		/// <summary>
		///     identify image from list (choose the image with best matching)
		/// </summary>
		/// <param name="img">
		///     image to look for
		/// </param>
		/// <param name="statReference">
		///     list of reference images
		/// </param>
		/// <returns>
		///     name (key of list)
		/// </returns>
		public static string IdentifyImage(ImageData img, Dictionary<string, ImageData> statReference)
		{
			double similar = 0.0;
			string keyword = string.Empty;
			foreach(KeyValuePair<string, ImageData> item in statReference)
			{
				// exakte übereinstimmung der Größen ermöglicht einen simplen vergleich
				if((img.Width == item.Value.Width) && (img.Height == item.Value.Height))
				{
					double s = Similarity(img, item.Value);
					if(s > similar)
					{
						keyword = item.Key;
						similar = s;
					}
				}
				else
				{
					if((img.Width > item.Value.Width) && (img.Height > item.Value.Height))
					{
						// im größeren suchen
						for(int column = 0; column < img.Width - item.Value.Width; column++)
						{
							for(int row = 0; row < img.Height - item.Value.Height; row++)
							{
								double s = Similarity(img, item.Value, 0, 0, item.Value.Width, item.Value.Height, column, row);
								if(s > similar)
								{
									keyword = item.Key;
									similar = s;
								}
							}
						}
					}
				}
			}

			return keyword;
		}

		/// <summary>
		///     calculate the difference of two colors (a-b)
		/// </summary>
		/// <param name="a">
		///     Color a
		/// </param>
		/// <param name="b">
		///     Color b
		/// </param>
		/// <returns>
		///     The <see cref="Color" />.
		/// </returns>
		public static Color ColorDifference(Color a, Color b)
		{
			int cr = a.R - b.R;
			int cg = a.G - b.G;
			int cb = a.B - b.B;
			if(cr < 0)
			{
				cr += 255;
			}
			if(cg < 0)
			{
				cg += 255;
			}
			if(cb < 0)
			{
				cb += 255;
			}
			return Color.FromArgb(cr, cg, cb);
		}

		/// <summary>
		///     get the red channel as array
		/// </summary>
		/// <param name="img">
		///     image
		/// </param>
		/// <returns>
		///     array[] of red channel
		/// </returns>
		public static uint[,] ExtractRedChannel(ImageData img)
		{
			uint[,] red = new uint[img.Width, img.Height];
			for(int column = 0; column < img.Width; column++)
			{
				for(int row = 0; row < img.Height; row++)
				{
					Color c = img.GetPixel(column, row);
					red[column, row] = c.R;
				}
			}

			return red;
		}

		/// <summary>
		///     find first occurence of color with tolerance
		/// </summary>
		/// <param name="img">
		///     image to look in
		/// </param>
		/// <param name="searchColor">
		///     color to look for
		/// </param>
		/// <param name="tolerance">
		///     tolerance
		/// </param>
		/// <returns>
		///     The <see cref="bool" /> 2dim array.
		/// </returns>
		public static bool[,] FindColors(ImageData img, Color searchColor, uint tolerance)
		{
			bool[,] grid = new bool[img.Width, img.Height];
			for(int column = 0; column < img.Width; column++)
			{
				for(int row = 0; row < img.Height; row++)
				{
					if(ColorsSimilar(img.GetPixel(column, row), searchColor, tolerance))
					{
						grid[column, row] = true;
					}
					else
					{
						grid[column, row] = false;
					}
				}
			}

			return grid;
		}
	}
}
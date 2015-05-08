// -----------------------------------------------------------------------
//  <copyright file="Filter.cs" company="Binary Overdrive">
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
	using System.Drawing;

	/// <summary>
	///     collection of filters
	/// </summary>
	public static class Filter
	{
		/// <summary>
		///     add two images
		/// </summary>
		/// <param name="img">
		///     ref Image
		/// </param>
		/// <param name="summand">
		///     image to add
		/// </param>
		public static void Add(ref ImageData img, ImageData summand)
		{
			if((img.Width == summand.Width) && (img.Height == summand.Height))
			{
				for(int column = 0; column < summand.Width; column++)
				{
					for(int row = 0; row < summand.Height; row++)
					{
						Color a = summand[column, row];
						Color b = img[column, row];

						int cr = a.R + b.R;
						int cg = a.G + b.G;
						int cb = a.B + b.B;

						if(cr > 255)
						{
							cr -= 255;
						}
						if(cg > 255)
						{
							cg -= 255;
						}
						if(cb > 255)
						{
							cb -= 255;
						}

						img[column, row] = Color.FromArgb(cr, cg, cb);
					}
				}
			}
		}

		/// <summary>
		///     subtract two images
		/// </summary>
		/// <param name="img">
		///     ref images
		/// </param>
		/// <param name="subtrahend">
		///     subtrahend
		/// </param>
		public static void Difference(ref ImageData img, ImageData subtrahend)
		{
			if((img.Width == subtrahend.Width) && (img.Height == subtrahend.Height))
			{
				for(int column = 0; column < subtrahend.Width; column++)
				{
					for(int row = 0; row < subtrahend.Height; row++)
					{
						Color a = subtrahend[column, row];
						Color b = img[column, row];

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

						img[column, row] = Color.FromArgb(cr, cg, cb);
					}
				}
			}
		}

		/// <summary>
		///     black and white image by replace
		///     all similar color to black by blackand
		///     all similiar colors to white by white
		/// </summary>
		/// <param name="img">
		///     ref image
		/// </param>
		/// <param name="tolerance">
		///     tolerance (0,...,255)
		/// </param>
		public static void BlackAndWhite(ref ImageData img, uint tolerance)
		{
			ReplaceSimilarColor(ref img, Color.Black, Color.Black, tolerance);
			ReplaceDifferentColor(ref img, Color.Black, Color.White, tolerance);
		}

		/// <summary>
		///     replace color by another color
		/// </summary>
		/// <param name="img">
		///     ref Image
		/// </param>
		/// <param name="searchColor">
		///     color to look for
		/// </param>
		/// <param name="replaceColor">
		///     color to replace with
		/// </param>
		/// <param name="tolerance">
		///     tolerance of reference color (0,...,255)
		/// </param>
		public static void ReplaceSimilarColor(ref ImageData img, Color searchColor, Color replaceColor, uint tolerance)
		{
			for(int outerX = 0; outerX < img.Width; outerX++)
			{
				for(int outerY = 0; outerY < img.Height; outerY++)
				{
					Color a = img[outerX, outerY];
					if(CommonFunctions.ColorsSimilar(a, searchColor, tolerance))
					{
						img[outerX, outerY] = replaceColor;
					}
				}
			}
		}

		/// <summary>
		///     replace color by another color by ignore a color
		/// </summary>
		/// <param name="img">
		///     ref Image
		/// </param>
		/// <param name="searchColor">
		///     color to ignore
		/// </param>
		/// <param name="replaceColor">
		///     color to replace with
		/// </param>
		/// <param name="tolerance">
		///     tolerance of reference color (0,...,255)
		/// </param>
		public static void ReplaceDifferentColor(ref ImageData img, Color searchColor, Color replaceColor, uint tolerance)
		{
			for(int outerX = 0; outerX < img.Width; outerX++)
			{
				for(int outerY = 0; outerY < img.Height; outerY++)
				{
					Color a = img[outerX, outerY];
					if(!CommonFunctions.ColorsSimilar(a, searchColor, tolerance))
					{
						img[outerX, outerY] = replaceColor;
					}
				}
			}
		}

		/// <summary>
		///     Convert image to grayscale
		/// </summary>
		/// <param name="img">
		///     ref image to convert
		/// </param>
		public static void Grayscale(ref ImageData img)
		{
			for(int column = 0; column < img.Width; column++)
			{
				for(int row = 0; row < img.Height; row++)
				{
					Color c = img[column, row];
					int grayScale = (int)((c.R * .3) + (c.G * .59) + (c.B * .11));
					img[column, row] = Color.FromArgb(grayScale, grayScale, grayScale);
				}
			}
		}

		/// <summary>
		///     invert image
		/// </summary>
		/// <param name="img">
		///     ref image to convert
		/// </param>
		public static void Invert(ref ImageData img)
		{
			for(int column = 0; column < img.Width; column++)
			{
				for(int row = 0; row < img.Height; row++)
				{
					Color c = img[column, row];
					img[column, row] = Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B);
				}
			}
		}

		/// <summary>
		///     mark a point in image
		/// </summary>
		/// <param name="img">
		///     ref image to mark
		/// </param>
		/// <param name="location">
		///     where to set marker
		/// </param>
		/// <param name="markColor">
		///     color of marker
		/// </param>
		/// <param name="size">
		///     size of marker (square)
		/// </param>
		public static void MarkPoint(ref ImageData img, Point location, Color markColor, uint size = 5)
		{
			for(int i = Convert.ToInt32(location.X - size); i < location.X + size; i++)
			{
				for(int j = Convert.ToInt32(location.Y - size); j < location.Y + size; j++)
				{
					img[i, j] = markColor;
				}
			}
		}

		/// <summary>
		///     Convert image to sepia
		/// </summary>
		/// <param name="img">
		///     ref image to convert
		/// </param>
		public static void Sepia(ref ImageData img)
		{
			for(int column = 0; column < img.Width; column++)
			{
				for(int row = 0; row < img.Height; row++)
				{
					Color c = img[column, row];
					int t = Convert.ToInt32(0.299 * c.R + 0.587 * c.G + 0.114 * c.B);
					img.SetPixel(column, row, Color.FromArgb((t > 206) ? 255 : t + 49, (t < 14) ? 0 : t - 14, (t < 56) ? 0 : t - 56));
				}
			}
		}

		/// <summary>
		///     apply threshold
		/// </summary>
		/// <param name="img">
		///     ref Image
		/// </param>
		/// <param name="threshold">
		///     threshold (0,...,255)
		/// </param>
		public static void Threshold(ref ImageData img, uint threshold)
		{
			for(int column = 0; column < img.Width; column++)
			{
				for(int row = 0; row < img.Height; row++)
				{
					Color c = img[column, row];
					if(c.R > threshold)
					{
						img[column, row] = Color.White;
					}
					else
					{
						img[column, row] = Color.Black;
					}
				}
			}
		}

		/// <summary>
		///     change the contrast of an image
		/// </summary>
		/// <param name="img">
		///     image to manipilate
		/// </param>
		/// <param name="contrast">
		///     value of contrast
		/// </param>
		public static void Contrast(ref ImageData img, double contrast)
		{
			contrast = (100.0 + contrast) / 100.0;
			contrast *= contrast;

			for(int y = 0; y < img.Height; y++)
			{
				for(int x = 0; x < img.Width; x++)
				{
					Color pixelColor = img[x, y];
					double redChannel = (((pixelColor.R / 255.0) - 0.5) * contrast + 0.5) * 255;
					double greenChannel = (((pixelColor.G / 255.0) - 0.5) * contrast + 0.5) * 255;
					double blueChannel = (((pixelColor.B / 255.0) - 0.5) * contrast + 0.5) * 255;

					redChannel = (redChannel > 255) ? 255 : redChannel;
					redChannel = (redChannel < 0) ? 0 : redChannel;
					greenChannel = (greenChannel > 255) ? 255 : greenChannel;
					greenChannel = (greenChannel < 0) ? 0 : greenChannel;
					blueChannel = (blueChannel > 255) ? 255 : blueChannel;
					blueChannel = (blueChannel < 0) ? 0 : blueChannel;

					img.SetPixel(x, y, Color.FromArgb((int)redChannel, (int)greenChannel, (int)blueChannel));
				}
			}
		}

		/// <summary>
		///     change the brightness of an image
		/// </summary>
		/// <param name="img">
		///     image to manipulate
		/// </param>
		/// <param name="brightness">
		///     brightness of new image
		/// </param>
		public static void Brightness(ref ImageData img, int brightness)
		{
			for(int y = 0; y < img.Height; y++)
			{
				for(int x = 0; x < img.Width; x++)
				{
					Color pixelColor = img[x, y];
					int redChannel = pixelColor.R + brightness;
					int greenChannel = pixelColor.G + brightness;
					int blueChannel = pixelColor.B + brightness;

					redChannel = (redChannel > 255) ? 255 : redChannel;
					redChannel = (redChannel < 0) ? 0 : redChannel;
					greenChannel = (greenChannel > 255) ? 255 : greenChannel;
					greenChannel = (greenChannel < 0) ? 0 : greenChannel;
					blueChannel = (blueChannel > 255) ? 255 : blueChannel;
					blueChannel = (blueChannel < 0) ? 0 : blueChannel;

					img.SetPixel(x, y, Color.FromArgb(redChannel, greenChannel, blueChannel));
				}
			}
		}

		/// <summary>
		///     decrease the depth of color
		/// </summary>
		/// <param name="img">
		///     image to manipulate
		/// </param>
		/// <param name="offset">
		///     offset of colordepth
		/// </param>
		public static void DecreaseColourDepth(ref ImageData img, int offset)
		{
			for(int y = 0; y < img.Height; y++)
			{
				for(int x = 0; x < img.Width; x++)
				{
					Color pixelColor = img[x, y];
					int redChannel = (pixelColor.R + (offset / 2)) - ((pixelColor.R + (offset / 2)) % offset) - 1;
					int greenChannel = (pixelColor.G + (offset / 2)) - ((pixelColor.G + (offset / 2)) % offset) - 1;
					int blueChannel = (pixelColor.B + (offset / 2)) - ((pixelColor.B + (offset / 2)) % offset) - 1;

					redChannel = (redChannel < 0) ? 0 : redChannel;
					greenChannel = (greenChannel < 0) ? 0 : greenChannel;
					blueChannel = (blueChannel < 0) ? 0 : blueChannel;

					img.SetPixel(x, y, Color.FromArgb(redChannel, greenChannel, blueChannel));
				}
			}
		}

		/// <summary>
		///     apply emboss effect on image
		/// </summary>
		/// <param name="img">
		///     image to manipulate
		/// </param>
		/// <param name="weight">
		///     weight of emboss effect
		/// </param>
		public static void Emboss(ref ImageData img, double weight)
		{
			ConvolutionMatrix cMatrix = new ConvolutionMatrix(3);
			cMatrix.SetAll(1);
			cMatrix.Matrix[0, 0] = -1;
			cMatrix.Matrix[1, 0] = 0;
			cMatrix.Matrix[2, 0] = -1;
			cMatrix.Matrix[0, 1] = 0;
			cMatrix.Matrix[1, 1] = weight;
			cMatrix.Matrix[2, 1] = 0;
			cMatrix.Matrix[0, 2] = -1;
			cMatrix.Matrix[1, 2] = 0;
			cMatrix.Matrix[2, 2] = -1;
			cMatrix.Factor = 4;
			cMatrix.Offset = 127;
			ApplyConvolution3X3(ref img, cMatrix);
		}

		/// <summary>
		///     apply gaussianblur to image
		/// </summary>
		/// <param name="img">
		///     image to manipulate
		/// </param>
		/// <param name="peakValue">
		///     parameter
		/// </param>
		public static void GaussianBlur(ref ImageData img, double peakValue)
		{
			ConvolutionMatrix cMatrix = new ConvolutionMatrix(3);
			cMatrix.SetAll(1);
			cMatrix.Matrix[0, 0] = peakValue / 4;
			cMatrix.Matrix[1, 0] = peakValue / 2;
			cMatrix.Matrix[2, 0] = peakValue / 4;
			cMatrix.Matrix[0, 1] = peakValue / 2;
			cMatrix.Matrix[1, 1] = peakValue;
			cMatrix.Matrix[2, 1] = peakValue / 2;
			cMatrix.Matrix[0, 2] = peakValue / 4;
			cMatrix.Matrix[1, 2] = peakValue / 2;
			cMatrix.Matrix[2, 2] = peakValue / 4;
			cMatrix.Factor = peakValue * 4;
			ApplyConvolution3X3(ref img, cMatrix);
		}

		/// <summary>
		///     sharpen an image
		/// </summary>
		/// <param name="img">
		///     image to manipulate
		/// </param>
		/// <param name="weight">
		///     weight
		/// </param>
		public static void Sharpen(ref ImageData img, double weight)
		{
			ConvolutionMatrix cMatrix = new ConvolutionMatrix(3);
			cMatrix.SetAll(1);
			cMatrix.Matrix[0, 0] = 0;
			cMatrix.Matrix[1, 0] = -2;
			cMatrix.Matrix[2, 0] = 0;
			cMatrix.Matrix[0, 1] = -2;
			cMatrix.Matrix[1, 1] = weight;
			cMatrix.Matrix[2, 1] = -2;
			cMatrix.Matrix[0, 2] = 0;
			cMatrix.Matrix[1, 2] = -2;
			cMatrix.Matrix[2, 2] = 0;
			cMatrix.Factor = weight - 8;
			ApplyConvolution3X3(ref img, cMatrix);
		}

		/// <summary>
		///     remove mean of image
		/// </summary>
		/// <param name="img">
		///     image to manipulate
		/// </param>
		/// <param name="weight">
		///     weight of effect
		/// </param>
		public static void RemoveMean(ref ImageData img, double weight)
		{
			ConvolutionMatrix cMatrix = new ConvolutionMatrix(3);
			cMatrix.SetAll(1);
			cMatrix.Matrix[0, 0] = -1;
			cMatrix.Matrix[1, 0] = -1;
			cMatrix.Matrix[2, 0] = -1;
			cMatrix.Matrix[0, 1] = -1;
			cMatrix.Matrix[1, 1] = weight;
			cMatrix.Matrix[2, 1] = -1;
			cMatrix.Matrix[0, 2] = -1;
			cMatrix.Matrix[1, 2] = -1;
			cMatrix.Matrix[2, 2] = -1;
			cMatrix.Factor = weight - 8;
			ApplyConvolution3X3(ref img, cMatrix);
		}

		/// <summary>
		///     blur the image
		/// </summary>
		/// <param name="img">
		///     image to manipulate
		/// </param>
		/// <param name="weight">
		///     weight of effect
		/// </param>
		public static void Blur(ref ImageData img, double weight)
		{
			ConvolutionMatrix cMatrix = new ConvolutionMatrix(3);
			cMatrix.SetAll(1);
			cMatrix.Matrix[1, 1] = weight;
			cMatrix.Factor = weight + 8;
			ApplyConvolution3X3(ref img, cMatrix);
		}

		/// <summary>
		///     marks the edges black and all other pixels white
		/// </summary>
		/// <param name="img">
		///     image to manipulate
		/// </param>
		public static void FindEdges(ref ImageData img)
		{
			Emboss(ref img, 4.0);
			ReplaceSimilarColor(ref img, Color.FromArgb(127, 132, 127), Color.White, 20);
			ReplaceDifferentColor(ref img, Color.White, Color.Black, 1);
		}

		/// <summary>
		///     The apply convolution 3 x 3.
		/// </summary>
		/// <param name="img">
		///     The img.
		/// </param>
		/// <param name="matrix">
		///     The matrix.
		/// </param>
		private static void ApplyConvolution3X3(ref ImageData img, ConvolutionMatrix matrix)
		{
			ImageData newImg = img.Clone();
			Color[,] pixelColor = new Color[3, 3];

			for(int y = 0; y < img.Height - 2; y++)
			{
				for(int x = 0; x < img.Width - 2; x++)
				{
					pixelColor[0, 0] = img[x, y];
					pixelColor[0, 1] = img[x, y + 1];
					pixelColor[0, 2] = img[x, y + 2];
					pixelColor[1, 0] = img[x + 1, y];
					pixelColor[1, 1] = img[x + 1, y + 1];
					pixelColor[1, 2] = img[x + 1, y + 2];
					pixelColor[2, 0] = img[x + 2, y];
					pixelColor[2, 1] = img[x + 2, y + 1];
					pixelColor[2, 2] = img[x + 2, y + 2];

					int alphaChannel = pixelColor[1, 1].A;

					int redChannel =
						(int)
						((((pixelColor[0, 0].R * matrix.Matrix[0, 0]) + (pixelColor[1, 0].R * matrix.Matrix[1, 0])
							+ (pixelColor[2, 0].R * matrix.Matrix[2, 0]) + (pixelColor[0, 1].R * matrix.Matrix[0, 1])
							+ (pixelColor[1, 1].R * matrix.Matrix[1, 1]) + (pixelColor[2, 1].R * matrix.Matrix[2, 1])
							+ (pixelColor[0, 2].R * matrix.Matrix[0, 2]) + (pixelColor[1, 2].R * matrix.Matrix[1, 2])
							+ (pixelColor[2, 2].R * matrix.Matrix[2, 2])) / matrix.Factor) + matrix.Offset);

					int greenChannel =
						(int)
						((((pixelColor[0, 0].G * matrix.Matrix[0, 0]) + (pixelColor[1, 0].G * matrix.Matrix[1, 0])
							+ (pixelColor[2, 0].G * matrix.Matrix[2, 0]) + (pixelColor[0, 1].G * matrix.Matrix[0, 1])
							+ (pixelColor[1, 1].G * matrix.Matrix[1, 1]) + (pixelColor[2, 1].G * matrix.Matrix[2, 1])
							+ (pixelColor[0, 2].G * matrix.Matrix[0, 2]) + (pixelColor[1, 2].G * matrix.Matrix[1, 2])
							+ (pixelColor[2, 2].G * matrix.Matrix[2, 2])) / matrix.Factor) + matrix.Offset);

					int blueChannel =
						(int)
						((((pixelColor[0, 0].B * matrix.Matrix[0, 0]) + (pixelColor[1, 0].B * matrix.Matrix[1, 0])
							+ (pixelColor[2, 0].B * matrix.Matrix[2, 0]) + (pixelColor[0, 1].B * matrix.Matrix[0, 1])
							+ (pixelColor[1, 1].B * matrix.Matrix[1, 1]) + (pixelColor[2, 1].B * matrix.Matrix[2, 1])
							+ (pixelColor[0, 2].B * matrix.Matrix[0, 2]) + (pixelColor[1, 2].B * matrix.Matrix[1, 2])
							+ (pixelColor[2, 2].B * matrix.Matrix[2, 2])) / matrix.Factor) + matrix.Offset);

					redChannel = (redChannel > 255) ? 255 : redChannel;
					redChannel = (redChannel < 0) ? 0 : redChannel;
					greenChannel = (greenChannel > 255) ? 255 : greenChannel;
					greenChannel = (greenChannel < 0) ? 0 : greenChannel;
					blueChannel = (blueChannel > 255) ? 255 : blueChannel;
					blueChannel = (blueChannel < 0) ? 0 : blueChannel;

					newImg.SetPixel(x + 1, y + 1, Color.FromArgb(alphaChannel, redChannel, greenChannel, blueChannel));
				}
			}

			img = newImg.Clone();
		}
	}
}
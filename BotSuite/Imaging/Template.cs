// -----------------------------------------------------------------------
//  <copyright file="Template.cs" company="Binary Overdrive">
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
	///     serach for patterns or images in a image
	/// </summary>
	public static class Template
	{
		/// <summary>
		///     search for binary patterns
		/// </summary>
		/// <param name="img">
		///     image to look in
		/// </param>
		/// <param name="pattern">
		///     pattern to look for
		/// </param>
		/// <param name="tolerance">
		///     tolerance (0,...,255)
		/// </param>
		/// <returns>
		///     The <see cref="Rectangle" />.
		/// </returns>
		public static Rectangle BinaryPattern(ImageData img, bool[,] pattern, uint tolerance = 0)
		{
			// simple
			Point location = Point.Empty;

			Color referenceColor = Color.Wheat;
			bool first = true;
			for(int outerColumn = 0; outerColumn < img.Width - pattern.GetLength(1); outerColumn++)
			{
				for(int outerRow = 0; outerRow < img.Height - pattern.GetLength(0); outerRow++)
				{
					for(int innerColumn = 0; innerColumn < pattern.GetLength(1); innerColumn++)
					{
						for(int innerRow = 0; innerRow < pattern.GetLength(0); innerRow++)
						{
							if(pattern[innerRow, innerColumn])
							{
								if(first)
								{
									referenceColor = img[outerColumn, outerRow];
									first = false;
								}
								else
								{
									if(CommonFunctions.ColorsSimilar(
										referenceColor,
										img[outerColumn + innerColumn, outerRow + innerRow],
										tolerance))
									{
										// ok
									}
									else
									{
										// schlecht passt nicht
										innerColumn = pattern.GetLength(1) + 10;
										first = true;
										break;
									}
								}
							}
							else
							{
								if(first == false)
								{
									// darf nicht passen!
									if(CommonFunctions.ColorsSimilar(
										referenceColor,
										img[outerColumn + innerColumn, outerRow + innerRow],
										tolerance))
									{
										// schlecht passt
										innerColumn = img.Width + 10;
										innerRow = img.Height + 10;
										first = true;
									}
								}
							}
						}
					}

					if(first == false)
					{
						// matched
						location.X = outerColumn;
						location.Y = outerRow;
						return new Rectangle(location.X, location.Y, pattern.GetLength(1), pattern.GetLength(0));
					}
				}
			}

			return Rectangle.Empty;
		}

		/// <summary>
		///     search for an image in another image
		///     return the best matching position
		/// </summary>
		/// <param name="img">
		///     image to look in
		/// </param>
		/// <param name="Ref">
		///     image to look for
		/// </param>
		/// <param name="tolerance">
		///     tolerance of similarity (0,...,255)
		/// </param>
		/// <returns>
		///     best matching position as rectangle
		/// </returns>
		public static Rectangle Image(ImageData img, ImageData Ref, uint tolerance = 0)
		{
			double bestScore = Math.Abs(byte.MaxValue - byte.MinValue) * 3;

			Point location = Point.Empty;
			bool found = false;

			for(int originalX = 0; originalX < img.Width - Ref.Width; originalX++)
			{
				for(int originalY = 0; originalY < img.Height - Ref.Height; originalY++)
				{
					Color currentInnerPictureColor = Ref[0, 0];
					Color currentOuterPictureColor = img[originalX, originalY];

					if(!CommonFunctions.ColorsSimilar(currentInnerPictureColor, currentOuterPictureColor, tolerance))
					{
						continue;
					}
					int currentScore = 0;
					bool allSimilar = true;
					for(int referenceX = 0; referenceX < Ref.Width; referenceX++)
					{
						if(!allSimilar)
						{
							break;
						}
						for(int referenceY = 0; referenceY < Ref.Height; referenceY++)
						{
							if(!allSimilar)
							{
								break;
							}
							currentInnerPictureColor = Ref[referenceX, referenceY];
							currentOuterPictureColor = img[originalX + referenceX, originalY + referenceY];

							if(!CommonFunctions.ColorsSimilar(currentInnerPictureColor, currentOuterPictureColor, tolerance))
							{
								allSimilar = false;
							}

							currentScore += Math.Abs(currentInnerPictureColor.R - currentOuterPictureColor.R)
											+ Math.Abs(currentInnerPictureColor.G - currentOuterPictureColor.G)
											+ Math.Abs(currentInnerPictureColor.B - currentOuterPictureColor.B);
						}
					}

					if(allSimilar)
					{
						if((currentScore / (double)(Ref.Width * Ref.Height)) < bestScore)
						{
							location.X = originalX;
							location.Y = originalY;
							bestScore = currentScore / (double)(Ref.Width * Ref.Height);
							found = true;
						}
					}
				}
			}

			return found ? new Rectangle(location.X, location.Y, Ref.Width, Ref.Height) : Rectangle.Empty;
		}

		/// <summary>
		///     search for an image in another image
		///     return all possible matchings
		/// </summary>
		/// <param name="img">
		///     image to look in
		/// </param>
		/// <param name="Ref">
		///     image to look for
		/// </param>
		/// <param name="tolerance">
		///     tolerance of similarity (0,...,255)
		/// </param>
		/// <returns>
		///     List of matching positions (datatype Rectange)
		/// </returns>
		public static List<Rectangle> AllImages(ImageData img, ImageData Ref, uint tolerance = 0)
		{
			List<Rectangle> retVal = new List<Rectangle>();

			for(int originalX = 0; originalX < img.Width - Ref.Width; originalX++)
			{
				for(int originalY = 0; originalY < img.Height - Ref.Height; originalY++)
				{
					Color currentInnerPictureColor = Ref[0, 0];
					Color currentOuterPictureColor = img[originalX, originalY];

					if(!CommonFunctions.ColorsSimilar(currentInnerPictureColor, currentOuterPictureColor, tolerance))
					{
						continue;
					}

					bool allSimilar = true;
					for(int referenceX = 0; referenceX < Ref.Width; referenceX++)
					{
						if(!allSimilar)
						{
							break;
						}
						for(int referenceY = 0; referenceY < Ref.Height; referenceY++)
						{
							if(!allSimilar)
							{
								break;
							}
							currentInnerPictureColor = Ref[referenceX, referenceY];
							currentOuterPictureColor = img[originalX + referenceX, originalY + referenceY];

							if(!CommonFunctions.ColorsSimilar(currentInnerPictureColor, currentOuterPictureColor, tolerance))
							{
								allSimilar = false;
							}
						}
					}

					if(allSimilar)
					{
						retVal.Add(new Rectangle(originalX, originalY, Ref.Width, Ref.Height));
					}
				}
			}

			return retVal;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Drawing;

namespace BotSuite.ImageLibrary
{
    /// <summary>
    /// serach for patterns or images in a image
    /// </summary>
    public static class Template
    {
        /// <summary>
        /// Searches for a binary pattern
        /// </summary>
        /// <param name="img">The image to look in</param>
        /// <param name="pattern">The pattern to look for</param>
        /// <param name="tolerance">Tolerance (0,...,255)</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData img = new ImageData(...);
        /// int[,] pattern = new int[,] {   
        ///         {1,1,0,0,0,0,0,0},
        ///         {0,0,1,0,0,0,0,0},
        ///         {0,0,1,0,0,0,0,0},
        ///         {0,1,0,0,0,0,0,0},
        ///         {1,0,0,0,0,0,0,0},
        ///         {1,1,1,0,0,0,0,0},
        ///         {0,0,0,0,0,0,0,0},
        ///         {0,0,0,0,0,0,0,0}
        ///     };
        /// Rectangle location = Template.BinaryPattern(img, pattern, 2);
        /// ]]>
        /// </code>
        /// </example>
        /// <returns></returns>
        public static Rectangle BinaryPattern(ImageData img, int[,] pattern, uint tolerance = 0)
        {
            Point location = Point.Empty;
            Color referenceColor = Color.Empty;

            for (int imageColumn = 0; imageColumn < img.Width - pattern.GetLength(1); imageColumn++)
            {
                for (int imageRow = 0; imageRow < img.Height - pattern.GetLength(0); imageRow++)
                {
                    bool patternMatch = true;
                    for (int patternColumn = 0; patternColumn < pattern.GetLength(1); patternColumn++)
                    {
                        if (!patternMatch) break;
                        for (int patternRow = 0; patternRow < pattern.GetLength(0); patternRow++)
                        {
                            if (!patternMatch) break;
                            if (pattern[patternRow, patternColumn] == 1)
                            {
                                if (referenceColor == Color.Empty)
                                {
                                    referenceColor = img[imageColumn, imageRow];
                                }
                                else
                                {
                                    if (!CommonFunctions.ColorsSimilar(referenceColor,
                                        img[imageColumn + patternColumn, imageRow + patternRow], tolerance))
                                    {
                                        patternMatch = false;
                                        referenceColor = Color.Empty;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (referenceColor != Color.Empty)
                                {
                                    if (CommonFunctions.ColorsSimilar(referenceColor,
                                        img[imageColumn + patternColumn, imageRow + patternRow], tolerance))
                                    {
                                        patternMatch = false;
                                        referenceColor = Color.Empty;
                                    }
                                }
                            }
                        }
                    }
                    if (patternMatch)
                    {
                        location.X = imageColumn;
                        location.Y = imageRow;
                        return new Rectangle(location.X, location.Y, pattern.GetLength(1), pattern.GetLength(0));
                    }
                }
            }

            return Rectangle.Empty;
        }

        /// <summary>
        /// Converts an Integer MultiArray to a Boolean MultiArray
        /// </summary>
        /// <param name="input">The input Integer MultiArray</param>
        /// <returns>The Boolean MultiArray</returns>
        public static bool[,] ToMultiArrayBool(this int[,] input)
        {
            int columnCount = input.GetUpperBound(0);
            int rowCount = input.GetLength(0);
            var result = new bool[rowCount, columnCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    result[i, j] = input[i, j] == 1;
                }
            }

            return result;
        }


        /// <summary>
        /// Searches for an image in another image
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// ImageData Search= new ImageData(...);
        /// // search with tolerance 25
        /// Rectangle Position = Template.Image(Img,Search,25);
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="img">Image to look in</param>
        /// <param name="Ref">Image to look for</param>
        /// <param name="tolerance">Tolerance of similarity (0,...,255)</param>
        /// <returns>The best matching position as a rectangle</returns>
        public static Rectangle Image(ImageData img, ImageData Ref, uint tolerance = 0)
        {
            Double bestScore = (Math.Abs(Byte.MaxValue - Byte.MinValue)*3);

            Point location = Point.Empty;
            Boolean found = false;

            for (int originalX = 0; originalX < img.Width - Ref.Width; originalX++)
            {
                for (int originalY = 0; originalY < img.Height - Ref.Height; originalY++)
                {
                    Color currentInnerPictureColor = Ref[0, 0];
                    Color currentOuterPictureColor = img[originalX, originalY];

                    if (CommonFunctions.ColorsSimilar(currentInnerPictureColor, currentOuterPictureColor, tolerance))
                    {
                        Int32 currentScore = 0;
                        Boolean allSimilar = true;
                        for (int referenceX = 0; referenceX < Ref.Width; referenceX++)
                        {
                            if (!allSimilar) break;
                            for (Int32 referenceY = 0; referenceY < Ref.Height; referenceY++)
                            {
                                if (!allSimilar) break;
                                currentInnerPictureColor = Ref[referenceX, referenceY];
                                currentOuterPictureColor = img[originalX + referenceX, originalY + referenceY];

                                if (
                                    !CommonFunctions.ColorsSimilar(currentInnerPictureColor, currentOuterPictureColor,
                                        tolerance))
                                    allSimilar = false;

                                currentScore +=
                                    (Math.Abs(currentInnerPictureColor.R - currentOuterPictureColor.R)
                                     + Math.Abs(currentInnerPictureColor.G - currentOuterPictureColor.G)
                                     + Math.Abs(currentInnerPictureColor.B - currentOuterPictureColor.B));
                            }
                        }

                        if (allSimilar)
                        {
                            if ((currentScore/(Double) (Ref.Width*Ref.Height)) < bestScore)
                            {
                                location.X = originalX;
                                location.Y = originalY;
                                bestScore = (currentScore/(Double) (Ref.Width*Ref.Height));
                                found = true;
                            }
                        }
                    }
                }
            }
            if (found)
                return new Rectangle(location.X, location.Y, Ref.Width, Ref.Height);
            return Rectangle.Empty;
        }

        /// <summary>
        /// Searches for an image in another image
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// ImageData Search= new ImageData(...);
        /// // search with tolerance 25
        /// List<Rectangle> Positions = Template.AllImages(Img,Search,25);
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="img">Image to look in</param>
        /// <param name="Ref">Image to look for</param>
        /// <param name="tolerance">Tolerance of similarity (0,...,255)</param>
        /// <returns>A Rectangle list of matching positions</returns>
        public static List<Rectangle> AllImages(ImageData img, ImageData Ref, uint tolerance = 0)
        {
            var retVal = new List<Rectangle>();

            for (int originalX = 0; originalX < img.Width - Ref.Width; originalX++)
            {
                for (int originalY = 0; originalY < img.Height - Ref.Height; originalY++)
                {
                    Color currentInnerPictureColor = Ref[0, 0];
                    Color currentOuterPictureColor = img[originalX, originalY];

                    if (CommonFunctions.ColorsSimilar(currentInnerPictureColor, currentOuterPictureColor, tolerance))
                    {
                        Boolean allSimilar = true;
                        for (int referenceX = 0; referenceX < Ref.Width; referenceX++)
                        {
                            if (!allSimilar)
                                break;
                            for (Int32 referenceY = 0; referenceY < Ref.Height; referenceY++)
                            {
                                if (!allSimilar)
                                    break;
                                currentInnerPictureColor = Ref[referenceX, referenceY];
                                currentOuterPictureColor = img[originalX + referenceX, originalY + referenceY];

                                if (
                                    !CommonFunctions.ColorsSimilar(currentInnerPictureColor, currentOuterPictureColor,
                                        tolerance))
                                    allSimilar = false;
                            }
                        }

                        if (allSimilar)
                        {
                            retVal.Add(new Rectangle(originalX, originalY, Ref.Width, Ref.Height));
                        }
                    }
                }
            }
            return retVal;
        }
    }
}

using System;
using System.Drawing;
using System.Collections.Generic;

namespace BotSuite.ImageLibrary
{
    /// <summary>
    /// serach for patterns or images in a image
    /// </summary>
    static public class Template
    {
        /// <summary>
        /// search for binary patterns
        /// </summary>
        /// <param name="Img">image to look in</param>
        /// <param name="Pattern">pattern to look for</param>
        /// <param name="Tolerance">tolerance (0,...,255)</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// bool[,] Pattern = new bool[] {   
        ///             {1,1,0,0,0,0,0,0},
        ///             {0,0,1,0,0,0,0,0},
        ///             {0,0,1,0,0,0,0,0},
        ///             {0,1,0,0,0,0,0,0},
        ///             {1,0,0,0,0,0,0,0},
        ///             {1,1,1,0,0,0,0,0},
        ///             {0,0,0,0,0,0,0,0},
        ///             {0,0,0,0,0,0,0,0}
        ///         };
        /// Rectangle Location = Template.BinaryPattern(Img,Pattern,2);
        /// ]]>
        /// </code>
        /// </example>
        /// <returns></returns>
        static public Rectangle BinaryPattern(ImageData Img, bool[,] Pattern, uint Tolerance = 0)
        {
            // simple
            Point location = Point.Empty;

            Color ReferenceColor = Color.Wheat;
            bool first = true;
            for (Int32 OuterColumn = 0; OuterColumn < Img.Width - Pattern.GetLength(1); OuterColumn++)
            {
                for (Int32 OuterRow = 0; OuterRow < Img.Height - Pattern.GetLength(0); OuterRow++)
                {
                    for (Int32 InnerColumn = 0; InnerColumn < Pattern.GetLength(1); InnerColumn++)
                    {
                        for (Int32 InnerRow = 0; InnerRow < Pattern.GetLength(0); InnerRow++)
                        {
                            if (Pattern[InnerRow, InnerColumn] == true)
                            {
                                if (first == true)
                                {
                                    ReferenceColor = Img[OuterColumn, OuterRow];
                                    first = false;
                                }
                                else
                                {
                                    if (CommonFunctions.ColorsSimilar(ReferenceColor, Img[OuterColumn + InnerColumn, OuterRow + InnerRow], Tolerance))
                                    {
                                        // ok
                                    }
                                    else
                                    {
                                        // schlecht passt nicht
                                        InnerColumn = Pattern.GetLength(1) + 10;
                                        InnerRow = Pattern.GetLength(0) + 10;
                                        first = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (first == false)
                                {
                                    // darf nicht passen!
                                    if (CommonFunctions.ColorsSimilar(ReferenceColor, Img[OuterColumn + InnerColumn, OuterRow + InnerRow], Tolerance))
                                    {
                                        // schlecht passt
                                        InnerColumn = Img.Width + 10;
                                        InnerRow = Img.Height + 10;
                                        first = true;
                                    }
                                }
                            }
                        }
                    }
                    if (first == false)
                    {
                        //matched
                        location.X = OuterColumn;
                        location.Y = OuterRow;
                        return new Rectangle(location.X, location.Y, Pattern.GetLength(1), Pattern.GetLength(0));
                    }

                }
            }

            return Rectangle.Empty;
        }
        /// <summary>
        /// search for an image in another image
        /// return the best matching position
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
        /// <param name="Img">image to look in</param>
        /// <param name="Ref">image to look for</param>
        /// <param name="Tolerance">tolerance of similarity (0,...,255)</param>
        /// <returns>best matching position as rectangle</returns>
        static public Rectangle Image(ImageData Img, ImageData Ref, uint Tolerance = 0)
        {
            Double bestScore = (Math.Abs(Byte.MaxValue - Byte.MinValue) * 3);

            Int32 currentScore = 0;
            Color CurrentInnerPictureColor;
            Color CurrentOuterPictureColor;
            Boolean allSimilar = true;
            Point location = Point.Empty;
            Boolean Found = false;

            for (Int32 originalX = 0; originalX < Img.Width - Ref.Width; originalX++)
            {
                for (Int32 originalY = 0; originalY < Img.Height - Ref.Height; originalY++)
                {
                    CurrentInnerPictureColor = Ref[0, 0];
                    CurrentOuterPictureColor = Img[originalX, originalY];

                    if (CommonFunctions.ColorsSimilar(CurrentInnerPictureColor, CurrentOuterPictureColor, Tolerance))
                    {
                        currentScore = 0;
                        allSimilar = true;
                        for (Int32 referenceX = 0; referenceX < Ref.Width; referenceX++)
                        {
                            if (!allSimilar)
                                break;
                            for (Int32 referenceY = 0; referenceY < Ref.Height; referenceY++)
                            {
                                if (!allSimilar)
                                    break;
                                CurrentInnerPictureColor = Ref[referenceX, referenceY];
                                CurrentOuterPictureColor = Img[originalX + referenceX, originalY + referenceY];

                                if (!CommonFunctions.ColorsSimilar(CurrentInnerPictureColor, CurrentOuterPictureColor, Tolerance))
                                    allSimilar = false;

                                currentScore +=
                                    (Math.Abs(CurrentInnerPictureColor.R - CurrentOuterPictureColor.R)
                                    + Math.Abs(CurrentInnerPictureColor.G - CurrentOuterPictureColor.G)
                                    + Math.Abs(CurrentInnerPictureColor.B - CurrentOuterPictureColor.B));
                            }
                        }

                        if (allSimilar)
                        {
                            if (((Double)currentScore / (Double)(Ref.Width * Ref.Height)) < bestScore)
                            {
                                location.X = originalX;
                                location.Y = originalY;
                                bestScore = ((Double)currentScore / (Double)(Ref.Width * Ref.Height));
                                Found = true;
                            }
                        }
                    }
                }
            }
            if (Found)
                return new Rectangle(location.X, location.Y, Ref.Width, Ref.Height);
            else
                return Rectangle.Empty;
        }
        /// <summary>
        /// search for an image in another image
        /// return all possible matchings
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
        /// <param name="Img">image to look in</param>
        /// <param name="Ref">image to look for</param>
        /// <param name="Tolerance">tolerance of similarity (0,...,255)</param>
        /// <returns>List of matching positions (datatype Rectange)</returns>
        static public List<Rectangle> AllImages(ImageData Img, ImageData Ref, uint Tolerance = 0)
        {
            Double bestScore = (Math.Abs(Byte.MaxValue - Byte.MinValue) * 3);
            List<Rectangle> RetVal = new List<Rectangle>();
            Int32 currentScore = 0;
            Color CurrentInnerPictureColor;
            Color CurrentOuterPictureColor;
            Boolean allSimilar = true;
            //Point location = Point.Empty;
            Boolean Found = false;

            for (Int32 originalX = 0; originalX < Img.Width - Ref.Width; originalX++)
            {
                for (Int32 originalY = 0; originalY < Img.Height - Ref.Height; originalY++)
                {
                    CurrentInnerPictureColor = Ref[0, 0];
                    CurrentOuterPictureColor = Img[originalX, originalY];

                    if (CommonFunctions.ColorsSimilar(CurrentInnerPictureColor, CurrentOuterPictureColor, Tolerance))
                    {
                        currentScore = 0;
                        allSimilar = true;
                        for (Int32 referenceX = 0; referenceX < Ref.Width; referenceX++)
                        {
                            if (!allSimilar)
                                break;
                            for (Int32 referenceY = 0; referenceY < Ref.Height; referenceY++)
                            {
                                if (!allSimilar)
                                    break;
                                CurrentInnerPictureColor = Ref[referenceX, referenceY];
                                CurrentOuterPictureColor = Img[originalX + referenceX, originalY + referenceY];

                                if (!CommonFunctions.ColorsSimilar(CurrentInnerPictureColor, CurrentOuterPictureColor, Tolerance))
                                    allSimilar = false;

                                currentScore +=
                                    (Math.Abs(CurrentInnerPictureColor.R - CurrentOuterPictureColor.R)
                                    + Math.Abs(CurrentInnerPictureColor.G - CurrentOuterPictureColor.G)
                                    + Math.Abs(CurrentInnerPictureColor.B - CurrentOuterPictureColor.B));
                            }
                        }

                        if (allSimilar)
                        {
                            RetVal.Add(new Rectangle(originalX, originalY, Ref.Width, Ref.Height));
                        }
                    }
                }
            }
            return RetVal;
        }
    }
}

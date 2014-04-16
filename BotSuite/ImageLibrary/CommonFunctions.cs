using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BotSuite.ImageLibrary
{
    /// <summary>
    ///     collection of common functions
    /// </summary>
    public static class CommonFunctions
    {
        /// <summary>
        ///     Tests if the colors are similar.
        /// </summary>
        /// <example>
        ///     <code>
        /// <![CDATA[
        /// Color A = Color.White;
        /// Color B = Color.Blue;
        /// bool similar = CommonFunctions.ColorsSimilar(A,B,50);
        /// bool match   = CommonFunctions.ColorsSimilar(A,B,0);
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="one">Color A</param>
        /// <param name="two">Color B</param>
        /// <param name="tolerance">Tolerance (0,...,255)</param>
        /// <returns>Similar or not</returns>
        public static Boolean ColorsSimilar(Color one, Color two, uint tolerance)
        {
            if (Math.Abs(one.R - two.R) > tolerance)
                return false;
            if (Math.Abs(one.G - two.G) > tolerance)
                return false;
            if (Math.Abs(one.B - two.B) > tolerance)
                return false;
            return true;
        }

        /// <summary>
        ///     Gets the average RGB values from a given rectangle in an image
        ///     By default the average RGB values from the whole image are calculated
        /// </summary>
        /// <example>
        ///     <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// double[] AverageColor = CommonFunctions.AverageRGBValues(img);
        /// int left = 0;
        /// int top = 5;
        /// double[] AverageColorShifted = CommonFunctions.AverageRGBValues(img,left,top);
        /// int width = 50;
        /// int height = 100;
        /// double[] AverageColorRectangle = CommonFunctions.AverageRGBValues(img,left,top,width,height);
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="img">The image to process</param>
        /// <param name="left">The left of the rectangle (default=0)</param>
        /// <param name="top">The top of the rectangle (default=0)</param>
        /// <param name="width">The width of the rectangle (default=full width)</param>
        /// <param name="height">The height of the rectangle (default=full height)</param>
        /// <returns>The average RGB values</returns>
        public static double[] AverageRGBValues(ImageData img, int left = 0, int top = 0, int width = -1,
            int height = -1)
        {
            long[] totals = {0, 0, 0};

            if (width == -1)
                width = img.Width;
            if (height == -1)
                height = img.Height;

            for (int x = left; x < left + width; x++)
            {
                for (int y = top; y < top + height; y++)
                {
                    Color currentColor = img.GetPixel(x, y);
                    totals[0] += currentColor.R;
                    totals[1] += currentColor.G;
                    totals[2] += currentColor.B;
                }
            }
            int count = width*height;
            double[] retvar = {(totals[0]/(double) count), (totals[1]/(double) count), (totals[2]/(double) count)};
            return retvar;
        }

        /// <summary>
        ///     Gets the average color from an image in a given rectangle
        ///     By default the the average color from the whole image is calculated
        /// </summary>
        /// <param name="img">The image to process</param>
        /// <param name="left">The left of the rectangle (default=0)</param>
        /// <param name="top">The top of the rectangle (default=0)</param>
        /// <param name="width">The width of the rectangle (default=full width)</param>
        /// <param name="height">The height of the rectangle (default=full height)</param>
        /// <returns>average color</returns>
        public static Color AverageColor(ImageData img, int left = 0, int top = 0, int width = -1, int height = -1)
        {
            double[] retvar = AverageRGBValues(img, left, top, width, height);
            return Color.FromArgb(Convert.ToInt32(retvar[0]), Convert.ToInt32(retvar[1]), Convert.ToInt32(retvar[2]));
        }

        /// <summary>
        ///     Calculates the similarity of image A in a given rectangle and a reference image B
        /// </summary>
        /// <param name="img">Image A</param>
        /// <param name="reference">Image B</param>
        /// <param name="left">The offset from left of image A</param>
        /// <param name="top">The offset from top of image A</param>
        /// <param name="width">The width of the rectangle (default: full width)</param>
        /// <param name="height">The height of the rectangle (default: full height)</param>
        /// <param name="offsetLeft">The left offset</param>
        /// <param name="offsetTop">The top offset</param>
        /// <returns>The similarity result (1=exact, 0=none)</returns>
        public static double Similarity(ImageData img, ImageData reference, int left = 0, int top = 0, int width = -1,
            int height = -1, int offsetLeft = 0, int offsetTop = 0)
        {
            double sim = 0.0;
            width = (width == -1) ? img.Width - left : width;
            height = (height == -1) ? img.Height - top : height;

            if ((img.Width == reference.Width) && (img.Height == reference.Height))
            {
                for (Int32 column = left; column < left + width; column++)
                {
                    for (Int32 row = top; row < top + height; row++)
                    {
                        Color a = img.GetPixel(offsetLeft + column, offsetTop + row);
                        Color b = reference.GetPixel(column, row);

                        int cr = Math.Abs(a.R - b.R);
                        int cg = Math.Abs(a.G - b.G);
                        int cb = Math.Abs(a.B - b.B);

                        sim += (cr + cg + cb)/3; //TODO: Fix possible loss of fraction
                    }
                }
                sim /= 255.0;
                sim /= (img.Height*img.Width);
            }
            return 1 - sim;
        }

        /// <summary>
        ///     Identifies the colors of the pixels in a rectangle and compares them with those from an image by a given list of
        ///     reference colors (root means square error
        ///     from average)
        /// </summary>
        /// <param name="img">The image to look in</param>
        /// <param name="statReference">The list of possible colors</param>
        /// <param name="left">The left of the rectangle (default: 0)</param>
        /// <param name="top">The top of the rectangle (default: 0)</param>
        /// <param name="width">The width of the rectangle (default: full width)</param>
        /// <param name="height">The height of the rectangle (default: full height)</param>
        /// <returns>The best matching color</returns>
        public static Color IdentifyColor(ImageData img, Dictionary<Color, List<double>> statReference, int left = 0,
            int top = 0, int width = -1, int height = -1)
        {
            double[] av = AverageRGBValues(img, left, top, width, height);

            double bestScore = 255;

            Color foo = Color.White;

            foreach (var item in statReference)
            {
                double currentScore = Math.Pow((item.Value[0]/255.0) - (av[0]/255.0), 2)
                                      + Math.Pow((item.Value[1]/255.0) - (av[1]/255.0), 2)
                                      + Math.Pow((item.Value[2]/255.0) - (av[2]/255.0), 2);
                if (currentScore < bestScore)
                {
                    foo = item.Key;
                    bestScore = currentScore;
                }
            }
            return foo;
        }

        /// <summary>
        ///     Identifies the colors of the pixels in a rectangle and compares them with those from an image by a given list of
        ///     reference colors (majority vote)
        /// </summary>
        /// <remarks>
        ///     This tests every pixel in the given rectangle and majority votes the color from the given dictionary of possible
        ///     colors
        /// </remarks>
        /// <param name="img">The image to look in</param>
        /// <param name="statReference">The list of possible colors</param>
        /// <param name="left">The left of the rectangle (default: 0)</param>
        /// <param name="top">The top of the rectangle (default: 0)</param>
        /// <param name="width">The width of the rectangle (default: full width)</param>
        /// <param name="height">The height of the rectangle (default: full height)</param>
        /// <returns>The best matching color</returns>
        public static Color IdentifyColorByVoting(ImageData img, Dictionary<Color, List<double>> statReference,
            int left = 0, int top = 0, int width = -1, int height = -1)
        {
            int[] votes = Enumerable.Repeat(0, statReference.Count).ToArray();

            if (width == -1)
                width = img.Width;
            if (height == -1)
                height = img.Height;

            for (int x = left; x < left + width; x++)
            {
                for (int y = top; y < top + height; y++)
                {
                    // color from image
                    Color currentColor = img.GetPixel(x, y);
                    int bestDist = 255*50;
                    int bestIndex = 0;
                    for (int i = 0; i < statReference.Count; i++)
                    {
                        List<double> RGB = statReference.ElementAt(i).Value;
                        // from from dictionary
                        Color cCol = Color.FromArgb(Convert.ToInt32(RGB.ElementAt(0)), Convert.ToInt32(RGB.ElementAt(1)),
                            Convert.ToInt32(RGB.ElementAt(2)));
                        // distance
                        int currentDist = Math.Abs(cCol.R - currentColor.R) + Math.Abs(cCol.G - currentColor.G) +
                                          Math.Abs(cCol.B - currentColor.B);
                        if (currentDist < bestDist)
                        {
                            bestDist = currentDist;
                            bestIndex = i;
                        }
                    }
                    votes[bestIndex]++;
                }
            }

            // this is faster than LINQ
            int m = -1;
            int ans = 0;
            for (int i = 0; i < votes.Length; i++)
            {
                if (votes[i] > m)
                {
                    m = votes[i];
                    ans = i;
                }
            }


            return statReference.ElementAt(ans).Key;
        }

        /// <summary>
        ///     Identifies the best matching color from a list of images
        /// </summary>
        /// <param name="givenColor">The color to look for</param>
        /// <param name="statReference">The list of images</param>
        /// <returns>The best matching color from the list</returns>
        public static Color IdentifyColor(Color givenColor, Dictionary<Color, List<double>> statReference)
        {
            var av = new double[] {givenColor.R, givenColor.G, givenColor.B};

            double bestScore = 255;

            Color foo = Color.White;

            foreach (var item in statReference)
            {
                double currentScore = Math.Abs((item.Value[0]/255.0) - (av[0]/255.0))
                                      + Math.Abs((item.Value[1]/255.0) - (av[1]/255.0))
                                      + Math.Abs((item.Value[2]/255.0) - (av[2]/255.0));
                if (currentScore < bestScore)
                {
                    foo = item.Key;
                    bestScore = currentScore;
                }
            }
            return foo;
        }

        /// <summary>
        ///     Identifies the best matching image from a list of images
        /// </summary>
        /// <param name="img">The image to look for</param>
        /// <param name="statReference">The list of images</param>
        /// <returns>The name of the best matching image in the list</returns>
        public static string IdentifyImage(ImageData img, Dictionary<string, ImageData> statReference)
        {
            double similar = 0.0;
            string keyword = "";
            foreach (var item in statReference)
            {
                // exact size match cause easier way of compare the image
                if ((img.Width == item.Value.Width) && (img.Height == item.Value.Height))
                {
                    double s = Similarity(img, item.Value);
                    if (s > similar)
                    {
                        keyword = item.Key;
                        similar = s;
                    }
                }
                else
                {
                    if ((img.Width > item.Value.Width) && (img.Height > item.Value.Height))
                    {
                        // search within the greater image
                        for (int column = 0; column < img.Width - item.Value.Width; column++)
                        {
                            for (int row = 0; row < img.Height - item.Value.Height; row++)
                            {
                                double s = Similarity(img, item.Value, 0, 0, item.Value.Width, item.Value.Height, column,
                                    row);
                                if (s > similar)
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
        ///     Calculates the difference between two colors (a-b)
        /// </summary>
        /// <param name="a">Color a</param>
        /// <param name="b">Color b</param>
        /// <returns></returns>
        public static Color ColorDifference(Color a, Color b)
        {
            int cr = a.R - b.R;
            int cg = a.G - b.G;
            int cb = a.B - b.B;
            if (cr < 0)
                cr += 255;
            if (cg < 0)
                cg += 255;
            if (cb < 0)
                cb += 255;
            return Color.FromArgb(cr, cg, cb);
        }

        /// <summary>
        ///     Retrieves the red channel as an array
        /// </summary>
        /// <param name="img">image</param>
        /// <returns>The red channel as an array</returns>
        public static uint[,] ExtractRedChannel(ImageData img)
        {
            var red = new uint[img.Width, img.Height];
            for (Int32 column = 0; column < img.Width; column++)
            {
                for (Int32 row = 0; row < img.Height; row++)
                {
                    Color c = img.GetPixel(column, row);
                    red[column, row] = c.R;
                }
            }
            return red;
        }

        /// <summary>
        ///     Finds all pixels matching a specified color
        /// </summary>
        /// <param name="img">The image to look in</param>
        /// <param name="searchColor">The color to look for</param>
        /// <param name="tolerance">The tolerance to use</param>
        /// <returns></returns>
        public static List<Point> FindColors(ImageData img, Color searchColor, uint tolerance)
        {
            var collection = new List<Point>();
            for (int column = 0; column < img.Width; column++)
            {
                for (int row = 0; row < img.Height; row++)
                {
                    if (ColorsSimilar(img.GetPixel(column, row), searchColor, tolerance))
                    {
                        collection.Add(new Point(column, row));
                    }
                }
            }
            return collection;
        }

        /// <summary>
        ///     Finds all pixels matching a specified color
        ///     /accelerated/
        /// </summary>
        /// <param name="img">The image to look in</param>
        /// <param name="searchColor">The color to look for</param>
        /// <param name="skipX">The X pixels to skip each time</param>
        /// <param name="skipY">The Y pixels to skip each time</param>
        /// <param name="tolerance">The tolerance to use</param>
        /// <returns></returns>
        public static List<Point> FindColors(ImageData img, Color searchColor, uint tolerance, int skipX = 1, int skipY = 1)
        {
            if (skipX < 1 && skipY < 1) return null; // Cannot be non-positive numbers

            var collection = new List<Point>();
            for (int column = 0; column < img.Width; column = column + skipX)
            {
                for (int row = 0; row < img.Height; row = row + skipY)
                {
                    if (ColorsSimilar(img.GetPixel(column, row), searchColor, tolerance))
                    {
                        collection.Add(new Point(column, row));
                    }
                }
            }
            return collection;
        }
    }
}

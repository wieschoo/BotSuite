using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BotSuite.ImageLibrary
{
    /// <summary>
    /// collection of common functions
    /// </summary>
    static public class CommonFunctions
    {
        /// <summary>
        /// test if the colors seems to be similar.
        /// </summary>
        /// <example>
        /// <code>
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
        /// <param name="tolerance">tolerance (0,...,255)</param>
        /// <returns>similar or not</returns>
        static public Boolean ColorsSimilar(Color one, Color two, uint tolerance)
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
        /// Get the average rgb-values from an image in a given rectangle 
        /// By default the function calculate the average rgb-values from the whole image
        /// </summary>
        /// <example>
        /// <code>
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
        /// <param name="Img">Image</param>
        /// <param name="Left">left of rectangle (default=0)</param>
        /// <param name="Top">top of rectangle (default=0)</param>
        /// <param name="Width">width of rectangle (default=full width)</param>
        /// <param name="Height">height of rectangle (default=full height)</param>
        /// <returns>averag rgb-values</returns>
        static public double[] AverageRGBValues(ImageData Img, int Left = 0, int Top = 0, int Width = -1, int Height = -1)
        {

            Color CurrentColor;
            long[] totals = new long[] { 0, 0, 0 };

            if (Width == -1)
                Width = Img.Width;
            if(Height==-1)
                Height = Img.Height;

            for (int x = Left; x < Left + Width; x++)
            {
                for (int y = Top; y < Top + Height; y++)
                {
                    CurrentColor = Img.GetPixel(x, y);
                    totals[0] += CurrentColor.R;
                    totals[1] += CurrentColor.G;
                    totals[2] += CurrentColor.B;

                }
            }
            int count = Width * Height;
            double[] retvar = new double[] { (totals[0] / (double)count), (totals[1] / (double)count), (totals[2] / (double)count) };
            return retvar;
        }
        /// <summary>
        /// Get the average color from an image in a given rectangle 
        /// By default the function calculate the average color from the whole image
        /// </summary>
        /// <param name="Img">Image</param>
        /// <param name="Left">left of rectangle (default=0)</param>
        /// <param name="Top">top of rectangle (default=0)</param>
        /// <param name="Width">width of rectangle (default=full width)</param>
        /// <param name="Height">height of rectangle (default=full height)</param>
        /// <returns>average color</returns>
        static public Color AverageColor(ImageData Img, int Left = 0, int Top = 0, int Width = -1, int Height = -1)
        {
            double[] retvar = CommonFunctions.AverageRGBValues(Img, Left, Top, Width, Height);
            return Color.FromArgb(Convert.ToInt32(retvar[0]), Convert.ToInt32(retvar[1]), Convert.ToInt32(retvar[2]));
        }
        /// <summary>
        /// calculate the similarity of image A in a given rectangle and a reference image B
        /// </summary>
        /// <param name="Img">image A</param>
        /// <param name="Reference">image B</param>
        /// <param name="Left">offset from left of image A</param>
        /// <param name="Top">offset from top of image A</param>
        /// <param name="Width">width of rectangle (default: full width)</param>
        /// <param name="Height">height of rectangle (default: full height)</param>
        /// <returns>similarity (1=exact,0=none)</returns>
        static public double Similarity(ImageData Img, ImageData Reference, int Left = 0, int Top = 0, int Width = -1, int Height = -1, int OffsetLeft = 0, int OffsetTop = 0)
        {
            double sim = 0.0;
            Width = (Width == -1) ? Img.Width - Left : Width;
            Height = (Height == -1) ? Img.Height - Top : Height;

            if ((Img.Width == Reference.Width) && (Img.Height == Reference.Height))
            {
                for (Int32 column = Left; column < Left + Width; column++)
                {
                    for (Int32 row = Top; row < Top + Height; row++)
                    {
                        Color a = Img.GetPixel(OffsetLeft + column, OffsetTop + row);
                        Color b = Reference.GetPixel(column, row);

                        int cr = Math.Abs(a.R - b.R);
                        int cg = Math.Abs(a.G - b.G);
                        int cb = Math.Abs(a.B - b.B);

                        sim += (cr + cg + cb) / 3;
                    }
                }
                sim /= 255.0;
                sim /= (Img.Height * Img.Width);
            }
            return 1 - sim;
        }
        /// <summary>
        /// identify the color of a part(rectangle) from an image by a given list of reference colors (root means square error from average)
        /// </summary>
        /// <param name="Img">image to look in</param>
        /// <param name="statReference">list of possible colors</param>
        /// <param name="Left">left of rectangle (default: 0)</param>
        /// <param name="Top">top of rectangle (default: 0)</param>
        /// <param name="Width">width of rectangle (default: full width)</param>
        /// <param name="Height">height of rectangle (default: full height)</param>
        /// <returns>Color</returns>
        static public Color IdentifyColor(ImageData Img, Dictionary<Color, List<double>> statReference, int Left = 0, int Top = 0, int Width = -1, int Height = -1)
        {
            double[] av = CommonFunctions.AverageRGBValues(Img, Left, Top, Width, Height);

            double bestScore = 255;
            double currentScore = 0;

            Color Foo = Color.White;

            foreach (KeyValuePair<Color, List<double>> item in statReference)
            {
                currentScore = Math.Pow((item.Value[0] / 255.0) - (av[0] / 255.0), 2)
                    + Math.Pow((item.Value[1] / 255.0) - (av[1] / 255.0), 2)
                    + Math.Pow((item.Value[2] / 255.0) - (av[2] / 255.0), 2);
                if (currentScore < bestScore)
                {
                    Foo = item.Key;
                    bestScore = currentScore;
                }
            }
            return Foo;
        }
        /// <summary>
        /// identify the color of a part(rectangle) from an image by a given list of reference colors (majority vote)
        /// </summary>
        /// <param name="Img">image to look in</param>
        /// <param name="statReference">list of possible colors</param>
        /// <param name="Left">left of rectangle (default: 0)</param>
        /// <param name="Top">top of rectangle (default: 0)</param>
        /// <param name="Width">width of rectangle (default: full width)</param>
        /// <param name="Height">height of rectangle (default: full height)</param>
        /// <returns>Color</returns>
        static public Color IdentifyColorByVoting(ImageData Img, Dictionary<Color, List<double>> statReference, int Left = 0, int Top = 0, int Width = -1, int Height = -1)
        {
            double[] av = CommonFunctions.AverageRGBValues(Img, Left, Top, Width, Height);

            double bestScore = 255;
            double currentScore = 0;

            Color Foo = Color.White;

            int[] votes = Enumerable.Repeat(0, statReference.Count).ToArray();

            if (Width == -1)
                Width = Img.Width;
            if (Height == -1)
                Height = Img.Height;

            for (int x = Left; x < Left + Width; x++)
            {
                for (int y = Top; y < Top + Height; y++)
                {
                    // color from image
                    Color CurrentColor = Img.GetPixel(x, y);
                    int best_dist = 255*50;
                    int best_idx = 0;
                    for (int i = 0; i < statReference.Count;i++ )
                    {
                        List<double> RGB = statReference.ElementAt(i).Value;
                        // from from dictionary
                        Color CCol = Color.FromArgb(Convert.ToInt32(RGB.ElementAt(0)), Convert.ToInt32(RGB.ElementAt(1)), Convert.ToInt32(RGB.ElementAt(2)));
                        // distance
                        int current_dist = Math.Abs(CCol.R - CurrentColor.R) + Math.Abs(CCol.G - CurrentColor.G) + Math.Abs(CCol.B - CurrentColor.B);
                        if (current_dist < best_dist)
                        {
                            best_dist = current_dist;
                            best_idx = i;
                        }
                    }
                    votes[best_idx]++;

                }
            }

            int m=-1;
            int ans = 0;
            for (int i=0;i<votes.Length;i++)
            {
                if(votes[i]>m){
                    m=votes[i];
                    ans=i;
                }
            }


            return statReference.ElementAt(ans).Key;
        }

        /// <summary>
        /// identify color from list (choose the image with best matching)
        /// </summary>
        /// <param name="GivenColor">color to look for</param>
        /// <param name="statReference">list of reference images</param>
        /// <returns>color from dictionary</returns>
        static public Color IdentifyColor(Color GivenColor, Dictionary<Color, List<double>> statReference)
        {
            double[] av = new double[3] { GivenColor.R,GivenColor.G,GivenColor.B};

            double bestScore = 255;
            double currentScore = 0;

            Color Foo = Color.White;

            foreach (KeyValuePair<Color, List<double>> item in statReference)
            {
                currentScore = Math.Abs((item.Value[0] / 255.0) - (av[0] / 255.0))
                    + Math.Abs((item.Value[1] / 255.0) - (av[1] / 255.0))
                    + Math.Abs((item.Value[2] / 255.0) - (av[2] / 255.0));
                if (currentScore < bestScore)
                {
                    Foo = item.Key;
                    bestScore = currentScore;
                }
            }
            return Foo;
        }
        /// <summary>
        /// identify image from list (choose the image with best matching)
        /// </summary>
        /// <param name="Img">image to look for</param>
        /// <param name="statReference">list of reference images</param>
        /// <returns>name (key of list)</returns>
        static public string IdentifyImage(ImageData Img, Dictionary<string, ImageData> statReference)
        {
            double similar = 0.0;
            string keyword = "";
            foreach (KeyValuePair<string, ImageData> item in statReference)
            {
                // exakte übereinstimmung der Größen ermöglicht einen simplen vergleich
                if ((Img.Width == item.Value.Width) && (Img.Height == item.Value.Height))
                {
                    double s = CommonFunctions.Similarity(Img, item.Value);
                    if (s > similar)
                    {
                        keyword = item.Key;
                        similar = s;
                    }
                }
                else
                {
                    if ((Img.Width > item.Value.Width) && (Img.Height > item.Value.Height))
                    {
                        // im größeren suchen
                        for (int column = 0; column < Img.Width - item.Value.Width; column++)
                        {
                            for (int row = 0; row < Img.Height - item.Value.Height; row++)
                            {
                                double s = CommonFunctions.Similarity(Img, item.Value, 0, 0, item.Value.Width, item.Value.Height, column, row);
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
        /// calculate the difference of two colors (a-b)
        /// </summary>
        /// <param name="a">Color a</param>
        /// <param name="b">Color b</param>
        /// <returns></returns>
        static public Color ColorDifference(Color a, Color b)
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
        /// get the red channel as array
        /// </summary>
        /// <param name="Img">image</param>
        /// <returns>array[] of red channel</returns>
        static public uint[,] ExtractRedChannel(ImageData Img)
        {
            uint[,] red = new uint[Img.Width, Img.Height];
            for (Int32 column = 0; column < Img.Width; column++)
            {
                for (Int32 row = 0; row < Img.Height; row++)
                {
                    Color c = Img.GetPixel(column, row);
                    red[column, row] = c.R;
                }
            }
            return red;
        }
        /// <summary>
        /// find first occurence of color with tolerance 
        /// </summary>
        /// <param name="Img">image to look in</param>
        /// <param name="SearchColor">color to look for</param>
        /// <param name="Tolerance">tolerance</param>
        /// <returns></returns>
        static public bool[,] FindColors(ImageData Img, Color SearchColor, uint Tolerance)
        {
            bool[,] grid = new bool[Img.Width, Img.Height];
            for (Int32 column = 0; column < Img.Width; column++)
            {
                for (Int32 row = 0; row < Img.Height; row++)
                {
                    if (CommonFunctions.ColorsSimilar(Img.GetPixel(column, row), SearchColor, Tolerance))
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

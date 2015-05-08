using System;
using System.Drawing;
namespace BotSuite.ImageLibrary
{
    /// <summary>
    /// collection of filters
    /// </summary>
    static public class Filter
    {
        /// <summary>
        /// add two images
        /// </summary>
        /// <param name="Img">ref Image</param>
        /// <param name="Summand">image to add</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Original = new ImageData(...);
        /// ImageData Summand = new ImageData(...);
        /// // apply filter to original
        /// Filter.Add(ref Original,Summand);
        /// ]]>
        /// </code>
        /// </example>
        /// <returns></returns>
        static public void Add(ref ImageData Img, ImageData Summand)
        {
            if ((Img.Width == Summand.Width) && (Img.Height == Summand.Height))
            {

                for (Int32 column = 0; column < Summand.Width; column++)
                {
                    for (Int32 row = 0; row < Summand.Height; row++)
                    {
                        Color a = Summand[column, row];
                        Color b = Img[column, row];

                        int cr = a.R + b.R;
                        int cg = a.G + b.G;
                        int cb = a.B + b.B;

                        if (cr > 255)
                            cr -= 255;
                        if (cg > 255)
                            cg -= 255;
                        if (cb > 255)
                            cb -= 255;

                        Img[column, row] = Color.FromArgb(cr, cg, cb);
                    }
                }
            }
        }
        /// <summary>
        /// subtract two images
        /// </summary>
        /// <param name="Img">ref images</param>
        /// <param name="Subtrahend">subtrahend</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Original = new ImageData(...);
        /// ImageData Subtrahend = new ImageData(...);
        /// // apply filter to original
        /// Filter.Difference(ref Original,Subtrahend);
        /// ]]>
        /// </code>
        /// </example>
        /// <returns></returns>
        static public void Difference(ref ImageData Img, ImageData Subtrahend)
        {
            if ((Img.Width == Subtrahend.Width) && (Img.Height == Subtrahend.Height))
            {

                for (Int32 column = 0; column < Subtrahend.Width; column++)
                {
                    for (Int32 row = 0; row < Subtrahend.Height; row++)
                    {
                        Color a = Subtrahend[column, row];
                        Color b = Img[column, row];

                        int cr = a.R - b.R;
                        int cg = a.G - b.G;
                        int cb = a.B - b.B;

                        if (cr < 0)
                            cr += 255;
                        if (cg < 0)
                            cg += 255;
                        if (cb < 0)
                            cb += 255;

                        Img[column, row] = Color.FromArgb(cr, cg, cb);
                    }
                }
            }
        }
        /// <summary>
        /// black and white image by replace 
        /// all similar color to black by blackand 
        /// all similiar colors to white by white
        /// </summary>
        /// <param name="Img">ref image</param>
        /// <param name="Tolerance">tolerance (0,...,255)</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// Filter.Grayscale(ref Img);
        /// // apply filter to original
        /// Filter.BlackAndWhite(ref Img,90);
        /// ]]>
        /// </code>
        /// </example>
        /// <see cref="blackandwhite"/>
        /// <returns></returns>
        static public void BlackAndWhite(ref ImageData Img, uint Tolerance)
        {
            Filter.ReplaceSimilarColor(ref Img, Color.Black, Color.Black, Tolerance);
            Filter.ReplaceDifferentColor(ref Img, Color.Black, Color.White, Tolerance);
        }
        /// <summary>
        /// replace color by another color
        /// </summary>
        /// <param name="Img">ref Image</param>
        /// <param name="SearchColor">color to look for</param>
        /// <param name="ReplaceColor">color to replace with</param>
        /// <param name="Tolerance">tolerance of reference color (0,...,255)</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// // apply filter to original and replace all colors 
        /// // which seems to be blue (tolerance 130) by red
        /// Filter.ReplaceSimilarColor(ref Img,Color.Blue,Color.Red,130);
        /// ]]>
        /// </code>
        /// </example>
        /// <see cref="replacecolor"/>
        /// <returns></returns>
        static public void ReplaceSimilarColor(ref ImageData Img, Color SearchColor, Color ReplaceColor, uint Tolerance)
        {
            for (Int32 OuterX = 0; OuterX < Img.Width; OuterX++)
            {
                for (Int32 OuterY = 0; OuterY < Img.Height; OuterY++)
                {
                    Color a = Img[OuterX, OuterY];
                    if (CommonFunctions.ColorsSimilar(a, SearchColor, Tolerance))
                    {
                        Img[OuterX, OuterY]= ReplaceColor;
                    }
                }
            }
        }
        /// <summary>
        /// replace color by another color by ignore a color
        /// </summary>
        /// <param name="Img">ref Image</param>
        /// <param name="SearchColor">color to ignore</param>
        /// <param name="ReplaceColor">color to replace with</param>
        /// <param name="Tolerance">tolerance of reference color (0,...,255)</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// // apply filter to original and replace all colors
        /// // which DOESN'T seems to be blue (tolerance 25) by red
        /// Filter.ReplaceDifferentColor(ref Img,Color.Blue,Color.Red,25);
        /// ]]>
        /// </code>
        /// </example>
        /// <returns></returns>
        static public void ReplaceDifferentColor(ref ImageData Img, Color SearchColor, Color ReplaceColor, uint Tolerance)
        {
            for (Int32 OuterX = 0; OuterX < Img.Width; OuterX++)
            {
                for (Int32 OuterY = 0; OuterY < Img.Height; OuterY++)
                {
                    Color a = Img[OuterX, OuterY];
                    if (!CommonFunctions.ColorsSimilar(a, SearchColor, Tolerance))
                    {
                        Img[OuterX, OuterY] = ReplaceColor;
                    }
                }
            }
        }
        /// <summary>
        /// Convert image to grayscale
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// // apply filter to original and convert to grayscale
        /// Filter.Grayscale(ref Img);
        /// ]]>
        /// </code>
        /// </example>
        /// <see cref="gray"/>
        /// <param name="Img">ref image to convert</param>
        /// <returns></returns>
        static public void Grayscale(ref ImageData Img)
        {
            for (Int32 column = 0; column < Img.Width; column++)
            {
                for (Int32 row = 0; row < Img.Height; row++)
                {
                    Color c = Img[column, row];
                    int grayScale = (int)((c.R * .3) + (c.G * .59) + (c.B * .11));
                    Img[column, row] = Color.FromArgb(grayScale, grayScale, grayScale);
                }
            }
        }
        /// <summary>
        /// invert image
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// // apply filter to original and  invert
        /// Filter.Invert(ref Img);
        /// ]]>
        /// </code>
        /// </example>
        /// <see cref="invert"/>
        /// <param name="Img">ref image to convert</param>
        /// <returns></returns>
        static public void Invert(ref ImageData Img)
        {
            for (Int32 column = 0; column < Img.Width; column++)
            {
                for (Int32 row = 0; row < Img.Height; row++)
                {
                    Color c = Img[column, row];
                    Img[column, row]= Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B);
                }
            }
        }
        /// <summary>
        /// mark a point in image 
        /// </summary>
        /// <param name="Img">ref image to mark</param>
        /// <param name="Location">where to set marker</param>
        /// <param name="MarkColor">color of marker</param>
        /// <param name="Size">size of marker (square)</param>
        /// <returns></returns>
        static public void MarkPoint(ref ImageData Img, Point Location, Color MarkColor, uint Size=5)
        {
            for (int i = Convert.ToInt32(Location.X - Size); i < Location.X + Size; i++)
            {
                for (int j = Convert.ToInt32(Location.Y - Size); j < Location.Y + Size; j++)
                {
                    Img[i, j] = MarkColor;
                }
            }
        }
        /// <summary>
        /// Convert image to sepia
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// // apply filter to original and convert to sepia
        /// Filter.Sepia(ref Img);
        /// ]]>
        /// </code>
        /// </example>
        /// <see cref="sepia"/>
        /// <param name="Img">ref image to convert</param>
        /// <returns></returns>
        static public void Sepia(ref ImageData Img)
        {
            int t;
            for (Int32 column = 0; column < Img.Width; column++)
            {
                for (Int32 row = 0; row < Img.Height; row++)
                {
                    Color c = Img[column, row];
                    t = Convert.ToInt32(0.299 * c.R + 0.587 * c.G + 0.114 * c.B);
                    Img.SetPixel(column, row, Color.FromArgb(((t > 206) ? 255 : t + 49), ((t < 14) ? 0 : t - 14), ((t < 56) ? 0 : t - 56)));
                }
            }
        }
        /// <summary>
        /// apply threshold
        /// </summary>
        /// <param name="Img">ref Image</param>
        /// <param name="threshold">threshold (0,...,255)</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// // it's better to grayscale first
        /// Filter.Grayscale(ref Img);
        /// // apply filter to image
        /// Filter.Threshold(ref Img, 20);
        /// ]]>
        /// </code>
        /// </example>
        /// <see cref="threshold"/>
        /// <returns></returns>
        static public void Threshold(ref ImageData Img,uint threshold)
        {
            for (Int32 column = 0; column < Img.Width; column++)
            {
                for (Int32 row = 0; row < Img.Height; row++)
                {
                    Color c = Img[column, row];
                    if (c.R > threshold)
                    {
                        Img[column, row] =  Color.FromArgb(255, 255, 255);
                    }
                    else
                    {
                        Img[column, row] = Color.FromArgb(0, 0, 0);
                    }

                }
            }
        }
        /// <summary>
        /// change the contrast of an image
        /// </summary>
        /// <param name="Img">image to manipilate</param>
        /// <param name="contrast">value of contrast</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// // apply filter to image
        /// Filter.Contrast(ref Img, 50.0);
        /// ]]>
        /// </code>
        /// </example>
        /// <see cref="contrast"/>
        /// <returns></returns>
        static public void Contrast(ref ImageData Img, double contrast)
        {
            double  RedChannel, GreenChannel, BlueChannel;

            Color PixelColor;

            contrast = (100.0 + contrast) / 100.0;
            contrast *= contrast;

            for (int y = 0; y < Img.Height; y++)
            {
                for (int x = 0; x < Img.Width; x++)
                {
                    PixelColor = Img[x, y];
                    RedChannel = (((PixelColor.R / 255.0) - 0.5) * contrast + 0.5) * 255;
                    GreenChannel = (((PixelColor.G / 255.0) - 0.5) * contrast + 0.5) * 255;
                    BlueChannel = (((PixelColor.B / 255.0) - 0.5) * contrast + 0.5) * 255;

                    RedChannel = (RedChannel > 255) ? 255 : RedChannel;
                    RedChannel = (RedChannel < 0) ? 0 : RedChannel;
                    GreenChannel = (GreenChannel > 255) ? 255 : GreenChannel;
                    GreenChannel = (GreenChannel < 0) ? 0 : GreenChannel;
                    BlueChannel = (BlueChannel > 255) ? 255 : BlueChannel;
                    BlueChannel = (BlueChannel < 0) ? 0 : BlueChannel;

                   

                    Img.SetPixel(x, y, Color.FromArgb((int)RedChannel, (int)GreenChannel, (int)BlueChannel));
                }
            }

        }
        /// <summary>
        /// change the brightness of an image
        /// </summary>
        /// <param name="Img">image to manipulate</param>
        /// <param name="brightness">brightness of new image</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// // apply filter to image
        /// Filter.Brightness(ref Img, 10);
        /// ]]>
        /// </code>
        /// </example>
        /// <see cref="brightness"/>
        /// <returns></returns>
        static public void Brightness(ref ImageData Img, int brightness)
        {
            int  RedChannel, GreenChannel, BlueChannel;
            Color PixelColor;

            for (int y = 0; y < Img.Height; y++)
            {
                for (int x = 0; x < Img.Width; x++)
                {
                    PixelColor = Img[x, y];

                    RedChannel = PixelColor.R + brightness;
                    GreenChannel = PixelColor.G + brightness;
                    BlueChannel = PixelColor.B + brightness;


                    RedChannel = (RedChannel > 255) ? 255 : RedChannel;
                    RedChannel = (RedChannel < 0) ? 0 : RedChannel;
                    GreenChannel = (GreenChannel > 255) ? 255 : GreenChannel;
                    GreenChannel = (GreenChannel < 0) ? 0 : GreenChannel;
                    BlueChannel = (BlueChannel > 255) ? 255 : BlueChannel;
                    BlueChannel = (BlueChannel < 0) ? 0 : BlueChannel;

                    Img.SetPixel(x, y, Color.FromArgb( RedChannel, GreenChannel, BlueChannel));
                }
            }

        }
        /// <summary>
        /// decrease the depth of color
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// // apply filter to image
        /// Filter.DecreaseColourDepth(ref Img, 40);
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="Img">image to manipulate</param>
        /// <param name="offset">offset of colordepth</param>
        /// <see cref="decreasecolourdepth"/>
        /// <returns></returns>
        static public void DecreaseColourDepth(ref ImageData Img, int offset)
        {
            int  RedChannel, GreenChannel, BlueChannel;
            Color PixelColor;

            for (int y = 0; y < Img.Height; y++)
            {
                for (int x = 0; x < Img.Width; x++)
                {
                    PixelColor = Img[x, y];
                    RedChannel = ((PixelColor.R + (offset / 2)) - ((PixelColor.R + (offset / 2)) % offset) - 1);
                    GreenChannel = ((PixelColor.G + (offset / 2)) - ((PixelColor.G + (offset / 2)) % offset) - 1);
                    BlueChannel = ((PixelColor.B + (offset / 2)) - ((PixelColor.B + (offset / 2)) % offset) - 1);

                    RedChannel = (RedChannel < 0) ? 0 : RedChannel;
                    GreenChannel = (GreenChannel < 0) ? 0 : GreenChannel;
                    BlueChannel = (BlueChannel < 0) ? 0 : BlueChannel;

                    Img.SetPixel(x, y, Color.FromArgb( RedChannel, GreenChannel, BlueChannel));
                }
            }

        }
        /// <summary>
        /// apply emboss effect on image
        /// </summary>
        /// <param name="Img">image to manipulate</param>
        /// <param name="weight">weight of emboss effect</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// // apply filter to image
        /// Filter.Emboss(ref Img, 4.0);
        /// ]]>
        /// </code>
        /// </example>
        /// <see cref="emboss"/>
        /// <returns></returns>
        static public void Emboss(ref ImageData Img, double weight)
        {
            ConvolutionMatrix CMatrix = new ConvolutionMatrix(3);
            CMatrix.SetAll(1);
            CMatrix.Matrix[0, 0] = -1;
            CMatrix.Matrix[1, 0] = 0;
            CMatrix.Matrix[2, 0] = -1;
            CMatrix.Matrix[0, 1] = 0;
            CMatrix.Matrix[1, 1] = weight;
            CMatrix.Matrix[2, 1] = 0;
            CMatrix.Matrix[0, 2] = -1;
            CMatrix.Matrix[1, 2] = 0;
            CMatrix.Matrix[2, 2] = -1;
            CMatrix.Factor = 4;
            CMatrix.Offset = 127;
            ApplyConvolution3x3(ref Img, CMatrix);

        }
        /// <summary>
        /// apply gaussianblur to image
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// // apply filter to image
        /// Filter.GaussianBlur(ref Img, 20.0);
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="Img">image to manipulate</param>
        /// <param name="peakValue">parameter</param>
        /// <see cref="gaussian"/>
        /// <returns></returns>
        static public void GaussianBlur(ref ImageData Img, double peakValue)
        {
            ConvolutionMatrix CMatrix = new ConvolutionMatrix(3);
            CMatrix.SetAll(1);
            CMatrix.Matrix[0, 0] = peakValue / 4;
            CMatrix.Matrix[1, 0] = peakValue / 2;
            CMatrix.Matrix[2, 0] = peakValue / 4;
            CMatrix.Matrix[0, 1] = peakValue / 2;
            CMatrix.Matrix[1, 1] = peakValue;
            CMatrix.Matrix[2, 1] = peakValue / 2;
            CMatrix.Matrix[0, 2] = peakValue / 4;
            CMatrix.Matrix[1, 2] = peakValue / 2;
            CMatrix.Matrix[2, 2] = peakValue / 4;
            CMatrix.Factor = peakValue * 4;
            ApplyConvolution3x3(ref Img, CMatrix);

        }
        /// <summary>
        /// sharpen an image
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// // apply filter to image
        /// Filter.Sharpen(ref Img, 4.0);
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="Img">image to manipulate</param>
        /// <param name="weight">weight</param>
        /// <see cref="sharpen"/>
        /// <returns></returns>
        static public void Sharpen(ref ImageData Img, double weight)
        {
            ConvolutionMatrix CMatrix = new ConvolutionMatrix(3);
            CMatrix.SetAll(1);
            CMatrix.Matrix[0, 0] = 0;
            CMatrix.Matrix[1, 0] = -2;
            CMatrix.Matrix[2, 0] = 0;
            CMatrix.Matrix[0, 1] = -2;
            CMatrix.Matrix[1, 1] = weight;
            CMatrix.Matrix[2, 1] = -2;
            CMatrix.Matrix[0, 2] = 0;
            CMatrix.Matrix[1, 2] = -2;
            CMatrix.Matrix[2, 2] = 0;
            CMatrix.Factor = weight - 8;
            ApplyConvolution3x3(ref Img, CMatrix);

        }

        /// <summary>
        /// remove mean of image
        /// </summary>
        /// <param name="Img">image to manipulate</param>
        /// <param name="weight">weight of effect</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// // apply filter to image
        /// Filter.RemoveMean(ref Img, 10.0);
        /// ]]>
        /// </code>
        /// </example>
        /// <returns></returns>
        static public void RemoveMean(ref ImageData Img,double weight)
        {
            ConvolutionMatrix CMatrix = new ConvolutionMatrix(3);
            CMatrix.SetAll(1);
            CMatrix.Matrix[0, 0] = -1;
            CMatrix.Matrix[1, 0] = -1;
            CMatrix.Matrix[2, 0] = -1;
            CMatrix.Matrix[0, 1] = -1;
            CMatrix.Matrix[1, 1] = weight;
            CMatrix.Matrix[2, 1] = -1;
            CMatrix.Matrix[0, 2] = -1;
            CMatrix.Matrix[1, 2] = -1;
            CMatrix.Matrix[2, 2] = -1;
            CMatrix.Factor = weight - 8;
            ApplyConvolution3x3(ref Img, CMatrix);
        }

        /// <summary>
        /// blur the image
        /// </summary>
        /// <param name="Img">image to manipulate</param>
        /// <param name="weight">weight of effect</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// // apply filter to image
        /// Filter.Blur(ref Img, 30.0);
        /// ]]>
        /// </code>
        /// </example>
        /// <see cref="blur"/>
        /// <returns></returns>
        static public void Blur(ref ImageData Img, double weight)
        {
            ConvolutionMatrix CMatrix = new ConvolutionMatrix(3);
            CMatrix.SetAll(1);
            CMatrix.Matrix[1, 1] = weight;
            CMatrix.Factor = weight + 8;
            ApplyConvolution3x3(ref Img, CMatrix);
        }

        /// <summary>
        /// marks the edges black and all other pixels white
        /// </summary>
        /// <param name="Img">image to manipulate</param>
        /// <see cref="findedges"/>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// // apply filter to image
        /// Filter.FindEdges(ref Img);
        /// ]]>
        /// </code>
        /// </example>
        /// <returns></returns>
        static public void FindEdges(ref ImageData Img)
        {
            Emboss(ref Img, 4.0);
            ReplaceSimilarColor(ref Img, Color.FromArgb(127, 132, 127), Color.White, 20);
            ReplaceDifferentColor(ref Img, Color.White, Color.Black, 1);
        }

 

        static private void ApplyConvolution3x3(ref ImageData Img, ConvolutionMatrix m)
        {
            ImageData newImg = Img.Clone();
            Color[,] pixelColor = new Color[3, 3];
            int A, RedChannel, GreenChannel, BlueChannel;

            for (int y = 0; y < Img.Height - 2; y++)
            {
                for (int x = 0; x < Img.Width - 2; x++)
                {
                    pixelColor[0, 0] = Img[x, y];
                    pixelColor[0, 1] = Img[x, y + 1];
                    pixelColor[0, 2] = Img[x, y + 2];
                    pixelColor[1, 0] = Img[x + 1, y];
                    pixelColor[1, 1] = Img[x + 1, y + 1];
                    pixelColor[1, 2] = Img[x + 1, y + 2];
                    pixelColor[2, 0] = Img[x + 2, y];
                    pixelColor[2, 1] = Img[x + 2, y + 1];
                    pixelColor[2, 2] = Img[x + 2, y + 2];

                    A = pixelColor[1, 1].A;

                    RedChannel = (int)((((pixelColor[0, 0].R * m.Matrix[0, 0]) +
                                 (pixelColor[1, 0].R * m.Matrix[1, 0]) +
                                 (pixelColor[2, 0].R * m.Matrix[2, 0]) +
                                 (pixelColor[0, 1].R * m.Matrix[0, 1]) +
                                 (pixelColor[1, 1].R * m.Matrix[1, 1]) +
                                 (pixelColor[2, 1].R * m.Matrix[2, 1]) +
                                 (pixelColor[0, 2].R * m.Matrix[0, 2]) +
                                 (pixelColor[1, 2].R * m.Matrix[1, 2]) +
                                 (pixelColor[2, 2].R * m.Matrix[2, 2]))
                                        / m.Factor) + m.Offset);

                  

                    GreenChannel = (int)((((pixelColor[0, 0].G * m.Matrix[0, 0]) +
                                 (pixelColor[1, 0].G * m.Matrix[1, 0]) +
                                 (pixelColor[2, 0].G * m.Matrix[2, 0]) +
                                 (pixelColor[0, 1].G * m.Matrix[0, 1]) +
                                 (pixelColor[1, 1].G * m.Matrix[1, 1]) +
                                 (pixelColor[2, 1].G * m.Matrix[2, 1]) +
                                 (pixelColor[0, 2].G * m.Matrix[0, 2]) +
                                 (pixelColor[1, 2].G * m.Matrix[1, 2]) +
                                 (pixelColor[2, 2].G * m.Matrix[2, 2]))
                                        / m.Factor) + m.Offset);


                    BlueChannel = (int)((((pixelColor[0, 0].B * m.Matrix[0, 0]) +
                                 (pixelColor[1, 0].B * m.Matrix[1, 0]) +
                                 (pixelColor[2, 0].B * m.Matrix[2, 0]) +
                                 (pixelColor[0, 1].B * m.Matrix[0, 1]) +
                                 (pixelColor[1, 1].B * m.Matrix[1, 1]) +
                                 (pixelColor[2, 1].B * m.Matrix[2, 1]) +
                                 (pixelColor[0, 2].B * m.Matrix[0, 2]) +
                                 (pixelColor[1, 2].B * m.Matrix[1, 2]) +
                                 (pixelColor[2, 2].B * m.Matrix[2, 2]))
                                        / m.Factor) + m.Offset);

                    RedChannel = (RedChannel > 255) ? 255 : RedChannel;
                    RedChannel = (RedChannel < 0) ? 0 : RedChannel;
                    GreenChannel = (GreenChannel > 255) ? 255 : GreenChannel;
                    GreenChannel = (GreenChannel < 0) ? 0 : GreenChannel;
                    BlueChannel = (BlueChannel > 255) ? 255 : BlueChannel;
                    BlueChannel = (BlueChannel < 0) ? 0 : BlueChannel;


                    newImg.SetPixel(x + 1, y + 1, Color.FromArgb(A, RedChannel, GreenChannel, BlueChannel));
                }
            }
            Img = newImg.Clone();
        
        }

        public class ConvolutionMatrix
        {
            public int MatrixSize = 3;

            public double[,] Matrix;
            public double Factor = 1;
            public double Offset = 1;

            public ConvolutionMatrix(int size)
            {
                MatrixSize = 3;
                Matrix = new double[size, size];
            }

            public void SetAll(double value)
            {
                for (int i = 0; i < MatrixSize; i++)
                {
                    for (int j = 0; j < MatrixSize; j++)
                    {
                        Matrix[i, j] = value;
                    }
                }
            }
        }
    }
}

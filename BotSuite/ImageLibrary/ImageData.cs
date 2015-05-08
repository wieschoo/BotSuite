using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace BotSuite.ImageLibrary
{
    /// <summary>
    /// class of faster image manipulation (by 600times faster than to default bitmap class)
    /// support 24bit and 32bit bitmap import
    /// and 24bit bitmap output
    /// </summary>
    public class ImageData
    {

        #region properties
        public int Height { get; private set; }
        public int Width { get; private set; }

        private byte[] BmpBytes;
        private int BmpStride;
        private PixelFormat BmpPixelFormat;
        private int RawFormatOffset;

        /// <summary>
        /// return or set size of bitmap
        /// </summary>
        /// <
        public Size Size
        {
            get
            {
                return new Size(Width, Height);
            }
            set
            {
                this.Height = Size.Height;
                this.Width = Size.Width;
            }
        }

        /// <summary>
        /// set a 24bit or 32bit bitmap
        /// or returns a 24bit bitmap
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// Bitmap Bmp = Img.Bitmap;
        /// Bitmap Bmp2 = new Bitmap(...);
        /// Img.Bitmap = Bmp2;
        /// ]]>
        /// </code>
        /// </example>
        public Bitmap Bitmap
        {
            get { return CreateBitmap(); }
            set { LoadBitmap(value); }
        }
        #endregion

        /// <summary>
        /// creates a new imagedata object from a given bitmap
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Bitmap Bmp = new Bitmap(...);
        /// ImageData Img = new ImageData(Bmp);
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="Bitmap">tagert bitmap</param>
        public ImageData(Bitmap Bitmap)
        {
            LoadBitmap(Bitmap);
        }

        /// <summary>
        /// creates a new imagedata object from a file
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData("path/to/bitmap/file.bmp");
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="Path">path to bitmap</param>
        public ImageData(string Path)
        {
            LoadBitmap(new Bitmap(Path));
        }

        /// <summary>
        /// creates a new imagedata object from another imagedata object
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// ImageData Img2 = new ImageData(Img);
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="Path">path to bitmap</param>
        public ImageData(ImageData Img)
        {
            LoadBitmap(Img.Bitmap);
        }

        /// <summary>
        /// store a bitmap into a file
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// Img.Save("path/to/bitmap/file.bmp");
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="Path">savepath</param>
        public void Save(string Path)
        {
            this.Bitmap.Save(Path);
        }

        /// <summary>
        /// clones an imagedata object
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// ImageData Img2 = Img.Clone();
        /// ]]>
        /// </code>
        /// </example>
        /// <returns>new imagedata object</returns>
        public ImageData Clone()
        {
            return new ImageData(this);
        }


        #region private methods
        public void LoadBitmap(Bitmap Bitmap)
        {
            // extract bitmap data
            BmpPixelFormat = Bitmap.PixelFormat;
            Width = Bitmap.Width;
            Height = Bitmap.Height;
            switch (Bitmap.PixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                    RawFormatOffset = 4;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    RawFormatOffset = 3;
                    break;
            }

            // load bytes
            Rectangle BmpRectangle = new Rectangle(0, 0, Bitmap.Width, Bitmap.Height);
            BitmapData bmpData = Bitmap.LockBits(BmpRectangle, ImageLockMode.ReadOnly, BmpPixelFormat);
            IntPtr ptr = bmpData.Scan0;
            BmpStride = bmpData.Stride;
            int ByteSize = BmpStride * Bitmap.Height;
            BmpBytes = new byte[ByteSize];
            System.Runtime.InteropServices.Marshal.Copy(ptr, BmpBytes, 0, ByteSize);
            Bitmap.UnlockBits(bmpData);
        }

        /// <summary>
        /// get image as 24bit bitmap or a part (rectangle) of it
        /// </summary>
        /// <param name="L">left of imagepart (default: 0 no offset)</param>
        /// <param name="T">top of imagepart (default: 0 no offset)</param>
        /// <param name="W">width of imagepart (default: full width) </param>
        /// <param name="H">height of imagepart (default: full height)</param>
        /// <returns></returns>
        public Bitmap CreateBitmap(int L = 0, int T = 0, int W = -1, int H = -1)
        {
            // First, we generate a Bitmap object holding the current bitmap (ReturnBitmap)
            W = (W == -1) ? Width - L : W;
            H = (H == -1) ? Height - T : H;

            Bitmap ReturnBitmap = new Bitmap(Width, Height);
            Rectangle r = new Rectangle(0, 0, Width, Height);

            BitmapData bmpData = ReturnBitmap.LockBits(r, ImageLockMode.ReadWrite, BmpPixelFormat);
            int InStride = bmpData.Stride;
            int ByteSize = InStride * ReturnBitmap.Height;
            IntPtr ptr = bmpData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(BmpBytes, 0, ptr, ByteSize);

            ReturnBitmap.UnlockBits(bmpData);

            Rectangle cropRect;

            // Then, if parameters are not default values, we crop the generated rectangle to the new size:
            if (!(L == 0 && T == 0 && W == Width && H == Height))
                cropRect = new Rectangle(L, T, W, H);
            else
                cropRect = new Rectangle(L, T, Width, Height);

            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(ReturnBitmap, new Rectangle(0, 0, target.Width, target.Height),
                                 cropRect,
                                 GraphicsUnit.Pixel);

            }

            return target;
        }
        #endregion

        /// <summary>
        /// get or set pixel color
        /// </summary>
        /// <param name="x">x coordinate (column)</param>
        /// <param name="y">y coordinate (row)</param>
        /// <returns>color of pixel</returns>
        public Color this[int x, int y]
        {
            get
            {
                if ((0 <= x) && (x < Width) && (0 <= y) && (y < Height))
                {
                    return Color.FromArgb(
                        BmpBytes[y * BmpStride + x * RawFormatOffset + 2],
                        BmpBytes[y * BmpStride + x * RawFormatOffset + 1],
                        BmpBytes[y * BmpStride + x * RawFormatOffset]
                    );
                }
                else
                {
                    return Color.Empty;
                }
            }
            set
            {
                if ((0 <= x) && (x < Width) && (0 <= y) && (y < Height))
                {
                    BmpBytes[y * BmpStride + x * RawFormatOffset + 2] = value.R;
                    BmpBytes[y * BmpStride + x * RawFormatOffset + 1] = value.G;
                    BmpBytes[y * BmpStride + x * RawFormatOffset] = value.B;
                }
            }
        }

        /// <summary>
        /// get color of pixel
        /// </summary>
        /// <param name="x">x coordinate (column)</param>
        /// <param name="y">y coordinate (row)</param>
        /// <returns>color of pixel</returns>
        public Color GetPixel(int x, int y)
        {
            return this[x, y];
        }
        /// <summary>
        /// set color of pixel 
        /// </summary>
        /// <param name="x">x coordinate (column)</param>
        /// <param name="y">y coordinate (row)</param>
        /// <param name="Color">color to set</param>
        public void SetPixel(int row, int col, Color Color)
        {
            this[row, col] = Color;
        }

        /// <summary>
        /// resize an image by a factor
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// // downscale an image to 80% of the original size
        /// ImageData ResizedImg = Img.Resize(0.8);
        /// 
        /// // or use another interpolation mode
        /// ImageData ResizedImg2 = Img.Resize(0.8,InterpolationMode.Bilinear);
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="Factor">Factor (0.80 means downscale by 80%)</param>
        /// /// <param name="Mode">Mode of Interpolation (Default: HighQualityBicubic, Possible parameter: HighQualityBicubic,HighQualityBilinear,Bilinear,...)</param>
        /// <returns></returns>
        public ImageData Resize(double Factor, InterpolationMode Mode = InterpolationMode.HighQualityBicubic)
        {
            //get the new size based on the percentage change
            int resizedW = (int)(Width * Factor);
            int resizedH = (int)(Height * Factor);

            return Resize(resizedW, resizedH, Mode);
        }

        /// <summary>
        /// resize an image
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// // downscale to image of size 100px x 120px
        /// ImageData ResizedImg = Img.Resize(100,120);
        /// 
        /// // or use another Interpolationmode
        /// ImageData ResizedImg2 = Img.Resize(100,120,InterpolationMode.Bilinear);
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="NewWidth">new Width of image</param>
        /// <param name="NewHeight">new Height of image</param>
        /// <param name="Mode">Mode of Interpolation (Default: HighQualityBicubic, Possible parameter: HighQualityBicubic,HighQualityBilinear,Bilinear,...)</param>
        /// <returns></returns>
        public ImageData Resize(int NewWidth, int NewHeight, InterpolationMode Mode = InterpolationMode.HighQualityBicubic)
        {
            //create a new Bitmap the size of the new image
            Bitmap bmp = new Bitmap(NewWidth, NewHeight, BmpPixelFormat);
            //create a new graphic from the Bitmap
            Graphics graphic = Graphics.FromImage((Image)bmp);
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //draw the newly resized image
            graphic.DrawImage(this.Bitmap, 0, 0, NewWidth, NewHeight);
            //dispose and free up the resources
            graphic.Dispose();
            //return the image
            ImageData ANS = new ImageData(bmp);
            return ANS;
        }

    }


}

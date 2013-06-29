using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


namespace BotSuite.ImageLibrary
{
    /// <summary>
    /// class of faster image manipulation (by 600times faster than to default bitmap class)
    /// support 24bit and 32bit bitmap import
    /// and 24bit bitmap output
    /// </summary>
    public class ImageData
    {
        private PixelFormat BmpPixelFormat;
        private byte[] Bytes;

        private Color[,] Pixel;

        private int Stride;
        private int ByteSize;

        private int _width = -1;
        private int _height = -1;
        /// <summary>
        /// width of image
        /// </summary>
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }
        /// <summary>
        /// height of image
        /// </summary>
        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

        /// <summary>
        /// create image by open a file of bitmap
        /// </summary>
        /// <param name="Path">path to bitmap</param>
        /// <returns></returns>
        public ImageData(string Path)
        {
            SetBitmap(new Bitmap(Path));
        }
        /// <summary>
        /// create image by bitmap
        /// </summary>
        /// <param name="B">bitmap</param>
        /// <returns></returns>
        public ImageData(Bitmap B)
        {
            SetBitmap(B);
        }
        /// <summary>
        /// empty initalisation of the class
        /// </summary>
        public ImageData()
        {

        }
        /// <summary>
        /// set size of the image
        /// </summary>
        /// <param name="width">width of image</param>
        /// <param name="height">height of image</param>
        public void SetSize(int width, int height)
        {
            _width = width;
            _height = height;
            Pixel = new Color[width, height];
        }
        /// <summary>
        /// set the pixel format of image
        /// </summary>
        /// <param name="F">the pixelformat</param>
        public void SetPixelFormat(PixelFormat F)
        {
            BmpPixelFormat = F;
        }
        /// <summary>
        /// create a deep copy of an image
        /// </summary>
        /// <returns></returns>
        public ImageData Clone()
        {
            Bitmap B = GetBitmap();
            ImageData NewImg = new ImageData();
            NewImg.SetSize(_width, _height);
            NewImg.SetPixelFormat(BmpPixelFormat);
            for (int y = 0; y < _height; y++)
                for (int x = 0; x < _width; x++)
                    NewImg.SetPixel(x,y,GetPixel(x, y));
            return NewImg;
        }
        /// <summary>
        /// load a bitmap into the image
        /// </summary>
        /// <param name="B">bitmap</param>
        private void SetBitmap(Bitmap B)
        {
            Pixel = new Color[B.Width, B.Height];
            Rectangle r = new Rectangle(0, 0, B.Width, B.Height);
            BmpPixelFormat = B.PixelFormat;
            BitmapData bmpData = B.LockBits(r, ImageLockMode.ReadOnly, BmpPixelFormat);
            IntPtr ptr = bmpData.Scan0;
            Stride = bmpData.Stride;
            ByteSize = Stride * B.Height;
            Bytes = new byte[ByteSize];
            System.Runtime.InteropServices.Marshal.Copy(ptr, Bytes, 0, ByteSize);
            B.UnlockBits(bmpData);
            Pixel = new Color[B.Width, B.Height];
            switch (B.PixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                    for (int y = 0; y < B.Height; y++)
                        for (int x = 0; x < B.Width; x++)
                            Pixel[x, y] = Color.FromArgb(Bytes[y * Stride + x * 4 + 2], Bytes[y * Stride + x * 4 + 1], Bytes[y * Stride + x * 4 ]);
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    for (int y = 0; y < B.Height; y++)
                        for (int x = 0; x < B.Width; x++)
                            Pixel[x, y] = Color.FromArgb(Bytes[y * Stride + x * 3 + 2], Bytes[y * Stride + x * 3 + 1], Bytes[y * Stride + x * 3]);
                    break;
            }
            _width = B.Width;
            _height = B.Height;


//             for (int y = 0; y < B.Height; y++)
//                 for (int x = 0; x < B.Width; x++)
//                     Pixel[x, y] = Color.FromArgb(Bytes[y * Stride + x * 3 + 2], Bytes[y * Stride + x * 3 + 1], Bytes[y * Stride + x * 3]);
//             _width = B.Width;
//             _height = B.Height;
        }

        /// <summary>
        /// get image as 24bit bitmap or a part (rectangle) of it
        /// </summary>
        /// <param name="L">left of imagepart (default: 0 no offset)</param>
        /// <param name="T">top of imagepart (default: 0 no offset)</param>
        /// <param name="W">width of imagepart (default: full width) </param>
        /// <param name="H">height of imagepart (default: full height)</param>
        /// <returns>get the image as a bitmap</returns>
        public Bitmap GetBitmap(int L=0, int T=0, int W = -1, int H = -1)
        {
            W = (W == -1) ? Width - L : W;
            H = (H == -1) ? Height - T : H;

            Bitmap ReturnBitmap = new Bitmap(W, H);

            Rectangle r = new Rectangle(0, 0, W, H);
            BitmapData bmpData = ReturnBitmap.LockBits(r, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            int InStride = bmpData.Stride;
            int ByteSize = InStride * ReturnBitmap.Height;
            byte[] InBytes = new byte[ByteSize];

            for (int y = 0; y < H; y++)
                for (int x = 0; x < W; x++)
                {
                    InBytes[y * InStride + x * 3 + 2] = Pixel[x + L, y + T].R;
                    InBytes[y * InStride + x * 3 + 1] = Pixel[x + L, y + T].G;
                    InBytes[y * InStride + x * 3] = Pixel[x + L, y + T].B;
                }

            IntPtr ptr = bmpData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(InBytes, 0, ptr, ByteSize);
            ReturnBitmap.UnlockBits(bmpData);
            return ReturnBitmap;
        }

        /// <summary>
        /// get image as 24bit bitmap
        /// </summary>
        /// <returns></returns>
        public Bitmap GetBitmap()
        {

            return GetBitmap(0, 0, -1, -1);
        }

        /// <summary>
        /// set color of pixel 
        /// </summary>
        /// <param name="x">x coordinate (column)</param>
        /// <param name="y">y coordinate (row)</param>
        /// <param name="c">color to set</param>
        /// <returns></returns>
        public void SetPixel(int x, int y, Color c)
        {
            if ((0 <= x) && (x < Width) && (0 <= y) && (y < Height))
                Pixel[x, y] = c;
        }
        /// <summary>
        /// get color of pixel
        /// </summary>
        /// <param name="x">x coordinate (column)</param>
        /// <param name="y">y coordinate (row)</param>
        /// <returns>color of pixel</returns>
        public Color GetPixel(int x, int y)
        {
//            if ((0 <= x) && (x < _width) && (0 <= y) && (y < _height))
                return Pixel[x, y];
//             else
//                 return Color.Empty;
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
        /// // or use another Interpolationmode
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
            graphic.DrawImage(GetBitmap(), 0, 0, NewWidth, NewHeight);
            //dispose and free up the resources
            graphic.Dispose();
            //return the image
            ImageData ANS = new ImageData(bmp);
            return ANS;
        }


    }
}

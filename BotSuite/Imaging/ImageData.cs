// -----------------------------------------------------------------------
//  <copyright file="ImageData.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite.Imaging
{
	using System;
	using System.Drawing;
	using System.Drawing.Drawing2D;
	using System.Drawing.Imaging;
	using System.Runtime.InteropServices;

	/// <summary>
	///     class of faster image manipulation (by 600times faster than to default bitmap class)
	///     support 24bit and 32bit bitmap import
	///     and 24bit bitmap output
	/// </summary>
	public class ImageData
	{
		/// <summary>
		///     Gets the height.
		/// </summary>
		public int Height { get; private set; }

		/// <summary>
		///     Gets the width.
		/// </summary>
		public int Width { get; private set; }

		/// <summary>
		///     The bmp bytes.
		/// </summary>
		private byte[] bmpBytes;

		/// <summary>
		///     The bmp stride.
		/// </summary>
		private int bmpStride;

		/// <summary>
		///     The bmp pixel format.
		/// </summary>
		private PixelFormat bmpPixelFormat;

		/// <summary>
		///     The raw format offset.
		/// </summary>
		private int rawFormatOffset;

		/// <summary>
		///     Gets or sets the size.
		/// </summary>
		public Size Size
		{
			get
			{
				return new Size(this.Width, this.Height);
			}
			set
			{
				this.Height = value.Height;
				this.Width = value.Width;
			}
		}

		/// <summary>
		///     set a 24bit or 32bit bitmap
		///     or returns a 24bit bitmap
		/// </summary>
		/// <example>
		///     <code>
		/// <![CDATA[
		/// ImageData Img = new ImageData(...);
		/// bmpBitmap Bmp = Img.bmpBitmap;
		/// bmpBitmap Bmp2 = new bmpBitmap(...);
		/// Img.bmpBitmap = Bmp2;
		/// ]]>
		/// </code>
		/// </example>
		public Bitmap Bitmap
		{
			get
			{
				return this.CreateBitmap();
			}
			set
			{
				this.LoadBitmap(value);
			}
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="ImageData" /> class.
		///     creates a new imagedata object from a given bitmap
		/// </summary>
		/// <example>
		///     <code>
		/// <![CDATA[
		/// bmpBitmap Bmp = new bmpBitmap(...);
		/// ImageData Img = new ImageData(Bmp);
		/// ]]>
		/// </code>
		/// </example>
		/// <param name="bitmap">
		///     tagert bitmap
		/// </param>
		public ImageData(Bitmap bitmap)
		{
			this.LoadBitmap(bitmap);
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="ImageData" /> class.
		///     creates a new imagedata object from a file
		/// </summary>
		/// <example>
		///     <code>
		/// <![CDATA[
		/// ImageData Img = new ImageData("path/to/bitmap/file.bmp");
		/// ]]>
		/// </code>
		/// </example>
		/// <param name="path">
		///     path to bitmap
		/// </param>
		public ImageData(string path)
		{
			this.LoadBitmap(new Bitmap(path));
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="ImageData" /> class.
		///     creates a new imagedata object from another imagedata object
		/// </summary>
		/// <example>
		///     <code>
		/// <![CDATA[
		/// ImageData Img = new ImageData(...);
		/// ImageData Img2 = new ImageData(Img);
		/// ]]>
		/// </code>
		/// </example>
		/// <param name="img">
		///     The Img.
		/// </param>
		public ImageData(ImageData img)
		{
			this.LoadBitmap(img.Bitmap);
		}

		/// <summary>
		///     store a bitmap into a file
		/// </summary>
		/// <example>
		///     <code>
		/// <![CDATA[
		/// ImageData Img = new ImageData(...);
		/// Img.Save("path/to/bitmap/file.bmp");
		/// ]]>
		/// </code>
		/// </example>
		/// <param name="path">
		///     savepath
		/// </param>
		public void Save(string path)
		{
			this.Bitmap.Save(path);
		}

		/// <summary>
		///     clones an imagedata object
		/// </summary>
		/// <example>
		///     <code>
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

		/// <summary>
		///     The load bitmap.
		/// </summary>
		/// <param name="bmpBitmap">
		///     The bitmap.
		/// </param>
		public void LoadBitmap(Bitmap bmpBitmap)
		{
			// extract bitmap data
			this.bmpPixelFormat = bmpBitmap.PixelFormat;
			this.Width = bmpBitmap.Width;
			this.Height = bmpBitmap.Height;
			switch (bmpBitmap.PixelFormat)
			{
				case PixelFormat.Format32bppArgb:
					this.rawFormatOffset = 4;
					break;
				case PixelFormat.Format24bppRgb:
					this.rawFormatOffset = 3;
					break;
			}

			// load bytes
			Rectangle bmpRectangle = new Rectangle(0, 0, bmpBitmap.Width, bmpBitmap.Height);
			BitmapData bmpData = bmpBitmap.LockBits(bmpRectangle, ImageLockMode.ReadOnly, this.bmpPixelFormat);
			IntPtr ptr = bmpData.Scan0;
			this.bmpStride = bmpData.Stride;
			int byteSize = this.bmpStride * bmpBitmap.Height;
			this.bmpBytes = new byte[byteSize];
			Marshal.Copy(ptr, this.bmpBytes, 0, byteSize);
			bmpBitmap.UnlockBits(bmpData);
		}

		/// <summary>
		///     get image as 24bit bitmap or a part (rectangle) of it
		/// </summary>
		/// <param name="left">
		///     left of imagepart (default: 0 no offset)
		/// </param>
		/// <param name="top">
		///     top of imagepart (default: 0 no offset)
		/// </param>
		/// <param name="width">
		///     width of imagepart (default: full width)
		/// </param>
		/// <param name="height">
		///     height of imagepart (default: full height)
		/// </param>
		/// <returns>
		///     The <see cref="Bitmap" />.
		/// </returns>
		public Bitmap CreateBitmap(int left = 0, int top = 0, int width = -1, int height = -1)
		{
			width = (width == -1) ? this.Width - left : width;
			height = (height == -1) ? this.Height - top : height;

			Bitmap returnBitmap = new Bitmap(this.Width, this.Height);
			Rectangle r = new Rectangle(0, 0, this.Width, this.Height);

			BitmapData bmpData = returnBitmap.LockBits(r, ImageLockMode.ReadWrite, this.bmpPixelFormat);
			int inStride = bmpData.Stride;
			int byteSize = inStride * returnBitmap.Height;
			IntPtr ptr = bmpData.Scan0;
			Marshal.Copy(this.bmpBytes, 0, ptr, byteSize);
			returnBitmap.UnlockBits(bmpData);

			if (!(left == 0 && top == 0 && width == Width && height == Height))
			{
				Rectangle cropRect = new Rectangle(left, top, width, height);
				Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);
				using (Graphics g = Graphics.FromImage(target))
				{
					g.DrawImage(returnBitmap, new Rectangle(0, 0, target.Width, target.Height), cropRect, GraphicsUnit.Pixel);
				}
			}

			return returnBitmap;
		}

		/// <summary>
		///     get or set pixel color
		/// </summary>
		/// <param name="x">
		///     x coordinate (column)
		/// </param>
		/// <param name="y">
		///     y coordinate (row)
		/// </param>
		/// <returns>
		///     color of pixel
		/// </returns>
		public Color this[int x, int y]
		{
			get
			{
				if ((0 <= x) && (x < this.Width) && (0 <= y) && (y < this.Height))
				{
					return Color.FromArgb(
						this.bmpBytes[y * this.bmpStride + x * this.rawFormatOffset + 2],
						this.bmpBytes[y * this.bmpStride + x * this.rawFormatOffset + 1],
						this.bmpBytes[y * this.bmpStride + x * this.rawFormatOffset]);
				}

				return Color.Empty;
			}

			set
			{
				if ((0 <= x) && (x < this.Width) && (0 <= y) && (y < this.Height))
				{
					this.bmpBytes[y * this.bmpStride + x * this.rawFormatOffset + 2] = value.R;
					this.bmpBytes[y * this.bmpStride + x * this.rawFormatOffset + 1] = value.G;
					this.bmpBytes[y * this.bmpStride + x * this.rawFormatOffset] = value.B;
				}
			}
		}

		/// <summary>
		///     get color of pixel
		/// </summary>
		/// <param name="x">
		///     x coordinate (column)
		/// </param>
		/// <param name="y">
		///     y coordinate (row)
		/// </param>
		/// <returns>
		///     color of pixel
		/// </returns>
		public Color GetPixel(int x, int y)
		{
			return this[x, y];
		}

		/// <summary>
		///     set color of pixel
		/// </summary>
		/// <param name="row">
		///     The row.
		/// </param>
		/// <param name="column">
		///     The col.
		/// </param>
		/// <param name="color">
		///     color to set
		/// </param>
		public void SetPixel(int row, int column, Color color)
		{
			this[row, column] = color;
		}

		/// <summary>
		///     resize an image by a factor
		/// </summary>
		/// <example>
		///     <code>
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
		/// <param name="factor">
		///     Factor (0.80 means downscale by 80%)
		/// </param>
		/// <param name="mode">
		///     Mode of Interpolation (Default: HighQualityBicubic, Possible parameter:
		///     HighQualityBicubic,HighQualityBilinear,Bilinear,...)
		/// </param>
		/// ///
		/// <returns>
		///     The <see cref="ImageData" />.
		/// </returns>
		public ImageData Resize(double factor, InterpolationMode mode = InterpolationMode.HighQualityBicubic)
		{
			// get the new size based on the percentage change
			int resizedW = (int)(this.Width * factor);
			int resizedH = (int)(this.Height * factor);

			return this.Resize(resizedW, resizedH, mode);
		}

		/// <summary>
		///     resize an image
		/// </summary>
		/// <example>
		///     <code>
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
		/// <param name="newWidth">
		///     new Width of image
		/// </param>
		/// <param name="newHeight">
		///     new Height of image
		/// </param>
		/// <param name="mode">
		///     Mode of Interpolation (Default: HighQualityBicubic, Possible parameter:
		///     HighQualityBicubic,HighQualityBilinear,Bilinear,...)
		/// </param>
		/// <returns>
		///     The <see cref="ImageData" />.
		/// </returns>
		public ImageData Resize(int newWidth, int newHeight, InterpolationMode mode = InterpolationMode.HighQualityBicubic)
		{
			// create a new bmpBitmap the size of the new image
			Bitmap bmp = new Bitmap(newWidth, newHeight, this.bmpPixelFormat);

			// create a new graphic from the bmpBitmap
			Graphics graphic = Graphics.FromImage(bmp);
			graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

			// draw the newly resized image
			graphic.DrawImage(this.Bitmap, 0, 0, newWidth, newHeight);

			// dispose and free up the resources
			graphic.Dispose();

			// return the image
			return new ImageData(bmp);
		}
	}
}
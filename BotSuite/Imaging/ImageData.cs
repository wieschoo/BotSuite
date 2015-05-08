// -----------------------------------------------------------------------
//  <copyright file="ImageData.cs" company="Binary Overdrive">
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
		private byte[] _bmpBytes;

		/// <summary>
		///     The bmp stride.
		/// </summary>
		private int _bmpStride;

		/// <summary>
		///     The bmp pixel format.
		/// </summary>
		private PixelFormat _bmpPixelFormat;

		/// <summary>
		///     The raw format offset.
		/// </summary>
		private int _rawFormatOffset;

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
		/// <returns>new imagedata object</returns>
		public ImageData Clone()
		{
			return new ImageData(this);
		}

		/// <summary>
		///     The load bitmap.
		/// </summary>
		/// <param name="bmp">
		///     The bitmap.
		/// </param>
		public void LoadBitmap(Bitmap bmp)
		{
			// extract bitmap data
			this._bmpPixelFormat = bmp.PixelFormat;
			this.Width = bmp.Width;
			this.Height = bmp.Height;
			switch(bmp.PixelFormat)
			{
				case PixelFormat.Format32bppArgb:
					this._rawFormatOffset = 4;
					break;
				case PixelFormat.Format24bppRgb:
					this._rawFormatOffset = 3;
					break;
			}

			// load bytes
			Rectangle bmpRectangle = new Rectangle(0, 0, bmp.Width, bmp.Height);
			BitmapData bmpData = bmp.LockBits(bmpRectangle, ImageLockMode.ReadOnly, this._bmpPixelFormat);
			IntPtr ptr = bmpData.Scan0;
			this._bmpStride = bmpData.Stride;
			int byteSize = this._bmpStride * bmp.Height;
			this._bmpBytes = new byte[byteSize];
			Marshal.Copy(ptr, this._bmpBytes, 0, byteSize);
			bmp.UnlockBits(bmpData);
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

			BitmapData bmpData = returnBitmap.LockBits(r, ImageLockMode.ReadWrite, this._bmpPixelFormat);
			int inStride = bmpData.Stride;
			int byteSize = inStride * returnBitmap.Height;
			IntPtr ptr = bmpData.Scan0;
			Marshal.Copy(this._bmpBytes, 0, ptr, byteSize);
			returnBitmap.UnlockBits(bmpData);

			if(!(left == 0 && top == 0 && width == this.Width && height == this.Height))
			{
				Rectangle cropRect = new Rectangle(left, top, width, height);
				Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);
				using(Graphics g = Graphics.FromImage(target))
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
				if((0 <= x) && (x < this.Width) && (0 <= y) && (y < this.Height))
				{
					return Color.FromArgb(
						this._bmpBytes[y * this._bmpStride + x * this._rawFormatOffset + 2],
						this._bmpBytes[y * this._bmpStride + x * this._rawFormatOffset + 1],
						this._bmpBytes[y * this._bmpStride + x * this._rawFormatOffset]);
				}

				return Color.Empty;
			}

			set
			{
				if((0 <= x) && (x < this.Width) && (0 <= y) && (y < this.Height))
				{
					this._bmpBytes[y * this._bmpStride + x * this._rawFormatOffset + 2] = value.R;
					this._bmpBytes[y * this._bmpStride + x * this._rawFormatOffset + 1] = value.G;
					this._bmpBytes[y * this._bmpStride + x * this._rawFormatOffset] = value.B;
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
		/// <param name="factor">
		///     Factor (0.80 means downscale by 80%)
		/// </param>
		/// <param name="interpolationModemode">
		///     Mode of Interpolation (Default: HighQualityBicubic, Possible parameter:
		///     HighQualityBicubic,HighQualityBilinear,Bilinear,...)
		/// </param>
		/// ///
		/// <returns>
		///     The <see cref="ImageData" />.
		/// </returns>
		public ImageData Resize(double factor, InterpolationMode interpolationModemode = InterpolationMode.HighQualityBicubic)
		{
			// get the new size based on the percentage change
			int resizedW = (int)(this.Width * factor);
			int resizedH = (int)(this.Height * factor);

			return this.Resize(resizedW, resizedH, interpolationModemode);
		}

		/// <summary>
		///     resize an image
		/// </summary>
		/// <param name="newWidth">
		///     new Width of image
		/// </param>
		/// <param name="newHeight">
		///     new Height of image
		/// </param>
		/// <param name="interpolationModemode">
		///     Mode of Interpolation (Default: HighQualityBicubic, Possible parameter:
		///     HighQualityBicubic,HighQualityBilinear,Bilinear,...)
		/// </param>
		/// <returns>
		///     The <see cref="ImageData" />.
		/// </returns>
		public ImageData Resize(
			int newWidth,
			int newHeight,
			InterpolationMode interpolationModemode = InterpolationMode.HighQualityBicubic)
		{
			// create a new bmpBitmap the size of the new image
			Bitmap bmp = new Bitmap(newWidth, newHeight, this._bmpPixelFormat);

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
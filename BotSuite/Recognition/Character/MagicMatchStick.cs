// -----------------------------------------------------------------------
//  <copyright file="MagicMatchStick.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite.Recognition.Character
{
	using System;

	/// <summary>
	///     one receptor (magicmatchstick) a line in the picture can be hitted by character (this results a true) or pass the
	///     character (this results a false)
	/// </summary>
	[Serializable]
	public class MagicMatchStick
	{
		/// <summary>
		///     description of the line
		/// </summary>
		protected int Xa;

		/// <summary>
		///     description of the line
		/// </summary>
		protected int Ya;

		/// <summary>
		///     description of the line
		/// </summary>
		protected int Xb;

		/// <summary>
		///     description of the line
		/// </summary>
		protected int Yb;

		/// <summary>
		///     description of the line
		/// </summary>
		protected int Left;

		/// <summary>
		///     description of the line
		/// </summary>
		protected int Top;

		/// <summary>
		///     description of the line
		/// </summary>
		protected int Right;

		/// <summary>
		///     description of the line
		/// </summary>
		protected int Bottom;

		/// <summary>
		///     calculates variables fo the line description
		/// </summary>
		protected float M;

		/// <summary>
		///     calculates variables fo the line description
		/// </summary>
		protected float Z;

		/// <summary>
		///     calculates variables fo the line description
		/// </summary>
		protected float Dy;

		/// <summary>
		///     calculates variables fo the line description
		/// </summary>
		protected float Dx;

		/// <summary>
		///     calculates variables fo the line description
		/// </summary>
		protected float C;

		/// <summary>
		///     calculates variables fo the line description
		/// </summary>
		protected float D;

		/// <summary>
		///     Initializes a new instance of the <see cref="MagicMatchStick" /> class.
		///     create one magic match stick
		/// </summary>
		/// <param name="xaIn">
		///     first x coordinate
		/// </param>
		/// <param name="yaIn">
		///     first y coordinate
		/// </param>
		/// <param name="xbIn">
		///     second x coordinate
		/// </param>
		/// <param name="ybIn">
		///     second y coordinate
		/// </param>
		public MagicMatchStick(int xaIn, int yaIn, int xbIn, int ybIn)
		{
			this.Xa = xaIn;
			this.Ya = yaIn;
			this.Xb = xbIn;
			this.Yb = ybIn;

			this.Top = Math.Min(yaIn, ybIn);
			this.Left = Math.Min(xaIn, xbIn);
			this.Bottom = Math.Max(yaIn, ybIn);
			this.Right = Math.Max(xaIn, xbIn);

			if (xaIn != xbIn)
			{
				this.M = (ybIn - yaIn) / (float)(xbIn - xaIn);
				this.Z = yaIn - this.M * xaIn;
				this.Dy = yaIn - ybIn;
				this.Dx = xbIn - xaIn;
				this.C = yaIn * (xaIn - xbIn) + xaIn * (ybIn - yaIn);
				this.D = (float)Math.Sqrt(this.Dy * this.Dy + this.Dx * this.Dx);
			}
		}

		// Check receptor state
		/// <summary>
		///     does the magic match stick lies over (x,y) ?
		/// </summary>
		/// <param name="x">
		///     x coordinate
		/// </param>
		/// <param name="y">
		///     y coordinate
		/// </param>
		/// <returns>
		///     cover / uncover as boolean
		/// </returns>
		public bool GetMagicMatchStickState(int x, int y)
		{
			// check, if the point is in receptors bounds
			if ((x < this.Left) || (y < this.Top) || (x > this.Right) || (y > this.Bottom))
			{
				return false;
			}

			// check for horizontal and vertical receptors
			if ((this.Xa == this.Xb) || (this.Ya == this.Yb))
			{
				return true;
			}

			// check if the point is on the receptors line
			if (Math.Abs(this.Dy * x + this.Dx * y + this.C) / this.D < 1)
			{
				return true;
			}

			return false;
		}
	}
}
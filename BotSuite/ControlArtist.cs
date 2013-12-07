// -----------------------------------------------------------------------
//  <copyright file="ControlArtist.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite
{
	using System.Drawing;

	/// <summary>
	///     This class provide functions to draw on the clientarea without computing the coordinate from absolute to relative
	/// </summary>
	public class ControlArtist
	{
		/// <summary>
		///     The form graphics.
		/// </summary>
		private readonly Graphics formGraphics;

		/// <summary>
		///     The client rectangle.
		/// </summary>
		private Rectangle clientRectangle;

		/// <summary>
		///     Initializes a new instance of the <see cref="ControlArtist" /> class.
		///     constructor to use this class
		/// </summary>
		/// <example>
		///     <code>
		/// <![CDATA[
		/// int LeftMargin = 10;
		/// int TopMargin = 10;
		/// int BottomMargin = 10;
		/// int RightMargin = 100;
		/// // initialise control artists in form
		/// ControlArtist BL = new ControlArtist(this.CreateGraphics(), this.ClientRectangle);
		/// // draw a target rectangle with margin to the clientarea-borders
		/// BL.DrawRectangle(LeftMargin,TopMargin,RightMargin,BottomMargin);
		/// ]]>
		/// </code>
		/// </example>
		/// <param name="gf">
		///     target graphic
		/// </param>
		/// <param name="cr">
		///     clientarea as rectangle
		/// </param>
		/// <returns>
		/// </returns>
		public ControlArtist(Graphics gf, Rectangle cr)
		{
			this.formGraphics = gf;
			this.clientRectangle = cr;
		}

		/// <summary>
		///     draw a rectangle on the control be using margins
		/// </summary>
		/// <param name="leftMargin">
		///     margin from left
		/// </param>
		/// <param name="topMargin">
		///     margin from top
		/// </param>
		/// <param name="rightMargin">
		///     margin from right(default: 0)
		/// </param>
		/// <param name="bottomMargin">
		///     margin from bottom(default: 0)
		/// </param>
		public void DrawRectangle(int leftMargin, int topMargin, int rightMargin = 0, int bottomMargin = 0)
		{
			this.formGraphics.DrawRectangle(
				new Pen(Color.Blue),
				new Rectangle(
					leftMargin,
					topMargin,
					this.clientRectangle.Width - leftMargin - rightMargin,
					this.clientRectangle.Height - topMargin - bottomMargin));
		}

		/// <summary>
		///     same as DrawRectangle but only returns the rectangle without drawing
		/// </summary>
		/// <param name="leftMargin">
		///     margin from left
		/// </param>
		/// <param name="topMargin">
		///     margin from top
		/// </param>
		/// <param name="rightMargin">
		///     margin from right(default: 0)
		/// </param>
		/// <param name="bottomMargin">
		///     margin from bottom(default: 0)
		/// </param>
		/// <returns>
		///     The <see cref="Rectangle" />.
		/// </returns>
		public Rectangle GetDrawRectangle(int leftMargin, int topMargin, int rightMargin, int bottomMargin)
		{
			return new Rectangle(
				leftMargin,
				topMargin,
				this.clientRectangle.Width - leftMargin - rightMargin,
				this.clientRectangle.Height - topMargin - bottomMargin);
		}

		// public void MakeTransparent(Control C)
		// {
		// C.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		// C.TransparencyKey = Color.FromKnownColor(KnownColor.Control);
		// C.Update();
		// }
	}
}
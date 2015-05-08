/* **************************************************************
 * Name:      BotSuite.NET
 * Purpose:   Framework for creating bots
 * Homepage:  http://www.wieschoo.com
 * Copyright: (c) 2013 wieschoo & enWare
 * License:   http://www.wieschoo.com/botsuite/license/
 * *************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BotSuite
{
    /// <summary>
    ///  This class provide functions to draw on the clientarea without computing the coordinate from absolute to relative
    /// </summary>
    public class ControlArtist
    {
        System.Drawing.Graphics formGraphics;
        Rectangle ClientRectangle;

        /// <summary>
        /// constructor to use this class
        /// </summary>
        /// <example>
        /// <code>
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
        /// <param name="GF">target graphic</param>
        /// <param name="CR">clientarea as rectangle</param>
        /// <returns></returns>
        public ControlArtist(System.Drawing.Graphics GF, Rectangle CR)
        {
            formGraphics = GF;
            ClientRectangle = CR;
        }

        /// <summary>
        /// draw a rectangle on the control be using margins
        /// </summary>
        /// <param name="LeftMargin">margin from left</param>
        /// <param name="TopMargin">margin from top</param>
        /// <param name="RightMargin">margin from right(default: 0)</param>
        /// <param name="BottomMargin">margin from bottom(default: 0)</param>
        /// <returns></returns>
        public void DrawRectangle(int LeftMargin, int TopMargin, int RightMargin=0, int BottomMargin=0)
        {
            formGraphics.DrawRectangle(new Pen(Color.Blue), new Rectangle(
                LeftMargin, TopMargin, ClientRectangle.Width - LeftMargin - RightMargin, ClientRectangle.Height - TopMargin - BottomMargin));
        }

        /// <summary>
        /// same as DrawRectangle but only returns the rectangle without drawing
        /// </summary>
        /// <param name="LeftMargin">margin from left</param>
        /// <param name="TopMargin">margin from top</param>
        /// <param name="RightMargin">margin from right(default: 0)</param>
        /// <param name="BottomMargin">margin from bottom(default: 0)</param>
        /// <returns></returns>
        public Rectangle GetDrawRectangle(int LeftMargin, int TopMargin, int RightMargin, int BottomMargin)
        {
            return new Rectangle(
                LeftMargin, TopMargin, ClientRectangle.Width - LeftMargin - RightMargin, ClientRectangle.Height - TopMargin - BottomMargin);
        }

    }
}

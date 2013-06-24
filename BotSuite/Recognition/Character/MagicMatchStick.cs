﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/// <summary>
/// OCR
/// </summary>
namespace BotSuite.Recognition.Character
{
    [Serializable]
    public class MagicMatchStick
    {
        #region PROTECTED PROPERTIES
        protected int xa, ya, xb, yb;
        protected int left, top, right, bottom;
        protected float m, z;
        protected float dy, dx, c, d; 
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// create one magic match stick
        /// </summary>
        /// <param name="x1">first x coordinate</param>
        /// <param name="y1">first y coordinate</param>
        /// <param name="x2">second x coordinate</param>
        /// <param name="y2">second y coordinate</param>
        /// <returns></returns>
        public MagicMatchStick(int xa_in, int ya_in, int xb_in, int yb_in)
        {
            xa = xa_in;
            ya = ya_in;
            xb = xb_in;
            yb = yb_in;

            top = Math.Min(ya_in, yb_in);
            left = Math.Min(xa_in, xb_in);
            bottom = Math.Max(ya_in, yb_in);
            right = Math.Max(xa_in, xb_in);

            if (xa_in != xb_in)
            {
                m = (float)(yb_in - ya_in) / (float)(xb_in - xa_in);
                z = (float)ya_in - m * xa_in;
                dy = ya_in - yb_in;
                dx = xb_in - xa_in;
                c = ya_in * (xa_in - xb_in) + xa_in * (yb_in - ya_in);
                d = (float)Math.Sqrt(dy * dy + dx * dx);
            }
        } 
        #endregion

        #region PATTERN CREATION
        // Check receptor state
        /// <summary>
        /// does the magic match stick lies over (x,y) ?
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>cover / uncover as boolean</returns>
        public bool GetMagicMatchStickState(int x, int y)
        {
            // check, if the point is in receptors bounds
            if ((x < left) || (y < top) || (x > right) || (y > bottom))
                return false;

            // check for horizontal and vertical receptors
            if ((xa == xb) || (ya == yb))
                return true;

            // check if the point is on the receptors line

            // more fast, but not very accurate
            //			if ((int)(k * x + z - y) == 0)
            //				return true;

            // more accurate version
            if (Math.Abs(dy * x + dx * y + c) / d < 1)
                return true;

            return false;
        } 
        #endregion
	}
}

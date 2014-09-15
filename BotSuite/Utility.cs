/* **************************************************************
 * Name:      BotSuite.NET
 * Purpose:   Framework for creating bots
 * Homepage:  http://www.wieschoo.com
 * Copyright: (c) 2013 wieschoo & enWare
 * License:   http://www.wieschoo.com/botsuite/license/
 * *************************************************************/

using System;
using System.Threading;

namespace BotSuite
{
    /// <summary>
    ///     commons functions
    /// </summary>
    public class Utility
    {
        private static readonly Random _random = new Random();

        /// <summary>
        ///     pause the current thread for x ms
        /// </summary>
        /// <param name="lower">min time</param>
        /// <param name="upper">max time </param>
        /// <returns></returns>
        public static void Delay(Int32 lower = 10, Int32 upper = 30)
        {
            Thread.Sleep(Random(lower, upper));
        }

        /// <summary>
        ///     pause the current thread for x ms
        /// </summary>
        /// <returns></returns>
        public static void Delay(Int32 x = 10000)
        {
            Thread.Sleep(x);
        }

//         public static void MakeFormTransparent(Form AppWindow)
//         {
//             AppWindow.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
//             AppWindow.TransparencyKey = Color.FromKnownColor(KnownColor.Control);
//             AppWindow.Update();
//         }

        /// <summary>
        ///     create a random integer (unif) between lower and upper
        /// </summary>
        /// <param name="lower">min number</param>
        /// <param name="upper">max number</param>
        /// <returns>random integer</returns>
        public static Int32 Random(Int32 lower, Int32 upper)
        {
            return _random.Next(lower, upper);
        }

        /// <summary>
        ///     create a random double between lower and upper
        /// </summary>
        /// <param name="lower">min number</param>
        /// <param name="upper">max number</param>
        /// <returns>random double</returns>
        public static Double Random(Double lower, Double upper)
        {
            return _random.NextDouble()*((upper - lower) + lower);
        }
    }
}
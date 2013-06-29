using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using BotSuite.ImageLibrary;

namespace BotSuite.Recognition.Character
{
    /// <summary>
    /// receptors for optical character recognition
    /// </summary>
    [Serializable]
    public class MagicMatchSticks
    {
        #region protected properties
        /// <summary>
        /// intern instance of PRNG
        /// </summary>
        protected Random rand = new Random();
        /// <summary>
        /// intern list of receptors
        /// </summary>
        protected List<MagicMatchStick> MagicStickList; 
        #endregion

        #region public access to properties
        /// <summary>
        /// handle the list of receptors
        /// </summary>
        /// <param name="index">index of receptor</param>
        /// <returns>receptor from list</returns>
        public MagicMatchStick this[int index]
        {
            get { return MagicStickList[index]; }
        }
        /// <summary>
        /// get the number of magic match sticks
        /// </summary>
        /// <returns>num of magic sticks</returns>
        public int Num()
        {
            return MagicStickList.Count;
        }

        /// <summary>
        /// add a magic stick to list
        /// </summary>
        /// <param name="Stick">magic stick</param>
        /// <returns></returns>
        public void Add(MagicMatchStick Stick)
        {
            MagicStickList.Add(Stick);
        } 
        #endregion

        #region construct and initalise
        /// <summary>
        /// Create a Container of Magic match Sticks
        /// </summary>
        /// <returns></returns>
        public MagicMatchSticks()
        {
            MagicStickList = new List<MagicMatchStick> { };
        }
        /// <summary>
        /// generate some magic sticks in [0,1]^2
        /// </summary>
        /// <param name="count">number of magic sticks</param>
        /// <returns></returns>
        public void Generate(int count)
        {
            for (int i = 0; i < count; i++)
            {
                int xa = rand.Next(100);
                int ya = rand.Next(100);
                int xb = rand.Next(100);
                int yb = rand.Next(100);

                int dx = xa - xb;
                int dy = ya - yb;
                int length = (int)Math.Sqrt(dx * dx + dy * dy);

                // if their length is to short, then take another
                if ((length < 9) || (length > 54))
                {
                    i--;
                    continue;
                }
                MagicStickList.Add(new MagicMatchStick(xa, ya, xb, yb));
            }
        } 
        #endregion

        #region create pattern for learning
        /// <summary>
        /// Get the pattern of the magics match sticks
        /// </summary>
        /// <param name="Image">image of character</param>
        /// <returns>pattern</returns>
        public float[] GetMagicMatchSticksState(ImageData Image)
        {
            int Width = Image.Width;        // width of image
            int Height = Image.Height;      // height of image
            int n = Num();                  // num of magicsticks
            float[] Pattern = new float[n];   // state

            int lastx = -1, lasty = -1;
            int tmpx = -1, tmpy = -1;

            for (int i = 0; i < n; i++)
            {
                Pattern[i] = 0.0f;
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    // is the pixel black?
                    if (Image.GetPixel(x, y).R == 0)
                    {
                        tmpx = Convert.ToInt16(x * 100 / Width);
                        tmpy = Convert.ToInt16(y * 100 / Height);

                        if ((tmpx != lastx) || (tmpy != lasty))
                        {
                            lastx = tmpx;
                            lasty = tmpy;
                            for (int i = 0; i < n; i++)
                            {
                                // skip already activated receptors
                                if (Pattern[i] == 1.0f)
                                    continue;

                                if (MagicStickList[i].GetMagicMatchStickState(tmpx, tmpy))
                                    Pattern[i] = 1.0f;
                            }
                        }

                    }
                }
            }

            return Pattern;
        } 
        #endregion
        
    }
     
}

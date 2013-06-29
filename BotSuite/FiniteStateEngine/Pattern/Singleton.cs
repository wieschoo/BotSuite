using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotSuite.FiniteStateEngine.Pattern
{
    /// <summary>
    /// singleton pattern
    /// </summary>
    /// <typeparam name="T">type of object</typeparam>
    public static class Singleton<T> where T : new()
    {
        /// <summary>
        /// private handle
        /// </summary>
        private static T pInstance;
        /// <summary>
        /// public constructor
        /// </summary>
        public static T Instance
        {
            get
            {
                if (pInstance == null)
                {
                    pInstance = new T();
                }

                return pInstance;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotSuite.FiniteStateEngine.Pattern
{
    public static class Singleton<T> where T : new()
    {
        private static T pInstance;

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

using System;
using System.Text.RegularExpressions;
// gigagames
namespace BotSuite
{
    class CommonRegex
    {
        /// <summary>
        /// search a pattern in a text 
        /// </summary>
        /// <param name="Source">text where looking for</param>
        /// <param name="Pattern">pattern to look for</param>
        /// <returns>array of result</returns>
        public static String[,] Search(String Pattern,String Source)
        {
            if (!Regex.IsMatch(Source, Pattern)) return null;
            MatchCollection aMatch = Regex.Matches(Source, Pattern, RegexOptions.None);
            String[,] RetString = new String[aMatch.Count, aMatch[0].Groups.Count - 1];
            Int32 i = 0;
            Int32 ii = 0;
            foreach (Match bMatch in aMatch)
            {
                do
                {
                    RetString[i, ii] = bMatch.Groups[ii + 1].Value;
                    if (ii != RetString.GetLength(1))
                    {
                        ii++;
                    }
                } while (ii < bMatch.Groups.Count - 1);
                ii = 0;
                i++;
            }
            return RetString;
        }





        /*
        public static String[,] Regexp(this String Source, String Pattern)
        {
            if (!Regex.IsMatch(Source, Pattern)) return null;
            MatchCollection aMatch = Regex.Matches(Source, Pattern, RegexOptions.None);
            String[,] RetString = new String[aMatch.Count - 1, aMatch[0].Groups.Count - 1];
            for (Int32 i = 0; i <= aMatch.Count - 1; i++)
            {
                //RetString[i, 0] = aMatch[i].Captures[0].Value;
                for (Int32 ii = 0; ii <= aMatch[i].Groups.Count - 2; ii++)
                {
                    RetString[i, ii] = aMatch[ii].Groups[0].Value;
                }
            }
            return RetString;
        }
        */


        /// <summary>
        /// returns a string between two strings
        /// </summary>
        /// <param name="Source">text where looking for</param>
        /// <param name="Start">before the string we are looking for</param>
        /// <param name="End">after the string we are looking for</param>
        /// <returns>all results</returns>
        public static String[] Between(String Start, String End, String Source)
        {
            string Pattern = Start + "(.*)?" + End;
            String[] tString;
            if (Regex.IsMatch(Source.ToString(), Pattern))
            {
                MatchCollection MC = Regex.Matches(Source, Pattern);
                tString = new String[MC.Count];
                for (int i = 0; i <= tString.Length - 1; i++)
                {
                    tString[i] = MC[i].Groups[1].Captures[0].Value;
                }
                return tString;
            }
            else
            {
                return tString = new String[1] { "" };
            }
        }
    }
}

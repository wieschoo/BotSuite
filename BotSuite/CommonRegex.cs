// -----------------------------------------------------------------------
//  <copyright file="CommonRegex.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite
{
	using System.Text.RegularExpressions;

	/// <summary>
	///     The common regex.
	/// </summary>
	public static class CommonRegex
	{
		/// <summary>
		///     search a pattern in a text
		/// </summary>
		/// <param name="source">
		///     text where looking for
		/// </param>
		/// <param name="pattern">
		///     pattern to look for
		/// </param>
		/// <param name="options">
		///     Options for regex
		/// </param>
		/// <returns>
		///     array of result
		/// </returns>
		public static string[,] Search(this string source, string pattern, RegexOptions options = RegexOptions.None)
		{
			if (!Regex.IsMatch(source, pattern, options))
			{
				return null;
			}

			MatchCollection matches = Regex.Matches(source, pattern, options);
			string[,] retString = new string[matches.Count, matches[0].Groups.Count - 1];
			int i = 0;
			int ii = 0;
			foreach (Match match in matches)
			{
				do
				{
					retString[i, ii] = match.Groups[ii + 1].Value;
					if (ii != retString.GetLength(1))
					{
						ii++;
					}
				}
				while (ii < match.Groups.Count - 1);

				ii = 0;
				i++;
			}

			return retString;
		}

		/// <summary>
		///     returns a string between two strings
		/// </summary>
		/// <param name="source">
		///     text where looking for
		/// </param>
		/// <param name="start">
		///     before the string we are looking for
		/// </param>
		/// <param name="end">
		///     after the string we are looking for
		/// </param>
		/// <returns>
		///     all results
		/// </returns>
		public static string[] Between(this string source, string start, string end)
		{
			string pattern = start + "(.*)?" + end;

			if (Regex.IsMatch(source, pattern))
			{
				MatchCollection matches = Regex.Matches(source, pattern);
				string[] result = new string[matches.Count];
				for (int i = 0; i <= result.Length - 1; i++)
				{
					result[i] = matches[i].Groups[1].Captures[0].Value;
				}

				return result;
			}

			return null;
		}
	}
}
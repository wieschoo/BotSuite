// -----------------------------------------------------------------------
//  <copyright file="Utility.cs" company="Binary Overdrive">
//      Copyright (c) Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>Framework for creating automation applications.</purpose>
//  <homepage>https://bitbucket.org/KarillEndusa/botsuite.net</homepage>
//  <license>https://bitbucket.org/KarillEndusa/botsuite.net/wiki/license</license>
// -----------------------------------------------------------------------

namespace BotSuite
{
	using System;
	using System.Threading;

	/// <summary>
	///     commons functions
	/// </summary>
	public class Utility
	{
	    /// <summary>
	    /// The random generator.
	    /// </summary>
	    private static readonly Random Random = new Random();

		/// <summary>
		///     pause the current thread for x ms
		/// </summary>
		/// <param name="lower">
		///     min time
		/// </param>
		/// <param name="upper">
		///     max time
		/// </param>
		public static void RandomDelay(int lower = 10, int upper = 30)
		{
			Delay(RandomInt(lower, upper));
		}

		/// <summary>
		///     pause the current thread for x ms
		/// </summary>
		/// <param name="x">
		///     The x.
		/// </param>
		public static void Delay(int x = 1000)
		{
			Thread.Sleep(x);
		}

		/// <summary>
		///     create a random integer between lower and upper
		/// </summary>
		/// <param name="lower">
		///     min number
		/// </param>
		/// <param name="upper">
		///     max number
		/// </param>
		/// <returns>
		///     random integer
		/// </returns>
		public static int RandomInt(int lower, int upper)
		{
			return Random.Next(lower, upper);
		}

		/// <summary>
		///     create a random double between lower and upper
		/// </summary>
		/// <param name="lower">
		///     min number
		/// </param>
		/// <param name="upper">
		///     max number
		/// </param>
		/// <returns>
		///     random double
		/// </returns>
		public static double RandomDouble(double lower, double upper)
		{
			return (Random.NextDouble() * (upper - lower)) + lower;
		}
	}
}
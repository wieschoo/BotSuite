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
	///     Common utility functions
	/// </summary>
	public class Utility
	{
		/// <summary>
		/// The random number generator.
		/// </summary>
		private static readonly Random _random = new Random();

		/// <summary>
		///     Pause the current thread for a random amount of time between <paramref name="lower"/> and <paramref name="upper"/>.
		/// </summary>
		/// <param name="lower">
		///     Minimum pause time.
		/// </param>
		/// <param name="upper">
		///     Maximum pause time.
		/// </param>
		public static void RandomDelay(int lower = 10, int upper = 30)
		{
			Utility.Delay(Utility.RandomInt(lower, upper));
		}

		/// <summary>
		///     Pause the current thread for a specified amount of time.
		/// </summary>
		/// <param name="x">
		///     The pause time.
		/// </param>
		public static void Delay(int x = 1000)
		{
			Thread.Sleep(x);
		}

		/// <summary>
		///     Create a random integer between <paramref name="lower"/> and <paramref name="upper"/>.
		/// </summary>
		/// <param name="lower">
		///     The lower bound for the random number.
		/// </param>
		/// <param name="upper">
		///     The upper bound for the random number.
		/// </param>
		/// <returns>
		///     A random integer between <paramref name="lower"/> and <paramref name="upper"/>.
		/// </returns>
		public static int RandomInt(int lower, int upper)
		{
			return Utility._random.Next(lower, upper);
		}

		/// <summary>
		///     Create a random double between <paramref name="lower"/> and <paramref name="upper"/>.
		/// </summary>
		/// <param name="lower">
		///     The lower bound for the random number.
		/// </param>
		/// <param name="upper">
		///     The upper bound for the random number.
		/// </param>
		/// <returns>
		///     A random double between <paramref name="lower"/> and <paramref name="upper"/>.
		/// </returns>
		public static double RandomDouble(double lower, double upper)
		{
			return (Utility._random.NextDouble() * (upper - lower)) + lower;
		}
	}
}
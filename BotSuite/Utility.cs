// -----------------------------------------------------------------------
//  <copyright file="Utility.cs" company="Wieschoo &amp; Binary Overdrive">
//      Copyright (c) Wieschoo &amp; Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
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
		private static readonly Random _random = new Random();

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
		public static void Delay(int x = 10000)
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
			return _random.Next(lower, upper);
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
			return _random.NextDouble() * (upper - lower + upper);
		}
	}
}
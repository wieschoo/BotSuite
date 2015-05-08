// -----------------------------------------------------------------------
//  <copyright file="UtilityTests.cs" company="Binary Overdrive">
//      Copyright (c) Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>Framework for creating automation applications.</purpose>
//  <homepage>https://bitbucket.org/KarillEndusa/botsuite.net</homepage>
//  <license>https://bitbucket.org/KarillEndusa/botsuite.net/wiki/license</license>
// -----------------------------------------------------------------------


namespace BotSuite.Tests
{
	using System.Diagnostics;
	using NUnit.Framework;

	/// <summary>
	/// The utility.
	/// </summary>
	[TestFixture]
	public class UtilityTests
	{
		/// <summary>
		/// The delaying for one second_ should stop thread for one second.
		/// </summary>
		[Test]
		public void DelayingForOneSecond_ShouldStopThreadForOneSecond()
		{
			Stopwatch stopwatch = new Stopwatch();

			stopwatch.Start();
			Utility.Delay();
			stopwatch.Stop();

			Assert.IsTrue(stopwatch.ElapsedMilliseconds > 990 && stopwatch.ElapsedMilliseconds < 1010, "Expected true.");
		}

		/// <summary>
		/// The random delay between numbers_ should delay between numbers.
		/// </summary>
		[Test]
		public void RandomDelayBetweenNumbers_ShouldDelayBetweenNumbers()
		{
			Stopwatch stopwatch = new Stopwatch();

			stopwatch.Start();
			Utility.RandomDelay();
			stopwatch.Stop();

			Assert.IsTrue(stopwatch.ElapsedMilliseconds > 8 && stopwatch.ElapsedMilliseconds < 32, "Expected true.");

			stopwatch.Reset();
		}

		/// <summary>
		/// The creating random integer between numbers_ should return integer between numbers.
		/// </summary>
		[Test]
		public void CreatingRandomIntegerBetweenNumbers_ShouldReturnIntegerBetweenNumbers()
		{
			int randomInt = Utility.RandomInt(0, 10);
			Assert.IsTrue(randomInt >= 0 && randomInt <= 10, "randomInt > 0 && randomInt < 10");
		}

		/// <summary>
		/// The creating random double between numbers_ should return double between numbers.
		/// </summary>
		[Test]
		public void CreatingRandomDoubleBetweenNumbers_ShouldReturnDoubleBetweenNumbers()
		{
			double randomDouble = Utility.RandomDouble(0, 10);
			Assert.IsTrue(randomDouble >= 0.0 && randomDouble <= 10.00, "randomDouble > 0 && randomDouble < 10");
		}
	}
}

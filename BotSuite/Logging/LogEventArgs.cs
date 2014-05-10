// -----------------------------------------------------------------------
//  <copyright file="LogEventArgs.cs" company="Wieschoo &amp; Binary Overdrive">
//      Copyright (c) Wieschoo &amp; Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite.Logging
{
	using System;

	/// <summary>
	///     The log event args.
	/// </summary>
	public class LogEventArgs : EventArgs
	{
		/// <summary>
		///     Gets the log message.
		/// </summary>
		public string LogMessage { get; private set; }

		/// <summary>
		///     Initializes a new instance of the <see cref="LogEventArgs" /> class.
		/// </summary>
		/// <param name="logMessage">
		///     The log Message.
		/// </param>
		public LogEventArgs(string logMessage)
		{
			this.LogMessage = logMessage;
		}
	}
}
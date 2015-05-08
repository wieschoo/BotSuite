// -----------------------------------------------------------------------
//  <copyright file="LogEventArgs.cs" company="Binary Overdrive">
//      Copyright (c) Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>Framework for creating automation applications.</purpose>
//  <homepage>https://bitbucket.org/KarillEndusa/botsuite.net</homepage>
//  <license>https://bitbucket.org/KarillEndusa/botsuite.net/wiki/license</license>
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
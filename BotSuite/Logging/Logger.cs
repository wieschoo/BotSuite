// -----------------------------------------------------------------------
//  <copyright file="Logger.cs" company="Wieschoo &amp; Binary Overdrive">
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
	///     A simple logger for the BotSuite
	/// </summary>
	public class Logger
	{
		/// <summary>
		///     The log.
		/// </summary>
		public event EventHandler<LogEventArgs> Log;

		/// <summary>
		///     The instance logger.
		/// </summary>
		private static Logger instanceLogger;

		/// <summary>
		///     Prevents a default instance of the <see cref="Logger" /> class from being created.
		/// </summary>
		private Logger()
		{
		}

		/// <summary>
		///     Logs the exception.
		/// </summary>
		/// <param name="exception">
		///     The exception.
		/// </param>
		public static void LogException(Exception exception)
		{
			AddLogEntryInternal(string.Format("{0}: {1}", exception.GetType(), exception.Message));
		}

		/// <summary>
		///     The add log entry internal.
		/// </summary>
		/// <param name="logEntry">
		///     The log entry.
		/// </param>
		private static void AddLogEntryInternal(string logEntry)
		{
			if (instanceLogger == null)
			{
				instanceLogger = new Logger();
			}

			instanceLogger.AddLogEntry(logEntry);
		}

		/// <summary>
		///     The add log entry.
		/// </summary>
		/// <param name="logEntry">
		///     The log entry.
		/// </param>
		public void AddLogEntry(string logEntry)
		{
			this.OnLog(new LogEventArgs(logEntry));
		}

		/// <summary>
		///     The on log.
		/// </summary>
		/// <param name="e">
		///     The e.
		/// </param>
		protected virtual void OnLog(LogEventArgs e)
		{
			EventHandler<LogEventArgs> handler = this.Log;
			if (handler != null)
			{
				handler(this, e);
			}
		}
	}
}
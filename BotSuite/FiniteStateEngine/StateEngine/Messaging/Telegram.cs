// -----------------------------------------------------------------------
//  <copyright file="Telegram.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite.FiniteStateEngine.StateEngine.Messaging
{
	using System;

	/// <summary>
	///     contains all information of a telegram
	/// </summary>
	public struct Telegram
	{
		/// <summary>
		///     id of sender
		/// </summary>
		public int Sender;

		/// <summary>
		///     id of receiver
		/// </summary>
		public int Reciever;

		/// <summary>
		///     id of message
		/// </summary>
		public int Message;

		/// <summary>
		///     time window when message should be send
		/// </summary>
		public DateTime DispatchTime;

		/// <summary>
		///     place for additional information or parameter
		/// </summary>
		public object AdditionalInfo;

		/// <summary>
		///     Initializes a new instance of the <see cref="Telegram" /> struct.
		///     construct a telegram
		/// </summary>
		/// <param name="sender">
		///     id of sender
		/// </param>
		/// <param name="reciever">
		///     id of receiver
		/// </param>
		/// <param name="message">
		///     id of message
		/// </param>
		/// <param name="dispatchTime">
		///     time window when message should be send
		/// </param>
		/// <param name="additionalInfo">
		///     place for additional information or parameter
		/// </param>
		public Telegram(int sender, int reciever, int message, DateTime dispatchTime, object additionalInfo)
		{
			this.Sender = sender;
			this.Reciever = reciever;
			this.Message = message;
			this.DispatchTime = dispatchTime;
			this.AdditionalInfo = additionalInfo;
		}
	}
}
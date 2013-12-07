// -----------------------------------------------------------------------
//  <copyright file="MessageDispatcher.cs" company="HoovesWare">
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
	using System.Collections.Generic;

	using global::BotSuite.FiniteStateEngine.Pattern;
	using global::BotSuite.FiniteStateEngine.StateEngine.Entites;

	/// <summary>
	///     class to handle messages
	/// </summary>
	public class MessageDispatcher
	{
		/// <summary>
		///     list of messages
		/// </summary>
		private readonly List<Telegram> lstDelayedMessages;

		/// <summary>
		///     discharge message
		/// </summary>
		/// <param name="reciever">
		/// </param>
		/// <param name="message">
		/// </param>
		private static void DischargeMessage(BaseEntity reciever, Telegram message)
		{
			if (!reciever.HandleMessage(message))
			{
				throw new Exception("No message handler for this message found!");
			}
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="MessageDispatcher" /> class.
		///     create new message dispatcher
		/// </summary>
		public MessageDispatcher()
		{
			this.lstDelayedMessages = new List<Telegram>();
		}

		/// <summary>
		///     dispatch a message
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
		/// <param name="delay">
		///     time to wait
		/// </param>
		/// <param name="additionalInfo">
		///     place for additional information or parameter
		/// </param>
		public void DispatchMessage(int sender, int reciever, int message, double delay, object additionalInfo)
		{
			Telegram telegram = new Telegram(sender, reciever, message, DateTime.Now.AddSeconds(delay), additionalInfo);

			if ((int)delay != 0)
			{
				this.lstDelayedMessages.Add(telegram);
			}
			else
			{
				DischargeMessage(Singleton<EntityManager>.Instance.GetEntityById(telegram.Reciever), telegram);
			}
		}

		/// <summary>
		///     handle messages
		/// </summary>
		public void HandleDelayedMessages()
		{
			for (int i = 0; i < this.lstDelayedMessages.Count; i++)
			{
				if (this.lstDelayedMessages[i].DispatchTime.CompareTo(DateTime.Now) >= 0)
				{
					continue;
				}

				DischargeMessage(
					Singleton<EntityManager>.Instance.GetEntityById(this.lstDelayedMessages[i].Reciever),
					this.lstDelayedMessages[i]);
				this.lstDelayedMessages.RemoveAt(i);

				i = 0;
			}
		}
	}
}
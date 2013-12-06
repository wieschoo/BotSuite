// -----------------------------------------------------------------------
//  <copyright file="BaseEntity.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite.FiniteStateEngine.StateEngine.Entites
{
	using global::BotSuite.FiniteStateEngine.StateEngine.Messaging;

	/// <summary>
	///     The base entity.
	/// </summary>
	public abstract class BaseEntity
	{
		/// <summary>
		///     The id.
		/// </summary>
		public int Id;

		/// <summary>
		///     The update.
		/// </summary>
		public abstract void Update();

		/// <summary>
		///     The handle message.
		/// </summary>
		/// <param name="message">
		///     The message.
		/// </param>
		/// <returns>
		///     The <see cref="bool" />.
		/// </returns>
		public abstract bool HandleMessage(Telegram message);
	}
}
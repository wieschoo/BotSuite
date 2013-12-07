// -----------------------------------------------------------------------
//  <copyright file="State.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite.FiniteStateEngine.StateEngine
{
	using global::BotSuite.FiniteStateEngine.StateEngine.Messaging;

	// Eine abstrakte Klasse welche einen Zustand repräsentiert
	/// <summary>
	///     The state.
	/// </summary>
	/// <typeparam name="T">
	/// </typeparam>
	public abstract class State<T>
		where T : class
	{
		// Jeder Zustand bekommt einen eindeutigen Namen
		/// <summary>
		///     The name.
		/// </summary>
		public string Name = typeof(T).Name + "_NOT_DEFINED";

		// Eine abstrakte Funktion welche das Verhalten des Zustands beinhaltet
		/// <summary>
		///     The enter.
		/// </summary>
		/// <param name="parent">
		///     The parent.
		/// </param>
		public abstract void Enter(T parent);

		/// <summary>
		///     The run.
		/// </summary>
		/// <param name="parent">
		///     The parent.
		/// </param>
		public abstract void Run(T parent);

		/// <summary>
		///     The leave.
		/// </summary>
		/// <param name="parent">
		///     The parent.
		/// </param>
		public abstract void Leave(T parent);

		/// <summary>
		///     The on message.
		/// </summary>
		/// <param name="sender">
		///     The sender.
		/// </param>
		/// <param name="message">
		///     The message.
		/// </param>
		/// <returns>
		///     The <see cref="bool" />.
		/// </returns>
		public abstract bool OnMessage(T sender, Telegram message);
	}
}
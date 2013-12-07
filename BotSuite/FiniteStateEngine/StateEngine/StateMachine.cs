// -----------------------------------------------------------------------
//  <copyright file="StateMachine.cs" company="HoovesWare">
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

	/// <summary>
	///     The state machine.
	/// </summary>
	/// <typeparam name="T">
	/// </typeparam>
	public class StateMachine<T>
		where T : class
	{
		// Wenn wir zwischen vorherigem und aktuellem State umschalten wollten ganz nützlich

		/// <summary>
		///     The current state.
		/// </summary>
		private State<T> currentState;

		/// <summary>
		///     The global state.
		/// </summary>
		private State<T> globalState;

		// Der Besitzer der Stateengine und somit auch der Zustände
		/// <summary>
		///     The state machine owner.
		/// </summary>
		private readonly T stateMachineOwner;

		// Der Konstruktur setzt einfach den Parent vom generischen Type T
		/// <summary>
		///     Initializes a new instance of the <see cref="StateMachine{T}" /> class.
		/// </summary>
		/// <param name="parent">
		///     The parent.
		/// </param>
		public StateMachine(T parent)
		{
			this.stateMachineOwner = parent;
		}

		// Ändert den aktuellen Zustand in einen neuen
		// Der zweite Parameter gibt an ob der neue Zustand aufgerufen werden soll
		/// <summary>
		///     The change state.
		/// </summary>
		/// <param name="newState">
		///     The new state.
		/// </param>
		/// <param name="runNewState">
		///     The run new state.
		/// </param>
		public void ChangeState(State<T> newState, bool runNewState)
		{
			if (this.currentState != null)
			{
				this.currentState.Leave(this.stateMachineOwner);
			}

			this.currentState = newState;

			this.currentState.Enter(this.stateMachineOwner);

			if (runNewState)
			{
				this.RunCurrentState();
			}
		}

		// Ändert den globalen State, der unanhängig von CurrentState ist
		/// <summary>
		///     The change global state.
		/// </summary>
		/// <param name="newState">
		///     The new state.
		/// </param>
		/// <param name="runNewGlobalState">
		///     The run new global state.
		/// </param>
		public void ChangeGlobalState(State<T> newState, bool runNewGlobalState)
		{
			if (this.globalState != null)
			{
				this.globalState.Leave(this.stateMachineOwner);
			}

			this.globalState = newState;

			this.globalState.Enter(this.stateMachineOwner);

			if (runNewGlobalState)
			{
				this.globalState.Run(this.stateMachineOwner);
			}
		}

		// Ruft den aktuell gesetzen Zustand auf
		/// <summary>
		///     The run current state.
		/// </summary>
		public void RunCurrentState()
		{
			if (this.currentState != null)
			{
				this.currentState.Run(this.stateMachineOwner);
			}

			if (this.globalState != null)
			{
				this.globalState.Run(this.stateMachineOwner);
			}
		}

		/// <summary>
		///     The handle message.
		/// </summary>
		/// <param name="message">
		///     The message.
		/// </param>
		/// <returns>
		///     The <see cref="bool" />.
		/// </returns>
		public bool HandleMessage(Telegram message)
		{
			// Prüfen ob der aktuelle State mit der Message umgehen kann, wenn nicht...
			if (this.currentState != null && this.currentState.OnMessage(this.stateMachineOwner, message))
			{
				return true;
			}

			// Prüfen on der globale State mit der Message umgehen kann
			if (this.globalState != null && this.globalState.OnMessage(this.stateMachineOwner, message))
			{
				return true;
			}

			return false;
		}
	}
}
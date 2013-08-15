using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BotSuite.FiniteStateEngine.StateEngine.Messaging;

namespace BotSuite.FiniteStateEngine.StateEngine
{
    public class StateMachine<T> where T : class
    {
        // Wenn wir zwischen vorherigem und aktuellem State umschalten wollten ganz nützlich
        private State<T> PreviousState;
        private State<T> CurrentState;
        private State<T> GlobalState;

        // Der Besitzer der Stateengine und somit auch der Zustände
        private T StateMachineOwner;

        // Der Konstruktur setzt einfach den Parent vom generischen Type T
        public StateMachine(T Parent)
        {
            this.StateMachineOwner = Parent;
        }

        // Ändert den aktuellen Zustand in einen neuen
        // Der zweite Parameter gibt an ob der neue Zustand aufgerufen werden soll
        public void ChangeState(State<T> NewState, bool RunNewState)
        {
            if (this.CurrentState != null)
                this.CurrentState.Leave(StateMachineOwner);

            this.PreviousState = CurrentState;
            this.CurrentState = NewState;

            this.CurrentState.Enter(StateMachineOwner);

            if (RunNewState)
                this.RunCurrentState();
        }

        // Ändert den globalen State, der unanhängig von CurrentState ist
        public void ChangeGlobalState(State<T> NewState, bool RunNewGlobalState)
        {
            if (this.GlobalState != null)
                GlobalState.Leave(StateMachineOwner);

            this.GlobalState = NewState;

            GlobalState.Enter(StateMachineOwner);

            if (RunNewGlobalState)
                GlobalState.Run(StateMachineOwner);
        }

        // Ruft den aktuell gesetzen Zustand auf
        public void RunCurrentState()
        {
            if (this.CurrentState != null)
                this.CurrentState.Run(StateMachineOwner);

            if (this.GlobalState != null)
                this.GlobalState.Run(StateMachineOwner);
        }

        public bool HandleMessage(Telegram Message)
        {
            // Prüfen ob der aktuelle State mit der Message umgehen kann, wenn nicht...
            if (CurrentState != null && CurrentState.OnMessage(this.StateMachineOwner, Message))
                return true;

            // Prüfen on der globale State mit der Message umgehen kann
            if (GlobalState != null && GlobalState.OnMessage(this.StateMachineOwner, Message))
                return true;

            return false;
        }
    }
}

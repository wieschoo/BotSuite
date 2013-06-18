using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BotSuite.FiniteStateEngine.StateEngine.Messaging;

namespace BotSuite.FiniteStateEngine.StateEngine
{
    // Eine abstrakte Klasse welche einen Zustand repräsentiert
    public abstract class State<T> where T : class
    {
        // Jeder Zustand bekommt einen eindeutigen Namen
        public String Name = typeof(T).Name + "_NOT_DEFINED";

        // Eine abstrakte Funktion welche das Verhalten des Zustands beinhaltet
        public abstract void Enter(T Parent);
        public abstract void Run(T Parent);
        public abstract void Leave(T Parent);

        public abstract bool OnMessage(T Sender, Telegram Message);
    }
}

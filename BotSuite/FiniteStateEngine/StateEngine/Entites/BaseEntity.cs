using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BotSuite.FiniteStateEngine.StateEngine.Messaging;

namespace BotSuite.FiniteStateEngine.StateEngine.Entites
{
    public abstract class BaseEntity
    {
        public Int32 ID;

        public abstract void Update();
        public abstract bool HandleMessage(Telegram Message);
    }
}

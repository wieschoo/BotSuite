using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotSuite.FiniteStateEngine.StateEngine.Messaging
{
    public struct Telegram
    {
        public Int32 Sender; // ID des Senders
        public Int32 Reciever; // ID des Empfängers
        public Int32 Message; // ID der Message 

        public DateTime DispatchTime; // Zeit, in der die Message abgeschickt werden sollte
        public Object AdditionalInfo; // Zusätzlicher Parameter

        public Telegram(Int32 _Sender, Int32 _Reciever, Int32 _Message, DateTime _DispatchTime, Object _AdditionalInfo)
        {
            this.Sender = _Sender;
            this.Reciever = _Reciever;
            this.Message = _Message;
            this.DispatchTime = _DispatchTime;
            this.AdditionalInfo = _AdditionalInfo;
        }
    }
}

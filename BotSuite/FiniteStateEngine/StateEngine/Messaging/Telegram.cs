using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotSuite.FiniteStateEngine.StateEngine.Messaging
{
    /// <summary>
    /// contains all information of a telegram
    /// </summary>
    public struct Telegram
    {
        /// <summary>
        /// id of sender
        /// </summary>
        public Int32 Sender;
        /// <summary>
        /// id of receiver
        /// </summary>
        public Int32 Reciever;
        /// <summary>
        /// id of message
        /// </summary>
        public Int32 Message;

        /// <summary>
        /// time window when message should be send
        /// </summary>
        public DateTime DispatchTime;
        /// <summary>
        /// place for additional information or parameter
        /// </summary>
        public Object AdditionalInfo;

        /// <summary>
        /// construct a telegram
        /// </summary>
        /// <param name="_Sender">id of sender</param>
        /// <param name="_Reciever">id of receiver</param>
        /// <param name="_Message">id of message</param>
        /// <param name="_DispatchTime">time window when message should be send</param>
        /// <param name="_AdditionalInfo">place for additional information or parameter</param>
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

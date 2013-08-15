using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BotSuite.FiniteStateEngine.StateEngine.Entites;

namespace BotSuite.FiniteStateEngine.StateEngine.Messaging
{
    /// <summary>
    /// class to handle messages
    /// </summary>
    public class MessageDispatcher
    {
        /// <summary>
        /// list of messages
        /// </summary>
        private List<Telegram> lstDelayedMessages;

        /// <summary>
        /// discharge message
        /// </summary>
        /// <param name="Reciever"></param>
        /// <param name="Message"></param>
        private void DischargeMessage(BaseEntity Reciever, Telegram Message)
        {
            if (!Reciever.HandleMessage(Message))
            {
                throw new Exception("No message handler for this message found!");
            }
        }
        /// <summary>
        /// create new message dispatcher
        /// </summary>
        public MessageDispatcher()
        {
            lstDelayedMessages = new List<Telegram>();
        }
        /// <summary>
        /// dispatch a message
        /// </summary>
        /// <param name="_Sender">id of sender</param>
        /// <param name="_Reciever">id of receiver</param>
        /// <param name="_Message">id of message</param>
        /// <param name="Delay">time to wait</param>
        /// <param name="_AdditionalInfo">place for additional information or parameter</param>
        public void DispatchMessage(Int32 _Sender, Int32 _Reciever, Int32 _Message, double Delay, Object _AdditionalInfo)
        {
            Messaging.Telegram Message = new Telegram(_Sender, _Reciever, _Message, DateTime.Now.AddSeconds(Delay), _AdditionalInfo);

            if (Delay != 0)
            {
                this.lstDelayedMessages.Add(Message);
            }
            else
            {
                DischargeMessage(Pattern.Singleton<EntityManager>.Instance.GetEntityByID(Message.Reciever), Message);
            }
        }
        /// <summary>
        /// handle messages
        /// </summary>
        public void HandleDelayedMessages()
        {
            for (int i = 0; i < lstDelayedMessages.Count; i++)
            {
                if (lstDelayedMessages[i].DispatchTime.CompareTo(DateTime.Now) < 0)
                {
                    DischargeMessage(Pattern.Singleton<EntityManager>.Instance.GetEntityByID(lstDelayedMessages[i].Reciever), lstDelayedMessages[i]);
                    this.lstDelayedMessages.RemoveAt(i);

                    i = 0;
                }
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BotSuite.FiniteStateEngine.StateEngine.Entites;

namespace BotSuite.FiniteStateEngine.StateEngine.Messaging
{
    public class MessageDispatcher
    {
        private List<Telegram> lstDelayedMessages;

        private void DischargeMessage(BaseEntity Reciever, Telegram Message)
        {
            if (!Reciever.HandleMessage(Message))
            {
                throw new Exception("No message handler for this message found!");
            }
        }

        public MessageDispatcher()
        {
            lstDelayedMessages = new List<Telegram>();
        }

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

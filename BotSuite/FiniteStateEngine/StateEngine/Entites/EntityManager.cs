using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotSuite.FiniteStateEngine.StateEngine.Entites
{
    public class EntityManager
    {
        private Dictionary<Int32, BaseEntity> dicEntites;

        public EntityManager()
        {
            dicEntites = new Dictionary<int, BaseEntity>();
        }

        public void RegisterEntity(BaseEntity Entity)
        {
            dicEntites.Add(Entity.ID, Entity);
        }

        public void RemoveEntity(BaseEntity Entity)
        {
            if (dicEntites.ContainsKey(Entity.ID))
                dicEntites.Remove(Entity.ID);
        }

        public BaseEntity GetEntityByID(Int32 ID)
        {
            if (dicEntites.ContainsKey(ID))
                return dicEntites[ID];

            return null;
        }
    }
}

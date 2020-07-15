using System;

namespace Egoal.Events.Bus.Entities
{
    [Serializable]
    public class EntityEventData<TEntity> : EventData, IEventDataWithInheritableGenericArgument
    {
        public TEntity Entity { get; private set; }

        public EntityEventData(TEntity entity)
        {
            Entity = entity;
        }

        public virtual object[] GetConstructorArgs()
        {
            return new object[] { Entity };
        }
    }
}

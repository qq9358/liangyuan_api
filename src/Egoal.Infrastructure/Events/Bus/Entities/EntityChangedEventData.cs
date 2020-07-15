using System;

namespace Egoal.Events.Bus.Entities
{
    [Serializable]
    public class EntityChangedEventData<TEntity> : EntityEventData<TEntity>
    {
        public EntityChangedEventData(TEntity entity)
            : base(entity)
        {

        }
    }
}

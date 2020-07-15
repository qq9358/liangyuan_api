using System;

namespace Egoal.Events.Bus.Entities
{
    [Serializable]
    public class EntityCreatedEventData<TEntity> : EntityChangedEventData<TEntity>
    {
        public EntityCreatedEventData(TEntity entity)
            : base(entity)
        {

        }
    }
}

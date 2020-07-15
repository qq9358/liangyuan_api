using System;

namespace Egoal.Events.Bus.Entities
{
    [Serializable]
    public class EntityDeletedEventData<TEntity> : EntityChangedEventData<TEntity>
    {
        public EntityDeletedEventData(TEntity entity)
           : base(entity)
        {

        }
    }
}

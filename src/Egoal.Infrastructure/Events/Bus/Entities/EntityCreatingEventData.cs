using System;

namespace Egoal.Events.Bus.Entities
{
    [Serializable]
    public class EntityCreatingEventData<TEntity> : EntityChangingEventData<TEntity>
    {
        public EntityCreatingEventData(TEntity entity)
            : base(entity)
        {

        }
    }
}

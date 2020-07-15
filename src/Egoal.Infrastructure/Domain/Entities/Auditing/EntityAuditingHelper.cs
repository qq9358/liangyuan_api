using Egoal.Extensions;
using System;

namespace Egoal.Domain.Entities.Auditing
{
    public static class EntityAuditingHelper
    {
        public static void SetCreationAuditProperties(object entityAsObj, int? userId)
        {
            if (!(entityAsObj is IHasCreationTime))
            {
                return;
            }

            var entityWithCreationTime = entityAsObj as IHasCreationTime;
            if (entityWithCreationTime.CTime == default(DateTime))
            {
                entityWithCreationTime.CTime = DateTime.Now;
            }

            if (!(entityAsObj is ICreationAudited))
            {
                return;
            }

            if (!userId.HasValue)
            {
                return;
            }

            var entity = entityAsObj as ICreationAudited;
            if (entity.CID.HasValue)
            {
                return;
            }

            entity.CID = userId;
        }

        public static void SetModificationAuditProperties(object entityAsObj, int? userId)
        {
            if (entityAsObj is IHasModificationTime)
            {
                entityAsObj.As<IHasModificationTime>().MTime = DateTime.Now;
            }

            if (entityAsObj is IModificationAudited)
            {
                entityAsObj.As<IModificationAudited>().MID = userId;
            }
        }

        public static void SetDeletionAuditProperties(object entityAsObj, int? userId)
        {
            if (entityAsObj is IHasDeletionTime)
            {
                var entity = entityAsObj.As<IHasDeletionTime>();

                if (entity.DeletionTime == null)
                {
                    entity.DeletionTime = DateTime.Now;
                }
            }

            if (entityAsObj is IDeletionAudited)
            {
                var entity = entityAsObj.As<IDeletionAudited>();

                if (entity.DeleterUserId.HasValue)
                {
                    return;
                }

                entity.DeleterUserId = userId;
            }
        }
    }
}

﻿using System;

namespace Egoal.Domain.Entities
{
    public class EntityTypeInfo
    {
        public Type EntityType { get; private set; }
        public Type DeclaringType { get; private set; }

        public EntityTypeInfo(Type entityType, Type declaringType)
        {
            EntityType = entityType;
            DeclaringType = declaringType;
        }
    }
}

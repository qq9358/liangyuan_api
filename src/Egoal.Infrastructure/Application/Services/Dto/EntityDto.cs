using System;

namespace Egoal.Application.Services.Dto
{
    [Serializable]
    public class EntityDto<TPrimaryKey> : IEntityDto<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }

        public EntityDto()
        {

        }

        public EntityDto(TPrimaryKey id)
        {
            Id = id;
        }
    }

    [Serializable]
    public class EntityDto : EntityDto<int>, IEntityDto
    {
        public EntityDto()
        {

        }

        public EntityDto(int id)
            : base(id)
        {
        }
    }
}

namespace Egoal.Application.Services.Dto
{
    public interface IEntityDto<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }

    public interface IEntityDto : IEntityDto<int>
    {

    }
}

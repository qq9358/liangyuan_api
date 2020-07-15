namespace Egoal.Domain.Entities.Auditing
{
    public interface ICreationAudited : IHasCreationTime
    {
        int? CID { get; set; }
    }
}

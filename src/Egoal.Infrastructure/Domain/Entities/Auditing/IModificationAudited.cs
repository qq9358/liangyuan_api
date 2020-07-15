namespace Egoal.Domain.Entities.Auditing
{
    public interface IModificationAudited : IHasModificationTime
    {
        int? MID { get; set; }
    }
}

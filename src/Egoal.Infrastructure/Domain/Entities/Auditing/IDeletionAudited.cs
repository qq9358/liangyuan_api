namespace Egoal.Domain.Entities.Auditing
{
    public interface IDeletionAudited : IHasDeletionTime
    {
        int? DeleterUserId { get; set; }
    }
}

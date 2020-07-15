namespace Egoal.Application.Services.Dto
{
    public interface IPagedResultRequest : ILimitedResultRequest
    {
        int SkipCount { get; set; }
    }
}

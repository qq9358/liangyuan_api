namespace Egoal.Application.Services.Dto
{
    public interface ILimitedResultRequest
    {
        int MaxResultCount { get; set; }
    }
}

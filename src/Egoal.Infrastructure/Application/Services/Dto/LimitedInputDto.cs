using System.ComponentModel.DataAnnotations;

namespace Egoal.Application.Services.Dto
{
    public class LimitedInputDto : ILimitedResultRequest
    {
        [Range(1, int.MaxValue)]
        public virtual int MaxResultCount { get; set; } = 20;
    }
}

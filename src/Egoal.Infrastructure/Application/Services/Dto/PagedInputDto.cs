using System.ComponentModel.DataAnnotations;

namespace Egoal.Application.Services.Dto
{
    public class PagedInputDto : LimitedInputDto, IPagedResultRequest
    {
        [Range(0, int.MaxValue)]
        public virtual int SkipCount { get; set; }
        public bool ShouldPage { get; set; } = true;
        public int StartRowNum { get { return SkipCount + 1; } }
        public int EndRowNum { get { return SkipCount + MaxResultCount; } }
    }
}

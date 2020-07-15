using System;
using System.Collections.Generic;

namespace Egoal.Application.Services.Dto
{
    [Serializable]
    public class PagedResultDto<T> : ListResultDto<T>, IPagedResult<T>
    {
        public int TotalCount { get; set; }

        public PagedResultDto()
        {

        }

        public PagedResultDto(int totalCount, IList<T> items)
            : base(items)
        {
            TotalCount = totalCount;
        }
    }
}

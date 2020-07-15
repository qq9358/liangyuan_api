using System.Collections.Generic;

namespace Egoal.Application.Services.Dto
{
    public interface IListResult<T>
    {
        IList<T> Items { get; set; }
    }
}

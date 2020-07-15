using System;
using System.Collections.Generic;

namespace Egoal.Application.Services.Dto
{
    [Serializable]
    public class ListResultDto<T> : IListResult<T>
    {
        public IList<T> Items
        {
            get { return _items ?? (_items = new List<T>()); }
            set { _items = value; }
        }
        private IList<T> _items;

        public ListResultDto()
        {

        }

        public ListResultDto(IList<T> items)
        {
            Items = items;
        }
    }
}

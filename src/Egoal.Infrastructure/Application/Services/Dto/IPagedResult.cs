﻿namespace Egoal.Application.Services.Dto
{
    public interface IPagedResult<T> : IListResult<T>, IHasTotalCount
    {

    }
}

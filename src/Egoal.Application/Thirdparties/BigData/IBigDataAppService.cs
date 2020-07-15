using Egoal.Thirdparties.BigData.Dto;
using System;
using System.Threading.Tasks;

namespace Egoal.Thirdparties.BigData
{
    public interface IBigDataAppService
    {
        Task<ResponseDto> ProcessRequestAsync(RequestDto request);
        Task AddApiLogAsync(RequestDto request, Exception ex = null);
    }
}

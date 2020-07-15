using Egoal.Application.Services.Dto;
using System;

namespace Egoal.Stadiums.Dto
{
    public class SeatForSaleDto : EntityDto<long>
    {
        public decimal? StatusCacheId { get; set; }
        public SeatStatus? StatusID { get; set; }
        public DateTime? LockTime { get; set; }
    }
}

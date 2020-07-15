using Egoal.Application.Services.Dto;

namespace Egoal.Common.Dto
{
    public class ChangCiDto : EntityDto
    {
        public string Name { get; set; }
        public string SortCode { get; set; }
        public int? CcTypeId { get; set; }
        public string Stime { get; set; }
        public string Etime { get; set; }
        public int? Minutes { get; set; }
    }
}

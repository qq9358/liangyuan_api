using Egoal.Domain.Entities;

namespace Egoal.Scenics
{
    public class Scenic : Entity
    {
        public string Language { get; set; }
        public string ScenicName { get; set; }
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
        public string OpeningHours { get; set; }
        public string Photos { get; set; }
        public string ScenicIntro { get; set; }
        public string ScenicFeature { get; set; }
        public string BookNotes { get; set; }
        public string NoticeTitle { get; set; }
        public string NoticeContent { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string Address { get; set; }
        public string ContactInfo { get; set; }

        /// <summary>
        /// 首页链接
        /// </summary>
        public string IndexLink { get; set; }

        /// <summary>
        /// 免票政策
        /// </summary>
        public string FreeTicketPolicy { get; set; }

        /// <summary>
        /// 优惠政策
        /// </summary>
        public string FavouredPolicy { get; set; }

        /// <summary>
        /// 开具发票
        /// </summary>
        public string Invoicing { get; set; }

        /// <summary>
        /// 重要说明
        /// </summary>
        public string ImportantNote { get; set; }

        /// <summary>
        /// 安全须知
        /// </summary>
        public string SafetyInstructions { get; set; }
    }
}

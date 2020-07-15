using Egoal.Annotations;
using Egoal.Application.Services.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Scenics.Dto
{
    public class ScenicDto : EntityDto
    {
        public ScenicDto()
        {
            PhotoList = new List<PhotoDto>();
        }

        [Display(Name = "语言")]
        [MaximumLength(10)]
        public string Language { get; set; }

        [Display(Name = "景区名称")]
        [MustFillIn]
        [MaximumLength(50)]
        public string ScenicName { get; set; }

        [Display(Name = "开园时间")]
        [MaximumLength(5)]
        public string OpenTime { get; set; }

        [Display(Name = "闭园时间")]
        [MaximumLength(5)]
        public string CloseTime { get; set; }

        public string OpeningHours { get; set; }

        public List<PhotoDto> PhotoList { get; set; }

        [Display(Name = "景区简介")]
        [MustFillIn]
        public string ScenicIntro { get; set; }
        public string ScenicFeature { get; set; }
        public string BookNotes { get; set; }

        [Display(Name = "公告标题")]
        [MaximumLength(50)]
        public string NoticeTitle { get; set; }
        public string NoticeContent { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }

        [Display(Name = "景区地址")]
        [MaximumLength(200)]
        public string Address { get; set; }
        public string ContactInfo { get; set; }

        public string WxSubscribeUrl { get; set; }

        [Display(Name = "首页链接")]
        [MaximumLength(500)]
        /// <summary>
        /// 首页链接
        /// </summary>
        public string IndexLink { get; set; }

        [Display(Name = "免票政策")]
        [MaximumLength(500)]
        /// <summary>
        /// 免票政策
        /// </summary>
        public string FreeTicketPolicy { get; set; }

        [Display(Name = "优惠政策")]
        [MaximumLength(500)]
        /// <summary>
        /// 优惠政策
        /// </summary>
        public string FavouredPolicy { get; set; }

        [Display(Name = "开具发票")]
        [MaximumLength(500)]
        /// <summary>
        /// 开具发票
        /// </summary>
        public string Invoicing { get; set; }

        [Display(Name = "重要说明")]
        [MaximumLength(500)]
        /// <summary>
        /// 重要说明
        /// </summary>
        public string ImportantNote { get; set; }

        [Display(Name = "安全须知")]
        [MaximumLength(500)]
        /// <summary>
        /// 安全须知
        /// </summary>
        public string SafetyInstructions { get; set; }
    }

    public class PhotoDto
    {
        public string Name { get; set; }
        public long Uid { get; set; }
        public string Url { get; set; }
    }
}

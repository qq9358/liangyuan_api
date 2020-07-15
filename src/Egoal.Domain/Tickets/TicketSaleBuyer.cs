using Egoal.Domain.Entities;
using Egoal.Extensions;
using Egoal.Members;
using System;

namespace Egoal.Tickets
{
    public class TicketSaleBuyer : Entity<long>
    {
        public long? TicketId { get; set; }
        public Guid? TradeId { get; set; }
        public string OrderListNo { get; set; }
        public int? CertTypeId { get; set; }
        public string CertTypeName { get; set; }
        public string CertNo { get; set; }
        public string BuyerName { get; set; }
        public string Sex { get; set; }
        public string Nation { get; set; }
        public string Birthday
        {
            get
            {
                return _birthday;
            }
            set
            {
                _birthday = value;

                if (!value.IsNullOrEmpty())
                {
                    var date = value;
                    if (date.Substring(5, 5).IsIn("02-29", "02-30"))
                    {
                        date = $"{date.Substring(0, 5)}02-28";
                    }
                    var birth = date.To<DateTime>();
                    Age = DateTime.Now.Year - birth.Year;
                    if (birth.AddYears(Age.Value) > DateTime.Now.Date)
                    {
                        Age--;
                    }
                }
            }
        }
        private string _birthday;
        public string CertSdate { get; set; }
        public string CertEdate { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public string Mobile { get; set; }
        public string Sdate { get; set; }
        public int? ProvinceId { get; set; }
        public string ChinaCityId { get; set; }
        public byte[] Photo { get; set; }
        public DateTime? Stime { get; set; }
        public DateTime? Etime { get; set; }
        public string Memo { get; set; }
        public bool? CertBlackListHandleFlag { get; set; }
        public string Area { get; set; }
        public int? Age { get; set; }
        public DateTime? Ctime { get; set; }
        public int? ParkId { get; set; }
        public Guid? SyncCode { get; set; } = Guid.NewGuid();

        public virtual TicketSale TicketSale { get; set; }

        public void SetIdCardNo(string idCardNo)
        {
            CertTypeId = DefaultCertType.二代身份证;
            CertTypeName = "二代身份证";
            CertNo = idCardNo;
            Birthday = $"{idCardNo.Substring(6, 4)}-{idCardNo.Substring(10, 2)}-{idCardNo.Substring(12, 2)}";
            ProvinceId = idCardNo.Substring(0, 2).To<int>();
            ChinaCityId = idCardNo.Substring(0, 4);
            Sex = idCardNo.Substring(16, 1).To<int>() % 2 == 0 ? "女" : "男";
        }

        public void SetArea()
        {
            switch (CertTypeId)
            {
                case DefaultCertType.二代身份证:
                    {
                        if (ChinaCityId == "4401")
                        {
                            Area = "广州";
                        }
                        else if (ProvinceId == 44)
                        {
                            Area = "省内";
                        }
                        else
                        {
                            Area = "省外";
                        }
                        break;
                    }
                case DefaultCertType.卡式台胞证:
                case DefaultCertType.台胞证:
                case DefaultCertType.港澳通行证:
                case DefaultCertType.电子港澳通行证:
                    {
                        Area = "省外";
                        break;
                    }
                case DefaultCertType.护照:
                case DefaultCertType.护照签证:
                    {
                        Area = "国外";
                        break;
                    }
                default:
                    {
                        Area = "未知";
                        break;
                    }
            }
        }
    }
}

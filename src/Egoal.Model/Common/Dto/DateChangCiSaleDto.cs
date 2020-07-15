using System;

namespace Egoal.Common.Dto
{
    public class DateChangCiSaleDto
    {
        public DateTime Date { get; set; }
        public int? GroundID { get; set; }
        public int ChangCiId { get; set; }
        public string ChangCiName { get; set; }
        public string STime { get; set; }
        public string ETime { get; set; }
        public int ChangCiNum { get; set; }
        public int SaleNum { get; set; }
        public int SurplusNum
        {
            get
            {
                return ChangCiNum - SaleNum;
            }
        }
    }
}

using Egoal.Domain.Entities;

namespace Egoal.Tickets
{
    public class InvoiceInfo : Entity
    {
        public string ListNo { get; set; }
        public InvoiceType Type { get; set; }
        public InvoiceGMFType GMFType { get; set; }
        public InvoiceStatus Status { get; set; }
        public decimal JE { get; set; }
        public decimal SE { get; set; }
        public string FPDM { get; set; }
        public string FPHM { get; set; }
        public string KPR { get; set; }
        public string GMF_MC { get; set; }
        public string GMF_NSRSBH { get; set; }
        public string GMF_DZDH { get; set; }
        public string GMF_YHZH { get; set; }
        public string GMF_Email { get; set; }
        public InvoiceChannel Channel { get; set; }
        public string CreateTime { get; set; }
        public int BDFlag { get; set; }
    }
}

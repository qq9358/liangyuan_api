using Egoal.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Tickets.Dto
{
    public class InvoiceInput
    {
        [Display(Name = "单号")]
        [MustFillIn]
        public string ListNo { get; set; }

        public InvoiceGMFType GMFType { get; set; }

        public string Name { get; set; }

        [Display(Name = "手机号码")]
        [MobileNumber]
        public string Mobile { get; set; }

        [Display(Name = "邮箱地址")]
        [Email]
        public string Email { get; set; }

        public string InvoiceTitle { get; set; }
        public string TaxAccount { get; set; }
        public string Address { get; set; }
        public string BankAccount { get; set; }
        public string Telephone { get; set; }
    }
}

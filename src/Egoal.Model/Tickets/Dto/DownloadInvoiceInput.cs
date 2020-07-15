using Egoal.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Tickets.Dto
{
    public class DownloadInvoiceInput
    {
        [Display(Name = "单号")]
        [MustFillIn]
        public string ListNo { get; set; }
    }
}

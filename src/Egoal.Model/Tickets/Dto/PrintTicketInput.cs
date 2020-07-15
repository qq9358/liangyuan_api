using Egoal.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Tickets.Dto
{
    public class PrintTicketInput : IValidatableObject
    {
        public string ListNo { get; set; }
        public string TicketCode { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ListNo.IsNullOrEmpty() && TicketCode.IsNullOrEmpty())
            {
                yield return new ValidationResult("至少提供一个查询参数", new[] { "ListNo", "TicketCode" });
            }
        }
    }
}

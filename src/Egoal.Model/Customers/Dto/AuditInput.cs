using Egoal.Application.Services.Dto;
using System;

namespace Egoal.Customers.Dto
{
    public class AuditInput : EntityDto<Guid>
    {
        public bool Agree { get; set; }
        public string Memo { get; set; }
    }
}

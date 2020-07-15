using Egoal.Application.Services.Dto;
using Egoal.Members;
using System;
using System.Collections.Generic;
using System.Text;

namespace Egoal.Customers.Dto
{
    public class GetCustomersInput : PagedInputDto
    {
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string CertNo { get; set; }
        public int? CustomerTypeId { get; set; }
        public MemberStatus? StatusId { get; set; }
    }
}

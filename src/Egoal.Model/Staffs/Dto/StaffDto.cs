using Egoal.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Egoal.Staffs.Dto
{
    public class StaffDto : EntityDto
    {
        public string Name { get; set; }
        public int? ParkId { get; set; }
    }
}

using Egoal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Egoal.Authorization
{
    public class Role : Entity
    {
        public string Name { get; set; }
        public int? Level { get; set; }
        public string SortCode { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Egoal.Application.Services.Dto
{
    [Serializable]
    public class TreeNodeDto
    {
        public TreeNodeDto()
        {
            Nodes = new List<TreeNodeDto>();
        }

        public string DisplayText { get; set; }
        public string Value { get; set; }
        public bool Disabled { get; set; }
        public List<TreeNodeDto> Nodes { get; set; }
    }
}

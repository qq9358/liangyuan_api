using Egoal.Domain.Entities;
using System;

namespace Egoal.Staffs
{
    public class ExplainerWorkRecord : Entity
    {
        public string ListNo { get; set; }
        public int StaffId { get; set; }
        public int TimeslotId { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime? CompleteTime { get; set; }

        public void ChangeExplainer(int newExplainerId)
        {
            StaffId = newExplainerId;
        }
    }
}

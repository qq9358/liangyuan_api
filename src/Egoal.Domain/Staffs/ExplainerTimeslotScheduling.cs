using Egoal.Domain.Entities;

namespace Egoal.Staffs
{
    public class ExplainerTimeslotScheduling : Entity
    {
        public string Date { get; set; }
        public int TimeslotId { get; set; }
        public int PublicQuantity { get; set; }
        public int PublicBookedQuantity { get; set; }
        public int ReservedQuantity { get; set; }
        public int ReservedBookedQuantity { get; set; }
        public bool IsReserved { get; set; }

        public void CancelPublicTimeslot()
        {
            PublicBookedQuantity--;
        }

        public void CancelReservedTimeslot()
        {
            ReservedBookedQuantity--;
        }
    }
}

using Egoal.Domain.Entities;

namespace Egoal.DynamicCodes
{
    public class ListNo : Entity<long>
    {
        public const int DefaultListNoLength = 8;
        public const int DefaultTicketCodeLength = 6;

        public static string GetLockKey(string listNo)
        {
            return $"ListNo:{listNo}";
        }

        public string ListNoTypeID { get; set; }
        public string ListDate { get; set; }
        public int Lens { get; set; }
    }
}

using Egoal.Domain.Entities;

namespace Egoal.Stadiums
{
    public class Stadium : Entity
    {
        public string Name { get; set; }
        public string SortCode { get; set; }
        public int? StadiumTypeId { get; set; }
        public int? SeatTypeId { get; set; }
        public int? SeatNum { get; set; }
        public int? Xcount { get; set; }
        public int? Ycount { get; set; }
        public string SeatCodePrefix { get; set; }
        public int? SeatCodeLen { get; set; }
        public int? SeatCodeStartIndex { get; set; }
        public int? StartRowNum { get; set; }
        public int? StartColumnCode { get; set; }
        public byte[] SeatPhoto { get; set; }
        public string Memo { get; set; }
    }
}

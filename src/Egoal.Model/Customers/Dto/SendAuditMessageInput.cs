namespace Egoal.Customers.Dto
{
    public class SendAuditMessageInput
    {
        public string OpenId { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Date { get; set; }
        public string Remark { get; set; }
    }
}

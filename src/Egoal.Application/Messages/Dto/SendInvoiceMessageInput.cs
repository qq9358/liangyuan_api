namespace Egoal.Messages.Dto
{
    public class SendInvoiceMessageInput
    {
        public string ListNo { get; set; }
        public string Email { get; set; }
        public string InvoiceDate { get; set; }
        public string SellerName { get; set; }
        public decimal TotalMoney { get; set; }
        public string InvoiceCode { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceUrl { get; set; }
        public string Mobile { get; set; }
    }
}

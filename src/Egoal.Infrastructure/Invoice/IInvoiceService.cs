using System.Threading.Tasks;

namespace Egoal.Invoice
{
    public interface IInvoiceService
    {
        Task<InvoiceResponse> InvoiceAsync(InvoiceRequest request);
        Task<DownloadResponse> DownloadAsync(DownloadRequest request);
        Task<QueryResponse> QueryAsync(QueryRequest request);
    }
}

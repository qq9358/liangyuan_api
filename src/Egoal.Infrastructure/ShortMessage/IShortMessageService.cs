using System.Threading.Tasks;

namespace Egoal.ShortMessage
{
    public interface IShortMessageService
    {
        string SupplierUrl { get; }
        int VariableMaxLength { get; }
        int GetChargingQuantity(MessageInfo message);
        Task SendAsync(MessageInfo message);
    }
}

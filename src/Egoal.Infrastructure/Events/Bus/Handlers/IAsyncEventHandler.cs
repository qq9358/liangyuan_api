using System.Threading.Tasks;

namespace Egoal.Events.Bus.Handlers
{
    public interface IAsyncEventHandler<in TEventData> : IEventHandler
    {
        Task HandleEventAsync(TEventData eventData);
    }
}

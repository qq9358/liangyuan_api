using System;
using System.Threading.Tasks;

namespace Egoal.Events.Bus.Handlers.Internals
{
    internal class AsyncActionEventHandler<TEventData> :
        IAsyncEventHandler<TEventData>
    {
        public Func<TEventData, Task> Action { get; private set; }
        public AsyncActionEventHandler(Func<TEventData, Task> handler)
        {
            Action = handler;
        }
        public async Task HandleEventAsync(TEventData eventData)
        {
            await Action(eventData);
        }
    }
}

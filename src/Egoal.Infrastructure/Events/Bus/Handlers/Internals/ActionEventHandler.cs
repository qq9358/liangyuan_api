using System;

namespace Egoal.Events.Bus.Handlers.Internals
{
    internal class ActionEventHandler<TEventData> : IEventHandler<TEventData>
    {
        public Action<TEventData> Action { get; private set; }

        public ActionEventHandler(Action<TEventData> handler)
        {
            Action = handler;
        }

        public void HandleEvent(TEventData eventData)
        {
            Action(eventData);
        }
    }
}

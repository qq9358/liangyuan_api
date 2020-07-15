using Egoal.Events.Bus.Handlers;
using System;

namespace Egoal.Events.Bus.Factories.Internals
{
    internal class SingleInstanceHandlerFactory : IEventHandlerFactory
    {
        public IEventHandler HandlerInstance { get; private set; }

        public SingleInstanceHandlerFactory(IEventHandler handler)
        {
            HandlerInstance = handler;
        }

        public IEventHandler GetHandler()
        {
            return HandlerInstance;
        }

        public Type GetHandlerType()
        {
            return HandlerInstance.GetType();
        }

        public void ReleaseHandler(IEventHandler handler)
        {

        }
    }
}

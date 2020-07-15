using Egoal.Events.Bus.Handlers;
using System;

namespace Egoal.Events.Bus.Factories.Internals
{
    internal class TransientEventHandlerFactory<THandler> : IEventHandlerFactory
        where THandler : IEventHandler, new()
    {
        public IEventHandler GetHandler()
        {
            return new THandler();
        }

        public Type GetHandlerType()
        {
            return typeof(THandler);
        }

        public void ReleaseHandler(IEventHandler handler)
        {
            if (handler is IDisposable)
            {
                (handler as IDisposable).Dispose();
            }
        }
    }
}

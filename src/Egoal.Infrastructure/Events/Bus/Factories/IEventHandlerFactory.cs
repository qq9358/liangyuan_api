using Egoal.Events.Bus.Handlers;
using System;

namespace Egoal.Events.Bus.Factories
{
    public interface IEventHandlerFactory
    {
        IEventHandler GetHandler();
        Type GetHandlerType();
        void ReleaseHandler(IEventHandler handler);
    }
}

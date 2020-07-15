using Egoal.Events.Bus.Handlers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Egoal.Events.Bus.Factories.Internals
{
    public class IocHandlerFactory : IEventHandlerFactory
    {
        public Type HandlerType { get; }

        public IServiceProvider ServiceProvider { get; }
        private IServiceScope _serviceScope;

        public IocHandlerFactory(IServiceProvider serviceProvider, Type handlerType)
        {
            ServiceProvider = serviceProvider;
            HandlerType = handlerType;
        }

        public IEventHandler GetHandler()
        {
            _serviceScope = ServiceProvider.CreateScope();
            return (IEventHandler)_serviceScope.ServiceProvider.GetRequiredService(HandlerType);
        }

        public Type GetHandlerType()
        {
            return HandlerType;
        }

        public void ReleaseHandler(IEventHandler handler)
        {
            _serviceScope.Dispose();
            _serviceScope = null;
        }
    }
}

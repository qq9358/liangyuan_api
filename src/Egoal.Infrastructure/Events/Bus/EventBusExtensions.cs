using Egoal.Events.Bus.Factories.Internals;
using Egoal.Events.Bus.Handlers;
using System;
using System.Linq;
using System.Reflection;

namespace Egoal.Events.Bus
{
    public static class EventBusExtensions
    {
        public static void RegisterEventHandler(this IEventBus eventBus, Assembly assembly, IServiceProvider serviceProvider)
        {
            Func<Type, bool> predicat = t => typeof(IEventHandler).IsAssignableFrom(t);

            var handlerTypes = assembly.GetTypes().Where(predicat);
            foreach (var handlerType in handlerTypes)
            {
                var handlerInterfaces = handlerType.GetInterfaces().Where(predicat);
                foreach (var handlerInterface in handlerInterfaces)
                {
                    var genericArgs = handlerInterface.GetGenericArguments();
                    if (genericArgs.Length == 1)
                    {
                        var eventType = genericArgs[0];

                        eventBus.Register(eventType, new IocHandlerFactory(serviceProvider, handlerType));
                    }
                }
            }
        }
    }
}

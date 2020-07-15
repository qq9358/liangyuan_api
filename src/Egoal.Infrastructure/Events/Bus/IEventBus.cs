using Egoal.Events.Bus.Factories;
using Egoal.Events.Bus.Handlers;
using System;
using System.Threading.Tasks;

namespace Egoal.Events.Bus
{
    public interface IEventBus
    {
        #region Register

        IDisposable Register<TEventData>(Action<TEventData> action) where TEventData : IEventData;
        IDisposable AsyncRegister<TEventData>(Func<TEventData, Task> action) where TEventData : IEventData;
        IDisposable Register<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData;
        IDisposable AsyncRegister<TEventData>(IAsyncEventHandler<TEventData> handler) where TEventData : IEventData;
        IDisposable Register<TEventData, THandler>() where TEventData : IEventData where THandler : IEventHandler, new();
        IDisposable Register(Type eventType, IEventHandler handler);
        IDisposable Register<TEventData>(IEventHandlerFactory factory) where TEventData : IEventData;
        IDisposable Register(Type eventType, IEventHandlerFactory factory);

        #endregion

        #region Unregister

        void Unregister<TEventData>(Action<TEventData> action) where TEventData : IEventData;
        void AsyncUnregister<TEventData>(Func<TEventData, Task> action) where TEventData : IEventData;
        void Unregister<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData;
        void AsyncUnregister<TEventData>(IAsyncEventHandler<TEventData> handler) where TEventData : IEventData;
        void Unregister(Type eventType, IEventHandler handler);
        void Unregister<TEventData>(IEventHandlerFactory factory) where TEventData : IEventData;
        void Unregister(Type eventType, IEventHandlerFactory factory);
        void UnregisterAll<TEventData>() where TEventData : IEventData;
        void UnregisterAll(Type eventType);

        #endregion

        #region Trigger

        void Trigger<TEventData>(TEventData eventData) where TEventData : IEventData;
        void Trigger<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData;
        void Trigger(Type eventType, IEventData eventData);
        void Trigger(Type eventType, object eventSource, IEventData eventData);
        Task TriggerAsync<TEventData>(TEventData eventData) where TEventData : IEventData;
        Task TriggerAsync<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData;
        Task TriggerAsync(Type eventType, IEventData eventData);
        Task TriggerAsync(Type eventType, object eventSource, IEventData eventData);


        #endregion
    }
}

namespace Egoal.Events.Bus.Handlers
{
    public interface IEventHandler<in TEventData> : IEventHandler
    {
        void HandleEvent(TEventData eventData);
    }

    public interface IEventHandler
    {

    }
}

namespace Egoal.Events.Bus
{
    public interface IEventDataWithInheritableGenericArgument
    {
        object[] GetConstructorArgs();
    }
}

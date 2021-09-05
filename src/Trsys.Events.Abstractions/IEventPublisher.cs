namespace Trsys.Events.Abstractions
{
    public interface IEventPublisher
    {
        void Publish<T>(T e) where T : IEvent;
    }
}

using Trsys.Events.Abstractions;

namespace Trsys.CopyTrading.Abstractions
{
    public interface IEventQueue
    {
        void Enqueue(IEvent @event);
    }
}

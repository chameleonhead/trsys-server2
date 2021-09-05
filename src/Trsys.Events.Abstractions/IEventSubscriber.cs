using System;
using System.Threading.Tasks;
namespace Trsys.Events.Abstractions
{
    public interface IEventSubscriber
    {
        void Subscribe(Func<IEvent, Task> eventHandler);
    }
}

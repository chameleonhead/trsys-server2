using System.Collections.Generic;

namespace Trsys.Events.Abstractions
{
    public interface IEventSubscriber
    {
        void Subscribe(IEventHandler handler);
        void Subscribe(IEnumerable<IEventHandler> handlers);
    }
}

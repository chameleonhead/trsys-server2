using System.Threading;
using System.Threading.Tasks;

namespace Trsys.Events.Abstractions
{
    public interface IEventHandler
    {
        Task Handle(IEvent e, CancellationToken cancellationToken = default);
    }
}

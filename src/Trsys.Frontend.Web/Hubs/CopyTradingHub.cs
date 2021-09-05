using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Trsys.Frontend.Hubs
{
    public class CopyTradingHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
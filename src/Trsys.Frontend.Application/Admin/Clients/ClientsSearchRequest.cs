using MediatR;
using System.Collections.Generic;

namespace Trsys.Frontend.Application.Admin.Clients
{
    public class ClientsSearchRequest : IRequest<ClientsSearchResponse>
    {
        public List<string> KeyTypes { get; set; } = new();
        public bool ConnectedOnly { get; set; }
        public List<bool> IsActive { get; set; } = new();
        public string Text { get; set; }
    }
}

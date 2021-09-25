using System.Collections.Generic;
using Trsys.Frontend.Application.Dtos;

namespace Trsys.Frontend.Application.Admin.Clients
{
    public class ClientsSearchResponse
    {
        public List<SecretKeyDto> Clients { get; set; }
    }
}
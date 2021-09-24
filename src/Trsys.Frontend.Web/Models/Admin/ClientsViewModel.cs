using System.Collections.Generic;
using Trsys.Frontend.Application.Admin.Clients;
using Trsys.Frontend.Application.Dtos;

namespace Trsys.Frontend.Web.Models.Admin
{
    public class ClientsViewModel
    {
        public ClientsSearchRequest SearchConditions { get; set; }
        public List<SecretKeyDto> Clients { get; set; } = new();
    }
}

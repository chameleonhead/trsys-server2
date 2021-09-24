using System.Collections.Generic;

namespace Trsys.Frontend.Web.Models.Admin
{
    public class ClientsViewModel
    {
        public ClientsSearchConditionsViewModel SearchConditions { get; set; }
        public List<ClientSummaryViewModel> Clients { get; set; } = new();
    }

    public class ClientsSearchConditionsViewModel
    {
        public List<string> KeyType { get; set; } = new();
        public bool ConnectedOnly { get; set; }
        public List<bool> IsActive { get; set; }
        public string Text { get; set; }
    }

    public class ClientSummaryViewModel
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string KeyType { get; set; }
        public string Desctiption { get; set; }
        public bool IsActive { get; set; }
        public bool IsConnected { get; set; }
    }
}

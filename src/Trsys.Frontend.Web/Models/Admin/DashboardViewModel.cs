using System.Collections.Generic;

namespace Trsys.Frontend.Web.Models.Admin
{
    public class DashboardViewModel
    {
        public List<DashboardItemViewModel> DashboardItems { get; } = new();
        public List<DashboardMessageViewModel> Messages { get; } = new();
    }

    public class DashboardItemViewModel
    {
        public string Header { get; set; }
        public List<DashboardItemLineViewModel> Lines { get; set; } = new();
        public string LinkText { get; set; }
        public string LinkTitle { get; set; }
        public string LinkUri { get; set; }
    }

    public class DashboardItemLineViewModel
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public string ValueClass { get; set; }
    }

    public class DashboardMessageViewModel
    {
        public string Message { get; set; }
        public string Uri { get; set; }
    }
}

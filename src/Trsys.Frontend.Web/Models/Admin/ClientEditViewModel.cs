using Trsys.Frontend.Application.Admin.ClientEdit;
using Trsys.Frontend.Application.Dtos;

namespace Trsys.Frontend.Web.Models.Admin
{
    public class ClientEditViewModel
    {
        public SecretKeyDto SecretKey { get; set; }
        public ClientEditRequest Request { get; set; }
    }
}

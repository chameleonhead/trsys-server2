using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Trsys.Frontend.Application.Admin.ClientAdd
{
    public class ClientAddRequest : IRequest<ClientAddResponse>
    {
        [Required(ErrorMessage = "この項目は必須です。")]
        public string Key { get; set; }
        [Required(ErrorMessage = "この項目は必須です。")]
        public string KeyType { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}

using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Trsys.Frontend.Application.Admin.ClientEdit
{
    public class ClientEditRequest : IRequest<ClientEditResponse>
    {
        [Required]
        public string SecretKeyId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}

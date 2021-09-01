using System.Threading.Tasks;

namespace Trsys.Backoffice.ReadModels.Users
{
    public interface IUserStore
    {
        Task<User> FindByIdAsync(string userId);
        Task<User> FindByUsernameAsync(string username);
    }
}

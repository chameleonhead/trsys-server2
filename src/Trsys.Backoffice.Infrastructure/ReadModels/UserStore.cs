using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Trsys.Backoffice.ReadModels.Users;

namespace Trsys.Backoffice.Infrastructure.ReadModels
{
    public class UserStore : IUserStore
    {
        private readonly TrsysBackofficeContext db;

        public UserStore(TrsysBackofficeContext db)
        {
            this.db = db;
        }

        public Task<User> FindByIdAsync(string userId)
        {
            return db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public Task<User> FindByUsernameAsync(string username)
        {
            return db.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}

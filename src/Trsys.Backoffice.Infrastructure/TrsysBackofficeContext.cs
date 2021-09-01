using Microsoft.EntityFrameworkCore;
using Trsys.Backoffice.ReadModels.Users;

namespace Trsys.Backoffice.Infrastructure
{
    public class TrsysBackofficeContext : DbContext
    {
        public TrsysBackofficeContext(DbContextOptions<TrsysBackofficeContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}

using Dapper.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dapper.WepApi.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options): base(options) { }

        public DbSet<User> users { get; set; }
    }
}

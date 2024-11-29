using Ecommerce.Gateway.YARP.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Gateway.YARP.Context
{
    public sealed class AppdbContext : DbContext
    {
        public AppdbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}

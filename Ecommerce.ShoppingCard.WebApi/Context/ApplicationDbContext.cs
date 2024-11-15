using Microsoft.EntityFrameworkCore;
using Ecommerce.ShoppingCard.WebApi.Entities;

namespace Ecommerce.ShoppingCard.WebApi.Context
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ShoppingCardes> shoppingCards { get; set; }
    }
}

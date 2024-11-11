using Microsoft.EntityFrameworkCore;

namespace Ecommerce.ShoppingCard.WebApi.Context
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}

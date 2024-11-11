using Microsoft.EntityFrameworkCore;
using Ecommerce.Products.WebApi.Entities;


namespace Ecommerce.Products.WebApi.Context
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(buiilder =>
            {
                buiilder.Property(p => p.Price).HasColumnType("money");
            });
        }
    }
}

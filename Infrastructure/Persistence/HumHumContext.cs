using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public sealed class HumHumContext : DbContext
{

    public HumHumContext(DbContextOptions<HumHumContext> options) : base(options)
    {

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
       => modelBuilder.ApplyConfigurationsFromAssembly(typeof(HumHumContext).Assembly);


    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }

}

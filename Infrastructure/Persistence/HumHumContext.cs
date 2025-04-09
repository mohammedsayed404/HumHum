using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public sealed class HumHumContext : IdentityDbContext<ApplicationUser>
{

    public HumHumContext(DbContextOptions<HumHumContext> options) : base(options)
    {

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HumHumContext).Assembly);

    }


    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }

}

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations;

internal sealed class RestaurantConfigurations : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.ToTable(nameof(Restaurant));
        builder.Property(restaurant => restaurant.Name).HasColumnType("varchar(50)");
        builder.HasIndex(restaurant => restaurant.Name).IsUnique();

        builder.Property(restaurant => restaurant.PublicImageId).HasColumnType("varchar(100)");
        builder.Property(restaurant => restaurant.ImageUrl).HasColumnType("varchar(100)");



        builder.HasMany(restaurant => restaurant.Products)
              .WithOne(product => product.Restaurant)
              .HasForeignKey(product => product.RestaurantId)
              .OnDelete(DeleteBehavior.NoAction);

        builder.HasQueryFilter(restaurant => !restaurant.IsDeleted);

    }
}

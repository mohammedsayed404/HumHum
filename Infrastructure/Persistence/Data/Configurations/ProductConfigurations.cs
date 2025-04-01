using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations;

internal sealed class ProductConfigurations : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(nameof(Product));


        builder.Property(product => product.Name).HasColumnType("varchar(50)");
        builder.Property(product => product.Description).HasColumnType("varchar(2000)");
        builder.Property(product => product.PublicImageId).HasColumnType("varchar(100)");
        builder.Property(product => product.ImageUrl).HasColumnType("varchar(100)");

        builder.Property(product => product.Price)
                .HasColumnType("decimal(18,2)");



        builder.HasQueryFilter(product => !product.IsDeleted);




    }
}

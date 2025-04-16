using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations;

internal sealed class ProductCategoryConfigurations : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable(nameof(ProductCategory));
        builder.Property(productType => productType.Name).HasColumnType("varchar(50)");

        builder.Property(productType => productType.PublicImageId).HasColumnType("varchar(100)");
        builder.Property(productType => productType.ImageUrl).HasColumnType("varchar(100)");


        builder.HasMany(productType => productType.Products)
              .WithOne(product => product.Category)
              .HasForeignKey(product => product.CategoryId)
              .OnDelete(DeleteBehavior.NoAction);

        builder.HasQueryFilter(productType => !productType.IsDeleted);


    }
}

using Domain.Entities.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations;

internal sealed class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(orderItem => orderItem.Price).HasColumnType("decimal(18,2)");


        builder.ComplexProperty(orderItem => orderItem.Product, product =>
        {
            product.Property(product => product.Name).HasColumnType("varchar(50)");
            product.Property(product => product.ImageUrl).HasColumnType("varchar(100)");

            builder.Property(product => product.Price)
                    .HasColumnType("decimal(18,2)");

        });



    }
}

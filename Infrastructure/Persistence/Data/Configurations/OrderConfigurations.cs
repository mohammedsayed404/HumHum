using Domain.Entities.Aggregates;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations;

internal sealed class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {

        builder.ToTable(nameof(Order));

        builder.Property(order => order.UserEmail).HasColumnType("varchar(256)");
        builder.Property(order => order.Subtotal).HasColumnType("decimal(18,2)");
        builder.Ignore(order => order.Total);

        builder.Property(order => order.PaymentStatus)
            .HasConversion(
              PaymentStatusToDb => PaymentStatusToDb.ToString(),
              PaymentStatusFromDb => Enum.Parse<OrderPaymentStatus>(PaymentStatusFromDb)

            ).HasColumnType("varchar(50)");


        builder.ComplexProperty(order => order.ShippingAddress, address =>
        {
            address.Property(address => address.FirstName).HasColumnType("varchar(50)");
            address.Property(address => address.LastName).HasColumnType("varchar(50)");
            address.Property(address => address.Street).HasColumnType("varchar(50)");
            address.Property(address => address.City).HasColumnType("varchar(50)");
            address.Property(address => address.Country).HasColumnType("varchar(50)");
        });



        builder.HasMany(order => order.OrderItems)
               .WithOne(item => item.Order)
               .OnDelete(DeleteBehavior.Cascade);







    }
}

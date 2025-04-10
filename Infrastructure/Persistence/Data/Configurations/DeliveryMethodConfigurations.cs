using Domain.Entities.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations;

internal sealed class DeliveryMethodConfigurations : IEntityTypeConfiguration<DeliveryMethod>
{
    public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
    {

        builder.ToTable(nameof(DeliveryMethod));

        builder.Property(delivery => delivery.ShortName).HasColumnType("varchar(50)");
        builder.Property(delivery => delivery.Description).HasColumnType("varchar(100)");
        builder.Property(delivery => delivery.DeliveryTime).HasColumnType("varchar(50)");
        builder.Property(delivery => delivery.Price).HasColumnType("decimal(18,2)");



        builder.HasMany(delivery => delivery.Orders)
               .WithOne(order => order.DeliveryMethod)
               .HasForeignKey(order => order.DeliveryMethodId)
               .OnDelete(DeleteBehavior.SetNull);

    }
}

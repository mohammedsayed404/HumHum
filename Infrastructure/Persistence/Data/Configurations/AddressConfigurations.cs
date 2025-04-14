using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations;

internal class AddressConfigurations : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable(nameof(Address));


        builder.Property(address => address.FirstName).HasColumnType("varchar(50)");
        builder.Property(address => address.LastName).HasColumnType("varchar(50)");
        builder.Property(address => address.Street).HasColumnType("varchar(50)");
        builder.Property(address => address.City).HasColumnType("varchar(50)");
        builder.Property(address => address.Country).HasColumnType("varchar(50)");

    }
}

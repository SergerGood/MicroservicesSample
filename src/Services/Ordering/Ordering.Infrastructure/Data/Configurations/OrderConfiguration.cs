using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;

namespace Ordering.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                orderId => orderId.Value,
                dbId => OrderId.Of(dbId));

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .IsRequired();

        builder.HasMany(x => x.OrderItems)
            .WithOne()
            .HasForeignKey(x => x.OrderId);

        builder.ComplexProperty(x => x.OrderName, propertyBuilder =>
        {
            propertyBuilder.Property(p => p.Value)
                .HasColumnName(nameof(Order.OrderName))
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.ComplexProperty(x => x.ShippingAddress, propertyBuilder =>
        {
            propertyBuilder.Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            propertyBuilder.Property(p => p.LastName)
                .HasMaxLength(50)
                .IsRequired();

            propertyBuilder.Property(p => p.EmailAddress)
                .HasMaxLength(50);

            propertyBuilder.Property(p => p.AddressLine)
                .HasMaxLength(180)
                .IsRequired();

            propertyBuilder.Property(p => p.Country)
                .HasMaxLength(50);

            propertyBuilder.Property(p => p.State)
                .HasMaxLength(50);

            propertyBuilder.Property(p => p.ZipCode)
                .HasMaxLength(5)
                .IsRequired();
        });

        builder.ComplexProperty(x => x.BillingAddress, propertyBuilder =>
        {
            propertyBuilder.Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            propertyBuilder.Property(p => p.LastName)
                .HasMaxLength(50)
                .IsRequired();

            propertyBuilder.Property(p => p.EmailAddress)
                .HasMaxLength(50);

            propertyBuilder.Property(p => p.AddressLine)
                .HasMaxLength(180)
                .IsRequired();

            propertyBuilder.Property(p => p.Country)
                .HasMaxLength(50);

            propertyBuilder.Property(p => p.State)
                .HasMaxLength(50);

            propertyBuilder.Property(p => p.ZipCode)
                .HasMaxLength(5)
                .IsRequired();
        });

        builder.ComplexProperty(x => x.Payment, propertyBuilder =>
        {
            propertyBuilder.Property(p => p.CardName)
                .HasMaxLength(50);

            propertyBuilder.Property(p => p.CardName)
                .HasMaxLength(24)
                .IsRequired();

            propertyBuilder.Property(p => p.Expiration)
                .HasMaxLength(10);

            propertyBuilder.Property(p => p.Cvv)
                .HasMaxLength(3);

            propertyBuilder.Property(p => p.PaymentMethod);
        });

        builder.Property(x => x.Status)
            .HasDefaultValue(OrderStatus.Draft)
            .HasConversion(
                status => status.ToString(),
                dbStatus => Enum.Parse<OrderStatus>(dbStatus)
            );

        builder.Property(x => x.TotalPrice);
    }
}
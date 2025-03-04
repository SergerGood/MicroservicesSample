using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountContext(DbContextOptions<DiscountContext> options) : DbContext(options)
{
    public DbSet<Coupon> Coupons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>()
            .HasData(
                new Coupon { Id = 1, ProductName = "IPhone", Description = "IPhone", Amount = 1 },
                new Coupon { Id = 2, ProductName = "Honor", Description = "Honor", Amount = 2 }
            );
    }
}
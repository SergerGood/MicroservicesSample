namespace Ordering.Domain.Models;

public class OrderItem(
    Guid orderId,
    Guid productId,
    int quantity,
    decimal price)
    : Entity<Guid>
{
    public Guid OrderId { get; } = orderId;
    public Guid ProductId { get; } = productId;
    public int Quantity { get; } = quantity;
    public decimal Price { get; } = price;
}
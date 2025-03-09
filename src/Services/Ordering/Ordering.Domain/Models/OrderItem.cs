namespace Ordering.Domain.Models;

public class OrderItem(
    OrderId orderId,
    ProductId productId,
    int quantity,
    decimal price)
    : Entity<OrderItemId>
{
    public OrderId OrderId { get; } = orderId;
    public ProductId ProductId { get; } = productId;
    public int Quantity { get; } = quantity;
    public decimal Price { get; } = price;
}
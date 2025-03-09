namespace Ordering.Domain.Models;

public class Order : Aggregate<Guid>
{
    private readonly List<OrderItem> _orderItems = [];
    
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
    
    public Guid CustomerId { get; private set; }

    public string? OrderName { get; set; }

    public Address? ShippingAddress { get; set; }
    
    public Address? BillingAddress { get; set; }

    public Payment? Payment { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public decimal TotalPrice
    {
        get => OrderItems.Sum(x => x.Price * x.Quantity);
        private set { }
    }
}
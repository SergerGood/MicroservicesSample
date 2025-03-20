namespace Ordering.Application.Orders.Queries.GetOrderByCustomer;

public record GetOrderByCustomerResult(IEnumerable<OrderDto> Orders);
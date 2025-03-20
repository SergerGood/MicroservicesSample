namespace Ordering.Application.Orders.Queries.GetOrdersByName;

public record GetOrdersByNameQuery(string Name) : IQuery<GetOrdersByNameResult>;
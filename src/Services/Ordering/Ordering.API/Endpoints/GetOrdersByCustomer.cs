using Ordering.Application.Orders.Queries.GetOrdersByCustomer;

namespace Ordering.API.Endpoints;

public record GetOrdersByCustomerResponse(IEnumerable<OrderDto> Orders);

public class GetOrdersByCustomer : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/customers/{customerId}", Handle)
            .WithName("GetOrdersByCustomer")
            .Produces<GetOrdersByCustomerResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get orders by Customer")
            .WithDescription("Get orders by Customer");
    }

    private static async Task<IResult> Handle(Guid customerId, ISender sender)
    {
        var result = await sender.Send(new GetOrdersByCustomerQuery(customerId));
        var response = result.Adapt<GetOrdersByCustomerResponse>();

        return Results.Ok(response);
    }
}
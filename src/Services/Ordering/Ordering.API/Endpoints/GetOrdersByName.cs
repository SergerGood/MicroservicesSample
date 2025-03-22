using Ordering.Application.Orders.Queries.GetOrdersByName;

namespace Ordering.API.Endpoints;

public record GetOrdersByNameResponse(IEnumerable<OrderDto> Orders);

public class GetOrdersByName : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{orderName}", Handle)
            .WithName("GetOrdersByName")
            .Produces<GetOrdersByNameResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Get orders by name")
            .WithDescription("Get orders by name");
    }

    private static async Task<IResult> Handle(string orderName, ISender sender)
    {
        var result = await sender.Send(new GetOrdersByNameQuery(orderName));
        var response = result.Adapt<GetOrdersByNameResponse>();

        return Results.Ok(response);
    }
}
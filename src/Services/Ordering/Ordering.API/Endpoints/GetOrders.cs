using BuildingBlocks.Pagination;
using Ordering.Application.Orders.Queries.GetOrders;

namespace Ordering.API.Endpoints;

public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);

public class GetOrders : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", Handle)
            .WithName("GetOrders")
            .Produces<GetOrdersResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get orders")
            .WithDescription("Get orders");
    }

    private static async Task<IResult> Handle([AsParameters] PaginatedRequest request, ISender sender)
    {
        var result = await sender.Send(new GetOrdersQuery(request));
        var response = result.Adapt<GetOrdersResponse>();

        return Results.Ok(response);
    }
}
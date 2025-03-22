using Ordering.Application.Orders.Commands.UpdateOrder;

namespace Ordering.API.Endpoints;

public record UpdateOrderRequest(OrderDto Order);

public record UpdateOrderResponse(bool IsSuccess);

public class UpdateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/order", Handle)
            .WithName("UpdateOrder")
            .Produces<UpdateOrderResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Updates order")
            .WithDescription("Updates order");
    }

    private static async Task<IResult> Handle(UpdateOrderRequest request, ISender sender)
    {
        var command = request.Adapt<UpdateOrderCommand>();
        var result = await sender.Send(command);
        var response = result.Adapt<UpdateOrderResponse>();

        return Results.Ok(response);
    }
}
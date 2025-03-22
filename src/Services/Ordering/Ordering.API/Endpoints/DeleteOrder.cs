using Ordering.Application.Orders.Commands.DeleteOrder;

namespace Ordering.API.Endpoints;

public record DeleteOrderResponse(bool IsSuccess);

public class DeleteOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{id}", Handle)
            .WithName("DeleteOrder")
            .Produces<DeleteOrderResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Delete order")
            .WithDescription("Delete order");
    }

    private static async Task<IResult> Handle(Guid id, ISender sender)
    {
        var result = await sender.Send(new DeleteOrderCommand(id));
        var response = result.Adapt<DeleteOrderResponse>();

        return Results.Ok(response);
    }
}
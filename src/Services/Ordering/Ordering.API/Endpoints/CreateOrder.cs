using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.API.Endpoints;

public record CreateOrderRequest(OrderDto Order);

public record CreateOrderResponse(Guid Id);

public class CreateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", Handle)
            .WithName("CreateOrder")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDisplayName("Create Order")
            .WithDescription("Creates a new order");
    }

    private static async Task<IResult> Handle(CreateOrderRequest request, ISender sender)
    {
        var command = request.Adapt<CreateOrderCommand>();
        var result = await sender.Send(command);
        var response = result.Adapt<CreateOrderResponse>();

        return Results.Created($"/orders/{response.Id}", response);
    }
}
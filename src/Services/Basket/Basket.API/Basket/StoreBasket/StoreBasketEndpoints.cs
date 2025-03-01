namespace Basket.API.Basket.StoreBasket;

public record StoreBasketRequest(ShoppingCart Cart);

public record StoreBasketResponse(string UserName);

public class StoreBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", Handle)
            .WithName("CreateProduct")
            .Produces<StoreBasketResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product");
    }

    private static async Task<IResult> Handle(StoreBasketRequest request, ISender sender)
    {
        var command = request.Adapt<StoreBasketCommand>();
        var result = await sender.Send(command);
        var response = result.Adapt<StoreBasketResponse>();

        return Results.Created($"/basket/{response.UserName}", response);
    }
}
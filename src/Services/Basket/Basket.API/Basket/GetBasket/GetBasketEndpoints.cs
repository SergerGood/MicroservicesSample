namespace Basket.API.Basket.GetBasket;

public record GetBasketRequest(string UserName);

public record GetBasketResponse(ShoppingCart Cart);

public class GetBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}", Handle)
            .WithName("GetBasket")
            .Produces<GetBasketResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Basket")
            .WithDescription("Get Basket");
    }

    private static async Task<IResult> Handle(string userName, ISender sender)
    {
        var request = new GetBasketRequest(userName);
        var result = await sender.Send(request);
        var response = result.Adapt<GetBasketResponse>();

        return Results.Ok(response);
    }
}
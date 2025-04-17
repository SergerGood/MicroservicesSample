using Basket.API.Dtos;

namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketRequest(BasketCheckoutDto BasketCheckout);

public record CheckoutBasketResponse(bool IsSuccess);

public class CheckoutBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/checkout", Handle)
            .WithName("CheckoutBasket")
            .Produces<CheckoutBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Checkout basket")
            .WithDescription("Checkout basket");
    }

    private static async Task<IResult> Handle(CheckoutBasketRequest request, ISender sender)
    {
        var command = request.Adapt<CheckoutBasketCommand>();
        var result = await sender.Send(command);
        var response = result.Adapt<CheckoutBasketResponse>();

        return Results.Ok(response);
    }
}
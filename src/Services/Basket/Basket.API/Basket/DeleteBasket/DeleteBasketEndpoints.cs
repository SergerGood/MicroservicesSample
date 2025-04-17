namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketResponse(bool IsSuccess);

public class DeleteBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", Handle)
            .WithName("DeleteProduct")
            .Produces<DeleteBasketResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Product")
            .WithDescription("Delete Product");
    }

    private static async Task<IResult> Handle(string userName, ISender sender)
    {
        var result = await sender.Send(new DeleteBasketCommand(userName));
        var response = result.Adapt<DeleteBasketResult>();

        return Results.Ok(response);
    }
}
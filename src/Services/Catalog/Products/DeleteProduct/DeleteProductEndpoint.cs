namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductResponse(bool IsSuccess);

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id:guid}", Handle)
            .WithName("DeleteProduct")
            .Produces<DeleteProductResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Deletes a product")
            .WithDescription("Deletes a product from the catalog");
    }

    private static async Task<IResult> Handle(Guid id, ISender sender)
    {
        var command = new DeleteProductCommand(id);
        var result = await sender.Send(command);
        var response = result.Adapt<DeleteProductResponse>();

        return Results.Ok(response);
    }
}
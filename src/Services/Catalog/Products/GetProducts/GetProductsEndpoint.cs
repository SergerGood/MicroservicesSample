namespace Catalog.API.Products.GetProducts;

public record GetProductsResponse(IEnumerable<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", Handle)
            .WithName("GetProducts")
            .Produces<GetProductsResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get products")
            .WithDescription("Get products");
    }

    private static async Task<IResult> Handle(ISender sender)
    {
        var result = await sender.Send(new GetProductsQuery());
        var response = result.Adapt<GetProductsResult>();

        return Results.Ok(response);
    }
}
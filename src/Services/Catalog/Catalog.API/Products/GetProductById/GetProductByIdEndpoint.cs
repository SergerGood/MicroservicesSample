namespace Catalog.API.Products.GetProductById;

public record GetProductByIdResponse(Product Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", Handle)
            .WithName("GetProductById")
            .Produces<GetProductByIdResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get product by Id")
            .WithDescription("Get product by Id from the catalog");
    }

    private static async Task<IResult> Handle(Guid id, ISender sender)
    {
        var query = new GetProductByIdQuery(id);
        var result = await sender.Send(query);
        var response = result.Adapt<GetProductByIdResponse>();

        return Results.Ok(response);
    }
}
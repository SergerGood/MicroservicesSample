namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price);

public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", Handler)
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Creates a new product")
            .WithDescription("Creates a new product in the catalog");
    }

    private static async Task<IResult> Handler(CreateProductRequest request, ISender sender)
    {
        var command = request.Adapt<CreateProductCommand>();
        var result = await sender.Send(command);
        var response = result.Adapt<CreateProductResponse>();

        return Results.Created($"/products/{response.Id}", response);
    }
}
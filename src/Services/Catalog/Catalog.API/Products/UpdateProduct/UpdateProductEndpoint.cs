﻿namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(
    Guid Id,
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price);

public record UpdateProductResponse(bool IsSuccess);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", Handle)
            .WithName("UpdateProduct")
            .Produces<UpdateProductResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Updates a product")
            .WithDescription("Updates a product in the catalog");
    }

    private static async Task<IResult> Handle(UpdateProductRequest request, ISender sender)
    {
        var command = request.Adapt<UpdateProductCommand>();
        var result = await sender.Send(command);
        var response = result.Adapt<UpdateProductResponse>();

        return Results.Ok(response);
    }
}
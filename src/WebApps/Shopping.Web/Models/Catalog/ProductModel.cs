﻿namespace Shopping.Web.Models.Catalog;

public class ProductModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public List<string> Category { get; set; } = [];
    public string Description { get; set; } = null!;
    public string ImageFile { get; set; } = null!;
    public decimal Price { get; set; }
}

public record GetProductsResponse(IEnumerable<ProductModel> Products);

public record GetProductByIdResponse(ProductModel Product);

public record GetProductByCategoryResponse(IEnumerable<ProductModel> Products);
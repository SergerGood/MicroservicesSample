﻿using Shopping.Web.Models.Catalog;

namespace Shopping.Web.Pages;

public class ProductListModel(
    ICatalogService catalogService,
    IBasketService basketService,
    ILogger<ProductListModel> logger)
    : PageModel
{
    public IEnumerable<string> CategoryList { get; set; } = [];
    public IEnumerable<ProductModel> ProductList { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    public string? SelectedCategory { get; set; }

    public async Task<IActionResult> OnGetAsync(string categoryName)
    {
        var response = await catalogService.GetProducts();

        CategoryList = response.Products.SelectMany(x => x.Category).Distinct();

        if (!string.IsNullOrEmpty(categoryName))
        {
            ProductList = response.Products.Where(x => x.Category.Contains(categoryName));
            SelectedCategory = categoryName;
        }
        else
        {
            ProductList = response.Products;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
    {
        logger.LogInformation("Add to cart button clicked");

        var productResponse = await catalogService.GetProductById(productId);
        var basket = await basketService.LoadUserBasket();

        basket.Items.Add(new ShoppingCartItemModel
        {
            ProductId = productId,
            ProductName = productResponse.Product.Name,
            Price = productResponse.Product.Price,
            Quantity = 1,
            Color = "Black"
        });

        await basketService.StoreBasket(new StoreBasketRequest(basket));

        return RedirectToPage("Cart");
    }
}
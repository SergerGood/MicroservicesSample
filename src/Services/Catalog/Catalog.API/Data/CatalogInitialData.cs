using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        await using var session = store.LightweightSession();

        if (await session.Query<Product>().AnyAsync(cancellation))
        {
            return;
        }

        session.Store(GetPreconfiguredProducts());
        await session.SaveChangesAsync(cancellation);
    }

    private static IEnumerable<Product> GetPreconfiguredProducts()
    {
        yield return new Product
        {
            Id = new Guid("01953286-fb4a-4a43-a727-f21d8a6a81dd"),
            Name = "LG G7",
            Description = "This is a phone",
            ImageFile = "product-lg.png",
            Price = 240.00M,
            Category = ["Smart Phone"]
        };
    }
}
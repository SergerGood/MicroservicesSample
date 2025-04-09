var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddRefitClient<ICatalogService>()
    .ConfigureHttpClient(client => ConfigureClient(client, builder));

builder.Services.AddRefitClient<IBasketService>()
    .ConfigureHttpClient(client => ConfigureClient(client, builder));

builder.Services.AddRefitClient<IOrderingService>()
    .ConfigureHttpClient(client => ConfigureClient(client, builder));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();
return;

void ConfigureClient(HttpClient httpClient, WebApplicationBuilder webApplicationBuilder)
{
    var uriString = webApplicationBuilder.Configuration["ApiSettings:GatewayAddress"]
        ?? throw new InvalidOperationException("Gateway Address not configured");

    httpClient.BaseAddress = new Uri(uriString);
}
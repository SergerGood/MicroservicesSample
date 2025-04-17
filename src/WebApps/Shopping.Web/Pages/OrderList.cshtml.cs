using Shopping.Web.Models.Ordering;

namespace Shopping.Web.Pages;

public class OrderListModel(IOrderingService orderingService) : PageModel
{
    public IEnumerable<OrderModel> Orders { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        var customerId = new Guid("F3A5F583-675E-47E8-A4DB-68477E4DD787");

        var response = await orderingService.GetOrdersByCustomer(customerId);
        Orders = response.Orders;

        return Page();
    }
}
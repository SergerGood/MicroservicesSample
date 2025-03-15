namespace Ordering.Infrastructure.Data.Extensions;

public class InitialData
{
    public static IEnumerable<Customer> Customers =>
    [
        Customer.Create(CustomerId.Of(new Guid("F3A5F583-675E-47E8-A4DB-68477E4DD787")), "bill", "bill@gmail.com"),
        Customer.Create(CustomerId.Of(new Guid("C337CC1B-069C-474E-88C1-FB7282D5F52A")), "john", "john@gmail.com")
    ];

    public static IEnumerable<Product> Products =>
    [
        Product.Create(ProductId.Of(new Guid("6538770E-1C26-40CF-BDC5-7070736C489D")), "Samsung", 100),
        Product.Create(ProductId.Of(new Guid("436E143C-B470-4350-8C2C-768B95CA29FA")), "Iphone X", 111)
    ];

    public static IEnumerable<Order> OrdersWithItems
    {
        get
        {
            var address1 = Address.Of("mehmet", "ozkaya", "mehmet@gmail.com", "Bahcelievler No:4", "Turkey", "Istanbul", "38050");
            var address2 = Address.Of("john", "doe", "john@gmail.com", "Broadway No:1", "England", "Nottingham", "08050");

            var payment1 = Payment.Of("mehmet", "5555555555554444", "12/28", "355", "1");
            var payment2 = Payment.Of("john", "8885555555554444", "06/30", "222", "2");

            var order1 = Order.Create(
                OrderId.Of(Guid.NewGuid()),
                CustomerId.Of(new Guid("F3A5F583-675E-47E8-A4DB-68477E4DD787")),
                OrderName.Of("ORD_1"),
                address1,
                address1,
                payment1);
            order1.Add(ProductId.Of(new Guid("6538770E-1C26-40CF-BDC5-7070736C489D")), 2, 500);
            order1.Add(ProductId.Of(new Guid("436E143C-B470-4350-8C2C-768B95CA29FA")), 1, 400);

            var order2 = Order.Create(
                OrderId.Of(Guid.NewGuid()),
                CustomerId.Of(new Guid("C337CC1B-069C-474E-88C1-FB7282D5F52A")),
                OrderName.Of("ORD_2"),
                address2,
                address2,
                payment2);
            order2.Add(ProductId.Of(new Guid("6538770E-1C26-40CF-BDC5-7070736C489D")), 1, 650);
            order2.Add(ProductId.Of(new Guid("436E143C-B470-4350-8C2C-768B95CA29FA")), 2, 450);

            return [order1, order2];
        }
    }
}
namespace Ordering.Domain.Models;

public class Customer(string name, string email) : Entity<CustomerId>
{
    public string Name { get; } = name;
    public string Email { get; } = email;
}
namespace Ordering.Domain.Models;

public class Customer(string name, string email) : Entity<Guid>
{
    public string Name { get; } = name;
    public string Email { get; } = email;
}
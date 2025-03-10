namespace Ordering.Domain.ValueObjects;

public record CustomerId
{
    private CustomerId(Guid value) => Value = value;

    public Guid Value { get; }

    public static CustomerId Of(Guid value)
    {
        if (value == Guid.Empty)
            throw new DomainException("Customer id cannot be empty");

        return new CustomerId(value);
    }
}
namespace BuildingBlocks.Messaging.Events;

public record BasketCheckoutEvent : IntegrationEvent
{
    public required string UserName { get; init; }
    public required Guid CustomerId { get; init; }
    public decimal TotalPrice { get; set; }

    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string EmailAddress { get; init; }
    public required string AddressLine { get; init; }
    public required string Country { get; init; }
    public required string State { get; init; }
    public required string ZipCode { get; init; }

    public required string CardName { get; init; }
    public required string CardNumber { get; init; }
    public required string Expiration { get; init; }
    public required string Cvv { get; init; }
    public required string PaymentMethod { get; init; }
}
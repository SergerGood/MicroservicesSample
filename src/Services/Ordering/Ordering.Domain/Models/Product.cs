﻿namespace Ordering.Domain.Models;

public class Product(string name, decimal price) : Entity<ProductId>
{
    public string Name { get; } = name;
    public decimal Price { get; } = price;
}
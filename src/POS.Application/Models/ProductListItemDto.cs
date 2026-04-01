namespace POS.Application.Models;

public sealed record ProductListItemDto(
    Guid Id,
    string Name,
    string? Barcode,
    decimal Price,
    decimal QuantityOnHand,
    string? ImagePath);

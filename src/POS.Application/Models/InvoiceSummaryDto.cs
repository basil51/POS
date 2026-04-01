namespace POS.Application.Models;

/// <summary>Totals breakdown for the current invoice, displayed in the cart panel.</summary>
public sealed record InvoiceSummaryDto(
    decimal Subtotal,
    decimal TaxPercent,
    decimal TaxAmount,
    decimal Total);

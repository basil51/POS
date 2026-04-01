namespace POS.Core.Enums;

public enum InvoiceStatus
{
    Open      = 0,
    Paid      = 1,
    Cancelled = 2,
    Held      = 3   // parked mid-sale; stock not yet decremented
}

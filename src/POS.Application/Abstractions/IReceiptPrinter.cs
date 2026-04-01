using POS.Application.Models;

namespace POS.Application.Abstractions;

public interface IReceiptPrinter
{
    void Print(ReceiptDto receipt);
}

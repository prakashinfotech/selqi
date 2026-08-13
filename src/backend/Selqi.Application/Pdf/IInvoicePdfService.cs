namespace Selqi.Application.Pdf;

public interface IInvoicePdfService
{
    byte[] Generate(InvoiceData data);
}

public sealed record InvoiceData(
    string OrderNumber,
    DateTime PlacedAt,
    string CustomerName,
    string CustomerEmail,
    string DeliveryAddress,
    IReadOnlyList<InvoiceLineItem> Items,
    decimal SubTotal,
    decimal DeliveryCharge,
    decimal Total);

public sealed record InvoiceLineItem(string Name, string Sku, int Qty, decimal UnitPrice);

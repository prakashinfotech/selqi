using Selqi.Application.Pdf;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Selqi.Infrastructure.Pdf;

internal sealed class QuestPdfInvoiceService : IInvoicePdfService
{
    public byte[] Generate(InvoiceData data)
    {
        return Document.Create(doc =>
        {
            doc.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(10).FontColor(Colors.Black));

                page.Content().Column(col =>
                {
                    // Header
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("Selqi").Bold().FontSize(22).FontColor("#4f46e5");
                            c.Item().Text("selqi.co.uk").FontSize(9).FontColor("#888888");
                        });
                        row.ConstantItem(160).Column(c =>
                        {
                            c.Item().Text("INVOICE").Bold().FontSize(16).AlignRight().FontColor("#333333");
                            c.Item().Text($"Order {data.OrderNumber}").FontSize(9).AlignRight().FontColor("#555555");
                            c.Item().Text(data.PlacedAt.ToString("dd MMM yyyy, HH:mm")).FontSize(9).AlignRight().FontColor("#555555");
                        });
                    });

                    col.Item().PaddingVertical(12).LineHorizontal(1).LineColor("#e5e5e5");

                    // Bill to / Deliver to
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("Bill to").Bold().FontSize(9).FontColor("#888888");
                            c.Item().PaddingTop(4).Text(data.CustomerName).Bold();
                            c.Item().Text(data.CustomerEmail).FontColor("#555555");
                        });
                        row.ConstantItem(200).Column(c =>
                        {
                            c.Item().Text("Deliver to").Bold().FontSize(9).FontColor("#888888");
                            c.Item().PaddingTop(4).Text(data.DeliveryAddress).FontColor("#333333").LineHeight(1.4f);
                        });
                    });

                    col.Item().PaddingVertical(16).LineHorizontal(1).LineColor("#e5e5e5");

                    // Items table
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn(4);
                            cols.RelativeColumn(2);
                            cols.ConstantColumn(40);
                            cols.ConstantColumn(70);
                            cols.ConstantColumn(70);
                        });

                        // Header row
                        static IContainer HeaderCell(IContainer c) => c
                            .Background("#f5f5f5").Border(1).BorderColor("#e5e5e5")
                            .Padding(8).AlignMiddle();

                        table.Header(h =>
                        {
                            h.Cell().Element(HeaderCell).Text("Product").Bold().FontSize(9).FontColor("#767676");
                            h.Cell().Element(HeaderCell).Text("SKU").Bold().FontSize(9).FontColor("#767676");
                            h.Cell().Element(HeaderCell).AlignCenter().Text("Qty").Bold().FontSize(9).FontColor("#767676");
                            h.Cell().Element(HeaderCell).AlignRight().Text("Unit Price").Bold().FontSize(9).FontColor("#767676");
                            h.Cell().Element(HeaderCell).AlignRight().Text("Line Total").Bold().FontSize(9).FontColor("#767676");
                        });

                        // Data rows
                        static IContainer DataCell(IContainer c) => c
                            .Border(1).BorderColor("#f0f0f0").Padding(8).AlignMiddle();

                        foreach (var item in data.Items)
                        {
                            table.Cell().Element(DataCell).Text(item.Name);
                            table.Cell().Element(DataCell).Text(item.Sku).FontColor("#888888").FontSize(9);
                            table.Cell().Element(DataCell).AlignCenter().Text(item.Qty.ToString());
                            table.Cell().Element(DataCell).AlignRight().Text($"£{item.UnitPrice:F2}");
                            table.Cell().Element(DataCell).AlignRight().Text($"£{item.UnitPrice * item.Qty:F2}").Bold();
                        }
                    });

                    col.Item().PaddingTop(12).AlignRight().Column(totals =>
                    {
                        void TotalRow(string label, decimal amount, bool bold = false)
                        {
                            totals.Item().Row(row =>
                            {
                                row.ConstantItem(140).Text(label).FontColor("#555555");
                                var valueCell = row.ConstantItem(80).AlignRight();
                                if (bold)
                                    valueCell.Text($"£{amount:F2}").Bold().FontColor("#4f46e5").FontSize(13);
                                else
                                    valueCell.Text($"£{amount:F2}").FontColor("#333333").FontSize(10);
                            });
                        }

                        TotalRow("Subtotal", data.SubTotal);
                        TotalRow("Delivery", data.DeliveryCharge);
                        totals.Item().PaddingVertical(4).LineHorizontal(1).LineColor("#e5e5e5");
                        TotalRow("Total", data.Total, bold: true);
                    });

                    col.Item().PaddingTop(32).PaddingBottom(8).LineHorizontal(1).LineColor("#e5e5e5");
                    col.Item().Text("Thank you for shopping with Selqi.")
                        .FontSize(9).FontColor("#888888").AlignCenter();
                    col.Item().Text("489-499 Avebury Blvd, Milton Keynes, MK9 2NW · selqi.co.uk")
                        .FontSize(8).FontColor("#aaaaaa").AlignCenter();
                });
            });
        }).GeneratePdf();
    }
}

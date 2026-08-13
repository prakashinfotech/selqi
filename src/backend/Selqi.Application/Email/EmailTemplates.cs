namespace Selqi.Application.Email;

internal static class EmailTemplates
{
    private const string Brand = "#4f46e5";
    private const string BrandDark = "#3730a3";
    private const string TextMain = "#1c1c1c";
    private const string TextMuted = "#555555";
    private const string BgPage = "#f5f5f5";
    private const string BgCard = "#ffffff";

    public static string PasswordReset(string firstName, string resetUrl) => Layout(
        previewText: "Reset your Selqi password",
        body: $"""
            {Greeting(firstName)}
            <p style="margin:0 0 20px;font-size:15px;line-height:1.6;color:{TextMuted};">
              We received a request to reset the password for your Selqi account.
              Click the button below to choose a new password. This link expires in
              <strong>1 hour</strong>.
            </p>
            {CtaButton("Reset my password", resetUrl)}
            <p style="margin:24px 0 0;font-size:13px;color:{TextMuted};">
              If you didn't request a password reset, you can safely ignore this email —
              your password will not be changed.
            </p>
            <p style="margin:12px 0 0;font-size:13px;color:{TextMuted};">
              Or copy and paste this URL into your browser:<br>
              <a href="{resetUrl}" style="color:{Brand};word-break:break-all;">{resetUrl}</a>
            </p>
        """);

    public static string OrderConfirmed(
        string firstName,
        string orderNumber,
        decimal total,
        IList<(string Name, int Qty, decimal UnitPrice)> items,
        string deliveryAddress) => Layout(
        previewText: $"Order {orderNumber} confirmed — thank you!",
        body: $"""
            {Greeting(firstName)}
            <p style="margin:0 0 20px;font-size:15px;line-height:1.6;color:{TextMuted};">
              Thanks for your order! We've received it and it's being prepared.
            </p>
            {OrderBadge(orderNumber, "Confirmed", "#008542")}
            {ItemsTable(items)}
            {OrderTotal(total)}
            <p style="margin:20px 0 4px;font-size:14px;font-weight:700;color:{TextMain};">Delivering to</p>
            <p style="margin:0 0 24px;font-size:14px;color:{TextMuted};">{deliveryAddress}</p>
            {CtaButton("View my order", "#")}
        """);

    public static string OrderProcessing(string firstName, string orderNumber) => Layout(
        previewText: $"Your order {orderNumber} is being processed",
        body: $"""
            {Greeting(firstName)}
            <p style="margin:0 0 20px;font-size:15px;line-height:1.6;color:{TextMuted};">
              Good news — your order is now being processed and prepared for dispatch.
              We'll send you another update once it has shipped.
            </p>
            {OrderBadge(orderNumber, "Processing", "#1e40af")}
            <p style="margin:20px 0 0;font-size:13px;color:{TextMuted};">
              If you have any questions, please don't hesitate to contact us.
            </p>
            {CtaButton("View my order", "#")}
        """);

    public static string OrderShipped(
        string firstName,
        string orderNumber,
        decimal total,
        IList<(string Name, int Qty, decimal UnitPrice)> items,
        string deliveryAddress) => Layout(
        previewText: $"Your order {orderNumber} is on its way!",
        body: $"""
            {Greeting(firstName)}
            <p style="margin:0 0 20px;font-size:15px;line-height:1.6;color:{TextMuted};">
              Great news — your order is on its way to you!
            </p>
            {OrderBadge(orderNumber, "Shipped", "#0c509f")}
            {ItemsTable(items)}
            {OrderTotal(total)}
            <p style="margin:20px 0 4px;font-size:14px;font-weight:700;color:{TextMain};">Delivering to</p>
            <p style="margin:0 0 24px;font-size:14px;color:{TextMuted};">{deliveryAddress}</p>
            {CtaButton("Track my order", "#")}
        """);

    public static string OrderDelivered(string firstName, string orderNumber, decimal total) => Layout(
        previewText: $"Your order {orderNumber} has been delivered",
        body: $"""
            {Greeting(firstName)}
            <p style="margin:0 0 20px;font-size:15px;line-height:1.6;color:{TextMuted};">
              Your order has been delivered. We hope you enjoy your purchase!
            </p>
            {OrderBadge(orderNumber, "Delivered", "#008542")}
            {OrderTotal(total)}
            <p style="margin:20px 0 20px;font-size:15px;line-height:1.6;color:{TextMuted};">
              If anything isn't quite right, our team is here to help.
            </p>
            {CtaButton("View my order", "#")}
        """);

    public static string OrderCancelled(string firstName, string orderNumber, decimal total) => Layout(
        previewText: $"Your order {orderNumber} has been cancelled",
        body: $"""
            {Greeting(firstName)}
            <p style="margin:0 0 20px;font-size:15px;line-height:1.6;color:{TextMuted};">
              Your order has been cancelled. If you were charged, a full refund will
              be returned to your original payment method within 3–5 business days.
            </p>
            {OrderBadge(orderNumber, "Cancelled", "#4f46e5")}
            {OrderTotal(total)}
            <p style="margin:20px 0 20px;font-size:15px;line-height:1.6;color:{TextMuted};">
              If you have any questions about your cancellation, please contact us.
            </p>
            {CtaButton("Contact support", "#")}
        """);

    private static string Greeting(string firstName) =>
        $"""<p style="margin:0 0 16px;font-size:18px;font-weight:700;color:{TextMain};">Hi {firstName},</p>""";

    private static string CtaButton(string label, string url) => $"""
        <table width="100%" cellpadding="0" cellspacing="0" style="margin:28px 0;">
          <tr>
            <td align="center">
              <a href="{url}"
                 style="display:inline-block;padding:14px 36px;background:{Brand};color:#ffffff;
                        font-size:15px;font-weight:700;text-decoration:none;border-radius:4px;">
                {label}
              </a>
            </td>
          </tr>
        </table>
        """;

    private static string OrderBadge(string orderNumber, string status, string statusColor) => $"""
        <table width="100%" cellpadding="0" cellspacing="0"
               style="margin:0 0 20px;border:1px solid #e5e5e5;border-radius:6px;overflow:hidden;">
          <tr style="background:#fafafa;">
            <td style="padding:12px 16px;font-size:13px;color:{TextMuted};">Order number</td>
            <td style="padding:12px 16px;font-size:13px;text-align:right;">
              <span style="display:inline-block;padding:3px 10px;border-radius:99px;
                           background:{statusColor};color:#fff;font-size:12px;font-weight:700;">
                {status}
              </span>
            </td>
          </tr>
          <tr>
            <td colspan="2" style="padding:10px 16px;font-size:16px;font-weight:700;color:{TextMain};">
              {orderNumber}
            </td>
          </tr>
        </table>
        """;

    private static string ItemsTable(IList<(string Name, int Qty, decimal UnitPrice)> items)
    {
        var rows = string.Join('\n', items.Select(i => $"""
            <tr>
              <td style="padding:10px 0;font-size:14px;color:{TextMain};border-bottom:1px solid #f0f0f0;">
                {i.Name}
              </td>
              <td style="padding:10px 0;font-size:14px;color:{TextMuted};text-align:center;
                         border-bottom:1px solid #f0f0f0;">
                ×{i.Qty}
              </td>
              <td style="padding:10px 0;font-size:14px;color:{TextMain};text-align:right;
                         border-bottom:1px solid #f0f0f0;font-weight:600;">
                £{i.UnitPrice * i.Qty:F2}
              </td>
            </tr>
            """));

        return $"""
            <table width="100%" cellpadding="0" cellspacing="0" style="margin:0 0 4px;">
              <thead>
                <tr>
                  <th style="padding:0 0 8px;font-size:12px;font-weight:700;color:{TextMuted};
                             text-align:left;border-bottom:2px solid #e5e5e5;">Item</th>
                  <th style="padding:0 0 8px;font-size:12px;font-weight:700;color:{TextMuted};
                             text-align:center;border-bottom:2px solid #e5e5e5;">Qty</th>
                  <th style="padding:0 0 8px;font-size:12px;font-weight:700;color:{TextMuted};
                             text-align:right;border-bottom:2px solid #e5e5e5;">Price</th>
                </tr>
              </thead>
              <tbody>
                {rows}
              </tbody>
            </table>
            """;
    }

    private static string OrderTotal(decimal total) => $"""
        <table width="100%" cellpadding="0" cellspacing="0" style="margin:0 0 8px;">
          <tr>
            <td style="padding:12px 0;font-size:16px;font-weight:700;color:{TextMain};">
              Order total
            </td>
            <td style="padding:12px 0;font-size:18px;font-weight:900;color:{Brand};text-align:right;">
              £{total:F2}
            </td>
          </tr>
        </table>
        """;

    private static string Layout(string previewText, string body) => $"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
          <meta charset="UTF-8">
          <meta name="viewport" content="width=device-width,initial-scale=1">
          <meta http-equiv="X-UA-Compatible" content="IE=edge">
          <title>Selqi</title>
        </head>
        <body style="margin:0;padding:0;background:{BgPage};font-family:-apple-system,BlinkMacSystemFont,'Segoe UI',Arial,sans-serif;">
          <div style="display:none;max-height:0;overflow:hidden;color:{BgPage};">
            {previewText}&#847;&zwnj;&#847;&zwnj;&#847;&zwnj;&#847;&zwnj;
          </div>
          <table width="100%" cellpadding="0" cellspacing="0" style="background:{BgPage};padding:32px 16px;">
            <tr>
              <td align="center">
                <table width="100%" cellpadding="0" cellspacing="0"
                       style="max-width:560px;background:{BgCard};border-radius:8px;
                              overflow:hidden;box-shadow:0 2px 8px rgba(0,0,0,0.06);">
                  <tr>
                    <td style="background:{Brand};padding:24px 32px;text-align:center;">
                      <span style="display:inline-block;background:#fff;color:{Brand};
                                   font-size:20px;font-weight:900;padding:6px 18px;
                                   border-radius:4px;letter-spacing:0.5px;">
                        Selqi
                      </span>
                    </td>
                  </tr>
                  <tr>
                    <td style="padding:32px 32px 24px;">
                      {body}
                    </td>
                  </tr>
                  <tr>
                    <td style="background:#fafafa;border-top:1px solid #eeeeee;
                               padding:20px 32px;text-align:center;">
                      <p style="margin:0 0 6px;font-size:12px;color:{TextMuted};">
                        © {DateTime.UtcNow.Year} Selqi (learning project). All rights reserved.
                      </p>
                      <p style="margin:0;font-size:12px;color:#aaaaaa;">
                        489-499 Avebury Blvd, Milton Keynes, MK9 2NW
                      </p>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </body>
        </html>
        """;
}

using System.Net;
using System.Net.Mail;
using Selqi.Application.Email;
using Microsoft.Extensions.Options;

namespace Selqi.Infrastructure.Email;

public sealed class SmtpEmailService(IOptions<EmailOptions> options) : IEmailService
{
    public Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default)
        => SendCoreAsync(to, subject, htmlBody, null, null, null, ct);

    public Task SendWithAttachmentAsync(
        string to,
        string subject,
        string htmlBody,
        string attachmentName,
        byte[] attachment,
        CancellationToken ct = default)
        => SendCoreAsync(to, subject, htmlBody, attachmentName, attachment, "application/pdf", ct);

    private async Task SendCoreAsync(
        string to,
        string subject,
        string htmlBody,
        string? attachmentName,
        byte[]? attachmentBytes,
        string? mediaType,
        CancellationToken ct)
    {
        var opts = options.Value;

        using var message = new MailMessage
        {
            From = new MailAddress(opts.SenderEmail, opts.SenderName),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(to);

        MemoryStream? attachStream = null;
        if (attachmentBytes is not null && attachmentName is not null)
        {
            attachStream = new MemoryStream(attachmentBytes);
            message.Attachments.Add(new Attachment(attachStream, attachmentName, mediaType));
        }

        try
        {
            using var client = new SmtpClient(opts.Host, opts.Port)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(opts.SenderEmail, opts.Password)
            };
            await client.SendMailAsync(message, ct);
        }
        finally
        {
            attachStream?.Dispose();
        }
    }
}

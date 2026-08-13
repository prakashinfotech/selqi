namespace Selqi.Application.Email;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default);

    Task SendWithAttachmentAsync(
        string to,
        string subject,
        string htmlBody,
        string attachmentName,
        byte[] attachment,
        CancellationToken ct = default);
}

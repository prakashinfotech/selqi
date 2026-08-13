namespace Selqi.Infrastructure.Email;

public sealed class EmailOptions
{
    public const string SectionName = "Email";

    public string SenderEmail { get; set; } = string.Empty;
    public string SenderName { get; set; } = "Selqi";
    public string Password { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public string FrontendUrl { get; set; } = "http://localhost:4200";
}

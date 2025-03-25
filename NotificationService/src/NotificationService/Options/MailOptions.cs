namespace NotificationService.Options;

/// <summary>
/// Опции почты
/// </summary>
public class MailOptions
{
    public const string SECTION_NAME = "MailOptions";

    public string From { get; set; } = string.Empty;
    
    public string FromDisplayName { get; set; } = string.Empty;
    
    public string Host { get; set; } = string.Empty;
    
    public int Port { get; set; }
    
    public string UserName { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public bool UseSSL { get; set; }
}
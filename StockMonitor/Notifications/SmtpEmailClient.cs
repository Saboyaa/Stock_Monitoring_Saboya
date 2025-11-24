using System.Net;
using System.Net.Mail;

namespace StockMonitor.Notifications;

public class SmtpEmailClient : IEmailClient
{
    private readonly EmailConfig _config;

    public SmtpEmailClient(EmailConfig config)
    {
        _config = config;
    }

    public void Send(string from, string to, string subject, string body)
    {
        using var client = new SmtpClient(_config.SmtpHost, _config.SmtpPort)
        {
            EnableSsl = _config.EnableSsl,
            Timeout = 10000
        };

        if (!string.IsNullOrWhiteSpace(_config.SmtpUser))
            client.Credentials = new NetworkCredential(_config.SmtpUser, _config.SmtpPass);

        var mail = new MailMessage(from, to)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };

        client.Send(mail);
    }
}

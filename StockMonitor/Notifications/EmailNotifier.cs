using System;
using System.Net;
using System.Net.Mail;
using DotNetEnv;

namespace StockMonitor.Notifications;

public class EmailNotifier : INotifier
{
    private readonly string _from;
    private readonly string _to;
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string? _smtpUser;
    private readonly string? _smtpPass;
    private readonly bool _enableSsl;

    public EmailNotifier()
    {
        // Lê do .env (DotNetEnv)
        _from = Env.GetString("MAIL_FROM") ?? "noreply@example.com";
        _to = Env.GetString("MAIL_TO") ?? _from;
        _smtpHost = Env.GetString("SMTP_HOST") ?? "localhost";

        // SMTP_PORT pode ser string no .env -> parse com fallback
        var smtpPortStr = Env.GetString("SMTP_PORT");
        if (!int.TryParse(smtpPortStr, out _smtpPort))
            _smtpPort = 25;

        _smtpUser = Env.GetString("SMTP_USER");
        _smtpPass = Env.GetString("SMTP_PASS");

        // SMTP_SSL pode ser "true"/"false" -> parse com fallback
        var smtpSslStr = Env.GetString("SMTP_SSL");
        if (!bool.TryParse(smtpSslStr, out _enableSsl))
            _enableSsl = false;
    }

    public void Notify(string message)
    {
        try
        {
            using var client = new SmtpClient(_smtpHost, _smtpPort)
            {
                EnableSsl = _enableSsl
            };

            if (!string.IsNullOrWhiteSpace(_smtpUser))
                client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);

            var mail = new MailMessage(_from, _to)
            {
                Subject = "StockMonitor - Alerta de preço",
                Body = message
            };

            client.Send(mail);
        }
        catch (Exception ex)
        {
            // Não queremos quebrar a aplicação se o envio falhar — apenas log
            Console.WriteLine($"[WARN] EmailNotifier falhou: {ex.Message}");
        }
    }
}

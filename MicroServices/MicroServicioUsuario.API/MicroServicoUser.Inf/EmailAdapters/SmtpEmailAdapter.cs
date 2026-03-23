using System.Net;
using System.Net.Mail;
using MicroServicioUser.Dom.Entities;
using MicroServicioUser.Dom.Interfaces;

namespace MicroServicoUser.Inf.EmailAdapters;



public class SmtpEmailAdapter : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public SmtpEmailAdapter(SmtpSettings smtpSettings)
    {
        _smtpSettings = smtpSettings;
    }
    public async Task<bool> SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true)
    {
        try
        {
            var smtpHost = _smtpSettings.Host;
            var smtpPort = _smtpSettings.Port;
            var smtpUser = _smtpSettings.User;
            var smtpPass = _smtpSettings.Password;
            var fromEmail = _smtpSettings.FromEmail;
            var fromName = _smtpSettings.FromName;

            if (string.IsNullOrWhiteSpace(smtpHost) || string.IsNullOrWhiteSpace(smtpUser))
            {
<<<<<<< HEAD
=======
               // _logger.LogWarning("Email configuration is missing. Email will not be sent.");
>>>>>>> AnalisisSonarEstablishment
                return false;
            }

            using var message = new MailMessage();
            message.From = new MailAddress(fromEmail ?? smtpUser, fromName);
            message.To.Add(new MailAddress(toEmail));
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = isHtml;

            using var smtpClient = new SmtpClient(smtpHost, smtpPort);
            smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);
            smtpClient.EnableSsl = true;

            await smtpClient.SendMailAsync(message);
<<<<<<< HEAD
            return true;
        }
        catch 
        {
=======
           // _logger.LogInformation("Email sent successfully to {Email}", toEmail);
            return true;
        }
        catch (Exception ex)
        {
            // _logger.LogError(ex, "Failed to send email to {Email}", toEmail);
>>>>>>> AnalisisSonarEstablishment
            return false;
        }
    }
}
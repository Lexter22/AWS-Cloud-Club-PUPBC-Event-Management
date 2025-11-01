
using System;
using MailKit.Net.Smtp;
using MimeKit;
using AWSCloudClub_Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MailKit.Security;

namespace AWSCloudClub_BusinessLogic
{
    public class EventEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EventEmailService> _logger;

        public EventEmailService(IOptions<EmailSettings> options, ILogger<EventEmailService> logger)
        {
            _settings = options?.Value ?? new EmailSettings();
            _logger = logger;
        }
        public async System.Threading.Tasks.Task SendEmailAsync(string recipientEmail, Events ev, System.Threading.CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(recipientEmail)) throw new ArgumentException("recipientEmail is required", nameof(recipientEmail));
            if (ev == null) throw new ArgumentNullException(nameof(ev));

            var fromName = _settings.FromName ?? "AWS Cloud Club";
            var fromAddress = _settings.FromAddress ?? "no-reply@awscloudclub.local";
            var toName = _settings.ToName ?? "Recipient";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromAddress));
            message.To.Add(new MailboxAddress(toName, recipientEmail));

            var safeEventName = string.IsNullOrWhiteSpace(ev.EventName) ? "(no title)" : ev.EventName;
            message.Subject = $"AWS Cloud Club - New Event: {safeEventName}";

            var textBody =
                "A new event has been created on AWS Cloud Club." +
                "\n\n" +
                $"Event ID: {ev.EventID}\n" +
                $"Event Name: {ev.EventName}\n" +
                $"Event Date: {ev.EventDate}\n" +
                $"Description: {ev.Description}\n\n" +
                "--\nAWS Cloud Club";

            string HtmlEncode(string s) => System.Net.WebUtility.HtmlEncode(s ?? string.Empty);
            var htmlBody = $@"<html><body>
            <h2>New event created</h2>
                <p>A new event has been created on AWS Cloud Club.</p>
                    <table>
                        <tr><td><strong>Event ID:</strong></td><td>{HtmlEncode(ev.EventID)}</td></tr>
                        <tr><td><strong>Event Name:</strong></td><td>{HtmlEncode(ev.EventName)}</td></tr>
                        <tr><td><strong>Event Date:</strong></td><td>{HtmlEncode(ev.EventDate)}</td></tr>
                        <tr><td><strong>Description:</strong></td><td>{HtmlEncode(ev.Description).Replace("\n", "<br/>")}</td></tr>
                        </table>
                <p>--<br/>AWS Cloud Club</p>
            </body></html>";

            var builder = new BodyBuilder
            {
                TextBody = textBody,
                HtmlBody = htmlBody
            };
            message.Body = builder.ToMessageBody();

            var host = _settings.SmtpHost ?? "sandbox.smtp.mailtrap.io";
            var port = (_settings.SmtpPort > 0) ? _settings.SmtpPort : 2525;
            var username = _settings.SmtpUsername ?? string.Empty;
            var password = _settings.SmtpPassword ?? string.Empty;
            var enableTls = _settings.EnableTls;

            try
            {
                using var client = new SmtpClient();
                var socketOption = enableTls ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
                await client.ConnectAsync(host, port, socketOption, cancellationToken).ConfigureAwait(false);

                if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
                {
                    await client.AuthenticateAsync(username, password, cancellationToken).ConfigureAwait(false);
                }

                await client.SendAsync(message, cancellationToken).ConfigureAwait(false);
                await client.DisconnectAsync(true, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
               
                _logger?.LogError(ex, "Failed to send event email (async)");
            }
        }
        public async System.Threading.Tasks.Task SendEmailAsync(Events ev, System.Threading.CancellationToken cancellationToken = default)
        {
            var configuredRecipient = _settings.ToAddress ?? string.Empty;
            if (string.IsNullOrWhiteSpace(configuredRecipient))
            {
                throw new InvalidOperationException("No recipient configured!");
            }

            await SendEmailAsync(configuredRecipient, ev, cancellationToken).ConfigureAwait(false);
        }
    }
}


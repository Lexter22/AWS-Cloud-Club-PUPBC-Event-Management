
using System;
using MailKit.Net.Smtp;
using MimeKit;
using AWSCloudClub_Common.Models;
using Microsoft.Extensions.Configuration;
using MailKit.Security;

namespace AWSCloudClub_BusinessLogic
{
    public class EventEmailService
    {
        private readonly IConfiguration _configuration;

        public EventEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            
        }
        public async System.Threading.Tasks.Task SendEmailAsync(string recipientEmail, Events ev, System.Threading.CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(recipientEmail)) throw new ArgumentException("recipientEmail is required", nameof(recipientEmail));
            if (ev == null) throw new ArgumentNullException(nameof(ev));

            var fromName = _configuration["EmailSettings:FromName"] ?? "AWS Cloud Club";
            var fromAddress = _configuration["EmailSettings:FromAddress"] ?? "no-reply@awscloudclub.local";
            var toName = _configuration["EmailSettings:ToName"] ?? "Recipient";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromAddress));
            message.To.Add(new MailboxAddress(toName, recipientEmail));

            var safeEventName = string.IsNullOrWhiteSpace(ev.EventName) ? "(no title)" : ev.EventName;
            message.Subject = $"AWS Cloud Club - New Event: {safeEventName}";

            // Build both Text and HTML bodies
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

            var host = _configuration["EmailSettings:SmtpHost"] ?? "sandbox.smtp.mailtrap.io";
            var portString = _configuration["EmailSettings:SmtpPort"] ?? "2525";
            var username = _configuration["EmailSettings:SmtpUsername"] ?? string.Empty;
            var password = _configuration["EmailSettings:SmtpPassword"] ?? string.Empty;
            var enableTlsString = _configuration["EmailSettings:EnableTls"] ?? "true";

            if (!int.TryParse(portString, out var port)) port = 2525;
            var enableTls = bool.TryParse(enableTlsString, out var parsedTls) ? parsedTls : true;

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
        public async System.Threading.Tasks.Task SendEmailAsync(Events ev, System.Threading.CancellationToken cancellationToken = default)
        {
            var configuredRecipient = _configuration["EmailSettings:ToAddress"];
            if (string.IsNullOrWhiteSpace(configuredRecipient))
            {
                throw new InvalidOperationException("No recipient configured!");
            }

            await SendEmailAsync(configuredRecipient, ev, cancellationToken).ConfigureAwait(false);
        }
    }
}


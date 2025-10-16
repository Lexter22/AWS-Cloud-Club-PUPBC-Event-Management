using MailKit.Net.Smtp;
using MimeKit;
using AWSCloudClub_Common.Models;

namespace AWSCloudClub_BusinessLogic
{
    public class EventEmailService
    {
        public async System.Threading.Tasks.Task SendEmailAsync(Events ev, System.Threading.CancellationToken cancellationToken = default)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("AWS Cloud Club", "no-reply@awscloudclub.local"));
            message.To.Add(new MailboxAddress("Recipient", "test@inbox.mailtrap.io"));
            message.Subject = "AWS Cloud Club - New Event Created";

            var builder = new BodyBuilder();
            builder.TextBody = "There is a new event!\n\n" +
                $"Event Name: {ev.EventName}\n" +
                $"Event Date: {ev.EventDate}\n" +
                $"Description: {ev.Description}\n\n" +
                "See you there!\n" +
                "AWS Cloud Club PUP";

            builder.HtmlBody = System.Net.WebUtility.HtmlEncode(builder.TextBody).Replace("\n", "<br/>");

            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                var smtpHost = "sandbox.smtp.mailtrap.io";
                var smtpPort = 2525;
                var tls = MailKit.Security.SecureSocketOptions.StartTls;
                await client.ConnectAsync(smtpHost, smtpPort, tls, cancellationToken).ConfigureAwait(false);

                var userName = "522cdd6c690fc9";
                var password = "4dc314154b4c0e";

                await client.AuthenticateAsync(userName, password, cancellationToken).ConfigureAwait(false);

                await client.SendAsync(message, cancellationToken).ConfigureAwait(false);
                await client.DisconnectAsync(true, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}

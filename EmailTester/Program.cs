using System;
using AWSCloudClub_BusinessLogic;
using AWSCloudClub_Common.Models;

class Program
{   
    static  AWSCloudClub_BusinessLogic.EventEmailService svc = new AWSCloudClub_BusinessLogic.EventEmailService();
    static async System.Threading.Tasks.Task Main()
    {
        var ev = new Events {
            EventID = "LASADJAKJD-TEST-001",
            EventName = "AWS CLOUD CLUB PUP NEW EVENT!",
            EventDate = DateTime.UtcNow.ToString("o"),
            Description = "Testing EventEmailService from a console app"
        };

        try
        {
            await svc.SendEmailAsync(ev);
            Console.WriteLine("Email sent. Check Mailtrap.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Send failed: " + ex);
        }
    }
}
using System;

namespace AWSCloudClub_BusinessLogic
{
    public class EmailSettings
    {
        // this class holds email configuration settings
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string ToName { get; set; } 
        public string ToAddress { get; set; } 
        public string SmtpHost { get; set; } 
        public int SmtpPort { get; set; } 
        public string SmtpUsername { get; set; } 
        public string SmtpPassword { get; set; } 
        public bool EnableTls { get; set; } = true;
    }
}

namespace Egoal.Net.Mail
{
    public class EmailOptions
    {
        public string SmtpHost { get; set; } = "127.0.0.1";
        public int SmtpPort { get; set; } = 465;
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
        public bool SmtpUseSSL { get; set; } = true;
        public bool SmtpUseDefaultCredentials { get; set; }
        public string EmailDefaultFromAddress { get; set; }
        public string EmailDefaultFromDisplayName { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;


namespace MailingWebAPI
{
    public class MessageToEmails
    {
        [Required]
        public List<string> Emails { get; set; }
        [Required]
        public string Message { get; set; }
    }
}

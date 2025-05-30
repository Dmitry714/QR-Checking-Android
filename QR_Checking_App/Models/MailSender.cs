using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace QR_Checking_App
{
    public class MailSender
    {
        public static async Task<bool> SendEmail(string subject, string body, string Email)
        {
            try
            {
                string fromAddress = @"qr.checking@gmail.com";
                string fromPassword = @"lqpkwkpjhrfzgmfq";
                string toAddress = Email;

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("", fromAddress));
                message.To.Add(new MailboxAddress("", toAddress));
                message.Subject = subject;
                message.Body = new TextPart("plain")
                {
                    Text = body
                };

                var smtpClient = new SmtpClient();
                smtpClient.Connect("smtp.gmail.com", 465, true);
                smtpClient.Authenticate(fromAddress, fromPassword);

                await smtpClient.SendAsync(message);
                smtpClient.Disconnect(true);
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}

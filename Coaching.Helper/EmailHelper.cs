using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Coaching.Helper
{
    public static class EmailHelper
    {
        private static string Email = "bot.test.dev@gmail.com";
        private static string Password = "P@ssw0rd.123147";

        public static void SendMails(IEnumerable<string> toAddresses, string subject, string htmlBody)
        {
            Task.Run(new Action(async () =>
            {
                foreach (var address in toAddresses)
                {
                    await _sendMail(address, subject, htmlBody);
                }
            }));
        }
        public static async Task _sendMail(string toAddress, string subject, string htmlBody)
        {
            try
            {
                var host = "smtp.gmail.com";
                var port = 587;
                var fromAddress = Email;
                var password = Password;
                const string fromName = "Coaching";

                var client = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress, password)
                };

                var mail = new MailMessage
                {
                    From = new MailAddress(fromAddress, fromName),
                    Subject = subject,
                    SubjectEncoding = Encoding.UTF8,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = true,
                    Body = htmlBody
                };

                mail.To.Add(toAddress);

                await client.SendMailAsync(mail);

                client.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

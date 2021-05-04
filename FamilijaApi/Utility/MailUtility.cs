using FamilijaApi.Models;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FamilijaApi.Utility
{
    public static class MailUtility
    {
        public async static Task<bool> CreateMessageWithAttachment(Email email)
        {
            MailMessage mm = new MailMessage();

            var fromAddress = new MailAddress("familijaapi@gmail.com");
            foreach (var item in email.To)
            {   
                MailAddress.TryCreate(item, out var mail);
                mm.To.Add(mail);

            }
            const string fromPassword = "familija97";

            mm.From = fromAddress;
            mm.Body = email.Body;
            mm.Subject = email.Subject;


            var smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            await smtp.SendMailAsync(mm);
            
            return true;
        }
    }

    
}



using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace FamilijaApi.Utility
{
    public static class MailUtility
    {
        public async static Task<bool> CreateMessageWithAttachment()
        {
            var fromAddress = new MailAddress("lazarndrc@gmail.com", "Lazar Andric");
            var toAddress = new MailAddress("19petrovic97@gmail.com", "Baltazar");
            const string fromPassword = "Lakilaki97";
          

            MailMessage mm = new MailMessage();
            mm.To.Add(toAddress);
            mm.Body = "body";
            mm.Subject = "subject";
            mm.From = fromAddress;



            var smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.UseDefaultCredentials = true;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword);
            await smtp.SendMailAsync(mm);
            return true;
        }
    }
}

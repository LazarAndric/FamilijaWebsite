using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FamilijaApi.Models
{
    public class Email 
    {
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }




    }
}

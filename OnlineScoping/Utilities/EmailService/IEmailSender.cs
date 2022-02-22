using OnlineScoping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Utilities.EmailService
{
    public interface IEmailSender
    {
        void SendEmail(EmailMessage message);

        string SendEmail1(EmailMessage message);
    }
}

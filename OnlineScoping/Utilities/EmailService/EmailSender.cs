using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using OnlineScoping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Utilities.EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }
        public void SendEmail(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        public string  SendEmail1(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);
            return Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            // emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
            return emailMessage;
        }

        private string Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, false);
                    //client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.None);
                      client.AuthenticationMechanisms.Remove("XOAUTH2");
                       client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    client.Send(mailMessage);
                    client.Disconnect(true);
                    client.Dispose();
                    return "success";
                }
                catch (Exception se)
                {
                    string message = se.Message;
                    Exception seie = se.InnerException;
                    if (seie != null)
                    {
                        message += "; " + seie.Message;
                        Exception seieie = seie.InnerException;
                        if (seieie != null)
                        {
                            message += "; " + seieie.Message;
                        }
                    }
                    client.Disconnect(true);
                    client.Dispose();
                    return message;
                }
                
            }
        }

   
    }
}

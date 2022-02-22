using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using OnlineScoping.Utilities.EmailService;
using OnlineScoping.Models;
using System.Net.Mail;

namespace OnlineScoping.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Utilities.EmailService.IEmailSender _emailSender;
        private readonly EmailConfiguration _emailConfig;
        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, Utilities.EmailService.IEmailSender emailSender,
            EmailConfiguration emailConfig)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _emailConfig = emailConfig;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]

            public string Email { get; set; }
            public string UserName { get; set; }


        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var data = new ApplicationUser { UserName = Input.UserName, Email = Input.Email };
                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user == null)
                {
                    TempData["error"] = "Invalid Email.";

                    return RedirectToPage("./ForgotPassword");
                  
                }             
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code, Email = Input.Email },
                    protocol: Request.Scheme);
       
                string body =
                    "<br/>Reset your password by clicking on the following link" +
                    " <br/><br/><a href='" + callbackUrl + "'>Click here</a>" +
                    "<br/><br/>Regards" +
                    "<br/> ProCheckUp Ltd." +
                    "<br/><p>Copyright © 2020 <a target='blank' href='https://procheckup.com/'>ProCheckUp Ltd.</a>, All rights reserved.</p>";
                string subject = "Reset Password";

                //var message = new EmailMessage(new string[] { Input.Email }, subject, body);
                //_emailSender.SendEmail(message);
                SmtpClient mySmtpClient = new SmtpClient(_emailConfig.SmtpServer);
                mySmtpClient.Port = _emailConfig.Port;
                mySmtpClient.EnableSsl = false;
                mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mySmtpClient.UseDefaultCredentials = false;
                MailAddress from = new MailAddress(_emailConfig.From);
                MailAddress to = new MailAddress(Input.Email);
                MailMessage myMail = new System.Net.Mail.MailMessage(from, to);
                myMail.Subject = subject;
                myMail.Body = body;
                myMail.IsBodyHtml = true;
                try
                {
                    mySmtpClient.Send(myMail);
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }
                catch (Exception se)
                {
                    string message1 = se.Message;
                    Exception seie = se.InnerException;
                    if (seie != null)
                    {
                        message1 += ", " + seie.Message;
                        Exception seieie = seie.InnerException;
                        if (seieie != null)
                        {
                            message1 += ", " + seieie.Message;
                        }
                    }
                    TempData["ForgotPassword"] = message1;
                    return RedirectToPage("./ForgotPassword");
                }           
            }

            return Page();
        }


    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OnlineScoping.Models;
using OnlineScoping.Data;
using System.Net.Mail;
using OnlineScoping.Utilities.EmailService;
using Microsoft.EntityFrameworkCore;


namespace OnlineScoping.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly OnlineScopingContext _context;
        private readonly EmailConfiguration _emailConfig;
        private readonly Utilities.EmailService.IEmailSender _emailSender;
        public LoginModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, 
            ILogger<LoginModel> logger,
            OnlineScopingContext context, Utilities.EmailService.IEmailSender emailSender, EmailConfiguration emailConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _emailSender = emailSender;
            _emailConfig = emailConfig;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {           
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "User Name")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        //public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        //{
        //    returnUrl = returnUrl ?? Url.Content("~/");
        //    string callurl = Url.Content("~/AzureAD/Account/SignIn");
        //    if (ModelState.IsValid)
        //    {
        //        if (Input.UserName.Contains("@"))
        //        {
        //            Input.UserName = (_context.Users.Any(p => p.Email == Input.UserName)) ?
        //              _context.Users.SingleOrDefault(p => p.Email == Input.UserName).UserName :
        //              Input.UserName;

        //        }

        //        This doesn't count login failures towards account lockout
        //         To enable password failures to trigger account lockout, set lockoutOnFailure: true
        //        var data = _context.Users.Where(m => m.UserName == Input.UserName).FirstOrDefault();
        //        if (data != null)
        //        {
        //            if (data.Is2Fa == true)
        //            {
        //                eturnUrl = Url.Content("~/AzureAD/Account/SignIn");
        //                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
        //                Random rnd = new Random();
        //                string Code = "";
        //                for (int i = 0; i < 4; i++)
        //                {
        //                    Code += rnd.Next(1, 9);
        //                }
        //                var _id = data.Id;
        //                TempData["_id"] = _id;
        //                TempData["ps"] = Input.Password;
        //                var msg = SendVerificationEmail(data.Email, data.UserName, Code);
        //                if (msg == "success")
        //                {
        //                    var res = _context.Users.Where(x => x.Id == _id).SingleOrDefault();
        //                    if (res != null)
        //                    {
        //                        res.OTP = Code;
        //                        _context.SaveChanges();
        //                    }
        //                    return RedirectToAction("EmailConfirmOTPPage", "Home");
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError(string.Empty, "Invalid Email.");
        //                    return Page();
        //                }
        //            }
        //            else
        //            {
        //                var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
        //                if (result.Succeeded)
        //                {
        //                    _logger.LogInformation("User logged in.");
        //                    TempData["LastLoginDate"] = "Userloggedin.";
        //                    return LocalRedirect(returnUrl);
        //                }
        //                if (result.RequiresTwoFactor)
        //                {
        //                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
        //                }
        //                if (result.IsLockedOut)
        //                {
        //                    _logger.LogWarning("User account locked out.");
        //                    return RedirectToPage("./Lockout");
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        //                    return Page();
        //                }
        //            }

        //            If we got this far, something failed, redisplay form
        //    return Page();
        //        }
        //    }
        //    return Page();
        //}

        //public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        //{
        //    returnUrl = returnUrl ?? Url.Content("~/");


        //    //  string callurl = Url.Content("~/AzureAD/Account/SignIn");
        //    if (ModelState.IsValid)
        //    {
        //        if (Input.UserName.Contains("@"))
        //        {

        //            Input.UserName = (_context.Users.Any(p => p.Email == Input.UserName)) ?
        //              _context.Users.SingleOrDefault(p => p.Email == Input.UserName).UserName :
        //              Input.UserName;

        //        }
        //        // This doesn't count login failures towards account lockout
        //        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        //        var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
        //        if (result.Succeeded)
        //        {
        //            _logger.LogInformation("User logged in.");
        //            TempData["LastLoginDate"] = "Userloggedin.";
        //            if (Input.UserName != null)
        //            {
        //                var data = _context.Users.Where(m => m.UserName == Input.UserName).FirstOrDefault();
        //                if (data != null)
        //                {
        //                    if (data.Is2Fa == true)
        //                    {
        //                        //returnUrl = Url.Content("~/AzureAD/Account/SignIn");
        //                       // return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
        //                        Random rnd = new Random();
        //                        string Code = "";
        //                        for (int i = 0; i < 4; i++)
        //                        {
        //                            Code += rnd.Next(1, 9);
        //                        }
        //                        var _id = data.Id;
        //                        TempData["_id"] = _id;
        //                        TempData["ps"] = Input.Password;
        //                        var msg = SendVerificationEmail(data.Email, data.UserName, Code);
        //                        if (msg == "success")
        //                        {
        //                            var res = _context.Users.Where(x => x.Id == _id).SingleOrDefault();
        //                            if (res != null)
        //                            {
        //                                res.OTP = Code;
        //                                _context.SaveChanges();
        //                            }
        //                            return RedirectToAction("EmailConfirmOTPPage", "Home", new { _id });
        //                        }
        //                        else
        //                        {
        //                            ModelState.AddModelError(string.Empty, "Invalid Email.");
        //                            return Page();
        //                        }
        //                        //eturnUrl = Url.Content("~/AzureAD/Account/SignIn");
        //                        //  return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });

        //                    }
        //                    else
        //                    {

        //                    }
        //                }
        //            }
        //            return LocalRedirect(returnUrl);
        //        }
        //        if (result.RequiresTwoFactor)
        //        {
        //            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
        //        }
        //        if (result.IsLockedOut)
        //        {
        //            _logger.LogWarning("User account locked out.");
        //            return RedirectToPage("./Lockout");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        //            return Page();
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return Page();
        //}

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            //  string callurl = Url.Content("~/AzureAD/Account/SignIn");
            if (ModelState.IsValid)
            {
                if (Input.UserName.Contains("@"))
                {
                    Input.UserName = (_context.Users.Any(p => p.Email == Input.UserName)) ?
                      _context.Users.SingleOrDefault(p => p.Email == Input.UserName).UserName :
                      Input.UserName;

                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var data = _context.Users.Where(m => m.UserName == Input.UserName).FirstOrDefault();
                if (data != null)
                {
                    if (data.Is2Fa == true)
                    {
                        //eturnUrl = Url.Content("~/AzureAD/Account/SignIn");
                        //  return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                        Random rnd = new Random();
                        string Code = "";
                        for (int i = 0; i < 4; i++)
                        {
                            Code += rnd.Next(0, 9);
                        }
                        var _id = data.Id;
                        TempData["ps"] = Input.Password;
                        var msg = SendVerificationEmail(data.Email, data.UserName, Code);
                        if (msg == "success")
                        {
                            var res = _context.Users.Where(x => x.Id == _id).SingleOrDefault();
                            if (res != null)
                            {
                                res.OTP = Code;
                                _context.SaveChanges();
                            }
                            return RedirectToAction("EmailConfirmOTPPage", "Home",new { _id });
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid Email.");
                            return Page();
                        }
                    }
                    else
                    {
                        var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            _logger.LogInformation("User logged in.");
                            TempData["LastLoginDate"] = "Userloggedin.";
                            return LocalRedirect(returnUrl);
                        }
                        if (result.RequiresTwoFactor)
                        {
                            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                        }
                        if (result.IsLockedOut)
                        {
                            _logger.LogWarning("User account locked out.");
                            return RedirectToPage("./Lockout");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return Page();
                        }
                    }

                    // If we got this far, something failed, redisplay form
                    //  return Page();
                }
            }
            return Page();
        }





        private string SendVerificationEmail(string emailId, string FirstName,string otp)
        {
            string CurrentYear = DateTime.Now.Year.ToString();
            string body = "<br/><br/>Hi " + FirstName + "" +
           "<br/>Your Login OTP Is " +otp+
           " <br/>" +
           "<br/><br/>Regards" +
           "<br/> ProCheckUp Ltd." +
           "<br/><p>Copyright © " + CurrentYear + " <a target='blank' href='https://procheckup.com/'>ProCheckUp Ltd.</a>, All rights reserved.</p>";
            string subject = "Confirmation OTP";
            SmtpClient mySmtpClient = new SmtpClient(_emailConfig.SmtpServer);
            mySmtpClient.Port = _emailConfig.Port;
            mySmtpClient.EnableSsl = false;
            mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            mySmtpClient.UseDefaultCredentials = false;
            MailAddress from = new MailAddress(_emailConfig.From);
            MailAddress to = new MailAddress(emailId);
            MailMessage myMail = new System.Net.Mail.MailMessage(from, to);
            myMail.Subject = subject;
            myMail.Body = body;
            myMail.IsBodyHtml = true;
            try
            {
                mySmtpClient.Send(myMail);
                return "success";
            }
            catch (Exception se)
            {
                string message = se.Message;
                Exception seie = se.InnerException;
                if (seie != null)
                {
                    message += ", " + seie.Message;
                    Exception seieie = seie.InnerException;
                    if (seieie != null)
                    {
                        message += ", " + seieie.Message;
                    }
                }
                return message;
            }

        }




    }
}

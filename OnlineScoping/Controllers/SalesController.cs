using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineScoping.Data;
using OnlineScoping.Models;
using OnlineScoping.Services;
using OnlineScoping.Utilities.EmailService;
using OnlineScoping.ViewModels;

namespace OnlineScoping.Controllers
{
    public class SalesController : Controller
    {
        private readonly ISalesService _service;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly OnlineScopingContext _context;
        private readonly EmailConfiguration _emailConfig;
        public SalesController(
          ISalesService service,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            SignInManager<ApplicationUser> signInManager,
            OnlineScopingContext context,
            EmailConfiguration emailConfig)
        {
            _service = service;
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _context = context;
            _emailConfig = emailConfig;
        }

        // GET: Sales
        public async Task<IActionResult> Index()
        {
            SalesViewModel model = new SalesViewModel();

        //    var list = (await _service.GetSales()).Select(item => new SalesViewModel(item)).ToList();
            model.Salesviewmodel =  _service.GetSalesAdmin();
            return View(model);

            //return View(await _service.GetSales());
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sales = await _service.GetSaleById(id);
            if (sales == null)
            {
                return NotFound();
            }

            return View(new SalesViewModel(sales));
        }

        // GET: Sales/Create
        public IActionResult Create()
        {
            SalesViewModel model = new SalesViewModel();
            model.msg = false;
            return View(model);
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FirstName,LastName,Email,IsEmailSent,MobileNumber,TempUserName")] SalesViewModel model)
        {
            if (ModelState.IsValid)
            {

                var data = new SalesViewModel { Email = model.Email };
                var user = _service.EmailExists(data.Email);
                if (user == true)
                {
                    ModelState.AddModelError("Email", "Email Already Exists.");
                    return View(model);
                }
                var usere = _service.UserEmailExists(model.Email);
                if (usere == true)
                {
                    ModelState.AddModelError("Email", "Email Already Exists.");
                    return View(model);
                }
                var usernme = _service.UsernameExists(model.TempUserName);
                if (usernme == true)
                {
                    ModelState.AddModelError("TempUserName", "UserName Already Exists.");
                    return View(model);
                }
                var Sid=   await _service.CreateSales(model.GetSales());
                TempData["SuccessMessage"] = "successfully";
                model.msg = true;
                model.CustomerId = Sid;
                return View(model);
            }
            model.msg = false;
            return View(model);
        }

        // GET: Sales/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sales = await _service.GetSaleById(id);
            if (sales == null)
            {
                return NotFound();
            }
            return View(new SalesViewModel(sales));
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CustomerId,FirstName,LastName,Email,IsEmailSent,MobileNumber,TempUserName")] SalesViewModel sales)
        {
            if (id != sales.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //var data = new SalesViewModel { Email = sales.Email };
                    //var user = _service.EmailExists(data.Email);
                    //if (user == true)
                    //{
                    //    ModelState.AddModelError("Email", "Email Already Exists.");
                    //    return View(sales);
                    //}
                    await _service.UpdateSales(sales.GetSales());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_service.SalesExists(sales.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["UpdateMessage"] = "successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(sales);
        }

        // GET: Sales/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sales = await _service.GetSaleById(id);
            if (sales == null)
            {
                return NotFound();
            }

            return View(new SalesViewModel(sales));
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _service.DeleteSalesById(id);

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> SendMail(Guid id)
        {

            var Sale = await _service.GetSaleById(id);
         var msg=  SendVerificationLinkEmail12(Sale.Email, Sale.CustomerId, Sale.FirstName);
            if (msg == "success")
            {
                var Sales = await _service.GetSaleById(id);

                try
                {
                    await _service.UpdateSalesIsEmailSent(Sales);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_service.SalesExists(Sales.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SalesMailsuccessMessage"] = "Mail send successfully !";

                return RedirectToAction(nameof(Index));
            }else
            {
                TempData["SalesMailErrerMessage"] = msg;
                return RedirectToAction(nameof(Index));
            }
        }



        [AllowAnonymous]
        public async Task<IActionResult> EmailRedirect(Guid userId)
        {
            AccountModel model = new AccountModel();
            var Sale = await _service.GetSaleById(userId);
            if(Sale!=null)
            {
                model.Email = Sale.Email;
                model.RoleName = "Sales";
                model.UserName = Sale.TempUserName;
                model.Id = Sale.CustomerId;

                return View(model);
            }else
            {       
                return RedirectToAction("ErrorMessage","Error");
            }

        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmailRedirect(AccountModel Model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser { UserName = Model.UserName, Email = Model.Email };

                    var usere = _service.UserEmailExists(Model.Email);
                    if (usere == true)
                    {
                        ModelState.AddModelError("Email", "Email Already Exists.");
                        return View(Model);
                    }
                    var usernme = _service.UsernameExists(Model.UserName);
                    if (usernme == true)
                    {
                        ModelState.AddModelError("UserName", "UserName Already Exists.");
                        return View(Model);
                    }

                    var result = await _userManager.CreateAsync(user, Model.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Sales");
                        var Sale = await _service.GetSaleById(Model.Id);

                        try
                        {


                            await _service.UpdateSalesUserId(Sale, user.Id);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!_service.SalesExists(Sale.CustomerId))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                        await _signInManager.SignOutAsync();
                        // return RedirectToPage("/Account/Login", new { area = "Identity" });
                        return RedirectToAction("RegisterConfirmations", "Home");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }

                }
            }catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(Model);
        }


        private void SendVerificationLinkEmail(string emailId, Guid id, string FirstName)
        {
            var varifyUrl = Request.Scheme + "://scoping.procheckup.com/Sales/EmailRedirect?userId=" + id;              
            string body = "<br/><br/>Hi "+ FirstName + "" +
                "<br/>You are invited to join us by clicking on the following link" +
                " <br/><br/><a href='" + varifyUrl + "'>Click here to join</a>" +
                "<br/><br/>Regards" +
                "<br/> ProCheckUp Ltd." +
                "<br/><p>Copyright © 2020 <a target='blank' href='https://procheckup.com/'>ProCheckUp Ltd.</a>, All rights reserved.</p>";
            string subject = "You have been invited";       
            var message = new EmailMessage(new string[] { emailId }, subject, body);
            _emailSender.SendEmail(message);
        }

        private string SendVerificationLinkEmail12(string emailId, Guid id, string FirstName)
        {
            var varifyUrl = Request.Scheme + "://scoping.procheckup.com/Sales/EmailRedirect?userId=" + id;
           // var varifyUrl = Request.Scheme + "://localhost:44368/Sales/EmailRedirect?userId=" + id;
            string body = "<br/><br/>Hi " + FirstName + "" +
              "<br/>You are invited to join us by clicking on the following link" +
              " <br/><br/><a href='" + varifyUrl + "'>Click here to join</a>" +
              "<br/><br/>Regards" +
              "<br/> ProCheckUp Ltd." +
              "<br/><p>Copyright © 2020 <a target='blank' href='https://procheckup.com/'>ProCheckUp Ltd.</a>, All rights reserved.</p>";
            string subject = "You have been invited";
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
                    message += ";" + seie.Message;
                    Exception seieie = seie.InnerException;
                    if (seieie != null)
                    {
                        message += ";" + seieie.Message;
                    }
                }
                return message;
            }

        }

        [HttpPost]

        public async Task<IActionResult> SalesChangePassword(CustomerViewModel model)
        {
            try
            {

            var customer = _context.Sales.Where(m => m.CustomerId == model.Accountmodel.Id).FirstOrDefault();
            if (customer.UserId != null)
            {
                    var user = _context.Users.Where(m => m.Id == customer.UserId).FirstOrDefault();        

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var result = await _userManager.ResetPasswordAsync(user, token, model.Accountmodel.Password);
                TempData["PasswordUpdateMessage"] = "successfully";

                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["PasswordUpdateErrerMessage"] = "successfully";
                return RedirectToAction(nameof(Index));
            }

            }catch(Exception ex)
            {
                TempData["PasswordUpdateErrerMessage"] = "successfully";
                return RedirectToAction(nameof(Index));
            }

        }

        public async Task<IActionResult> AuthEnableDisable(SalesViewModel model)
        {

            var customer = _context.Sales.Where(m => m.CustomerId == model.authmodel.Id).FirstOrDefault();
            if (customer.UserId != null)
            {
                var user = _context.Users.Where(m => m.Id == customer.UserId).FirstOrDefault();
                var msg = await _service.AuthUpdate(user.Id, model.authmodel.Auth);


                TempData["SalesAuthEnableDisable"] = "successfully";

                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["PasswordUpdateErrerMessage"] = "successfully";
                return RedirectToAction(nameof(Index));
            }
        }


    }
}

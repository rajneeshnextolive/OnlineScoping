using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineScoping.Areas.Identity.Pages.Account;
using OnlineScoping.Data;
using OnlineScoping.Models;
using OnlineScoping.Services;
using OnlineScoping.Utilities.EmailService;
using OnlineScoping.ViewModels;

namespace OnlineScoping.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ICustomersService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly OnlineScopingContext _context;
        private readonly EmailConfiguration _emailConfig;
        public CustomersController(ICustomersService service,
            UserManager<ApplicationUser> userManager
            , IEmailSender emailSender,
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

        public IActionResult Index(string val)
        {
            CustomerViewModel newModel = new CustomerViewModel();
            List<CustomerViewModel> model = new List<CustomerViewModel>();
            //ClaimsPrincipal currentUser = User;          
            var UserId = _userManager.GetUserId(User); // Get user id:
            bool available = User.IsInRole("Admin");
            if (available)
            {
                if (val == "email")
                {
                    newModel.CustomerViewModelList = _service.GetCustomersEmailVerified();
                    newModel.TempData = "Email";
                }
                else if (val == "replied")
                {
                    newModel.CustomerViewModelList = _service.GetCustomersReplied();
                    newModel.TempData = "replied";
                }
                else
                {
                    newModel.CustomerViewModelList = _service.GetCustomersAdmin();
                    newModel.TempData = "customer";
                }
            }
            else
            {
                if (val == "email")
                {
                    newModel.CustomerViewModelList = _service.NewGetCustomersEmailVerified(new Guid(UserId));
                    newModel.TempData = "Email";
                }
                else if (val == "replied")
                {
                    newModel.CustomerViewModelList = _service.NewGetCustomersReplied(new Guid(UserId));
                    newModel.TempData = "replied";
                }
                else
                {
                    newModel.CustomerViewModelList = _service.NewGetCustomers(new Guid(UserId));
                    newModel.TempData = "customer";
                }
            }

            newModel.QuestionnaireList = _service.QuestionnaireAllList();

            return View(newModel);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // var customer = await _service.GetCustomerById(id);
            var customer = await _service.NewGetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }
            // return View(new CustomerViewModel(customer));
            return View(customer);
        }

        public IActionResult Create()
        {
            CustomerViewModel model = new CustomerViewModel();

            model.SalseList = _service.GetSales();
            model.msg = false;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FirstName,LastName,Email,IsEmailSent,CreatedBy,Client,Address1,Address2,City,MobileNumber,State,Country,Zip,TempUserName")] CustomerViewModel model)
        {
            var UserId = _userManager.GetUserId(User); // Get user id:
            if (model.CreatedBy == Guid.Empty)
            {
                model.CreatedBy = new Guid(UserId);
            }

            if (ModelState.IsValid)
            {
                var data = new CustomerViewModel { Email = model.Email };
                var user = _service.EmailExists(data.Email);

                if (user == true)
                {
                    ModelState.AddModelError("Email", " Email Already Exists.");
                    model.SalseList = _service.GetSales();
                    return View(model);
                }
                var usere = _service.UserEmailExists(model.Email);
                if (usere == true)
                {
                    ModelState.AddModelError("Email", "Email Already Exists.");
                    model.SalseList = _service.GetSales();
                    return View(model);
                }
                var usernme = _service.UsernameExists(model.TempUserName);
                if (usernme == true)
                {
                    ModelState.AddModelError("TempUserName", "UserName Already Exists.");
                    model.SalseList = _service.GetSales();
                    return View(model);
                }


                var Cid = await _service.CreateCustomer(model.GetCustomer());
                TempData["SuccessMessage"] = "successfully";
                model.msg = true;
                model.CustomerId = Cid;
                model.SalseList = _service.GetSales();
                return View(model);
            }
            model.SalseList = _service.GetSales();
            model.msg = false;
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            CustomerViewModel model = new CustomerViewModel();
            if (id == null)
            {
                return NotFound();
            }

            model = await _service.NewGetCustomerById(id);

            model.SalseList = _service.GetSales();
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CustomerId,FirstName,LastName,Email,IsEmailSent,CreatedBy,Client,Address1,Address2,City,MobileNumber,State,Country,Zip,TempUserName")] CustomerViewModel model)
        {

            if (id != model.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //var data = new CustomerViewModel { Email = model.Email };
                    //var user = _service.EmailExists(data.Email);

                    //if (user == true)
                    //{
                    //    ModelState.AddModelError("Email", " Email Already Exists.");
                    //    model.SalseList = _service.GetSales();
                    //    return View(model);
                    //}


                    await _service.UpdateCustomer(model.GetCustomer());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_service.CustomerExists(model.CustomerId))
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
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _service.NewGetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }

            // return View(new CustomerViewModel(customer));
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _service.DeleteCustomerById(id);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public JsonResult SelectQuestionnaire(string id, string CustomerId)
        {
            return Json(nameof(Index));
        }




        public async Task<IActionResult> SendMail(Guid id)
        {

            var customer = await _service.GetCustomerById(id);
            var msg = SendVerificationLinkEmail1(customer.Email, customer.CustomerId, customer.FirstName);
            //var msg=    SendVerificationLinkEmail(customer.Email, customer.CustomerId, customer.FirstName);
            if (msg == "success")
            {
                var customers = await _service.GetCustomerById(id);
                try
                {
                    await _service.UpdateCustomerIsEmailSent(customers);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_service.CustomerExists(customers.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["MailsuccessMessage"] = "Mail send successfully !";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["MailErrerMessage"] = msg;
                return RedirectToAction(nameof(Index));
            }

        }

        [AllowAnonymous]
        public async Task<IActionResult> EmailRedirect(Guid userId)
        {
            AccountModel model = new AccountModel();
            var customer = await _service.GetCustomerById(userId);
            if (customer != null)
            {
                model.Email = customer.Email;
                model.RoleName = "Customer";
                model.UserName = customer.TempUserName;
                model.Id = customer.CustomerId;

                return View(model);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
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
                        await _userManager.AddToRoleAsync(user, "Customer");
                        var customer = await _service.GetCustomerById(Model.Id);
                        try
                        {
                            await _service.UpdateCustomerUserId(customer, user.Id);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!_service.CustomerExists(customer.CustomerId))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                        await _signInManager.SignOutAsync();
                        //return RedirectToPage("/Account/Login", new { area = "Identity" });
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
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

            }
            return View(Model);
        }


        [HttpPost]

        public async Task<IActionResult> ChangePassword(CustomerViewModel model)
        {
            try
            {
                var customer = _context.Customer.Where(m => m.CustomerId == model.Accountmodel.Id).FirstOrDefault();
                if (customer.UserId != null)
                {
                    var user = _context.Users.Where(m => m.Id == customer.UserId).FirstOrDefault();
                    //   var user = await _userManager.FindByIdAsync(customer.UserId);


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
            }
            catch (Exception ex)
            {
                TempData["PasswordUpdateErrerMessage"] = "successfully";
                return RedirectToAction(nameof(Index));
            }

        }

       


        private string SendVerificationLinkEmail(string emailId, Guid id, string FirstName)
        {
            string CurrentYear = DateTime.Now.Year.ToString();
            //var varifyUrl = Request.Scheme + "://38.17.52.106:2500/Customers/EmailRedirect?userId=" + id;
            var varifyUrl = Request.Scheme + "://scoping.procheckup.com/Customers/EmailRedirect?userId=" + id;
            //var varifyUrl = Request.Scheme + "://localhost:44368/Customers/EmailRedirect?userId=" + id;     
            string body = "<br/><br/>Hi " + FirstName + "" +
                "<br/>You are invited to join us by clicking on the following link" +
                " <br/><br/><a href='" + varifyUrl + "'>Click here to join</a>" +
                "<br/><br/>Regards" +
                "<br/> ProCheckUp Ltd." +
                "<br/><p>Copyright © " + CurrentYear + " <a target='blank' href='https://procheckup.com/'>ProCheckUp Ltd.</a>, All rights reserved.</p>";
            string subject = "You have been invited";

            var message = new EmailMessage(new string[] { emailId }, subject, body);
            return _emailSender.SendEmail1(message);
            // return View();
        }



        private string SendVerificationLinkEmail1(string emailId, Guid id, string FirstName)
        {
            string CurrentYear = DateTime.Now.Year.ToString();
            var varifyUrl = Request.Scheme + "://scoping.procheckup.com/Customers/EmailRedirect?userId=" + id;
            string body = "<br/><br/>Hi " + FirstName + "" +
           "<br/>You are invited to join us by clicking on the following link" +
           " <br/><br/><a href='" + varifyUrl + "'>Click here to join</a>" +
           "<br/><br/>Regards" +
           "<br/> ProCheckUp Ltd." +
           "<br/><p>Copyright © " + CurrentYear + " <a target='blank' href='https://procheckup.com/'>ProCheckUp Ltd.</a>, All rights reserved.</p>";
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

        public async Task<IActionResult> CustomerEditProfile(Guid? id)
        {
            CustomerViewModel model = new CustomerViewModel();
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var customer = _context.Customer.Where(m => m.UserId == id.ToString()).FirstOrDefault();
                if (customer != null)
                {
                    model = await _service.NewGetCustomerById(customer.CustomerId);

                    model.SalseList = _service.GetSales();
                }
            }


            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> CustomerEditProfile(Guid id, [Bind("CustomerId,FirstName,LastName,Email,IsEmailSent,CreatedBy,Client,Address1,Address2,City,MobileNumber,State,Country,Zip,TempUserName")] CustomerViewModel model)
        {



            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateCustomer(model.GetCustomer());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_service.CustomerExists(model.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["UpdateMessage"] = "successfully";

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        private string SendProjectMail(string emailId, string UserName, string FirstName, string ProjectName)
        {
            string CurrentYear = DateTime.Now.Year.ToString();
            string body = "<p><br/><br/>Hi</p>";
            string subject = "New Project";

            SmtpClient mySmtpClient = new SmtpClient(_emailConfig.SmtpServer);
            mySmtpClient.Port = _emailConfig.Port;
            mySmtpClient.EnableSsl = false;
            mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            mySmtpClient.UseDefaultCredentials = false;
            MailAddress from = new MailAddress(_emailConfig.From);
            MailAddress to = new MailAddress(emailId);
            MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

            // myMail.CC.Add(email1);
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


        [HttpPost]
        public IActionResult AddProjectNewFunction(HomeViewModel model)
        {
            try
            {
                var UserId = _userManager.GetUserId(User); // Get user id:
                var customer = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();
                if (customer != null)
                {
                    model.ProjectsViewData.CustomerId = customer.CustomerId;
                }
                if (model.ProjectsViewData != null)
                {
                    try
                    {
                        if (model.ProjectsViewData.ProjectsId != Guid.Empty)
                        {
                            _service.UpdateProjectNewFunction(model.ProjectsViewData);
                            TempData["ProjectMessage"] = "Project Update succesfully !";
                        }
                        else
                        {
                            model.ProjectsViewData.RequestStatus = "1";
                            var a = _service.AddNewProjectNewFunction(model.ProjectsViewData);
                            //var data = _context.Projects.Where(m => m.CustomerId==pr).FirstOrDefault();
                            if (a != Guid.Empty)
                            {
                                // var custemail = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();

                                // var SalesUserData = _context.Users.Where(m => m.Id == custemail.CreatedBy.ToString()).FirstOrDefault();
                                //   var msg = SendCreateProjectEmail(SalesUserData.Email, SalesUserData.UserName, custemail.FirstName, model.ProjectsViewData.ProjectName);                           
                                SendVerificationLinkEmail1("banshbahadur9559@gmail.com", a, "test");
                                TempData["ProjectMessage"] = "New Project Save succesfully !";
                               
                            }
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    TempData["ProjectErrerMessage"] = "Error in Project !";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["ProjectErrerMessage"] = "Error in Project !";
                return RedirectToAction("Index", "Home");
            }
        }




        [HttpPost]
        public async Task<IActionResult> AddProject(HomeViewModel model)
        {
            try
            {
                var UserId = _userManager.GetUserId(User); // Get user id:
                var customer = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();
                if (customer != null)
                {
                    model.ProjectsViewData.CustomerId = customer.CustomerId;
                }
                if (model.ProjectsViewData != null)
                {
                    try
                    {
                        if (model.ProjectsViewData.ProjectsId != Guid.Empty)
                        {
                            await _service.UpdateProject(model.ProjectsViewData);
                            TempData["ProjectMessage"] = "Project Update succesfully !";
                        }
                        else
                        {
                            var SalesUserData = _context.Users.Where(m => m.Id == customer.CreatedBy.ToString()).FirstOrDefault();
                            model.ProjectsViewData.RequestStatus = "1";
                            var a = await _service.AddNewProject(model.ProjectsViewData);
                            //var data = _context.Projects.Where(m => m.CustomerId==pr).FirstOrDefault();
                            if (a != Guid.Empty)
                            {
                                var data = _context.Projects.Where(m => m.ProjectsId == a).FirstOrDefault();
                                if (customer != null)
                                {
                                    var msg = SendCreateProjectEmail(SalesUserData.Email, SalesUserData.UserName, customer.FirstName, data.ProjectName);
                                    if (msg == "success")
                                    {
                                        TempData["ProjectMessage"] = "New Project Save succesfully !";
                                    }
                                    else
                                    {
                                        TempData["ProjectMessage"] = "New Project Save succesfully !" + msg;
                                    }

                                }

                            }
                        }

                     
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    TempData["ProjectErrerMessage"] = "Error in Project !";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["ProjectErrerMessage"] = "Error in Project !";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        private string SendCreateProjectEmail(string emailId, string UserName, string FirstName, string ProjectName)
        {

            string CurrentYear = DateTime.Now.Year.ToString();
            string body = "<br/><br/>Hi " + UserName + "" +
                "<br/>" + FirstName + " created a new project called:"+ ProjectName + "" +          
                "<br/><br/>Thanks" +
                "<br/> ProCheckUp Ltd." +
                "<br/><p>Copyright © " + CurrentYear + " <a target='blank' href='https://procheckup.com/'>ProCheckUp Ltd.</a>, All rights reserved.</p>";
            string subject = "New Project";

            SmtpClient mySmtpClient = new SmtpClient(_emailConfig.SmtpServer);
            mySmtpClient.Port = _emailConfig.Port;
            mySmtpClient.EnableSsl = false;
            mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            mySmtpClient.UseDefaultCredentials = false;
            MailAddress from = new MailAddress(_emailConfig.From);
            MailAddress to = new MailAddress(emailId);
            MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

            // myMail.CC.Add(email1);
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




        //public async Task<IActionResult> AddProject(HomeViewModel model)
        //{
        //    try
        //    {
        //        var UserId = _userManager.GetUserId(User); // Get user id:
        //        var customer = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();
        //        if (customer != null)
        //        {
        //            model.ProjectsViewData.CustomerId = customer.CustomerId;
        //        }
        //        if (model.ProjectsViewData != null)
        //        {
        //            try
        //            {
        //                if (model.ProjectsViewData.ProjectsId != Guid.Empty)
        //                {
        //                    await _service.UpdateProject(model.ProjectsViewData);
        //                    TempData["ProjectMessage"] = "Project Update succesfully !";
        //                }
        //                else
        //                {
        //                    await _service.AddNewProject(model.ProjectsViewData);
        //                    TempData["ProjectMessage"] = "Project Save succesfully !";

        //                    var customeremial = _context.Customer.Where(m => m.CustomerId == customer.CustomerId).FirstOrDefault();

        //                    var SalesUserData = _context.Users.Where(m => m.Id == customeremial.CreatedBy.ToString()).FirstOrDefault();
        //                    string CurrentYear = DateTime.Now.Year.ToString();
        //                    string body = "<br/><br/>Hi " + SalesUserData.UserName + "," +
        //                        "<br/>" + customeremial.FirstName + "" + "has created a new project called: " + model.ProjectsViewData.ProjectName + "." +
        //                   //"<br/> The status of your project: " + SalesUserData.ProjectName + " has changed to: " + SalesUserData.RequestStatus + "." +
        //                   " <br/> " +
        //                           "<br/><br/>Thanks," +
        //                           " <br/><br/> " +
        //                           "<br/> ProCheckUp Team" +
        //                           "<br/><p>Copyright ©" + CurrentYear + " <a target='blank' href='https://procheckup.com/'>ProCheckUp Ltd.</a>, All rights reserved.</p>";
        //                    string subject = "New Project";

        //                    SmtpClient mySmtpClient = new SmtpClient(_emailConfig.SmtpServer);
        //                    mySmtpClient.Port = _emailConfig.Port;
        //                    mySmtpClient.EnableSsl = false;
        //                    mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //                    mySmtpClient.UseDefaultCredentials = false;
        //                    MailAddress from = new MailAddress(_emailConfig.From);
        //                    MailAddress to = new MailAddress(SalesUserData.Email);
        //                    MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

        //                    // myMail.CC.Add(email1);
        //                    myMail.Subject = subject;
        //                    myMail.Body = body;
        //                    myMail.IsBodyHtml = true;
        //                    try
        //                    {
        //                        mySmtpClient.Send(myMail);
        //                        //return "success";
        //                    }
        //                    catch (Exception se)
        //                    {
        //                        string message = se.Message;
        //                        Exception seie = se.InnerException;
        //                        if (seie != null)
        //                        {
        //                            message += ", " + seie.Message;
        //                            Exception seieie = seie.InnerException;
        //                            if (seieie != null)
        //                            {
        //                                message += ", " + seieie.Message;
        //                            }
        //                        }
        //                        //return message;
        //                    }













        //                    var data = model.ProjectsViewData;
        //                   // var custemail = _context.Customer.Where(m => m.CustomerId == customer.CustomerId).FirstOrDefault();

        //                   // var SalesUserData = _context.Users.Where(m => m.Id == custemail.CreatedBy.ToString()).FirstOrDefault();
        //                   // string CurrentYear = DateTime.Now.Year.ToString();
        //                   // string body = "<br/><br/>Hi " + SalesUserData.UserName + "," +
        //                   //     "<br/><br/>" + custemail.FirstName + "" + " has created a new project called: " + data.ProjectName + "." +
        //                   //" <br/> " +
        //                   //        "<br/><br/>Thanks," +
        //                   //        "<br/> ProCheckUp Team" +
        //                   //        "<br/><p>Copyright ©" + CurrentYear + " <a target='blank' href='https://procheckup.com/'>ProCheckUp Ltd.</a>, All rights reserved.</p>";
        //                   // string subject = "New Project";

        //                   // SmtpClient mySmtpClient = new SmtpClient(_emailConfig.SmtpServer);
        //                   // mySmtpClient.Port = _emailConfig.Port;
        //                   // mySmtpClient.EnableSsl = false;
        //                   // mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //                   // mySmtpClient.UseDefaultCredentials = false;
        //                   // MailAddress from = new MailAddress(_emailConfig.From);
        //                   // MailAddress to = new MailAddress(SalesUserData.Email);
        //                   // MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

        //                   // myMail.Subject = subject;
        //                   // myMail.Body = body;
        //                   // myMail.IsBodyHtml = true;
        //                   // try
        //                   // {
        //                   //     mySmtpClient.Send(myMail);
        //                   // }
        //                   // catch (Exception se)
        //                   // {
        //                   //     string message = se.Message;
        //                   //     Exception seie = se.InnerException;
        //                   //     if (seie != null)
        //                   //     {
        //                   //         message += ", " + seie.Message;
        //                   //         Exception seieie = seie.InnerException;
        //                   //         if (seieie != null)
        //                   //         {
        //                   //             message += ", " + seieie.Message;
        //                   //         }
        //                   //     }
        //                   // }
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            TempData["ProjectErrerMessage"] = "Error in Project !";
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ProjectErrerMessage"] = "Error in Project !";
        //        return RedirectToAction("Index", "Home");
        //    }


        //}


        public async Task<IActionResult> DeleteProject(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    await _service.DeleteNewProject(id);
                    TempData["ProjectMessage"] = "Project deleted succesfully !";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["ProjectErrerMessage"] = "Error in deleting project !";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["ProjectErrerMessage"] = "Error in deleting project !";
                return RedirectToAction("Index", "Home");
            }


        }


        public IActionResult ReceivedProposal()
        {
            ReceivedProposalModel model = new ReceivedProposalModel();
            List<CustomerRequestsViewModel> CustomerRequestslist = new List<CustomerRequestsViewModel>();
            try
            {
                var UserId = _userManager.GetUserId(User); // Get user id:
                var customer = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();

                model.CustomerRequestsList = _service.CustomerRequestListByCustomerId(customer.CustomerId);
                model.MethodologyeViewModelList = (from e in _context.Methodologye
                                                   select new MethodologyeViewModel
                                                   {
                                                       FileName = e.FileName,
                                                       FileContents = e.FileContents,
                                                       Id = e.Id,
                                                       CreatedDate = e.CreatedDate,
                                                       Type = e.Type

                                                   }).ToList();
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        public IActionResult RequestProposal()
        {
            ReceivedProposalModel model = new ReceivedProposalModel();
            List<CustomerRequestsViewModel> CustomerRequestslist = new List<CustomerRequestsViewModel>();
            try
            {
                var UserId = _userManager.GetUserId(User); // Get user id:
                var customer = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();

                model.CustomerRequestsList = _service.CustomerRequestProposalAllListByCustomerId(customer.CustomerId);
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        public async Task<IActionResult> AuthEnableDisable(CustomerViewModel model)
        {

            var customer = _context.Customer.Where(m => m.CustomerId == model.authmodel.Id).FirstOrDefault();
            if (customer.UserId != null)
            {
                var user = _context.Users.Where(m => m.Id == customer.UserId).FirstOrDefault();
                var msg = await _service.AuthUpdate(user.Id, model.authmodel.Auth);


                TempData["AuthEnableDisable"] = "successfully";

                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["PasswordUpdateErrerMessage"] = "successfully";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult CheckMail()
        {
           var a= SendCreateProjectEmail("banshbahadur68@gmail.com", "test", "test", "test");
              if(a== "success")
            {
                TempData["ProjectMessage"] = a;

            }
            else
            {
                TempData["ProjectMessage"] = a;

            }

            return RedirectToAction("Index", "Home");
          


        }

    }
}

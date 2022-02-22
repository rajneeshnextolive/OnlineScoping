using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineScoping.Data;
using OnlineScoping.Models;
using OnlineScoping.Services;
using OnlineScoping.Utilities.EmailService;
using OnlineScoping.ViewModels;

namespace OnlineScoping.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OnlineScopingContext _context;
        private readonly ICustomersService _service;
        private readonly Ihome _homeservice;
        private readonly EmailConfiguration _emailConfig;
        public HomeController(SignInManager<ApplicationUser> signInManager,ICustomersService service, Ihome homeservice, ILogger<HomeController> logger, IEmailSender emailSender, UserManager<ApplicationUser> userManager, OnlineScopingContext context, EmailConfiguration emailConfig)
        {
            _signInManager = signInManager;
            _service = service;
            _logger = logger;
            _emailSender = emailSender;
            _userManager = userManager;
            _context = context;
            _emailConfig = emailConfig;
            _homeservice = homeservice;
        }


        public async Task<IActionResult> Index()
        {
            HomeViewModel model = new HomeViewModel();

            var a = User.Identity;
            //var message = new EmailMessage(new string[] { "bansh@nextolive.com" }, "Test email", "This is the content from our email.");
            //_emailSender.SendEmail(message);
            var id = _userManager.GetUserId(User); // Get user id:
            var giid = new Guid(id);
            var admin = _context.Users.Where(m => m.Id == id).FirstOrDefault();

            // var user =  _userManager.FindByEmailAsync(admin.Email);
            // Get the roles for the user
            var roles = await _userManager.GetRolesAsync(admin);

            //  model.CustomerViewList = (await _service.GetCustomers()).Select(item => new CustomerViewModel(item)).ToList();
            if (roles[0] == "Sales")
            {
                model.CustomerViewList = _service.NewGetCustomers(new Guid(admin.Id));
            }
            else
            {
                model.CustomerViewList = _service.GetCustomersAdmin();
            }

            var customer = _context.Customer.Where(m => m.UserId == id).FirstOrDefault();
            if (roles[0] == "Customer")
            {
                if (customer != null && customer.IsCustomerUpdate != true)
                {
                    return RedirectToAction("CustomerEditProfile", "Customers", new { id = giid });
                }
                TempData["MyList"] = customer.IsCustomerUpdate;
                //return RedirectToAction("CustomerEditProfile", "Customers", new { id = giid });

            }

            if (customer != null)
            {
                model.Questionnaire = customer.Questionnaire;
                model.CustomerId = customer.CustomerId;
            }
            else
            {
                model.Questionnaire = false;

            }

            var x = model.CustomerViewList.Count();
            TempData["data"] = Convert.ToString(x);

            //total no. of email sent to customers
            var recordCount = model.CustomerViewList.Count(a => a.IsEmailSent == true);
            TempData["count"] = recordCount;

            var recordCntVerifies = model.CustomerViewList.Count(a => a.CustomerQuestionnaircount != 0);
            TempData["RepliedEmail"] = recordCntVerifies;

           // model.QuestionnaireList = _service.QuestionnaireAllList();
            model.QuestionnaireList = _service.QuestionnaireAllClientList();
            model.CustomerQuestionnairList = _service.CustomerQuestionnairList(giid);
           // model.ProjectsViewList = _service.ProjectsViewModelList(giid);
            model.ProjectsViewList = _service.ProjectsViewModelListWithFile(giid);

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> MyProfile(string name)
        {
            HomeViewModel model = new HomeViewModel();
            CustomerViewModel Cmodel = new CustomerViewModel();
            SalesViewModel Smodel = new SalesViewModel();
            var admin = await _userManager.FindByNameAsync(name);

            var user = await _userManager.FindByEmailAsync(admin.Email);
            // Get the roles for the user
            var roles = await _userManager.GetRolesAsync(user);
            if (roles[0] == "Customer")
            {
                var customerdata = _context.Customer.Where(m => m.UserId == admin.Id).FirstOrDefault();
                if (customerdata != null)
                {
                    Cmodel.Zip = customerdata.Zip;
                    Cmodel.UserName = customerdata.UserId;
                    Cmodel.UpdatedDate = customerdata.UpdatedDate;
                    Cmodel.State = customerdata.State;
                    Cmodel.SendDate = customerdata.SendDate;
                    Cmodel.RepliedDate = customerdata.RepliedDate;
                    Cmodel.MobileNumber = customerdata.MobileNumber;
                    Cmodel.LastName = customerdata.LastName;
                    Cmodel.IsEmailSent = customerdata.IsEmailSent;
                    Cmodel.FirstName = customerdata.FirstName;
                    Cmodel.Email = customerdata.Email;
                    Cmodel.CustomerId = customerdata.CustomerId;
                    Cmodel.Address1 = customerdata.Address1;
                    Cmodel.Address2 = customerdata.Address2;
                    Cmodel.City = customerdata.City;
                    Cmodel.Client = customerdata.Client;
                    Cmodel.Country = customerdata.Country;
                    Cmodel.CreatedBy = customerdata.CreatedBy;
                    Cmodel.CreatedDate = customerdata.CreatedDate;
                    Cmodel.UserId = customerdata.UserId;
                }
            }
            else if (roles[0] == "Sales")
            {
                var Salesdata = _context.Sales.Where(m => m.UserId == admin.Id).FirstOrDefault();
                if (Salesdata != null)
                {
                    Smodel.CreatedDate = Salesdata.CreatedDate;
                    Smodel.CustomerId = Salesdata.CustomerId;
                    Smodel.Email = Salesdata.Email;
                    Smodel.FirstName = Salesdata.FirstName;
                    Smodel.IsEmailSent = Salesdata.IsEmailSent;
                    Smodel.LastName = Salesdata.LastName;
                    Smodel.MobileNumber = Salesdata.MobileNumber;
                    Smodel.UpdatedDate = Salesdata.UpdatedDate;
                    Smodel.UserId = Salesdata.UserId;
                }

            }
            else if (roles[0] == "Admin")
            {

            }

            model.CustomerViewData = Cmodel;
            model.SalesViewData = Smodel;
            model.Email = admin.Email;
            model.UserName = admin.UserName;
            model.RoleName = roles[0];
            model.UserId = admin.Id;
            model.FileUpload = admin.FileName;
            //   model.FileUpload = admin.FileUpload;

            return View(model);
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        public IActionResult Report1()
        {
            return View();
        }

        public IActionResult ReportNew()
        {
            return View();
        }

        public IActionResult PageNotFound(int statusCode)
        {

            return View(statusCode);
        }

        public IActionResult PageNotFound404(int statusCode)
        {

            return View(statusCode);
        }
        [AllowAnonymous]
        public IActionResult RegisterConfirmations()
        {

            return View();
        }


        [HttpPost]
        public IActionResult UpdateMyProfile(HomeViewModel model)
        {
            if (model.CustomerViewData.CustomerId != Guid.Empty)
            {
                var fname = string.Empty;
                if (model.CustomerViewData.MyImage != null)
                {
                    var allowedExtensions = new[] { ".png", ".jpeg", ".gif", ".jpg" };
                    var checkextension = Path.GetExtension(model.CustomerViewData.MyImage.FileName).ToLower();

                    if (!allowedExtensions.Contains(checkextension))
                    {
                        TempData["notice"] = "Select pdf or zip or rar less than 20Μ";
                        return RedirectToAction("MyProfile", new { name = model.UserName });
                    }

                    var fileString = model.CustomerViewData.MyImage.FileName.Split('.');
                    fileString[0] = fileString[0];
                    string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                    fname = tempGuid + "_" + fileString[0] + "." + fileString[1];
                    string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(model.CustomerViewData.MyImage.FileName);

                    //Get url To Save
                    string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
                    string file = Path.Combine(SavePath, fname);
                    using (var stream = new FileStream(file, FileMode.Create))
                    {
                        model.CustomerViewData.MyImage.CopyTo(stream);
                    }
                    var data2 = _context.Users.Find(model.UserId);
                    data2.FileName = fname;
                    _context.Update(data2);
                }



                var data = _context.Customer.Find(model.CustomerViewData.CustomerId);
                data.FirstName = model.CustomerViewData.FirstName;
                data.LastName = model.CustomerViewData.LastName;
                data.MobileNumber = model.CustomerViewData.MobileNumber;
                data.Zip = model.CustomerViewData.Zip;
                data.Address1 = model.CustomerViewData.Address1;
                data.Address2 = model.CustomerViewData.Address2;
                data.City = model.CustomerViewData.City;
                data.Country = model.CustomerViewData.Country;
                data.State = model.CustomerViewData.State;

                _context.Update(data);
                _context.SaveChanges();
            }
            TempData["ProfileUpdateMessage"] = "Profile Success";
            return RedirectToAction("MyProfile", new { name = model.UserName });
        }

        [HttpPost]
        public IActionResult UpdateMyProfileAdmin(HomeViewModel model)
        {
            if (model.UserId != null)
            {
                var fname = string.Empty;
                if (model.MyImage != null)
                {
                    var allowedExtensions = new[] { ".png", ".jpeg", ".gif", ".jpg" };
                    var checkextension = Path.GetExtension(model.MyImage.FileName).ToLower();

                    if (!allowedExtensions.Contains(checkextension))
                    {
                        TempData["notice"] = "Select pdf or zip or rar less than 20Μ";
                        return RedirectToAction("MyProfile", new { name = model.UserName });
                    }

                    var fileString = model.MyImage.FileName.Split('.');
                    fileString[0] = fileString[0];
                    string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                    fname = tempGuid + "_" + fileString[0] + "." + fileString[1];
                    string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(model.MyImage.FileName);

                    //Get url To Save
                    string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
                    string file = Path.Combine(SavePath, fname);
                    using (var stream = new FileStream(file, FileMode.Create))
                    {
                        model.MyImage.CopyTo(stream);
                    }
                    var data2 = _context.Users.Find(model.UserId);
                    data2.FileName = fname;
                    _context.Update(data2);
                }
                _context.SaveChanges();
            }

            return RedirectToAction("MyProfile", new { name = model.UserName });
        }

        public IActionResult UpdateMyProfileSales(HomeViewModel model)
        {

            if (model.SalesViewData.CustomerId != null)
            {
                var fname = string.Empty;
                if (model.SalesViewData.MyImage != null)
                {
                    var allowedExtensions = new[] { ".png", ".jpeg", ".gif", ".jpg" };
                    var checkextension = Path.GetExtension(model.SalesViewData.MyImage.FileName).ToLower();

                    if (!allowedExtensions.Contains(checkextension))
                    {
                        TempData["notice"] = "Select pdf or zip or rar less than 20Μ";
                        return RedirectToAction("MyProfile", new { name = model.UserName });
                    }

                    var fileString = model.SalesViewData.MyImage.FileName.Split('.');
                    fileString[0] = fileString[0];
                    string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                    fname = tempGuid + "_" + fileString[0] + "." + fileString[1];
                    string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(model.SalesViewData.MyImage.FileName);

                    //Get url To Save
                    string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
                    string file = Path.Combine(SavePath, fname);
                    using (var stream = new FileStream(file, FileMode.Create))
                    {
                        model.SalesViewData.MyImage.CopyTo(stream);
                    }
                    var data2 = _context.Users.Find(model.UserId);
                    data2.FileName = fname;
                    _context.Update(data2);
                }

                var data = _context.Sales.Find(model.SalesViewData.CustomerId);
                data.FirstName = model.SalesViewData.FirstName;
                data.LastName = model.SalesViewData.LastName;
                data.MobileNumber = model.SalesViewData.MobileNumber;

                _context.Update(data);



                _context.SaveChanges();
            }


            return RedirectToAction("MyProfile", new { name = model.UserName });
        }

        public async Task<IActionResult> RequestPraposal(Guid ProjectId)
        {
            try
            {
                var id = _userManager.GetUserId(User);
                var UserName = _context.Users.Where(m => m.Id == id).FirstOrDefault();
                var alldata = _context.Customer.Where(m => m.UserId == id).FirstOrDefault();
                var RequestMsg = await _homeservice.RequestPraposalAdd(alldata.CustomerId, ProjectId, alldata.CreatedBy, UserName.Id);
                if (RequestMsg > 0)
                {
                    var SalesUserData = _context.Users.Where(m => m.Id == alldata.CreatedBy.ToString()).FirstOrDefault();
                    var msg = SendMailRequesteproposal(SalesUserData.Email, UserName.UserName, SalesUserData.UserName);
                    TempData["RequestPraposalMsg"] = "Request proposal send successfully !";
                }
                else
                {
                    TempData["RequestPraposalErrorMsg"] = "Error in send Request !";
                }
            }
            catch (Exception ex)
            {
                TempData["RequestPraposalErrorMsg"] = "Error in send Request !";
            }


            return RedirectToAction("Index");
        }

        public string SendMailRequesteproposal(string email2, string UserName, string CreatedbyUserName)
        {
            string CurrentYear = DateTime.Now.Year.ToString();
            string body = "<br/><br/>Hi " + CreatedbyUserName + "" +
                "<br/>The customer" + UserName + " has completed the scope" +
                " <br/><br/>& the sub projects and requested a proposal." +
                "<br/><br/>Regards" +
                "<br/> ProCheckUp Ltd." +
                "<br/><p>Copyright © "+ CurrentYear + " <a target='blank' href='https://procheckup.com/'>ProCheckUp Ltd.</a>, All rights reserved.</p>";
            string subject = "Request proposal";

            SmtpClient mySmtpClient = new SmtpClient(_emailConfig.SmtpServer);
            mySmtpClient.Port = _emailConfig.Port;
            mySmtpClient.EnableSsl = false;
            mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            mySmtpClient.UseDefaultCredentials = false;
            MailAddress from = new MailAddress(_emailConfig.From);
            MailAddress to = new MailAddress(email2);
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

        public async Task<IActionResult> AcceptProposal(Guid CustomerId, Guid ProjectId)
        {
            try
            {
                var msg = await _homeservice.AcceptProposalRequest(CustomerId, ProjectId);

                if (msg == 1)
                {
                    var data = _context.CustomerRequests.Where(m => m.CustomerId == CustomerId && m.ProjectId == ProjectId).FirstOrDefault();
                    var customeremial = _context.Customer.Where(m => m.CustomerId == data.CustomerId).FirstOrDefault();

                    var SalesUserData = _context.Users.Where(m => m.Id == customeremial.CreatedBy.ToString()).FirstOrDefault();
                    if (data != null)
                    {
                        var RequestData = _context.CustomerRequests.Find(data.CustomerRequestsId);
                        if (RequestData.IsAccept == true)
                        {
                            string CurrentYear = DateTime.Now.Year.ToString();
                            string body = "<br/><br/>Hi Customer " + customeremial.Client + "" +
                            "<br/>The Salesman " + SalesUserData.UserName + " has accepted your proposal request" +
                            " <br/><br/> " +
                            "<br/><br/>Regards" +
                            "<br/> ProCheckUp Ltd." +
                            "<br/><p>Copyright © "+ CurrentYear + " <a target='blank' href='https://procheckup.com/'>ProCheckUp Ltd.</a>, All rights reserved.</p>";
                            string subject = "Accepted proposal";

                            SmtpClient mySmtpClient = new SmtpClient(_emailConfig.SmtpServer);
                            mySmtpClient.Port = _emailConfig.Port;
                            mySmtpClient.EnableSsl = false;
                            mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                            mySmtpClient.UseDefaultCredentials = false;
                            MailAddress from = new MailAddress(_emailConfig.From);
                            MailAddress to = new MailAddress(customeremial.Email);
                            MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

                            // myMail.CC.Add(email1);
                            myMail.Subject = subject;
                            myMail.Body = body;
                            myMail.IsBodyHtml = true;
                            try
                            {
                                mySmtpClient.Send(myMail);
                                //return "success";
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
                                //return message;
                            }
                        }
                    }
                }

                TempData["AcceptProposalMsg"] = "Request Accepted successfully !";
            }
            catch (Exception ex)
            {
                TempData["AcceptProposalErrerMessage"] = "Error in Request Accept !";
            }
            return RedirectToAction("Index", "Home");
        }




        //public async Task<IActionResult> AcceptProposal(Guid CustomerId, Guid ProjectId)
        //{
        //    try
        //    {
        //        var msg = await _homeservice.AcceptProposalRequest(CustomerId, ProjectId);

        //        if (msg == 1)
        //        {
        //            var data = _context.CustomerRequests.Where(m => m.CustomerId == CustomerId && m.ProjectId == ProjectId).FirstOrDefault();
        //            var customeremial = _context.Customer.Where(m => m.CustomerId == data.CustomerId).FirstOrDefault();

        //            var SalesUserData = _context.Users.Where(m => m.Id == customeremial.CreatedBy.ToString()).FirstOrDefault();
        //            if (data != null)
        //            {
        //                var RequestData = _context.CustomerRequests.Find(data.CustomerRequestsId);
        //                if (RequestData.IsAccept == true)
        //                {
        //                    string body = "<br/><br/>Hi Customer " + customeremial.Client + "" +
        //                    "<br/>The Salesman" + SalesUserData.UserName + " has accepted your proposal request" +
        //                    " <br/><br/> " +
        //                    "<br/><br/>Regards" +
        //                    "<br/> ProCheckUp Ltd." +
        //                    "<br/><p>Copyright © 2020 <a target='blank' href='https://procheckup.com/'>ProCheckUp Ltd.</a>, All rights reserved.</p>";
        //                    string subject = "Accepted proposal";

        //                    SmtpClient mySmtpClient = new SmtpClient(_emailConfig.SmtpServer);
        //                    mySmtpClient.Port = _emailConfig.Port;
        //                    mySmtpClient.EnableSsl = false;
        //                    mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //                    mySmtpClient.UseDefaultCredentials = false;
        //                    MailAddress from = new MailAddress(_emailConfig.From);
        //                    MailAddress to = new MailAddress(customeremial.Email);
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
        //                }
        //            }
        //        }

        //        TempData["AcceptProposalMsg"] = "Request Accepted successfully !";
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["AcceptProposalErrerMessage"] = "Error in Request Accept !";
        //    }
        //    return RedirectToAction("Index", "Home");
        //}

        public async Task<IActionResult> CustomerRecivedProposal(Guid CustomerId, Guid ProjectId)
        {
            try
            {
                var msg = await _homeservice.CustomerAcceptProposalRequest(CustomerId, ProjectId);
                //if (msg == 1)
                //{
                //    TempData["AcceptProposalMsg"] = "Request Accepted successfully !";
                //}
            }
            catch (Exception ex)
            {
                TempData["AcceptProposalErrerMessage"] = "Error in Request Accept !";
            }
            return RedirectToAction("ReceivedProposal", "Customers");
        }
        public async Task<IActionResult> CustomerUplodedDoc(Guid CustomerId, Guid ProjectId)
        {
            try
            {
                var msg = await _homeservice.CustomerDocUplod(CustomerId, ProjectId);

            }
            catch (Exception ex)
            {
                TempData["AcceptProposalErrerMessage"] = "Error !";
            }
            return RedirectToAction("ReceivedProposal", "Customers");
        }
        public IActionResult MethodologiesSampleReport()
        {
            MethodologyeViewModel model = new MethodologyeViewModel();
            model.MethodologiesList = (from a in _context.Methodologye
                                       select new MethodologyeViewModel
                                       {
                                           FileName=a.FileName,
                                           Type=a.Type,
                                           CreatedDate=a.CreatedDate,
                                           Id=a.Id,
                                           CreatedBy=a.CreatedBy,
                                           FileContents=a.FileContents,
                                           UpdatedDate=a.UpdatedDate
                                       }).ToList();

            return View(model);
        }
        [HttpPost]
        public IActionResult MethodologiesSampleReport(MethodologyeViewModel model)
        {
            try
            {
                string fname = string.Empty;
                if (model.FileData != null)
                {
                    var supportedTypes = new[] { "doc", "docx", "pdf" };
                    //Get file Extension
                    var fileExt = System.IO.Path.GetExtension(model.FileData.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        TempData["FileUploadErrorMessage"] = "File Extension Is InValid - Only this type of allow DOC/DOCX/PDF";

                    }
                    else
                    {
                        var fileString = model.FileData.FileName.Split('.');
                        fileString[0] = fileString[0];
                        string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                        fname = fileString[0] + "_" + tempGuid + "." + fileExt;
                        string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(model.FileData.FileName);
                        //Get url To Save
                        string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/Methodology");
                        string file = Path.Combine(SavePath, fname);                 
                        using (var stream = new FileStream(file, FileMode.Create))
                        {
                            model.FileData.CopyTo(stream);
                        }
                        Methodologye ud = new Methodologye();
                        ud.Id= Guid.NewGuid();
                        var id = _userManager.GetUserId(User);
                        ud.CreatedBy = new Guid(id);
                        ud.FileName = model.FileData.FileName;
                        ud.FileContents = "/Uploads/Methodology/" + fname;
                        ud.CreatedDate = DateTime.Now;
                        ud.UpdatedDate = DateTime.Now;
                        ud.Type = model.Type;
                        _context.Add(ud);
                        _context.SaveChanges();
                        TempData["MethodologiesSampleReportMsg"] = "Report Upload successfully !";
                    }
                }
            }
            catch (Exception)
            {
                TempData["MethodologiesSampleReportErrorMsg"] = "Error in Upload Report !";

                throw;
            }

            return RedirectToAction("MethodologiesSampleReport", "Home");
        }


        public IActionResult UpdateMethodologiesSampleReport(MethodologyeViewModel model)
        {
            try
            {
                string fname = string.Empty;
                if (model.FileData != null)
                {
                    var supportedTypes = new[] { "doc", "docx", "pdf" };
                    //Get file Extension
                    var fileExt = System.IO.Path.GetExtension(model.FileData.FileName).Substring(1);
                 
                    if (!supportedTypes.Contains(fileExt))
                    {
                        TempData["FileUploadErrorMessage"] = "File Extension Is InValid - Only this type of allow DOC/DOCX/PDF";

                    }
                    else
                    {
                        var fileString = model.FileData.FileName.Split('.');
                        fileString[0] = fileString[0];
                        string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                        fname = fileString[0] + "_" + tempGuid + "." + fileExt;
                        string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(model.FileData.FileName);
                        //Get url To Save
                        string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/Methodology");
                        string file = Path.Combine(SavePath, fname);
                        using (var stream = new FileStream(file, FileMode.Create))
                        {
                            model.FileData.CopyTo(stream);
                        }

                        var data = _context.Methodologye.Find(model.Id);
                        if (data != null)
                        {
                            string _imageToBeDeleted = Path.Combine("wwwroot" + data.FileContents);
                            if ((System.IO.File.Exists(_imageToBeDeleted)))
                            {
                                System.IO.File.Delete(_imageToBeDeleted);
                            }

                            data.FileName = model.FileData.FileName;
                            data.FileContents = "/Uploads/Methodology/" + fname;
                            data.UpdatedDate = DateTime.Now;
                            _context.Methodologye.Update(data);

                        }                     
                        _context.SaveChanges();
                        TempData["MethodologiesSampleReportMsg"] = "Report Update successfully !";
                    }
                }
            }
            catch (Exception)
            {
                TempData["MethodologiesSampleReportErrorMsg"] = "Error in Update Report !";

                throw;
            }

            return RedirectToAction("MethodologiesSampleReport", "Home");
        }


        public async Task<IActionResult> DeleteMethodologies(Guid Id)
        {
            try
            {
                if (Id != Guid.Empty)
                {
                    var data = _context.Methodologye.Find(Id);
                    if (data != null)
                    {
                        string _imageToBeDeleted = Path.Combine("wwwroot" + data.FileContents);
                        if ((System.IO.File.Exists(_imageToBeDeleted)))
                        {
                            System.IO.File.Delete(_imageToBeDeleted);
                        }
                        _context.Methodologye.Remove(data);
                        await _context.SaveChangesAsync();

                        TempData["MethodologiesSampleReportMsg"] = "Report Delete successfully !";
                        return RedirectToAction("MethodologiesSampleReport", "Home");
                    }
                }
            }catch(Exception ex)
            {
                TempData["MethodologiesSampleReportErrorMsg"] = "Error in Delete Report !";
                return RedirectToAction("MethodologiesSampleReport", "Home");
            }
            return RedirectToAction("MethodologiesSampleReport", "Home");

        }

        [HttpPost]
        public async Task<JsonResult> DeleteMethodologiesAjx(string id)
        {
            var Id = new Guid(id);
            try
            {
                if (Id != Guid.Empty)
                {
                    var data = _context.Methodologye.Find(Id);
                    if (data != null)
                    {
                        string _imageToBeDeleted = Path.Combine("wwwroot" + data.FileContents);
                        if ((System.IO.File.Exists(_imageToBeDeleted)))
                        {
                            System.IO.File.Delete(_imageToBeDeleted);
                        }
                        _context.Methodologye.Remove(data);
                        await _context.SaveChangesAsync();

                        TempData["MethodologiesSampleReportMsg"] = "Report Delete successfully !";
                        return Json("Success");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["MethodologiesSampleReportErrorMsg"] = "Error in Delete Report !";
                return Json("Error");
            }

            return Json("Success");
        }

        [HttpPost]
        public IActionResult UploadFile(HomeViewModel model)
        {
            if (model.UploadedDocumentsData.ProjectId != Guid.Empty && model.UploadedDocumentsData.CustomerId != Guid.Empty)
            {
                var fname = string.Empty;
                var id = _userManager.GetUserId(User);
                var projectsId = model.UploadedDocumentsData.ProjectId;
                var customerId = model.UploadedDocumentsData.CustomerId;
                if (model.UploadedDocumentsData.MyImage != null)
                {
                    try
                    {
                        var supportedTypes = new[] { "doc", "docx", "pdf", "xls", "xlsx", "png", "jpg", "jpeg", "json", "txt" };
                        //Get file Extension
                        var fileExt = System.IO.Path.GetExtension(model.UploadedDocumentsData.MyImage.FileName).Substring(1);
                        if (!supportedTypes.Contains(fileExt))
                        {
                            TempData["UploadFileErrorMessage"] = "File Extension Is InValid - Only this type of allow DOC/DOCX/WORD/PDF/EXCEL/JPG/PNG/JPEG/JSON File";

                        }
                        else if (model.UploadedDocumentsData.MyImage.Length > (31457280))
                        {
                            TempData["UploadFileErrorMessage"] = "Select file size Should Be less than 30 MB";
                        }
                        else
                        {
                            var fileString = model.UploadedDocumentsData.MyImage.FileName.Split('.');
                            fileString[0] = fileString[0];
                            string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                            fname = fileString[0] + "_" + tempGuid + "." + fileString[1];
                            string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(model.UploadedDocumentsData.MyImage.FileName);
                            //Get url To Save
                            string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/Documents");
                            string file = Path.Combine(SavePath, fname);
                            using (var stream = new FileStream(file, FileMode.Create))
                            {
                                model.UploadedDocumentsData.MyImage.CopyTo(stream);
                            }
                            UploadFiles ud = new UploadFiles();
                            ud.UploadedDocumentsId = Guid.NewGuid();
                            ud.CustomerId = model.UploadedDocumentsData.CustomerId;
                            ud.ProjectId = model.UploadedDocumentsData.ProjectId;
                            ud.FileName = model.UploadedDocumentsData.MyImage.FileName;
                            ud.FileContents = "/Uploads/Documents/" + fname;
                            ud.FileModifiedTime = DateTime.Now;
                            ud.Createdby = new Guid(id);
                            _context.Add(ud);
                            _context.SaveChanges();
                            var data = _context.Projects.Where(m => m.ProjectsId == projectsId).FirstOrDefault();
                            var customeremial = _context.Customer.Where(m => m.CustomerId == customerId).FirstOrDefault();

                            var SalesUserData = _context.Users.Where(m => m.Id == customeremial.CreatedBy.ToString()).FirstOrDefault();
                            string CurrentYear = DateTime.Now.Year.ToString();
                            string body = "<br/><br/>Hi " + SalesUserData.UserName + "," +
                                "<br/>" + customeremial.FirstName + " from " + "" + "has uploaded documents for " + data.ProjectName + "." +
                           //"<br/> The status of your project: " + SalesUserData.ProjectName + " has changed to: " + SalesUserData.RequestStatus + "." +
                           " <br/> " +
                                   "<br/>Thanks," +
                                   " <br/> " +
                                   "<br/> ProCheckUp Team" +
                                   "<br/><p>Copyright ©" + CurrentYear + " <a target='blank' href='https://procheckup.com/'>ProCheckUp Ltd.</a>, All rights reserved.</p>";
                            string subject = "Upload Document";

                            SmtpClient mySmtpClient = new SmtpClient(_emailConfig.SmtpServer);
                            mySmtpClient.Port = _emailConfig.Port;
                            mySmtpClient.EnableSsl = false;
                            mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                            mySmtpClient.UseDefaultCredentials = false;
                            MailAddress from = new MailAddress(_emailConfig.From);
                            MailAddress to = new MailAddress(SalesUserData.Email);
                            MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

                            // myMail.CC.Add(email1);
                            myMail.Subject = subject;
                            myMail.Body = body;
                            myMail.IsBodyHtml = true;
                            try
                            {
                                mySmtpClient.Send(myMail);
                                //return "success";
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
                                //return message;
                            }

                            TempData["UploadFileMessage"] = "File upload succesfully !";
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["UploadFileErrorMessage"] = "Error in uploading File !";
                    }

                }
            }
            return RedirectToAction("Index", "Home");

        }

        //[HttpPost]
        //public IActionResult UploadFile(HomeViewModel model)
        //{
        //    if (model.UploadedDocumentsData.ProjectId != Guid.Empty && model.UploadedDocumentsData.CustomerId != Guid.Empty)
        //    {
        //        var fname = string.Empty;
        //        var id = _userManager.GetUserId(User);
        //        if (model.UploadedDocumentsData.MyImage != null)
        //        {
        //            try
        //            {
        //                var supportedTypes = new[] { "doc", "docx", "pdf", "xls", "xlsx", "png", "jpg", "jpeg", "json", "txt" };
        //                //Get file Extension
        //                var fileExt = System.IO.Path.GetExtension(model.UploadedDocumentsData.MyImage.FileName).Substring(1);
        //                if (!supportedTypes.Contains(fileExt))
        //                {
        //                    TempData["UploadFileErrorMessage"] = "File Extension Is InValid - Only this type of allow DOC/DOCX/WORD/PDF/EXCEL/JPG/PNG/JPEG/JSON File";

        //                }
        //                else if (model.UploadedDocumentsData.MyImage.Length > (31457280))
        //                {
        //                    TempData["UploadFileErrorMessage"] = "Select file size Should Be less than 30 MB";
        //                }
        //                else
        //                {
        //                    var fileString = model.UploadedDocumentsData.MyImage.FileName.Split('.');
        //                    fileString[0] = fileString[0];
        //                    string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
        //                    fname = fileString[0] + "_" + tempGuid + "." + fileString[1];
        //                    string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(model.UploadedDocumentsData.MyImage.FileName);
        //                    //Get url To Save
        //                    string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/Documents");
        //                    string file = Path.Combine(SavePath, fname);
        //                    using (var stream = new FileStream(file, FileMode.Create))
        //                    {
        //                        model.UploadedDocumentsData.MyImage.CopyTo(stream);
        //                    }

        //                    UploadFiles ud = new UploadFiles();
        //                    ud.UploadedDocumentsId = Guid.NewGuid();
        //                    ud.CustomerId = model.UploadedDocumentsData.CustomerId;
        //                    ud.ProjectId = model.UploadedDocumentsData.ProjectId;
        //                    ud.FileName = model.UploadedDocumentsData.MyImage.FileName;
        //                    ud.FileContents = "/Uploads/Documents/" + fname;
        //                    ud.FileModifiedTime = DateTime.Now;
        //                    ud.Createdby = new Guid(id);
        //                    _context.Add(ud);
        //                    _context.SaveChanges();

        //                    TempData["UploadFileMessage"] = "File upload succesfully !";
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                TempData["UploadFileErrorMessage"] = "Error in uploading File !";
        //            }

        //        }
        //    }
        //    return RedirectToAction("Index", "Home");

        //}



        public async Task<IActionResult> DeleteFiles(Guid id, Guid customerId)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    var data = _context.UploadFiles.Find(id);
                    if (data != null)
                    {
                        string _imageToBeDeleted = Path.Combine("wwwroot" + data.FileContents);
                        if ((System.IO.File.Exists(_imageToBeDeleted)))
                        {
                            System.IO.File.Delete(_imageToBeDeleted);
                        }
                        _context.UploadFiles.Remove(data);
                        await _context.SaveChangesAsync();

                        TempData["UploadFileMessage"] = "File Delete successfully !";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["UploadFileErrorMessage"] = "File in Delete Proposal !";
            }
            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> DeleteProchechkupUploadsFiles(Guid id, Guid customerId)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    var data = _context.UploadedDocuments.Find(id);
                    if (data != null)
                    {
                        string _imageToBeDeleted = Path.Combine("wwwroot" + data.FileContents);
                        if ((System.IO.File.Exists(_imageToBeDeleted)))
                        {
                            System.IO.File.Delete(_imageToBeDeleted);
                        }
                        _context.UploadedDocuments.Remove(data);
                        await _context.SaveChangesAsync();

                        TempData["UploadFileMessage"] = "File Delete successfully !";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["UploadFileErrorMessage"] = "File in Delete Proposal !";
            }
            return RedirectToAction("Index", "Home");

        }

        public IActionResult Toggle()
        {
            PermissionsViewModel model = new PermissionsViewModel();
            model.PermissionsList = _service.PermissionsAllList();          
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> PermissionsUpdate(string Toggle = null, string PermissionsId = null)
        {
            var permissionsId = new Guid(PermissionsId);
            DbResponce a = new DbResponce();
            try
            {
                if (permissionsId != Guid.Empty)
                {
                    a = await _service.PermissionsUpdate(Toggle, permissionsId);
                }
                return Json(a.msg);
            }
            catch (Exception ex)
            {
                return Json("Error");
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
                            await _service.AddNewProject(model.ProjectsViewData);
                            TempData["ProjectMessage"] = "Project Save succesfully !";

                            var customeremial = _context.Customer.Where(m => m.CustomerId == customer.CustomerId).FirstOrDefault();

                            var SalesUserData = _context.Users.Where(m => m.Id == customeremial.CreatedBy.ToString()).FirstOrDefault();
                            string CurrentYear = DateTime.Now.Year.ToString();
                            string body = "<br/><br/>Hi " + SalesUserData.UserName + "," +
                                "<br/>" + customeremial.FirstName + "" + "has created a new project called: " + model.ProjectsViewData.ProjectName + "." +
                           //"<br/> The status of your project: " + SalesUserData.ProjectName + " has changed to: " + SalesUserData.RequestStatus + "." +
                           " <br/> " +
                                   "<br/><br/>Thanks," +
                                   " <br/><br/> " +
                                   "<br/> ProCheckUp Team" +
                                   "<br/><p>Copyright ©" + CurrentYear + " <a target='blank' href='https://procheckup.com/'>ProCheckUp Ltd.</a>, All rights reserved.</p>";
                            string subject = "New Project";

                            SmtpClient mySmtpClient = new SmtpClient(_emailConfig.SmtpServer);
                            mySmtpClient.Port = _emailConfig.Port;
                            mySmtpClient.EnableSsl = false;
                            mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                            mySmtpClient.UseDefaultCredentials = false;
                            MailAddress from = new MailAddress(_emailConfig.From);
                            MailAddress to = new MailAddress(SalesUserData.Email);
                            MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

                            // myMail.CC.Add(email1);
                            myMail.Subject = subject;
                            myMail.Body = body;
                            myMail.IsBodyHtml = true;
                            try
                            {
                                mySmtpClient.Send(myMail);
                                //return "success";
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
                                //return message;
                            }













                            var data = model.ProjectsViewData;
                            // var custemail = _context.Customer.Where(m => m.CustomerId == customer.CustomerId).FirstOrDefault();

                            // var SalesUserData = _context.Users.Where(m => m.Id == custemail.CreatedBy.ToString()).FirstOrDefault();
                            // string CurrentYear = DateTime.Now.Year.ToString();
                            // string body = "<br/><br/>Hi " + SalesUserData.UserName + "," +
                            //     "<br/><br/>" + custemail.FirstName + "" + " has created a new project called: " + data.ProjectName + "." +
                            //" <br/> " +
                            //        "<br/><br/>Thanks," +
                            //        "<br/> ProCheckUp Team" +
                            //        "<br/><p>Copyright ©" + CurrentYear + " <a target='blank' href='https://procheckup.com/'>ProCheckUp Ltd.</a>, All rights reserved.</p>";
                            // string subject = "New Project";

                            // SmtpClient mySmtpClient = new SmtpClient(_emailConfig.SmtpServer);
                            // mySmtpClient.Port = _emailConfig.Port;
                            // mySmtpClient.EnableSsl = false;
                            // mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                            // mySmtpClient.UseDefaultCredentials = false;
                            // MailAddress from = new MailAddress(_emailConfig.From);
                            // MailAddress to = new MailAddress(SalesUserData.Email);
                            // MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

                            // myMail.Subject = subject;
                            // myMail.Body = body;
                            // myMail.IsBodyHtml = true;
                            // try
                            // {
                            //     mySmtpClient.Send(myMail);
                            // }
                            // catch (Exception se)
                            // {
                            //     string message = se.Message;
                            //     Exception seie = se.InnerException;
                            //     if (seie != null)
                            //     {
                            //         message += ", " + seie.Message;
                            //         Exception seieie = seie.InnerException;
                            //         if (seieie != null)
                            //         {
                            //             message += ", " + seieie.Message;
                            //         }
                            //     }
                            // }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    return RedirectToAction("Index", "Home");
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
        [AllowAnonymous]
        public  ActionResult EmailConfirmOTPPage(string _id)
        {
            ViewBag.id = _id;
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> EmailConfirmOTPPage(string otp, string id, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var ps = Convert.ToString(TempData["ps"]);
            if (id!=null && id!="" && ps!=null && ps != "")
            {
                var res = _context.Users.Where(x => x.Id == id && x.OTP == otp).SingleOrDefault();
                if (res != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(res.UserName,ps,true, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        TempData["LastLoginDate"] = "Userloggedin.";
                        //return LocalRedirect(returnUrl);
                        return new JsonResult(new { message = "success"});
                    }
                    else
                    {
                        return new JsonResult(new { message = "error"});
                    }
                }
            }
            return new JsonResult(new { message = "error"});
        }

        [AllowAnonymous]
        public  JsonResult ResendOtp(string id)
        {
            if (id!=null)
            {
                var data = _context.Users.Where(m => m.Id == id).FirstOrDefault();
                //   var ps = Convert.ToString(TempData["ps"]);
                Random rnd = new Random();
                string Code = "";
                for (int i = 0; i < 4; i++)
                {
                    Code += rnd.Next(1, 9);
                }
                var _id = data.Id;
                var msg = SendVerificationEmail(data.Email, data.UserName, Code);
                if (msg == "success")
                {
                    var res = _context.Users.Where(x => x.Id == _id).SingleOrDefault();
                    if (res != null)
                    {
                        res.OTP = Code;
                        _context.SaveChanges();
                    }
                    return new JsonResult(new { message = "success" });
                }
                else
                {
                    return new JsonResult(new { message = "Error Messages" });
                }
            }
            else
            {
                return new JsonResult(new { message = "Login Again" });
            }
        }

        private string SendVerificationEmail(string emailId, string FirstName, string otp)
        {
            string CurrentYear = DateTime.Now.Year.ToString();
            string body = "<br/><br/>Hi " + FirstName + "" +
           "<br/>Your Login OTP Is " + otp +
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

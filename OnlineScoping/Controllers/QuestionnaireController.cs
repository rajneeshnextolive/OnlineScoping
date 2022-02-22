using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using OnlineScoping.Data;
using OnlineScoping.Models;
using OnlineScoping.Services;
using OnlineScoping.Utilities.EmailService;
using OnlineScoping.ViewModels;

namespace OnlineScoping.Controllers
{
    public class QuestionnaireController : Controller
    {
        private readonly IQuestionnaireService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OnlineScopingContext _context;
        private readonly EmailConfiguration _emailConfig;
        public QuestionnaireController(OnlineScopingContext context, IQuestionnaireService service, UserManager<ApplicationUser> userManager, EmailConfiguration emailConfig)
        {
            _context = context;
            _service = service;
            _userManager = userManager;
            _emailConfig = emailConfig;
        }

        public IActionResult Index(Guid Questionnaireid, Guid ProjectId)
        {
            var questionnaire = new QuestionnaireViewModel()
            {
                QuestionnaireId = Questionnaireid,
                ProjectId = ProjectId,
                QuestionnaireData = (from q in _context.Questionnaire
                                     where q.QuestionnaireId == Questionnaireid
                                     select new QuestionnaireViewModel
                                     {
                                         QuestionnaireId = q.QuestionnaireId,
                                         Name = q.Name
                                     }).FirstOrDefault(),
                Questions = new List<QuestionsViewModel>()
                {
                    new QuestionsViewModel()
                    {
                        QuestionId = Guid.NewGuid(),
                        SectionId = 1,
                        QuestionNumber = "1",
                        QuestionType = QuestionType.multipleResponseType,
                        QuestionText = "Reason for security services " ,
                        QuestionnaireId=Questionnaireid,
                        OptionsList = new List<OptionsViewModel>()
                        {
                            new OptionsViewModel()
                            {
                                OptionId1 = 1,
                                OptionText = "verification test (to verify issues found in previous pentest have been fixed)",
                            },
                             new OptionsViewModel()
                            {
                                OptionId1 = 2,
                                OptionText = "retest",
                            },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 3,
                                OptionText = "we suspect we’ve been hacked",
                             },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 4,
                                OptionText = "ITHC",
                             },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 5,
                                OptionText = "need a specific component tested only",
                             },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 6,
                                OptionText = "to better understand security posture of our environment",
                             },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 7,
                                OptionText = "required by third party",
                             },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 8,
                                OptionText = "PCI",
                             },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 9,
                                OptionText = "compliance/accreditation",
                             }


                        }
                    }
                }
            };
            return View(questionnaire);
        }

        [HttpPost]
        public IActionResult Index(QuestionnaireViewModel model)
        {
            return RedirectToAction("ChildQuestions", new { SectionId = model.Questions[0].ResponseOptionId1, QuestionnaireId = model.QuestionnaireId, ProjectId = model.ProjectId });
        }

        [HttpGet]
        public IActionResult ChildQuestions(string SectionId, Guid QuestionnaireId, Guid ProjectId, Guid id)
        {
            QuestionnaireViewModel model = new QuestionnaireViewModel();
            try
            {
                int Id = Convert.ToInt32(SectionId);
                model = _service.GetQuestionsById(Id, QuestionnaireId);
                model.CustomerId = id;
                model.ProjectId = ProjectId;
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChildQuestionsAsync(QuestionnaireViewModel model)
        {
            var id = _userManager.GetUserId(User);
            var UserName = _context.Users.Where(m => m.Id == id).FirstOrDefault();
            try
            {
                if (model.Questions == null)
                {
                    model.UserId = id;
                    await _service.ResponseQuestionsbySection(model);
                    var responce = await _service.CustomerUpdate(model);
                    return RedirectToAction("ENDofsurvey", new { SectionId = model.SectionId.ToString() });
                }
                ClaimsPrincipal currentUser = User;
                // Get user id:
                model.UserId = id;

                var a = new Guid("0e714cfd-eeaf-4578-a680-27058a139d35");
                if (model.Questions[0].SectionId == 4 && model.Questions[0].ResponseOptionId == a)
                {
                    return RedirectToAction("ChildQuestions", new { SectionId = "6", QuestionnaireId = model.QuestionnaireId });
                }
                else if (model.Questions[0].SectionId == 7 && model.Questions[0].ResponseOptionId == new Guid("af6d6ef3-46b2-4ef7-85fe-25e3994df1f8"))
                {
                    return RedirectToAction("ChildQuestions", new { SectionId = "6", QuestionnaireId = model.QuestionnaireId });
                }
                else
                {
                    var data = await _service.ResponseQuestions(model);
                    var responce = await _service.CustomerUpdate(model);


                    //var alldata = _context.Customer.Where(m => m.UserId == id).FirstOrDefault();
                    //var Userdata = _context.Users.Where(m => m.Id == alldata.CreatedBy.ToString()).FirstOrDefault();
                    //var varifyUrl = Request.Scheme + "://scoping.procheckup.com/Questionnaire/QuestionnaireResponseDetails?id=" + data.CustomerId + "&Projectid=" + data.ProjectId + "&QuestionnaireId=" + data.QuestionnaireId + "&Page=HomeIndex";

                    //var d = SendMailQuestionner(alldata.Email, Userdata.Email, varifyUrl, UserName.UserName, Userdata.UserName);


                    return RedirectToAction("ENDofsurvey", new { SectionId = data.SectionId.ToString() });
                }
            }
            catch (Exception ex) { throw; }
        }

        [HttpGet]
        public IActionResult ENDofsurvey(string SectionId)
        {
            QuestionResponseViewModel model = new QuestionResponseViewModel();
            model.SectionId = Convert.ToInt32(SectionId);
            return View(model);
        }


        public IActionResult ResponseDetails(Guid id, Guid QuestionnaireId, Guid Projectid, string Page = null)
        {

            QuestionnaireViewModel model = new QuestionnaireViewModel();
            try
            {

                //model = _service.GetResponseQuestionsById(id, QuestionnaireId);
                model = _service.ResponseDetailsByProjectId(id, QuestionnaireId, Projectid);
                model.SectionId = model.Questions[0].QuestionData.SectionId;
                model.CustomerId = id;
            }
            catch (Exception ex)
            {

            }
            model.Pages = Page;
            return View(model);
        }


        [HttpGet]
        public IActionResult ModuleWiseQuestions(Guid id, Guid ProjectId)
        {
            QuestionnaireViewModel model = new QuestionnaireViewModel();
            try
            {
                if (id == new Guid("a8837cf2-39cc-464e-a120-4248ea2cde0c"))
                {
                    return RedirectToAction("Index", new { Questionnaireid = id, ProjectId = ProjectId });
                }
                else if (id == new Guid("0A2DFC0B-1F17-447C-A7B8-067F9980325A"))
                {
                    return RedirectToAction("PCIASVquestionnaire", new { id = id, ProjectId = ProjectId });
                }
                else if (id == new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323"))
                {
                    return RedirectToAction("C_E_Questionaires", new { id = id, ProjectId = ProjectId });
                }
                else
                {
                    model = _service.GetQuestionsByModule(id);
                    model.ProjectId = ProjectId;
                    var data = _context.Projects.Where(m => m.ProjectsId == ProjectId).FirstOrDefault();
                    if (data != null)
                    {
                        model.ProjectName = data.ProjectName;
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ModuleWiseQuestions(QuestionnaireViewModel model)
        {
            var Id = model.QuestionnaireId;
            var id = _userManager.GetUserId(User);
            model.UserId = id;
            var UserName = _context.Users.Where(m => m.Id == id).FirstOrDefault();
            var data = await _service.ResponseModuleWiseQuestions(model);
            //var alldata = _context.Customer.Where(m => m.UserId == id).FirstOrDefault();
            //var Userdata = _context.Users.Where(m => m.Id == alldata.CreatedBy.ToString()).FirstOrDefault();
            //var varifyUrl = Request.Scheme + "://scoping.procheckup.com/Questionnaire/QuestionnaireResponseDetails?id=" + data.CustomerId+ "&Projectid="+data.ProjectId + "&QuestionnaireId="+data.QuestionnaireId+ "&Page=HomeIndex";

            //var d = SendMailQuestionner(alldata.Email, Userdata.Email, varifyUrl, UserName.UserName, Userdata.UserName);
            var custemail = _context.Customer.Where(m => m.UserId == id).FirstOrDefault();
            var ques = _context.Questionnaire.Where(m => m.QuestionnaireId == Id).FirstOrDefault();
            var SalesUserData = _context.Users.Where(m => m.Id == custemail.CreatedBy.ToString()).FirstOrDefault();
            if (custemail != null)
            {
                string CurrentYear = DateTime.Now.Year.ToString();
                string body = "<br/><br/>Hi " + SalesUserData.UserName + "," +
                    "<br/><br/>" + custemail.FirstName + " has " + "filled in the following questionnaire: " + ques.Name + "," + "for the following project:" + model.ProjectName + "." +
               " <br/>" +
                       "<br/><br/>Thanks," +
                       "<br/> ProCheckUp Team" +
                       "<br/><p>Copyright ©" + CurrentYear + " <a target='blank' href='https://procheckup.com/'>ProCheckUp Ltd.</a>, All rights reserved.</p>";
                string subject = "New Questionnaire Filled.";

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
            }


            TempData["QuestionnaireSuccessMessage"] = "successfully";
            return RedirectToAction("Index", "Home");
        }

        //public async Task<IActionResult> ModuleWiseQuestions(QuestionnaireViewModel model)
        //{
        //    var id = _userManager.GetUserId(User);
        //    model.UserId = id;
        //    var UserName = _context.Users.Where(m => m.Id == id).FirstOrDefault();
        //    var data = await _service.ResponseModuleWiseQuestions(model);
        //    //var alldata = _context.Customer.Where(m => m.UserId == id).FirstOrDefault();
        //    //var Userdata = _context.Users.Where(m => m.Id == alldata.CreatedBy.ToString()).FirstOrDefault();
        //    //var varifyUrl = Request.Scheme + "://scoping.procheckup.com/Questionnaire/QuestionnaireResponseDetails?id=" + data.CustomerId+ "&Projectid="+data.ProjectId + "&QuestionnaireId="+data.QuestionnaireId+ "&Page=HomeIndex";

        //    //var d = SendMailQuestionner(alldata.Email, Userdata.Email, varifyUrl, UserName.UserName, Userdata.UserName);

        //    TempData["QuestionnaireSuccessMessage"] = "successfully";
        //    return RedirectToAction("Index", "Home");
        //}




        public string SendMailQuestionner(string email1, string email2, string varifyUrl, string UserName, string CreatedbyUserName)
        {

            string body = "<br/><br/>Hi" + CreatedbyUserName + "" +
                "<br/>Please find the completed scope of Customer " + UserName + "" +
                " <br/><br/><a href='" + varifyUrl + "'>Click here</a>" +
                "<br/><br/>Regards" +
                "<br/> ProCheckUp Ltd." +
                "<br/><p>Copyright © 2020 <a target='blank' href='https://procheckup.com/'>ProCheckUp Ltd.</a>, All rights reserved.</p>";
            string subject = "Show Questionnaire";

            SmtpClient mySmtpClient = new SmtpClient(_emailConfig.SmtpServer);
            mySmtpClient.Port = _emailConfig.Port;
            mySmtpClient.EnableSsl = false;
            mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            mySmtpClient.UseDefaultCredentials = false;
            MailAddress from = new MailAddress(_emailConfig.From);
            MailAddress to = new MailAddress(email2);
            MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

            myMail.CC.Add(email1);
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

        public IActionResult QuestionnaireResponseDetails(Guid id, Guid Projectid, Guid QuestionnaireId, string Page = null)
        {
            QuestionnaireViewModel model = new QuestionnaireViewModel();

            try
            {
                if (QuestionnaireId == new Guid("a8837cf2-39cc-464e-a120-4248ea2cde0c"))
                {
                    return RedirectToAction("ResponseDetails", new { id = id, QuestionnaireId = QuestionnaireId, Projectid = Projectid, Page = Page });
                }
                else if (QuestionnaireId == new Guid("54515c61-e5c1-43d5-87ca-af8102a06323"))
                {
                    return RedirectToAction("CEQResponseDetails", new { Userid = id, id = QuestionnaireId, Projectid = Projectid, Page = Page });
                }
                else if (QuestionnaireId == new Guid("0A2DFC0B-1F17-447C-A7B8-067F9980325A"))
                {
                    // model = _service.GetResponseQuestionsById3(id, QuestionnaireId);
                    model = _service.GetResponseQuestionsByProjectId(id, Projectid, QuestionnaireId);
                    model.QuestionnaireId = QuestionnaireId;
                    model.CustomerId = id;
                }

                else
                {
                    model = _service.GetResponseQuestionsProjectById(id, Projectid, QuestionnaireId);
                    model.QuestionnaireId = QuestionnaireId;
                    model.CustomerId = id;
                }

            }
            catch (Exception ex)
            {

            }
            model.Pages = Page;
            return View(model);
        }


        public IActionResult CustomerQuestionnair(Guid id)
        {
            CustomerQuestionnairViewModel model = new CustomerQuestionnairViewModel();

            model.QuestionnaireList = _service.QuestionnaireAllList();
            model.CustomerQuestionnairList = _service.CustomerQuestionnairList(id);
            model.ProjectsViewList = _service.ProjectsViewModelList(id);
            model.UploadedDocViewList = _service.UploadedDocumentsViewModel(id);
            model.CustomerId = id;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CustomerReportData(CustomerQuestionnairViewModel model)
        {

            var msg = await _service.CustomerReportDataSave(model);
            if (msg.CustomerProposalId != null)
            {
                if (msg.QuestionnaireId == new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323"))
                {
                    return RedirectToAction("CEQCustomerReport", new { Userid = model.CustomerId, CustomerProposalId = msg.CustomerProposalId, ProjectId = model.ProjectId, id = model.QuestionnaireId });
                }
                else
                {
                    return RedirectToAction("CustomerReport", new { id = model.CustomerId, ProjectId = model.ProjectId, CustomerProposalId = msg.CustomerProposalId });

                }
            }
            else
            {
                return RedirectToAction("CustomerQuestionnair", new { id = model.CustomerId });
            }
        }
        public IActionResult CustomerReport1(Guid id, Guid ProjectId, Guid CustomerProposalId)
        {
            ProposalReportViewModel model = new ProposalReportViewModel();
            QuestionnaireReportDataModel CalModel = new QuestionnaireReportDataModel();
            List<QuestionnaireReportDataModel> QuestionnaireList = new List<QuestionnaireReportDataModel>();

            model.CustomerProposalViewModel = _service.CustomerProposalAllData(CustomerProposalId);
            Guid questid = model.CustomerProposalViewModel.QuestionnaireId;
            var qdata = _context.Questionnaire.Where(m => m.QuestionnaireId == questid).FirstOrDefault();
            if (qdata != null)
            {
                model.QName = qdata.Name;
            }

            model.CustomerViewModel = _service.CustomerAllData(id);
            //model.AllProjectsList = _service.AllProjectsData(id);
            // model.AllProjectsList = ProjectAllData(id);
            model.SalesViewModel = _service.SalesAllData(id);
            model.Projectsdata = _service.projectsdata(id, ProjectId);
            if (model.CustomerProposalViewModel.QuestionnaireId == new Guid("a8837cf2-39cc-464e-a120-4248ea2cde0c"))
            {
                model.QuestionnaireViewModel = _service.GetResponseQuestionsByProposalId(id, CustomerProposalId, ProjectId);
                model.QuestionnaireViewModel.SectionId = model.QuestionnaireViewModel.Questions[0].QuestionData.SectionId;
                model.QuestionnaireViewModel.QuestionnaireId = model.QuestionnaireViewModel.Questions[0].QuestionData.QuestionnaireId;
                // model.QuestionnaireList = _service.QuestionnaireAllListForReport();
                model.QuestionnaireList = _service.QuestionnaireListForReportWithDetails(id, ProjectId);

                foreach (var item in model.QuestionnaireList)
                {
                    CalModel.QuantityDays = item.Questions.Sum(x => x.Days) + item.Questions.Sum(x => x.ChildQuestionsList.Sum(y => y.Days));
                    CalModel.Name = item.Name;
                    CalModel.QuestionnaireId = item.QuestionnaireId;
                    CalModel.QuestionnairDetailsViewdata = item.QuestionnairDetailsViewdata;
                    CalModel.ProjectsViewdata = item.ProjectsViewdata;
                    CalModel.TotalPrice = ((item.Questions.Sum(x => x.Days) + item.Questions.Sum(x => x.ChildQuestionsList.Sum(y => y.Days))) * 900);
                    QuestionnaireList.Add(CalModel);
                    CalModel = new QuestionnaireReportDataModel();
                }
                model.QuestionnaireList = QuestionnaireList;
                model.TotalExcludingVAT = model.QuestionnaireList.Sum(m => m.TotalPrice);
                model.TotalExcludingDay = model.QuestionnaireList.Sum(m => m.QuantityDays);
            }
            else if (model.CustomerProposalViewModel.QuestionnaireId == new Guid("0A2DFC0B-1F17-447C-A7B8-067F9980325A"))
            {
                model.QuestionnaireViewModel = _service.GetResponseQuestionsByProjectId(id, ProjectId, questid);
                model.QuestionnaireViewModel.QuestionnaireId = model.QuestionnaireViewModel.Questions[0].QuestionData.QuestionnaireId;
                // model.QuestionnaireViewModel.DaysSum = model.QuestionnaireViewModel.Questions.Sum(x => x.Days) + model.QuestionnaireViewModel.Questions.Sum(x => x.ChildQuestionsList.Sum(y => y.Days));
                // model.QuestionnaireList = _service.QuestionnaireAllListForReport();
                model.QuestionnaireList = _service.QuestionnaireListForReportWithDetails(id, ProjectId);
                foreach (var item in model.QuestionnaireList)
                {
                    CalModel.QuantityDays = item.Questions.Sum(x => x.Days) + item.Questions.Sum(x => x.ChildQuestionsList.Sum(y => y.Days));
                    CalModel.Name = item.Name;
                    CalModel.QuestionnaireId = item.QuestionnaireId;
                    CalModel.QuestionnairDetailsViewdata = item.QuestionnairDetailsViewdata;
                    CalModel.ProjectsViewdata = item.ProjectsViewdata;
                    CalModel.TotalPrice = ((item.Questions.Sum(x => x.Days) + item.Questions.Sum(x => x.ChildQuestionsList.Sum(y => y.Days))) * 900);
                    QuestionnaireList.Add(CalModel);
                    CalModel = new QuestionnaireReportDataModel();
                }
                model.QuestionnaireList = QuestionnaireList;
                model.TotalExcludingVAT = model.QuestionnaireList.Sum(m => m.TotalPrice);
                model.TotalExcludingDay = model.QuestionnaireList.Sum(m => m.QuantityDays);
            }
            else
            {
                model.QuestionnaireViewModel = _service.GetResponseQuestionsProjectById(id, ProjectId, questid);
                model.Methodology_Deliverablesdata = _service.Methodology_DeliverablesdataProjectById(id, ProjectId, questid);
                // model.QuestionnaireList = _service.QuestionnaireAllListForReport();
                model.QuestionnaireList = _service.QuestionnaireListForReportWithDetails(id, ProjectId);


                model.QuestionnaireViewModel.QuestionnaireId = model.QuestionnaireViewModel.Questions[0].QuestionData.QuestionnaireId;
                if (model.QuestionnaireViewModel.QuestionnaireId == new Guid("ba34d0c7-9116-469f-b334-d369067cec7c"))
                {
                    decimal a2;
                    //  Days = (Variable.A5x1) + (Variable.A8 / 4) + (Variable.A9 / 4) + (Variable.A10 / 4) + (Variable.A11 / 4)
                    // Cost = 1400 + ((? Variable.A2 ==“yes”)*400) +(Variable.Days * Variable.dayrate) + ((int(Variable.A12 / 10)) * Variable.dayrate)
                    var A5 = model.QuestionnaireViewModel.Questions.Where(m => m.QuestionData.VariableName == "A5").FirstOrDefault();
                    var A8 = model.QuestionnaireViewModel.Questions.Where(m => m.QuestionData.VariableName == "A8").FirstOrDefault();
                    var A9 = model.QuestionnaireViewModel.Questions.Where(m => m.QuestionData.VariableName == "A9").FirstOrDefault();
                    var A10 = model.QuestionnaireViewModel.Questions.Where(m => m.QuestionData.VariableName == "A10").FirstOrDefault();
                    var A11 = model.QuestionnaireViewModel.Questions.Where(m => m.QuestionData.VariableName == "A11").FirstOrDefault();
                    var A2 = model.QuestionnaireViewModel.Questions.Where(m => m.QuestionData.VariableName == "A2").FirstOrDefault();
                    var A12 = model.QuestionnaireViewModel.Questions.Where(m => m.QuestionData.VariableName == "A12").FirstOrDefault();
                    var A14 = model.QuestionnaireViewModel.Questions.Where(m => m.QuestionData.VariableName == "A14").FirstOrDefault();
                    if (A2.ResponseOptionId == new Guid("D13CA5E1-439F-4247-9DD1-362B64A8D77E"))
                    {
                        a2 = 400;
                    }
                    else
                    {
                        a2 = 0;
                    }

                    model.QuestionnaireViewModel.DaysSum = (Convert.ToDecimal(A5.Responsetext) * 1) + ((Convert.ToDecimal(A8.Responsetext)) / 4) + ((Convert.ToDecimal(A9.Responsetext)) / 4) + ((Convert.ToDecimal(A10.Responsetext)) / 4) + ((Convert.ToDecimal(A11.Responsetext)) / 4);
                    model.QuestionnaireViewModel.CastSum = 1400 + a2 + (model.QuestionnaireViewModel.DaysSum * 900) + ((Convert.ToDecimal(A12.Responsetext) / 10) * model.QuestionnaireViewModel.DaysSum);
                    model.QuestionnaireViewModel.RequirementSum = "“representative sample of devices in use” " + A14.Responsetext + "";

                }
                else
                {
                    model.QuestionnaireViewModel.DaysSum = model.QuestionnaireViewModel.Questions.Sum(x => x.Days) + model.QuestionnaireViewModel.Questions.Sum(x => x.ChildQuestionsList.Sum(y => y.Days));
                }

                foreach (var item in model.QuestionnaireList)
                {
                    CalModel.QuantityDays = item.Questions.Sum(x => x.Days) + item.Questions.Sum(x => x.ChildQuestionsList.Sum(y => y.Days));
                    CalModel.Name = item.Name;
                    CalModel.QuestionnairDetailsViewdata = item.QuestionnairDetailsViewdata;
                    CalModel.ProjectsViewdata = item.ProjectsViewdata;
                    CalModel.QuestionnaireId = item.QuestionnaireId;
                    CalModel.TotalPrice = ((item.Questions.Sum(x => x.Days) + item.Questions.Sum(x => x.ChildQuestionsList.Sum(y => y.Days))) * 900);

                    //if (item.QuestionnaireId == new Guid("ba34d0c7-9116-469f-b334-d369067cec7c"))
                    //{                    
                    //     var A5 = model.QuestionnaireViewModel.Questions.Where(m => m.QuestionData.VariableName == "A5").FirstOrDefault();
                    //    var A8 = model.QuestionnaireViewModel.Questions.Where(m => m.QuestionData.VariableName == "A8").FirstOrDefault();
                    //    var A9 = model.QuestionnaireViewModel.Questions.Where(m => m.QuestionData.VariableName == "A9").FirstOrDefault();
                    //    var A10 = model.QuestionnaireViewModel.Questions.Where(m => m.QuestionData.VariableName == "A10").FirstOrDefault();
                    //    var A11 = model.QuestionnaireViewModel.Questions.Where(m => m.QuestionData.VariableName == "A11").FirstOrDefault();
                    //    var A2 = model.QuestionnaireViewModel.Questions.Where(m => m.QuestionData.VariableName == "A2").FirstOrDefault();
                    //    var A12 = model.QuestionnaireViewModel.Questions.Where(m => m.QuestionData.VariableName == "A12").FirstOrDefault();
                    //    var A14 = model.QuestionnaireViewModel.Questions.Where(m => m.QuestionData.VariableName == "A14").FirstOrDefault();                      
                    //    CalModel.TotalDaysSum = (Convert.ToDecimal(A5.Responsetext) * 1) + ((Convert.ToDecimal(A8.Responsetext)) / 4) + ((Convert.ToDecimal(A9.Responsetext)) / 4) + ((Convert.ToDecimal(A10.Responsetext)) / 4) + ((Convert.ToDecimal(A11.Responsetext)) / 4);

                    //}


                    QuestionnaireList.Add(CalModel);
                    CalModel = new QuestionnaireReportDataModel();
                }
                model.QuestionnaireList = QuestionnaireList;
                model.TotalExcludingVAT = model.QuestionnaireList.Sum(m => m.TotalPrice);
                model.TotalExcludingDay = model.QuestionnaireList.Sum(m => m.QuantityDays);



            }



            return View(model);
        }

        public IActionResult CustomerReport(Guid id, Guid ProjectId, Guid CustomerProposalId)
        {
            ProposalReportViewModel model = new ProposalReportViewModel();
            model.CustomerId = id;
            try
            {
                model.CustomerProposalViewModel = _service.CustomerProposalAllData(CustomerProposalId);
                model.CustomerViewModel = _service.CustomerAllData(id);
                model.ProjectsAllData = ProjectAllData(id, ProjectId);
                model.SalesViewModel = _service.SalesAllData(id);
                model.Projectsdata = _service.projectsdata(id, ProjectId);
                model.Methodology_DeliverablesdataList = _service.Methodology_DeliverablesdataProjectById1(id, ProjectId);
                model.QuestionnaireMethodology_DeliverablesList = _service.AdminMethodology_List_BYQuestionnaire(id, ProjectId);
                model.QuestionnaireList = QuestionnaireListForReport(id, ProjectId);
                model.TotalExcludingVAT = model.QuestionnaireList.Sum(m => m.TotalPrice);
                model.TotalExcludingDay = model.QuestionnaireList.Sum(m => m.QuantityDays);

                model.TotalDaysSum = model.ProjectsAllData.QDataList.Sum(m => m.DaysSum);
                model.TotalCastSum = model.ProjectsAllData.QDataList.Sum(m => m.CastSum);
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }



        private AllProjectsViewModel ProjectAllData(Guid id, Guid ProjectId)
        {
            AllProjectsViewModel model = new AllProjectsViewModel();
            ProjectCustomerQuestionnairModel qmodel = new ProjectCustomerQuestionnairModel();
            List<ProjectCustomerQuestionnairModel> qDataList = new List<ProjectCustomerQuestionnairModel>();

            model = _service.AllProjectsData(id, ProjectId);
            foreach (var item in model.QDataList)
            {
                qmodel.CustomerQuestionnairId = item.CustomerQuestionnairId;
                qmodel.QuestionnaireId = item.QuestionnaireId;
                qmodel.CustomerId = item.CustomerId;
                qmodel.ProjectId = item.ProjectId;
                qmodel.Questions = item.Questions;
                qmodel.QuestionsData = item.QuestionsData;
                qmodel.Name = item.Name;
                qmodel.DaysTesting = item.QuestionsData.Sum(x => x.Days) + item.QuestionsData.Sum(x => x.ChildQuestionsList.Sum(y => y.Days));
                if (item.QuestionnaireId == new Guid("ba34d0c7-9116-469f-b334-d369067cec7c"))
                {
                    decimal a2;
                    var A5 = item.QuestionsData.Where(m => m.VariableName == "A5").FirstOrDefault();
                    var A8 = item.QuestionsData.Where(m => m.VariableName == "A8").FirstOrDefault();
                    var A9 = item.QuestionsData.Where(m => m.VariableName == "A9").FirstOrDefault();
                    var A10 = item.QuestionsData.Where(m => m.VariableName == "A10").FirstOrDefault();
                    var A11 = item.QuestionsData.Where(m => m.VariableName == "A11").FirstOrDefault();
                    var A2 = item.QuestionsData.Where(m => m.VariableName == "A2").FirstOrDefault();
                    var A12 = item.QuestionsData.Where(m => m.VariableName == "A12").FirstOrDefault();
                    var A14 = item.QuestionsData.Where(m => m.VariableName == "A14").FirstOrDefault();
                    if (A2.ResponseOptionId == new Guid("D13CA5E1-439F-4247-9DD1-362B64A8D77E"))
                    {
                        a2 = 400;
                    }
                    else
                    {
                        a2 = 0;
                    }

                    qmodel.DaysSum = (Convert.ToDecimal(A5.Days) * 1) + ((Convert.ToDecimal(A8.Days)) / 4) + ((Convert.ToDecimal(A9.Days)) / 4) + ((Convert.ToDecimal(A10.Days)) / 4) + ((Convert.ToDecimal(A11.Days)) / 4);
                    qmodel.CastSum = 1400 + a2 + (qmodel.DaysSum * 900) + ((Convert.ToDecimal(A12.Cost) / 10) * 900);
                    qmodel.RequirementSum = "“representative sample of devices in use” " + A14.Responsetext + "";
                    qmodel.DaysReporting = qmodel.DaysSum;
                }

                qDataList.Add(qmodel);
                qmodel = new ProjectCustomerQuestionnairModel();
            }
            model.QDataList = qDataList;
            return model;
        }


        private List<QuestionnaireReportDataModel> QuestionnaireListForReport(Guid id, Guid ProjectId)
        {
            List<QuestionnaireReportDataModel> QuestionnaireList = new List<QuestionnaireReportDataModel>();
            List<QuestionnaireReportDataModel> QuestionnaireList1 = new List<QuestionnaireReportDataModel>();
            try
            {
                QuestionnaireReportDataModel CalModel = new QuestionnaireReportDataModel();
                QuestionnaireList = _service.QuestionnaireListForReportWithDetails(id, ProjectId);
                foreach (var item in QuestionnaireList)
                {
                    CalModel.QuantityDays = item.Questions.Sum(x => x.Days) + item.Questions.Sum(x => x.ChildQuestionsList.Sum(y => y.Days));
                    CalModel.Name = item.Name;
                    CalModel.ProjectsViewdata = item.ProjectsViewdata;
                    CalModel.QuestionnaireId = item.QuestionnaireId;
                    CalModel.TotalPrice = ((item.Questions.Sum(x => x.Days) + item.Questions.Sum(x => x.ChildQuestionsList.Sum(y => y.Days))) * 900);
                    QuestionnaireList1.Add(CalModel);
                    CalModel = new QuestionnaireReportDataModel();
                }
                QuestionnaireList = QuestionnaireList1;
            }
            catch (Exception)
            {

                throw;
            }

            return QuestionnaireList;
        }


        //PCI ASV assessment questionnaire
        [HttpGet]
        public IActionResult PCIASVquestionnaire(Guid id, Guid ProjectId)
        {
            QuestionnaireViewModel model = new QuestionnaireViewModel();
            try
            {
                model = _service.GetQuestionsByModule(id);
                var projectdata = _service.GetProjectById(ProjectId);
                var Userid = _userManager.GetUserId(User); // Get user id:
                model.ProjectId = ProjectId;
                var admin = _context.Customer.Where(m => m.UserId == Userid).FirstOrDefault();
                if (admin != null)
                {
                    model.CustomerName = "" + admin.FirstName + " " + admin.LastName + "";
                }
                if (projectdata != null)
                {
                    model.ProjectName = projectdata.ProjectName;
                }
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }
        [HttpPost]
        public async Task<JsonResult> SavePCIQuestionnaire(string rVSData = null, string ques2 = null, string ques3 = null, string ques4 = null, string ques5 = null, string ques1 = null, string ProjectName = null, string CustomerName = null, string ProjectId = null, string Methodolgy = null, string Methodolgy_FileName = null, string SampleReport = null, string SampleReport_FileName = null)
        {

            var UserId = _userManager.GetUserId(User);
            var giid = new Guid(UserId);

            if (rVSData != null)
            {
                var data1 = await _service.PCIQuestions(rVSData, ques2, ques3, ques4, ques5, UserId, ques1, ProjectName, CustomerName, ProjectId, Methodolgy, Methodolgy_FileName, SampleReport, SampleReport_FileName);
            }
            TempData["QuestionnaireSuccessMessage"] = "successfully";
            return Json("Success");
        }

        public IActionResult EditQuestionnaires(Guid id)
        {
            CustomerQuestionnairViewModel model = new CustomerQuestionnairViewModel();
            try
            {
                model.CustomerViewList = _service.CustomerViewList();
                model.CustomerId = id;
                if (id == Guid.Empty)
                {
                    model.CustomerQuestionnairList = _service.CustomerQuestionnairList(id);
                }
                else
                {
                    model.CustomerQuestionnairList = _service.CustomerQuestionnairList(id);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return View(model);
        }


        public IActionResult EditAllQuestionnaires(Guid id)
        {
            CustomerQuestionnairViewModel model = new CustomerQuestionnairViewModel();
            model.CustomerViewList = _service.CustomerViewList();
            model.CustomerId = id;
            model.QuestionnaireList = _service.QuestionnaireAllList();
            return View(model);
        }


        [HttpGet]
        public IActionResult UpdateAllQuestionnaires(Guid? id, Guid QuestionnaireId)
        {

            QuestionnaireViewModel model = new QuestionnaireViewModel();
            try
            {
                if (QuestionnaireId == new Guid("a8837cf2-39cc-464e-a120-4248ea2cde0c"))
                {
                    return RedirectToAction("SecurityScopingtoolQuestionnaire", new { QuestionnaireId = QuestionnaireId });
                }
                else if (QuestionnaireId == new Guid("0A2DFC0B-1F17-447C-A7B8-067F9980325A"))
                {
                    return RedirectToAction("UpdatepciQuestions", new { QuestionnaireId = QuestionnaireId });
                }
                else if (QuestionnaireId == new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323"))
                {
                    return RedirectToAction("UpdateAllCEQQuestionnaires", new { Userid = id, id = QuestionnaireId });
                }
                else
                {
                    model = _service.GetQuestionsById(QuestionnaireId);
                    model.QuestionnaireId = QuestionnaireId;
                }

            }
            catch (Exception ex)
            {

            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAllQuestionnaires(QuestionnaireViewModel model)
        {
            var id = _userManager.GetUserId(User);
            model.UserId = id;
            var data = await _service.UpdateAllQuestions(model);
            TempData["UpdateMessage"] = "successfully";
            return RedirectToAction("EditAllQuestionnaires", "Questionnaire");
        }
        [HttpGet]
        public IActionResult UpdateAllCEQQuestionnaires(string Userid, Guid id)
        {

            CyberEssentialsQuestionnaireModel model = new CyberEssentialsQuestionnaireModel();
            model.IndustrySectorList = _service.IndustrySectorListData();
            model.SectionList = _service.CEQSectionList();
            model.QuestionnaireId = id;
            model.Methodology_Deliverablesdata = _service.AdminMethodology_Deliverablesdata(new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323"));
            model.IndustrySectorList = _service.IndustrySectorListData();
            model.CEQQuestionsViewModelList = _service.CEQQuestionsList();
            model.SectionList = _service.CEQSectionList();
            model.MethodologyeList = _service.Methodologyelist();
            model.SampleReportList = _service.SampleReportlist();
            model.QuestionnaireId = id;        
            model.Organisation = "false";
            ViewBag.OrganisationData = 2;        
            model.Signature = "false";         
            model.Remote = "false";
            ViewBag.RvsData = 2;      
            model.Workstation = "false";
            ViewBag.WaAssesData = 2;          
            model.Cloud = "false";         
            model.Security = "false";         
            model.CustomerId = new Guid(Userid);

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> SaveQuestionnaireWithVariable(List<TestData1> optionValue = null, string ScopeTypeMethodolgy = null, string ScopeTypeMethodolgy_FileName = null, string ScopeTypeSampleReport = null, string ScopeTypeSampleReport_FileName = null, string Methodology_DeliverablesId = null)
        {
            List<string> a = new List<string>();

            var UserId = _userManager.GetUserId(User);

            var giid = new Guid(UserId);

            if (optionValue != null)
            {
                var data1 = await _service.saveQuestionnaire1(optionValue);
            }
            if (Methodology_DeliverablesId != null)
            {
                var data = await _service.saveMethodology_Deliverables(ScopeTypeMethodolgy, ScopeTypeMethodolgy_FileName, ScopeTypeSampleReport, ScopeTypeSampleReport_FileName, Methodology_DeliverablesId);
            }

            return Json("Success");
        }

        public IActionResult QuestionnaireDetails(Guid QuestionnaireId)
        {
            QuestionnaireViewModel model = new QuestionnaireViewModel();

            try
            {
                if (QuestionnaireId == new Guid("a8837cf2-39cc-464e-a120-4248ea2cde0c"))
                {
                    return RedirectToAction("SecurityScopingtoolQuestionnaire", new { QuestionnaireId = QuestionnaireId, temp = "Details" });
                }
                else if (QuestionnaireId == new Guid("54515c61-e5c1-43d5-87ca-af8102a06323"))
                {
                    return RedirectToAction("CEQQuestionnaireDetails", "Questionnaire", new { id = QuestionnaireId });
                }
                else if (QuestionnaireId == new Guid("0A2DFC0B-1F17-447C-A7B8-067F9980325A"))
                {
                    return RedirectToAction("pciQuestionsDetails", new { QuestionnaireId = QuestionnaireId });
                }
                else
                {
                    model = _service.GetQuestionsById(QuestionnaireId);
                    model.QuestionnaireId = QuestionnaireId;
                }

            }
            catch (Exception ex)
            {

            }

            return View(model);


        }
        //CEQQuestionnaireDetails
        [HttpGet]
        public IActionResult CEQQuestionnaireDetails(Guid id, string msg = null)
        {

            var Userid = _userManager.GetUserId(User);

            CyberEssentialsQuestionnaireModel model = new CyberEssentialsQuestionnaireModel();
            model.IndustrySectorList = _service.IndustrySectorListData();
            model.CEQQuestionsViewModelList = _service.CEQQuestionsList();
            model.SectionList = _service.CEQSectionList();
            model.Methodology_Deliverablesdata = _service.AdminMethodology_Deliverablesdata(new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323"));
            model.QuestionnaireId = id;
            model.Organisation = "false";
            ViewBag.OrganisationData = 2;
            model.Signature = "false";
            model.Remote = "false";
            ViewBag.RvsData = 2;
            model.Workstation = "false";
            ViewBag.WaAssesData = 2;

            model.Cloud = "false";
            model.Security = "false";

            if (msg != null)
            {
                TempData["Error"] = "";
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult UpdateQuestionnaires(Guid id, Guid QuestionnaireId, Guid ProjectId)
        {

            QuestionnaireViewModel model = new QuestionnaireViewModel();
            try
            {
                if (QuestionnaireId == new Guid("a8837cf2-39cc-464e-a120-4248ea2cde0c"))
                {
                    return RedirectToAction("SectionEdit", new { id = id, QuestionnaireId = QuestionnaireId, ProjectId = ProjectId });
                }
                else if (QuestionnaireId == new Guid("0A2DFC0B-1F17-447C-A7B8-067F9980325A"))
                {
                    return RedirectToAction("UpdatepciQuestionnaires", new { id = id, QuestionnaireId = QuestionnaireId, ProjectId = ProjectId });
                }
                else if (QuestionnaireId == new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323"))
                {
                    return RedirectToAction("UpdateCEQQuestionnaires", new { Userid = id, id = QuestionnaireId, ProjectId = ProjectId });
                }
                else
                {
                    model = _service.GetResponseQuestionsById1(id, QuestionnaireId, ProjectId);
                    model.QuestionnaireId = model.Questions[0].QuestionData.QuestionnaireId;
                    model.ProjectId = ProjectId;
                    model.CustomerId = id;
                }

            }
            catch (Exception ex)
            {

            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateQuestionnaires(QuestionnaireViewModel model)
        {
            try
            {
                var id = _userManager.GetUserId(User);
                model.UserId = id;
                var UserName = _context.Users.Where(m => m.Id == id).FirstOrDefault();
                var data = await _service.UpdateResponseModuleWiseQuestions(model);

                //var alldata = _context.Customer.Where(m => m.UserId == id).FirstOrDefault();
                //var Userdata = _context.Users.Where(m => m.Id == alldata.CreatedBy.ToString()).FirstOrDefault();
                //var varifyUrl = Request.Scheme + "://scoping.procheckup.com/Questionnaire/QuestionnaireResponseDetails?id=" + data.CustomerId + "&Projectid=" + data.ProjectId + "&QuestionnaireId=" + data.QuestionnaireId + "&Page=HomeIndex";

                //var d = SendMailQuestionner(alldata.Email, Userdata.Email, varifyUrl, UserName.UserName, Userdata.UserName);

                TempData["CustomerQuestionnaireUpdateMessage"] = "Questionnaire Update succesfully ! ";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["CustomerQuestionnaireUpdateErrorMessage"] = "Error in update questionnaire ! ";
                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult SectionEdit(Guid id, Guid QuestionnaireId, Guid ProjectId)
        {

            QuestionnaireViewModel model = new QuestionnaireViewModel();
            try
            {

                model = _service.GetResponseQuestionsById(id, QuestionnaireId, ProjectId);
                model.SectionId = model.Questions[0].QuestionData.SectionId;
                model.ProjectId = ProjectId;
            }
            catch (Exception ex)
            {

            }



            var questionnaire = new QuestionnaireViewModel()
            {
                SectionId = model.SectionId,
                QuestionnaireId = QuestionnaireId,
                CustomerId = id,
                ProjectId = ProjectId,
                QuestionnaireData = (from q in _context.Questionnaire
                                     where q.QuestionnaireId == QuestionnaireId
                                     select new QuestionnaireViewModel
                                     {
                                         QuestionnaireId = q.QuestionnaireId,
                                         Name = q.Name
                                     }).FirstOrDefault(),
                Questions = new List<QuestionsViewModel>()
                {
                    new QuestionsViewModel()
                    {
                        QuestionId = Guid.NewGuid(),
                        SectionId = 1,
                        QuestionNumber = "1",
                        QuestionType = QuestionType.multipleResponseType,
                        QuestionText = "Reason for security services " ,
                        QuestionnaireId=QuestionnaireId,
                        OptionsList = new List<OptionsViewModel>()
                        {
                            new OptionsViewModel()
                            {
                                OptionId1 = 1,
                                OptionText = "verification test (to verify issues found in previous pentest have been fixed)",
                            },
                             new OptionsViewModel()
                            {
                                OptionId1 = 2,
                                OptionText = "retest",
                            },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 3,
                                OptionText = "we suspect we’ve been hacked",
                             },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 4,
                                OptionText = "ITHC",
                             },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 5,
                                OptionText = "need a specific component tested only",
                             },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 6,
                                OptionText = "to better understand security posture of our environment",
                             },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 7,
                                OptionText = "required by third party",
                             },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 8,
                                OptionText = "PCI",
                             },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 9,
                                OptionText = "compliance/accreditation",
                             }


                        }
                    }
                }
            };
            return View(questionnaire);
        }


        [HttpPost]
        public IActionResult SectionEdit(QuestionnaireViewModel model)
        {
            return RedirectToAction("ChildQuestionsUpdate", new { id = model.CustomerId, SectionId = model.Questions[0].ResponseOptionId1, QuestionnaireId = model.QuestionnaireId, Sectionidold = model.SectionId, ProjectId = model.ProjectId });
        }

        [HttpGet]
        public IActionResult ChildQuestionsUpdate(Guid id, string SectionId, Guid QuestionnaireId, string Sectionidold, Guid ProjectId)
        {
            QuestionnaireViewModel model = new QuestionnaireViewModel();
            try
            {
                if (SectionId != null)
                {
                    model = _service.GetResponseQuestionsById(id, QuestionnaireId, ProjectId);
                    model.SectionId = model.Questions[0].QuestionData.SectionId;
                    model.QuestionnaireId = QuestionnaireId;
                    model.CustomerId = id;
                    model.ProjectId = ProjectId;
                }
                else
                {
                    // return RedirectToAction("ChildQuestions", new { SectionId = SectionId, QuestionnaireId = QuestionnaireId,id=id });
                }

            }
            catch (Exception ex)
            {

            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ChildQuestionsUpdate(QuestionnaireViewModel model)
        {
            var id = _userManager.GetUserId(User);
            var UserName = _context.Users.Where(m => m.Id == id).FirstOrDefault();
            var data = await _service.ChildQuestionUpdate(model);


            //var alldata = _context.Customer.Where(m => m.UserId == id).FirstOrDefault();
            //var Userdata = _context.Users.Where(m => m.Id == alldata.CreatedBy.ToString()).FirstOrDefault();
            //var varifyUrl = Request.Scheme + "://scoping.procheckup.com/Questionnaire/QuestionnaireResponseDetails?id=" + model.CustomerId + "&Projectid=" + model.ProjectId + "&QuestionnaireId=" + model.QuestionnaireId + "&Page=HomeIndex";

            //var d = SendMailQuestionner(alldata.Email, Userdata.Email, varifyUrl, UserName.UserName, Userdata.UserName);

            TempData["CustomerQuestionnaireUpdateMessage"] = "Questionnaire Update succesfully ! ";
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult UpdatepciQuestionnaires(Guid id, Guid QuestionnaireId, Guid ProjectId)
        {
            QuestionnaireViewModel model = new QuestionnaireViewModel();
            model = _service.GetResponseQuestionsById3(id, QuestionnaireId, ProjectId);
            model.QuestionnaireId = QuestionnaireId;
            model.CustomerId = id;
            return View(model);
        }

        [HttpPost]
        public IActionResult DeletePCIQuestionnaire(string id = null)
        {
            if (id == null)
            {
                return NotFound();
            }

            var UpdatepciQuestionnaires = _service.DeletePCITableQuest(new Guid(id));
            if (UpdatepciQuestionnaires == null)
            {
                return NotFound();
            }

            // return View(new CustomerViewModel(customer));
            return View(UpdatepciQuestionnaires);
        }

        [HttpPost]

        public async Task<JsonResult> UpdatePCIQuestionnaire(string rVSData = null, string ques2 = null, string ques3 = null, string ques4 = null, string ques5 = null, string ques1 = null, string res1 = null, string res2 = null, string res3 = null, string res4 = null, string res5 = null, string CustomerId = null, string ProjectId = null, string Methodolgy = null, string Methodolgy_FileName = null, string SampleReport = null, string SampleReport_FileName = null)
        {
            var UserId = _userManager.GetUserId(User);
            var giid = new Guid(UserId);

            string[] Data = { ques2, ques3, ques4, ques5, ques1, res1, res2, res3, res4, res5 };
            if (rVSData != null)
            {
                var data1 = await _service.UpdateResponseModuleWiseQuestionsPCI(rVSData, ques2, ques3, ques4, ques5, UserId, ques1, res1, res2, res3, res4, res5, CustomerId, ProjectId, Methodolgy, Methodolgy_FileName, SampleReport, SampleReport_FileName);
            }
            TempData["UpdateMessage"] = "successfully";
            return Json("Success");
        }

        //ceq update 
        [HttpGet]
        public IActionResult UpdateCEQQuestionnaires(string Userid, Guid id, Guid ProjectId)
        {

            CyberEssentialsQuestionnaireModel model = new CyberEssentialsQuestionnaireModel();

            model.IndustrySectorList = _service.IndustrySectorListData();
            model.CEQQuestionsViewModelList = _service.CEQQuestionsList2(id, Userid, ProjectId);
            model.SectionList = _service.CEQSectionList();
            model.QuestionnaireId = id;
            model.OrganisationData = _service.organisationData(id, Userid, ProjectId);
            if (model.OrganisationData != null)
            {
                model.Organisation = "true";
                ViewBag.OrganisationData = 1;
            }
            else
            {
                model.Organisation = "false";
                ViewBag.OrganisationData = 2;
            }

            model.CEQSignatureData = _service.cEQSignatureData(id, Userid, ProjectId);
            if (model.CEQSignatureData != null)
            {
                model.Signature = "true";
            }
            else
            {
                model.Signature = "false";
            }

            model.RvsData = _service.RvsData(id, Userid, ProjectId);
            if (model.RvsData.Count > 0)
            {
                model.Remote = "true";
                ViewBag.RvsData = 1;
            }
            else
            {
                model.Remote = "false";
                ViewBag.RvsData = 2;
            }
            model.WaAssesData = _service.WaAssesData(id, Userid, ProjectId);
            if (model.WaAssesData.Count > 0)
            {
                model.Workstation = "true";
                ViewBag.WaAssesData = 1;
            }
            else
            {
                model.Workstation = "false";
                ViewBag.WaAssesData = 2;
            }
            model.CEQCloudSharedServicesAssesList = _service.cEQCloudSharedServicesAssesList(id, Userid, ProjectId);
            if (model.CEQCloudSharedServicesAssesList.Count > 0)
            {
                model.Cloud = "true";
            }
            else
            {
                model.Cloud = "false";
            }

            model.CEQQuestionResponseViewModelList = _service.cEQQuestionResponseViewModelList(id, Userid, ProjectId);
            if (model.CEQQuestionResponseViewModelList.Count > 0)
            {
                model.CEQQuestionResponseResultList = _service.cEQQuestionResponseResult(model);
                model.Security = "true";
            }
            else
            {
                model.Security = "false";
            }

            //if (msg != null)
            //{
            //    TempData["Error"] = "";
            //}
            model.CustomerId = new Guid(Userid);
            model.ProjectId = ProjectId;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CyberEssentialsQuestionnaireUpdate(CyberEssentialsQuestionnaireModel model)
        {

            if (model.Remote == "true" && model.Organisation == "true" && model.Security == "true" && model.Signature == "true" && model.Workstation == "true" && model.Cloud == "true")
            {
                var UserId = _userManager.GetUserId(User);
                var UserName = _context.Users.Where(m => m.Id == UserId).FirstOrDefault();
                var fgfg = await _service.CyberEssentialsQuestionnaireUpdate(model.QuestionnaireId, model.CustomerId);
                //var alldata = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();
                //var Userdata = _context.Users.Where(m => m.Id == alldata.CreatedBy.ToString()).FirstOrDefault();
                //var varifyUrl = Request.Scheme + "://scoping.procheckup.com/Questionnaire/QuestionnaireResponseDetails?id=" + model.CustomerId + "&Projectid=" + model.ProjectId + "&QuestionnaireId=" + model.QuestionnaireId + "&Page=HomeIndex";

                //var d = SendMailQuestionner(alldata.Email, Userdata.Email, varifyUrl, UserName.UserName, Userdata.UserName);

                TempData["CustomerQuestionnaireUpdateMessage"] = "Questionnaire Update succesfully ! ";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("UpdateCEQQuestionnaires", new { id = model.QuestionnaireId, msg = "error" });
            }


        }

        //

        [HttpGet]
        public IActionResult C_E_Questionaires(Guid id, Guid ProjectId, string msg = null)
        {

            var Userid = _userManager.GetUserId(User);

            CyberEssentialsQuestionnaireModel model = new CyberEssentialsQuestionnaireModel();
            model.IndustrySectorList = _service.IndustrySectorListData();
            model.CEQQuestionsViewModelList = _service.CEQQuestionsList2(id, Userid, ProjectId);
            // model.CEQQuestionsViewModelList = _service.CEQQuestionsList();
            model.SectionList = _service.CEQSectionList();
            model.QuestionnaireId = id;
            model.ProjectId = ProjectId;
            model.OrganisationData = _service.organisationData(id, Userid, ProjectId);
            if (model.OrganisationData != null)
            {
                model.Organisation = "true";
                ViewBag.OrganisationData = 1;
            }
            else
            {
                model.Organisation = "false";
                ViewBag.OrganisationData = 2;
            }

            model.CEQSignatureData = _service.cEQSignatureData(id, Userid, ProjectId);
            if (model.CEQSignatureData != null)
            {
                model.Signature = "true";
            }
            else
            {
                model.Signature = "false";
            }

            model.RvsData = _service.RvsData(id, Userid, ProjectId);
            if (model.RvsData.Count > 0)
            {
                model.Remote = "true";
                ViewBag.RvsData = 1;
            }
            else
            {
                model.Remote = "false";
                ViewBag.RvsData = 2;
            }
            model.WaAssesData = _service.WaAssesData(id, Userid, ProjectId);
            if (model.WaAssesData.Count > 0)
            {
                model.Workstation = "true";
                ViewBag.WaAssesData = 1;
            }
            else
            {
                model.Workstation = "false";
                ViewBag.WaAssesData = 2;
            }
            model.CEQCloudSharedServicesAssesList = _service.cEQCloudSharedServicesAssesList(id, Userid, ProjectId);
            if (model.CEQCloudSharedServicesAssesList.Count > 0)
            {
                model.Cloud = "true";
            }
            else
            {
                model.Cloud = "false";
            }

            model.CEQQuestionResponseViewModelList = _service.cEQQuestionResponseViewModelList(id, Userid, ProjectId);
            if (model.CEQQuestionResponseViewModelList.Count > 0)
            {
                model.CEQQuestionResponseResultList = _service.cEQQuestionResponseResult(model);

                model.Security = "true";
            }
            else
            {
                model.Security = "false";
            }

            if (msg != null)
            {
                TempData["Error"] = "";
            }
            //   model.CustomerId = id;
            return View(model);
        }


        [HttpPost]
        public string SaveOrganisationDetails(string OrganisationName = null, string RegisteredAddress = null, string Company_CharityNumber = null, string Turnover = null, string MainContactName = null, string ContactJobTitle = null, string ContactEmail = null, string ContactTelephone = null, string CELevelDesired = null, string IndustrySector = null, string DateOfResponse = null, string NumberOfEmployees = null, string certificationSelect = null, string orgcustId = null, string ProjectId = null)
        {
            try
            {
                var Userid = _userManager.GetUserId(User);
                CEQOrgDetailsViewModel model = new CEQOrgDetailsViewModel();
                model.Nameoforganisation = OrganisationName;
                model.RegisteredAddress = RegisteredAddress;
                model.CompanyCharityNumber = Company_CharityNumber;
                model.Turnover = Turnover;
                model.Nameofmaincontact = MainContactName;
                model.ContactJobtitle = ContactJobTitle;
                model.ContactEmail = ContactEmail;
                model.ContactTelephone = ContactTelephone;
                model.CELevelDesired = CELevelDesired;
                model.Sector = IndustrySector;
                model.Dateofresponse = DateOfResponse;
                model.NumberofEmployees = NumberOfEmployees;
                model.CanCRESTpubliciseyoursuccessfulcertification = certificationSelect;
                model.UserId = Userid;
                if (ProjectId != null)
                {
                    model.ProjectId = new Guid(ProjectId);
                }
                if (orgcustId != null)
                {
                    model.CustomerId = new Guid(orgcustId);
                }
                var msg = _service.saveOrganisationDetails(model);

                return "Success";
            }
            catch (Exception e)
            {
                return "failure";
            }
        }


        public string SaveSignatureDetails(string SignatureFile = null, string PrintName = null, string JobTitle = null, string Organisation = null, string DateOfSignature = null, string BusinessUnit = null, string SignatureOrganisationName = null, string SignatureHtml = null, string ProjectId = null, string Title = null, string orgcustId = null)
        {
            try
            {
                var imgdata = SignatureFile.Split(',');
                if (imgdata.Length == 2)
                {
                    SignatureFile = imgdata[1];
                }
                var Userid = _userManager.GetUserId(User);
                CEQSignatureViewModel model = new CEQSignatureViewModel();
                model.Nameoforganisation = Organisation;
                model.FileName = SignatureFile;
                model.Name = PrintName;
                model.JobTitle = JobTitle;
                model.SignatureDate = DateOfSignature;
                model.Nameofbusiness = BusinessUnit;
                model.UserId = Userid;
                if (orgcustId == null)
                {
                    model.CustomerId = Guid.Empty;
                }
                else
                {
                    model.CustomerId = new Guid(orgcustId);
                }
                if (ProjectId != null)
                {
                    model.ProjectId = new Guid(ProjectId);
                }
                else
                {
                    model.ProjectId = Guid.Empty;
                }

                var msg = _service.saveSignatureDetails(model);



                return "Success";
            }
            catch (Exception e)
            {
                return "failure";
            }
        }

        public JsonResult UploadSigImage(string imageData, string filename)
        {
            var fname = string.Empty;
            var bytes = Convert.FromBase64String(imageData);// a.base64image 
            var id = _userManager.GetUserId(User);
            var fileString = filename.Split('.');
            fileString[0] = fileString[0];
            string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
            fname = tempGuid + "_" + fileString[0] + "." + fileString[1];
            string filedir = Path.Combine(Directory.GetCurrentDirectory(), "/lib/Content/QuetionnaireSigImages");
            if (!Directory.Exists(filedir))
            { //check if the folder exists;
                Directory.CreateDirectory(filedir);
            }

            string file = Path.Combine(filedir, fname);
            if (bytes.Length > 0)
            {
                using (var stream = new FileStream(file, FileMode.Create))
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                }

            }

            return Json(fname);
        }
        [HttpPost]
        public async Task<JsonResult> SaveWATQues(string WATabledata = null, string ProjectId = null, string orgcustId = null)
        {

            var UserId = _userManager.GetUserId(User);
            var giid = new Guid(UserId);

            if (WATabledata != null)
            {
                var data1 = await _service.SaveWAT(WATabledata, UserId, ProjectId, orgcustId);
            }

            return Json("Success");
        }

        [HttpPost]
        public async Task<JsonResult> SaveCloudQues(string CloudTabledata = null, string ProjectId = null, string orgcustId = null)
        {

            var UserId = _userManager.GetUserId(User);
            var giid = new Guid(UserId);

            if (CloudTabledata != null)
            {
                var data1 = await _service.SaveCloud(CloudTabledata, UserId, ProjectId, orgcustId);
            }

            return Json("Success");
        }

        [HttpPost]
        public async Task<JsonResult> SaveRVS(string rVSData = null, string[] options = null, string ProjectId = null, string orgcustId = null)
        {
            List<string> a = new List<string>();
            options = options.Where(c => c != null).ToArray();


            var UserId = _userManager.GetUserId(User);
            var giid = new Guid(UserId);

            if (rVSData != null)
            {
                var data1 = await _service.SaveRVSQuestions(rVSData, UserId, options, ProjectId, orgcustId);
            }

            return Json("Success");
        }

        [HttpPost]
        public async Task<IActionResult> CyberEssentialsQuestionnaire(CyberEssentialsQuestionnaireModel model)
        {

            if (model.Remote == "true" && model.Organisation == "true" && model.Security == "true" && model.Signature == "true" && model.Workstation == "true" && model.Cloud == "true")
            {
                var UserId = _userManager.GetUserId(User);
                var id = UserId;
                var UserName = _context.Users.Where(m => m.Id == id).FirstOrDefault();
                var fgfg = await _service.ECQFinalSubmit(model.QuestionnaireId, model.ProjectId, UserId);

                //var alldata = _context.Customer.Where(m => m.UserId == id).FirstOrDefault();
                //var Userdata = _context.Users.Where(m => m.Id == alldata.CreatedBy.ToString()).FirstOrDefault();
                //var varifyUrl = Request.Scheme + "://scoping.procheckup.com/Questionnaire/QuestionnaireResponseDetails?id=" + alldata.CustomerId + "&Projectid=" + model.ProjectId + "&QuestionnaireId=" + model.QuestionnaireId + "&Page=HomeIndex";

                //var d = SendMailQuestionner(alldata.Email, Userdata.Email, varifyUrl, UserName.UserName, Userdata.UserName);

                TempData["QuestionnaireSuccessMessage"] = "successfully";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("C_E_Questionaires", new { id = model.QuestionnaireId, msg = "error" });
            }


        }

        [HttpPost]
        public async Task<JsonResult> SaveQuestionnaire(List<TestData> optionValue = null, string ProjectId = null, string orgcustId = null)
        {
            List<string> a = new List<string>();

            var UserId = _userManager.GetUserId(User);

            var giid = new Guid(UserId);

            if (optionValue != null)
            {
                var data1 = await _service.saveQuestionnaire(optionValue, UserId, ProjectId, orgcustId);
            }

            return Json("Success");
        }

        public IActionResult CEQResponseDetails(string Userid, Guid id, Guid ProjectId, string Page = null)
        {

            CyberEssentialsQuestionnaireModel model = new CyberEssentialsQuestionnaireModel();
            try
            {


                //model.IndustrySectorList = _service.IndustrySectorListData();
                //model.CEQQuestionsViewModelList = _service.CEQQuestionsList2(id, Userid);
                //// model.CEQQuestionsViewModelList = _service.CEQQuestionsList();
                //model.SectionList = _service.CEQSectionList();
                //model.QuestionnaireId = id;
                //model.OrganisationData = _service.organisationData(id, Userid, ProjectId);

                model.IndustrySectorList = _service.IndustrySectorListData();
                model.CEQQuestionsViewModelList = _service.CEQQuestionsList2(id, Userid, ProjectId);
                // model.CEQQuestionsViewModelList = _service.CEQQuestionsList();
                model.SectionList = _service.CEQSectionList();
                model.QuestionnaireId = id;
                model.OrganisationData = _service.organisationData(id, Userid, ProjectId);
                if (model.OrganisationData != null)
                {
                    model.Organisation = "true";
                    ViewBag.OrganisationData = 1;
                }
                else
                {
                    model.Organisation = "false";
                    ViewBag.OrganisationData = 2;
                }

                model.CEQSignatureData = _service.cEQSignatureData(id, Userid, ProjectId);
                if (model.CEQSignatureData != null)
                {
                    model.Signature = "true";
                }
                else
                {
                    model.Signature = "false";
                }

                model.RvsData = _service.RvsData(id, Userid, ProjectId);
                if (model.RvsData.Count > 0)
                {
                    model.Remote = "true";
                    ViewBag.RvsData = 1;
                }
                else
                {
                    model.Remote = "false";
                    ViewBag.RvsData = 2;
                }
                model.WaAssesData = _service.WaAssesData(id, Userid, ProjectId);
                if (model.WaAssesData.Count > 0)
                {
                    model.Workstation = "true";
                    ViewBag.WaAssesData = 1;
                }
                else
                {
                    model.Workstation = "false";
                    ViewBag.WaAssesData = 2;
                }
                model.CEQCloudSharedServicesAssesList = _service.cEQCloudSharedServicesAssesList(id, Userid, ProjectId);
                if (model.CEQCloudSharedServicesAssesList.Count > 0)
                {

                    model.Cloud = "true";
                }
                else
                {
                    model.Cloud = "false";
                }

                model.CEQQuestionResponseViewModelList = _service.cEQQuestionResponseViewModelList(id, Userid, ProjectId);
                if (model.CEQQuestionResponseViewModelList.Count > 0)
                {
                    model.CEQQuestionResponseResultList = _service.cEQQuestionResponseResult(model);

                    model.Security = "true";
                }
                else
                {
                    model.Security = "false";
                }

                //if (msg != null)
                //{
                //    TempData["Error"] = "";
                //}


                return View(model);
            }
            catch (Exception ex)
            {

            }
            model.Pages = Page;
            return View(model);
        }


        public IActionResult CEQCustomerReport(Guid id, Guid CustomerProposalId, Guid ProjectId, string Userid)
        {
            CyberEssentialsQuestionnaireModel model = new CyberEssentialsQuestionnaireModel();

            model.CustomerProposalViewModel = _service.CustomerProposalAllData(CustomerProposalId);
            Guid questid = model.CustomerProposalViewModel.QuestionnaireId;
            model.CustomerViewModel = _service.CustomerAllData(new Guid(Userid));
            model.SalesViewModel = _service.SalesAllData(new Guid(Userid));

            //CyberEssentialsQuestionnaireModel model = new CyberEssentialsQuestionnaireModel();
            try
            {

                //model.IndustrySectorList = _service.IndustrySectorListData();
                //model.CEQQuestionsViewModelList = _service.CEQQuestionsList2(id, Userid);
                //// model.CEQQuestionsViewModelList = _service.CEQQuestionsList();
                //model.SectionList = _service.CEQSectionList();
                //model.QuestionnaireId = id;
                //model.OrganisationData = _service.organisationData(id, Userid, ProjectId);

                model.IndustrySectorList = _service.IndustrySectorListData();
                model.CEQQuestionsViewModelList = _service.CEQQuestionsList2(id, Userid, ProjectId);
                // model.CEQQuestionsViewModelList = _service.CEQQuestionsList();
                model.SectionList = _service.CEQSectionList();
                model.QuestionnaireId = id;
                model.OrganisationData = _service.organisationData(id, Userid, ProjectId);
                if (model.OrganisationData != null)
                {
                    model.Organisation = "true";
                    ViewBag.OrganisationData = 1;
                }
                else
                {
                    model.Organisation = "false";
                    ViewBag.OrganisationData = 2;
                }

                model.CEQSignatureData = _service.cEQSignatureData(id, Userid, ProjectId);
                if (model.CEQSignatureData != null)
                {
                    model.Signature = "true";
                }
                else
                {
                    model.Signature = "false";
                }

                model.RvsData = _service.RvsData(id, Userid, ProjectId);
                if (model.RvsData.Count > 0)
                {
                    model.Remote = "true";
                    ViewBag.RvsData = 1;
                }
                else
                {
                    model.Remote = "false";
                    ViewBag.RvsData = 2;
                }
                model.WaAssesData = _service.WaAssesData(id, Userid, ProjectId);
                if (model.WaAssesData.Count > 0)
                {
                    model.Workstation = "true";
                    ViewBag.WaAssesData = 1;
                }
                else
                {
                    model.Workstation = "false";
                    ViewBag.WaAssesData = 2;
                }
                model.CEQCloudSharedServicesAssesList = _service.cEQCloudSharedServicesAssesList(id, Userid, ProjectId);
                if (model.CEQCloudSharedServicesAssesList.Count > 0)
                {

                    model.Cloud = "true";
                }
                else
                {
                    model.Cloud = "false";
                }

                model.CEQQuestionResponseViewModelList = _service.cEQQuestionResponseViewModelList(id, Userid, ProjectId);
                if (model.CEQQuestionResponseViewModelList.Count > 0)
                {
                    model.CEQQuestionResponseResultList = _service.cEQQuestionResponseResult(model);

                    model.Security = "true";
                }
                else
                {
                    model.Security = "false";
                }

                return View(model);
            }
            catch (Exception ex)
            {

            }


            return View(model);
        }

        [HttpGet]
        public IActionResult UpdatepciQuestions(Guid QuestionnaireId)
        {
            QuestionnaireViewModel model = new QuestionnaireViewModel();
            model = _service.GetQuestionsById(QuestionnaireId);
            model.QuestionnaireId = QuestionnaireId;

            return View(model);
        }

        [HttpGet]
        public IActionResult pciQuestionsDetails(Guid QuestionnaireId)
        {
            QuestionnaireViewModel model = new QuestionnaireViewModel();
            model = _service.GetQuestionsById(QuestionnaireId);
            model.QuestionnaireId = QuestionnaireId;

            return View(model);
        }


        public IActionResult SecurityScopingtoolQuestionnaire(Guid Questionnaireid, string temp)
        {
            var questionnaire = new QuestionnaireViewModel()
            {
                QuestionnaireId = Questionnaireid,
                Pages = temp,
                QuestionnaireData = (from q in _context.Questionnaire
                                     where q.QuestionnaireId == Questionnaireid
                                     select new QuestionnaireViewModel
                                     {
                                         QuestionnaireId = q.QuestionnaireId,
                                         Name = q.Name
                                     }).FirstOrDefault(),
                Questions = new List<QuestionsViewModel>()
                {
                    new QuestionsViewModel()
                    {
                        QuestionId = Guid.NewGuid(),
                        SectionId = 1,
                        QuestionNumber = "1",
                        QuestionType = QuestionType.multipleResponseType,
                        QuestionText = "Reason for security services " ,
                        QuestionnaireId=Questionnaireid,
                        OptionsList = new List<OptionsViewModel>()
                        {
                            new OptionsViewModel()
                            {
                                OptionId1 = 1,
                                OptionText = "verification test (to verify issues found in previous pentest have been fixed)",
                            },
                             new OptionsViewModel()
                            {
                                OptionId1 = 2,
                                OptionText = "retest",
                            },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 3,
                                OptionText = "we suspect we’ve been hacked",
                             },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 4,
                                OptionText = "ITHC",
                             },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 5,
                                OptionText = "need a specific component tested only",
                             },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 6,
                                OptionText = "to better understand security posture of our environment",
                             },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 7,
                                OptionText = "required by third party",
                             },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 8,
                                OptionText = "PCI",
                             },
                             new OptionsViewModel()
                             {
                                 OptionId1 = 9,
                                OptionText = "compliance/accreditation",
                             }
                        }
                    }
                }
            };
            return View(questionnaire);
        }


        [HttpPost]
        public IActionResult SecurityScopingtoolQuestionnaire(QuestionnaireViewModel model)
        {
            if (model.Pages == "Details")
            {
                return RedirectToAction("SecurityScopingtoolChildQuestionsDetails", new { SectionId = model.Questions[0].ResponseOptionId1, QuestionnaireId = model.QuestionnaireId });
            }
            else
            {
                return RedirectToAction("SecurityScopingtoolChildQuestions", new { SectionId = model.Questions[0].ResponseOptionId1, QuestionnaireId = model.QuestionnaireId });
            }
        }


        [HttpGet]
        public IActionResult SecurityScopingtoolChildQuestions(string SectionId, Guid QuestionnaireId, Guid id)
        {
            QuestionnaireViewModel model = new QuestionnaireViewModel();
            try
            {
                int Id = Convert.ToInt32(SectionId);
                model = _service.GetQuestionsById(Id, QuestionnaireId);
                model.CustomerId = id;
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SecurityScopingtoolChildQuestionsAsync(QuestionnaireViewModel model)
        {
            try
            {
                if (model.Questions == null)
                {
                    return RedirectToAction("EditAllQuestionnaires", "Questionnaire");
                }
                else
                {

                }

                var data = await _service.ResponseQuestionsUpdate(model);
                TempData["UpdateMessage"] = "successfully";
                return RedirectToAction("EditAllQuestionnaires", "Questionnaire");

            }
            catch (Exception ex) { throw; }
        }

        [HttpGet]
        public IActionResult SecurityScopingtoolChildQuestionsDetails(string SectionId, Guid QuestionnaireId, Guid id)
        {
            QuestionnaireViewModel model = new QuestionnaireViewModel();
            try
            {
                int Id = Convert.ToInt32(SectionId);
                model = _service.GetQuestionsById(Id, QuestionnaireId);
                model.CustomerId = id;
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }


        [HttpGet]
        public IActionResult ScopeRequests()
        {
            HomeViewModel model = new HomeViewModel();
            try
            {
                var id = _userManager.GetUserId(User); // Get user id:
                var giid = new Guid(id);
                var customer = _context.Customer.Where(m => m.UserId == id).FirstOrDefault();

                if (customer != null)
                {
                    model.Questionnaire = customer.Questionnaire;
                    model.CustomerId = customer.CustomerId;
                }
                else
                {
                    model.Questionnaire = false;

                }

                model.QuestionnaireList = _service.QuestionnaireAllList();
                model.CustomerQuestionnairList = _service.ScopRequestCustomerQuestionnairList(giid);
                model.ProjectsViewList = _service.ProjectsViewModelListque(giid);

                if (model.CustomerQuestionnairList.Count <= 0)
                {
                    TempData["QuestionnaireCountMessage"] = "QuestionnaireCountMessage";
                }
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        public IActionResult DeleteCustomerQuestionnairs(Guid? id, Guid QuestionnaireId, Guid ProjectId)
        {
            int Result = 0;
            try
            {
                if (QuestionnaireId == new Guid("0A2DFC0B-1F17-447C-A7B8-067F9980325A"))
                {
                    Result = _service.DeleteCustomerPciQuestionnairsByProjectId(id, QuestionnaireId, ProjectId);
                }
                else if (QuestionnaireId == new Guid("54515c61-e5c1-43d5-87ca-af8102a06323"))
                {
                    Result = _service.DeleteCustomerCeqQuestionnairsByProjectId(id, QuestionnaireId, ProjectId);
                }
                else if (QuestionnaireId == new Guid("a8837cf2-39cc-464e-a120-4248ea2cde0c"))
                {
                    Result = _service.DeleteCustomerQuestionnairsByProjectId(id, QuestionnaireId, ProjectId);
                    //Result = _service.DeleteCustomerSecurityQuestionnairsByProjectId(id, QuestionnaireId, ProjectId);
                }
                else
                {
                    Result = _service.DeleteCustomerQuestionnairsByProjectId(id, QuestionnaireId, ProjectId);

                }
                if (Result == 1)
                {
                    TempData["CustomerQuestionnaireUpdateMessage"] = "Questionnaire Delete succesfully !";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["CustomerQuestionnaireUpdateErrorMessage"] = "Error in Delete questionnaire ! ";
                    return RedirectToAction("Index", "Home");
                }

            }
            catch (Exception ex)
            {
                TempData["CustomerQuestionnaireUpdateErrorMessage"] = "Error in Delete questionnaire ! ";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult UploadProposalFile(CustomerQuestionnairViewModel model)
        {
            if (model.UploadedDocumentsData.ProjectId != Guid.Empty && model.UploadedDocumentsData.CustomerId != Guid.Empty)
            {
                var fname = string.Empty;
                if (model.UploadedDocumentsData.MyImage != null)
                {
                    try
                    {
                        var supportedTypes = new[] { "doc", "docx", "pdf", "xls", "xlsx", "png", "jpg", "jpeg", "txt" };
                        //Get file Extension
                        var fileExt = System.IO.Path.GetExtension(model.UploadedDocumentsData.MyImage.FileName).Substring(1);
                        if (!supportedTypes.Contains(fileExt))
                        {
                            TempData["FileUploadErrorMessage"] = "File Extension Is InValid - Only this type of allow DOC/DOCX/WORD/PDF/EXCEL/JPG/PNG/JPEG File";

                        }
                        else if (model.UploadedDocumentsData.MyImage.Length > (31457280) || model.UploadedDocumentsData.MyImage.Length < (10000))
                        {
                            TempData["FileUploadErrorMessage"] = "Select file size Should Be less than 30 MB and greater than 10Kb";
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

                            UploadedDocuments ud = new UploadedDocuments();
                            ud.UploadedDocumentsId = Guid.NewGuid();
                            ud.CustomerId = model.UploadedDocumentsData.CustomerId;
                            ud.ProjectId = model.UploadedDocumentsData.ProjectId;
                            ud.FileName = model.UploadedDocumentsData.MyImage.FileName;
                            ud.FileContents = "/Uploads/Documents/" + fname;
                            ud.FileModifiedTime = DateTime.Now;
                            _context.Add(ud);
                            _context.SaveChanges();

                            var id = _userManager.GetUserId(User);
                            var UserName = _context.Users.Where(m => m.Id == id).FirstOrDefault();
                            //var data = _context.CustomerRequests.Where(m => m.CustomerId == CustomerId && m.ProjectId == ProjectId).FirstOrDefault();
                            var customeremial = _context.Customer.Where(m => m.CustomerId == model.UploadedDocumentsData.CustomerId).FirstOrDefault();

                            //var SalesUserData = _context.Users.Where(m => m.Id == customeremial.CreatedBy.ToString()).FirstOrDefault();
                            if (customeremial != null)
                            {
                                string CurrentYear = DateTime.Now.Year.ToString();
                                string body = "<br/><br/>Hi Customer " + customeremial.Client + "" +
                                "<br/> The Salesperson " + UserName + " has accepted your proposal" +
                                " <br/><br/> " +
                                "<br/><br/>Regards" +
                                "<br/> ProCheckUp Ltd." +
                                "<br/><p>Copyright ©" + CurrentYear +  " <a target='blank' href='https://procheckup.com/'>ProCheckUp Ltd.</a>, All rights reserved.</p>";
                                string subject = "Accepted Proposal";

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

                            TempData["FileUploadMessage"] = "Proposal upload succesfully !";
                        }

                    }
                    catch (Exception ex)
                    {
                        TempData["FileUploadErrorMessage"] = "Error in uploading Proposal !";
                    }

                }
            }
            return RedirectToAction("CustomerQuestionnair", new { id = model.UploadedDocumentsData.CustomerId });

        }



        public async Task<IActionResult> UpdateQuestions(QuestionnaireViewModel model)
        {
            try
            {
                if (model.QuestionUpdateData != null)
                {
                    var data = await _service.UpdateSingleQuestions(model);
                    if (data != 0)
                    {
                        TempData["UpdateSingleQuestion"] = "Question Update succesfully !";
                    }
                    else
                    {
                        TempData["UpdateSingleQuestionError"] = "Error in Update Question !";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["UpdateSingleQuestionError"] = "Error in Update Question !";
            }
            return RedirectToAction("UpdateAllQuestionnaires", new { id = model.QuestionUpdateData.QuestionnaireId, QuestionnaireId = model.QuestionUpdateData.QuestionnaireId });
        }

        public async Task<IActionResult> DeleteSingleQuestions(Guid id, Guid QuestionnaireId,int SectionId)
        {
            
            try
            {
              
                if (id != Guid.Empty)
                {
                    var data = await _service.DeleteSingleQuestions(id, QuestionnaireId);
                    if (data != 0)
                    {
                        QuestionnaireViewModel model = new QuestionnaireViewModel();
                        model.QuestionnaireId = QuestionnaireId;
                        var data1 = await _service.UpdateAllVar(model);
                        TempData["DeleteSingleQuestion"] = "Question Delete succesfully !";
                    }
                    else
                    {
                        TempData["DeleteSingleQuestionError"] = "Error in Delete Question !";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["DeleteSingleQuestionError"] = "Error in Delete Question !";
            }
            return RedirectToAction("UpdateAllQuestionnaires", new {  id = QuestionnaireId, QuestionnaireId = QuestionnaireId  });
           // return RedirectToAction("SecurityScopingtoolChildQuestions", new { SectionId = SectionId, QuestionnaireId = QuestionnaireId });

        }


        public async Task<IActionResult> DeleteSinglepciQuestions(Guid id, Guid QuestionnaireId)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    var data = await _service.DeleteSingleQuestions(id, QuestionnaireId);
                    if (data != 0)
                    {
                        TempData["SuccesMessage"] = "Question Delete succesfully !";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error in Delete Question !";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error in Delete Question !";
            }
            return RedirectToAction("UpdatepciQuestions", new { QuestionnaireId = QuestionnaireId });
        }



        public async Task<IActionResult> DeleteSingleChieldQuestions(Guid id, Guid QuestionnaireId)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    var data = await _service.DeleteSingleChieldQuestions(id, QuestionnaireId);
                    if (data != 0)
                    {
                        QuestionnaireViewModel model = new QuestionnaireViewModel();
                            model.QuestionnaireId = QuestionnaireId;
                        var data1 = await _service.UpdateAllVar(model);


                        TempData["DeleteSingleQuestion"] = "Question Delete succesfully !";
                    }
                    else
                    {
                        TempData["DeleteSingleQuestionError"] = "Error in Delete Question !";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["DeleteSingleQuestionError"] = "Error in Delete Question !";
            }
            return RedirectToAction("UpdateAllQuestionnaires", new { id = QuestionnaireId, QuestionnaireId = QuestionnaireId });
        }

        public async Task<IActionResult> CEQUpdateQuestions(CyberEssentialsQuestionnaireModel model)
        {
            try
            {
                if (model.CEQQuetUpdateData != null)
                {
                    var data = await _service.UpdateSingleCEQQuestions(model);
                    if (data != 0)
                    {
                        TempData["SuccesSingleQuestion"] = "Question Update succesfully !";
                    }
                    else
                    {
                        TempData["ErrorSingleQuestionError"] = "Error in Update Question !";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorSingleQuestionError"] = "Error in Update Question !";
            }
            return RedirectToAction("UpdateAllCEQQuestionnaires", new { Userid = model.CEQQuetUpdateData.QuestionnaireId, id = model.CEQQuetUpdateData.QuestionnaireId });
        }
        public async Task<IActionResult> CEQDeleteSingleQuestions(Guid id, Guid QuestionnaireId)
        {
            try
            {
                if (id != Guid.Empty && QuestionnaireId != Guid.Empty)
                {
                    var data = await _service.DeleteSingleCEQQuestions(id, QuestionnaireId);
                    if (data != 0)
                    {
                        TempData["SuccesSingleQuestion"] = "Question Delete succesfully !";
                    }
                    else
                    {
                        TempData["ErrorSingleQuestionError"] = "Error in Delete Question !";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorSingleQuestionError"] = "Error in Delete Question !";
            }
            return RedirectToAction("UpdateAllCEQQuestionnaires", new { Userid = QuestionnaireId, id = QuestionnaireId });
        }

        public async Task<IActionResult> AddNewQuestions(QuestionnaireViewModel model)
        {
            try
            {
                if (model.QuestionUpdateData != null)
                {
                    var data = await _service.AddNewSingleQuestions(model);
                    if (data != 0)
                    {
                        model.QuestionnaireId = model.QuestionUpdateData.QuestionnaireId;
                        var data1 = await _service.UpdateAllVar(model);
                        TempData["UpdateSingleQuestion"] = "Question Create succesfully !";
                    }
                    else
                    {
                        TempData["UpdateSingleQuestionError"] = "Error in Create Question !";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["UpdateSingleQuestionError"] = "Error in Create Question !";
            }
            return RedirectToAction("UpdateAllQuestionnaires", new { id = model.QuestionUpdateData.QuestionnaireId, QuestionnaireId = model.QuestionUpdateData.QuestionnaireId });
        }



        public async Task<IActionResult> UpdateSecurityScopingQuestions(QuestionnaireViewModel model)
        {
            try
            {
                if (model.QuestionUpdateData != null)
                {
                    var data = await _service.UpdateSingleQuestions(model);
                    if (data != 0)
                    {
                        TempData["UpdateSingleSecurityScopingQuestion"] = "Question Update succesfully !";
                    }
                    else
                    {
                        TempData["UpdateSingleSecurityScopingQuestionError"] = "Error in Update Question !";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["UpdateSingleSecurityScopingQuestionError"] = "Error in Update Question !";
            }
            return RedirectToAction("SecurityScopingtoolChildQuestions", new { SectionId = model.QuestionUpdateData.SectionId, QuestionnaireId = model.QuestionUpdateData.QuestionnaireId });
        }

        public async Task<IActionResult> AddNewSecurityScopingQuestions(QuestionnaireViewModel model)
        {
            try
            {
                if (model.QuestionUpdateData != null)
                {
                    var data = await _service.AddNewSecurityScopingSingleQuestions(model);
                    if (data != 0)
                    {
                        TempData["UpdateSingleSecurityScopingQuestion"] = "Question Create succesfully !";
                    }
                    else
                    {
                        TempData["UpdateSingleSecurityScopingQuestionError"] = "Error in Create Question !";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["UpdateSingleSecurityScopingQuestionError"] =ex.Message;
            }
            return RedirectToAction("SecurityScopingtoolChildQuestions", new { SectionId = model.QuestionUpdateData.SectionId, QuestionnaireId = model.QuestionUpdateData.QuestionnaireId });
        }
        public IActionResult CretaeEquestion(Guid id)
        {
            EquestionViewModel model = new EquestionViewModel();
            model.QuestionnaireId = id;
            model.EquestionViewModelData = _service.EquationData(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEquestion(EquestionViewModel model)
        {
            try
            {

                if (model.QuestionnaireId != Guid.Empty && model.CreatedFormula != null)
                {

                    var data = await _service.EquestionViewModel(model);

                    TempData["EquestSuccesMessage"] = "Equation Created successfully";

                    return RedirectToAction("UpdateAllQuestionnaires", new { id = model.QuestionnaireId, QuestionnaireId = model.QuestionnaireId });

                }
                else
                {
                    TempData["UpdateSingleQuestionError"] = "Error : Equation Not Created";
                    return RedirectToAction("UpdateAllQuestionnaires", "Questionnaire");
                }


            }
            catch (Exception ex) { throw; }
        }

        public async Task<IActionResult> AddNewQuestionnaire(CustomerQuestionnairViewModel model)
        {
            try
            {

                if (model.AddNewQuestionnairData.Name != null)
                {

                    var data = await _service.AddNewQuestionnaireService(model.AddNewQuestionnairData);

                    TempData["NewQuestionnaireSuccesMessage"] = "Scope Created successfully";

                }
                return RedirectToAction("EditAllQuestionnaires", "Questionnaire");

            }
            catch (Exception ex) { throw; }
        }

        public async Task<IActionResult> DeleteSingleQuestionnaire(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    var data = await _service._DeleteSingleQuestionnaire(id);
                    if (data != 0)
                    {
                        TempData["DeleteSingleQuestionnaireSuccesMessage"] = "Questionnaire Delete succesfully !";
                    }
                    else
                    {
                        TempData["DeleteSingleQuestionnaireError"] = "Error in Delete Questionnaire !";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["DeleteSingleQuestionnaireError"] = "Error in Delete Questionnaire !";
            }


            return RedirectToAction("EditAllQuestionnaires", "Questionnaire");
        }



        [HttpPost]
        public JsonResult AddNewPCIQuestionnaire(List<QuestOptions> options, string questionName = null, string questionType = null, string questioneerId = null)
        {
            try
            {
                bool flag = false;
                string message = "";

                if(questionType=="0")
                {
                    if(options.Count>0)
                    {
                        if (questioneerId != null && questionName != null && questionType != null && options != null)
                        {
                            var res = _service.AddNeQuestions(options, questionName, questionType, questioneerId);
                            if (res != null)
                            {
                                flag = true;
                                message = "Success";
                                TempData["SuccesMessage"] = "Question Created successfully";
                            }
                            else
                            {
                                flag = false;
                                message = "Error";
                                TempData["ErrorMessage"] = "Question Not Created";
                            }
                        }
                        else
                        {
                            flag = false;
                            message = "Error";
                            TempData["ErrorMessage"] = "Question Not Created all fields is required";
                        }
                    }
                    else
                    {
                        message = "Error";
                        TempData["ErrorMessage"] = "Question Not Created Options Name is required";
                    }
                }
                else
                {
                    if (questioneerId != null && questionName != null && questionType != null )
                    {
                        var res = _service.AddNeQuestions(options, questionName, questionType, questioneerId);
                        if (res != null)
                        {
                            flag = true;
                            message = "Success";
                            TempData["SuccesMessage"] = "Question Created successfully";
                        }
                        else
                        {
                            flag = false;
                            message = "Error";
                            TempData["ErrorMessage"] = "Question Not Created";
                        }
                    }
                    else
                    {
                        flag = false;
                        message = "Error";
                        TempData["ErrorMessage"] = "Question Not Created all fields is required";
                    }
                }

                return Json(message);

            }
            catch (Exception)
            {
                return Json("Error");
                throw;
            }

        }
        [HttpPost]
        public  async Task<JsonResult> AddNewSecuirtyQuestionnaire(List<QuestOptions> options, string questionName = null, string questionType = null, string questioneerId = null,string sectionId=null)
        {
            try
            {
                bool flag = false;
                string message = "";
              

                if (questionType == "0")
                {
                    if (options.Count > 0)
                    {
                        if (questioneerId != null && questionName != null && questionType != null && options != null && sectionId !=null)
                        {
                            var res =await _service.AddNewSecuirtyQuestions(options, questionName, questionType, questioneerId, sectionId);
                            if (res != null)
                            {
                                flag = true;
                                message = "Success";
                                TempData["UpdateSingleSecurityScopingQuestion"] = "Question Created successfully";
                            }
                            else
                            {
                                flag = false;
                                message = "Error";
                                TempData["UpdateSingleSecurityScopingQuestionError"] = "Question Not Created";
                            }
                        }
                        else
                        {
                            flag = false;
                            message = "Error";
                            TempData["UpdateSingleSecurityScopingQuestionError"] = "Question Not Created all fields is required";
                        }
                    }
                    else
                    {
                        message = "Error";
                        TempData["UpdateSingleSecurityScopingQuestionError"] = "Question Not Created Options Name is required";
                    }
                }
                else
                {
                    if (questioneerId != null && questionName != null && questionType != null && sectionId != null)
                    {
                        var res = await _service.AddNewSecuirtyQuestions(options, questionName, questionType, questioneerId, sectionId);
                        if (res != null)
                        {
                            flag = true;
                            message = "Success";
                            TempData["UpdateSingleSecurityScopingQuestion"] = "Question Created successfully";
                        }
                        else
                        {
                            flag = false;
                            message = "Error";
                            TempData["UpdateSingleSecurityScopingQuestionError"] = "Question Not Created";
                        }
                    }
                    else
                    {
                        flag = false;
                        message = "Error";
                        TempData["UpdateSingleSecurityScopingQuestionError"] = "Question Not Created all fields is required";
                    }
                }

                return Json(message);

            }
            catch (Exception)
            {
                return Json("Error");
                throw;
            }

        }





        [HttpPost]
        public async Task<JsonResult> AddNewSecuirtyQuestionnaire2(List<QuestOptions> options, string questionName = null, string questionType = null, string questioneerId = null, string sectionId = null)
        {
            var msg = "";
            try
            {
                if (questionType == "0")
                {
                    if (options.Count > 0)
                    {
                        if (questioneerId != null && questionName != null && questionType != null && options != null && sectionId != null)
                        {
                            var res = await _service.AddNewSecuirtyQuestions2(options, questionName, questionType, questioneerId, sectionId);
                            if (res != 0)
                            {
                                msg = "Success";
                               TempData["UpdateSingleSecurityScopingQuestion"] = "Question Created successfully";
                            }
                            else
                            {
                                msg = "Error";
                                TempData["UpdateSingleSecurityScopingQuestionError"] = "Error in create Question !";
                            }
                        }
                        else
                        {
                            msg = "Error";
                            TempData["UpdateSingleSecurityScopingQuestionError"] = "Question Not Created all fields is required";
                        }
                    }
                    else
                    {
                        msg = "";
                        TempData["UpdateSingleSecurityScopingQuestionError"] = "Question Not Created Options Name is required";
                    }
                }
                else
                {
                    if (questioneerId != null && questionName != null && questionType != null && sectionId != null)
                    {
                        var res = await _service.AddNewSecuirtyQuestions2(options, questionName, questionType, questioneerId, sectionId);
                        if (res != 0)
                        {
                            msg = "Success";
                            TempData["UpdateSingleSecurityScopingQuestion"] = "Question Created successfully";
                        }
                        else
                        {
                            msg = "Error";
                            TempData["UpdateSingleSecurityScopingQuestionError"] = "Error in create Question !";
                        }
                    }
                    else
                    {
                        msg = "Error";
                        TempData["UpdateSingleSecurityScopingQuestionError"] = "Question Not Created all fields is required";
                    }
                }

                return Json(msg);

            }
            catch (Exception)
            {
                TempData["UpdateSingleSecurityScopingQuestionError"] = "Error in create Question !";
                return Json("Error");
                throw;
            }

        }








        public async Task<IActionResult> DeleteProposals(Guid id, Guid customerId)
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

                        TempData["FileUploadMessage"] = "Proposal Delete successfully !";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["FileUploadErrorMessage"] = "Error in Delete Proposal !";
            }
            return RedirectToAction("CustomerQuestionnair", new { id = customerId });

        }

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

                        TempData["FileUploadMessage"] = "File Delete successfully !";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["FileUploadErrorMessage"] = "Error in Delete File !";
            }
            return RedirectToAction("CustomerQuestionnair", new { id = customerId });

        }

        //[HttpPost]
        //public async Task<JsonResult> StatusUpdate(string Status = null, string CustomerId = null, string ProjectsId = null)
        //{
        //    var UserId = _userManager.GetUserId(User);
        //    var projectsId = new Guid(ProjectsId);
        //    DbResponce a = new DbResponce();
        //    try
        //    {

        //        if (projectsId != Guid.Empty)
        //        {
        //          a=  await _service.UpdateProjectStatus(Status, CustomerId, ProjectsId);
        //           if(a.msg=="1")
        //            {
        //                TempData["StatusUpdateMessage"] = "Status Update succesfully !";
        //            }
        //            else
        //            {
        //                TempData["StatusUpdateErrorMessage"] = "Error in Update Status !";
        //            }
        //        }

        //        return Json(a.msg);

        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["StatusUpdateErrorMessage"] = "Error in Update Project !";
        //        return Json("Error");

        //    }
        //}


        [HttpPost]
        public async Task<JsonResult> StatusUpdate(string Status = null, string CustomerId = null, string ProjectsId = null)
        {
            var UserId = _userManager.GetUserId(User);
            var projectsId = new Guid(ProjectsId);
            DbResponce a = new DbResponce();
            try
            {

                if (projectsId != Guid.Empty)
                {
                    var data = _context.Projects.Where(m => m.ProjectsId == projectsId).FirstOrDefault();
                    var customeremial = _context.Customer.Where(m => m.CustomerId == data.CustomerId).FirstOrDefault();
                    var SalesUserData = _context.Projects.Where(m => m.ProjectsId == projectsId).FirstOrDefault();
                    a = await _service.UpdateProjectStatus(Status, CustomerId, ProjectsId);
                    if (SalesUserData.RequestStatus == "0")
                    {
                        SalesUserData.RequestStatus = "No Status";
                    }
                    else if (SalesUserData.RequestStatus == "1")
                    {
                        SalesUserData.RequestStatus = "Request Awaiting Review";

                    }
                    else if (SalesUserData.RequestStatus == "2")
                    {
                        SalesUserData.RequestStatus = "Request Under Review";

                    }

                    else if (SalesUserData.RequestStatus == "3")
                    {
                        SalesUserData.RequestStatus = "Scope Finalised/Pre Req’s required";

                    }

                    else if (SalesUserData.RequestStatus == "4")
                    {
                        SalesUserData.RequestStatus = "Booking Confirmed";

                    }

                    if (a.msg == "1")
                    {
                        string CurrentYear = DateTime.Now.Year.ToString();
                        string body = "<br/><br/>Hi " + customeremial.FirstName + "," +"<br/>" +
                               "<br/> The status of your project: " + SalesUserData.ProjectName + " has changed to: " + SalesUserData.RequestStatus + "." +
                        " <br/>" +
                                "<br/><br/>Thanks," +
                                "<br/> ProCheckUp Team" +
                                "<br/><p>Copyright ©" + CurrentYear + " <a target='blank' href='https://procheckup.com/'>ProCheckUp Ltd.</a>, All rights reserved.</p>";
                        string subject = "Status Updated";

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


                        TempData["StatusUpdateMessage"] = "Status Update succesfully !";
                    }
                    else
                    {
                        TempData["StatusUpdateErrorMessage"] = "Error in Update Status !";
                    }
                }

                return Json(a.msg);

            }
            catch (Exception ex)
            {
                TempData["StatusUpdateErrorMessage"] = "Error in Update Project !";
                return Json("Error");

            }
        }

        public async Task<IActionResult> AddNewSubQuestions(QuestionnaireViewModel model)
        {
            try
            {
                if (model.QuestionUpdateData != null)
                {
                    var data = await _service.AddNewSingleSubQuestions(model);
                    if (data != 0)
                    {
                        model.QuestionnaireId = model.QuestionUpdateData.QuestionnaireId;
                        var data1 = await _service.UpdateAllVar(model);
                        TempData["UpdateSingleQuestion"] = "Question Create succesfully !";
                    }
                    else
                    {
                        TempData["UpdateSingleQuestionError"] = "Error in Create Question !";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["UpdateSingleQuestionError"] = "Error in Create Question !";
            }
            return RedirectToAction("UpdateAllQuestionnaires", new { id = model.QuestionUpdateData.QuestionnaireId, QuestionnaireId = model.QuestionUpdateData.QuestionnaireId });
        }
        [HttpPost]
        public async Task<JsonResult> DragAndDropQuestion(string DraggId = null, string droppedId = null, string questioneerId = null)
        {
            try
            {
                bool flag = false;
                Guid draggId = new Guid(DraggId);     
                Guid DroppedId = new Guid(droppedId);     
                Guid QuestioneerId = new Guid(questioneerId);     
                string message = "";              
                    if (draggId!=Guid.Empty&& DroppedId!=Guid.Empty)
                    {
                       
                            var res = await _service.DragDropQuestion(draggId, DroppedId, QuestioneerId);
                            if (res != 0)
                            {
                        QuestionnaireViewModel model = new QuestionnaireViewModel();
                        model.QuestionnaireId = QuestioneerId;
                        var data1 = await _service.UpdateAllVar(model);


                        flag = true;
                                message = "Success";
                                TempData["DragAndDropSuccesMessage"] = "Question Move successfully";
                            }
                            else
                            {
                                flag = false;
                                message = "Error";
                                TempData["DragAndDropErrorMessage"] = "Error in Move Question !";
                            }             
                    }
                    else
                    {
                        message = "Error";
                        TempData["DragAndDropErrorMessage"] = "Error in Move Question !";
                    }              
                return Json(message);
            }
            catch (Exception)
            {
                return Json("Error");
                throw;
            }
        }


        [HttpGet]
        public JsonResult GetAllMultipleCheckBox(string QuestionId = null,string QuestioneerId = null)
        {
            try
            {
                bool flag = false;
                Guid questionId = new Guid(QuestionId);
               
                Guid questioneerId = new Guid(QuestioneerId);
                string message = "";
                if (questionId != Guid.Empty && questioneerId != Guid.Empty)
                {

                    var res = _service.GetAllMultipleCheckBoxList(questionId, questioneerId);
                    if (res != null)
                    {
                        return Json(res);
                    }
                    else
                    {                      
                        message = "Error";
                        TempData["DragAndDropErrorMessage"] = "Error in Move Question !";
                    }
                }
                else
                {
                    message = "Error";
                    TempData["DragAndDropErrorMessage"] = "Error in Move Question !";
                    
                }
                return Json(message);
            }
            catch (Exception)
            {
                return Json("Error");
                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateOptions(List<OptionUpdate> optionValue = null, string QuestionnaireId = null, string QuestionId = null, string QuestionName = null,string Type=null,bool isMandatory=false)
        {
            try
            {
                if (optionValue != null)
                {
                    var data1 = await _service.UpdateOptions(optionValue, QuestionId, QuestionName, Type, isMandatory);
                    TempData["UpdateSingleQuestion"] = "Question Update succesfully !";
                }
                return Json("Success");
                // return Json(a.msg);

            }
            catch
            {
                TempData["UpdateSingleQuestionError"] = "Error in Update Question !";
                return Json("Error");

            }
        }

        public async Task<IActionResult> EditQuestionnaire(CustomerQuestionnairViewModel model)
        {
            try
            {

                if (model.AddNewQuestionnairData.Name != null)
                {

                    var data = await _service.EditQuestionnaireService(model.AddNewQuestionnairData);

                    TempData["NewQuestionnaireSuccesMessage"] = "Scope Update successfully";

                }
                return RedirectToAction("EditAllQuestionnaires", "Questionnaire");

            }
            catch (Exception ex) { throw; }
        }

        [HttpPost]
        public async Task<JsonResult> HideUnHideQuestionnair(string Toggle = null, string QuestionnaireId = null)
        {
            var questionnaireId = new Guid(QuestionnaireId);
            DbResponce a = new DbResponce();
            try
            {
                if (questionnaireId != Guid.Empty)
                {
                    a = await _service.HideUnHideUpdate(Toggle, questionnaireId);
                }
                return Json(a.msg);
            }
            catch (Exception ex)
            {
                return Json("Error");
            }
        }


    }
}



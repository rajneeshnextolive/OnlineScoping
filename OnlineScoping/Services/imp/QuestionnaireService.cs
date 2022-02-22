using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineScoping.Data;
using OnlineScoping.Migrations;
using OnlineScoping.Models;
using OnlineScoping.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineScoping.Services.imp
{
    public class QuestionnaireService : IQuestionnaireService
    {
        private readonly OnlineScopingContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public QuestionnaireService(OnlineScopingContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public QuestionnaireViewModel GetQuestionsById(int? id, Guid QuestionnaireId)
        {
            try
            {
                QuestionsViewModel model = new QuestionsViewModel();

                var questionnaire = new QuestionnaireViewModel()
                {
                    MethodologyeList = (from j in _context.Methodologye
                                        where j.Type == "Methodologies"
                                        select new MethodologyeViewModel
                                        {
                                            FileName = j.FileName,
                                            Type = j.Type,
                                            UpdatedDate = j.UpdatedDate,
                                            Id = j.Id,
                                        }).ToList(),
                    SampleReportList = (from j in _context.Methodologye
                                        where j.Type == "SampleReport"
                                        select new MethodologyeViewModel
                                        {
                                            FileName = j.FileName,
                                            Type = j.Type,
                                            UpdatedDate = j.UpdatedDate,
                                            Id = j.Id,
                                        }).ToList(),

                    QuestionnaireData = (from q in _context.Questionnaire
                                         where q.QuestionnaireId == QuestionnaireId
                                         select new QuestionnaireViewModel
                                         {
                                             QuestionnaireId = q.QuestionnaireId,
                                             Name = q.Name
                                         }).FirstOrDefault(),
                    EquestionViewModelData = (from e in _context.Equestion
                                              where e.QuestionId == QuestionnaireId
                                              select new EquestionViewModel
                                              {
                                                  CostEquestion = e.CostEquestion,
                                                  DaysEquestion = e.DaysEquestion,
                                                  RequirmentEquestion = e.RequirmentEquestion
                                              }).FirstOrDefault(),
                    SectionId = id,
                    QuestionnaireId = QuestionnaireId,
                    Methodology_Deliverablesdata = (from m in _context.Admin_Methodology_Deliverables
                                                    where m.QuestionnaireId == QuestionnaireId
                                                    select new Methodology_DeliverablesViewModel
                                                    {
                                                        QuestionnaireId = m.QuestionnaireId,
                                                        ScopeTypeMethodolgy = m.ScopeTypeMethodolgy,
                                                        ScopeTypeMethodolgy_FileName = (from b in _context.Methodologye
                                                                                        where b.Id == m.MethodologyId
                                                                                        select new
                                                                                        {
                                                                                            b.FileName,
                                                                                        }).Single().FileName,
                                                        ScopeTypeSampleReport = m.ScopeTypeSampleReport,
                                                        ScopeTypeSampleReport_FileName = (from b in _context.Methodologye
                                                                                          where b.Id == m.MethodologyReportId
                                                                                          select new
                                                                                          {
                                                                                              b.FileName,
                                                                                          }).Single().FileName,
                                                        Methodology_DeliverablesId = m.Methodology_DeliverablesId,
                                                        MethodologyId = m.MethodologyId,
                                                        MethodologyReportId = m.MethodologyReportId
                                                    }).FirstOrDefault(),
                    Questions = (from a in _context.Questions
                                 where a.SectionId == id
                                 select new QuestionsViewModel
                                 {
                                     QuestionId = a.QuestionId,
                                     SectionId = a.SectionId,
                                     QuestionNumber = a.QuestionNumber,
                                     QuestionType = a.questionType,
                                     QuestionText = a.QuestionText,
                                     QuestionnaireId = QuestionnaireId,
                                     Days = a.Days,
                                     Cost = a.Cost,
                                     Requirement = a.Requirement,
                                     VariableName = a.VariableName,
                                     QuestionValue = a.QuestionValue,
                                     isMandatory = a.isMandatory,
                                     OptionsList = (from b in _context.Options
                                                    where b.QuestionId == a.QuestionId
                                                    select new OptionsViewModel
                                                    {
                                                        OptionId = b.OptionId,
                                                        OptionText = b.OptionText,
                                                    }).ToList(),
                                     ChildQuestionsList = (from c in _context.ChildQuestions
                                                           where c.QuestionId == a.QuestionId
                                                           select new ChildQuestionsViewModel
                                                           {
                                                               ChildQuestionId = c.ChildQuestionId,
                                                               QuestionNumber = c.QuestionNumber,
                                                               SectionId = c.SectionId,
                                                               QuestionType = c.questionType,
                                                               QuestionText = c.QuestionText,
                                                               QuestionnaireId = QuestionnaireId,
                                                               Days = c.Days,
                                                               Cost = c.Cost,
                                                               Requirement = c.Requirement,
                                                               VariableName = c.VariableName,
                                                               isMandatory = c.isMandatory,
                                                               OptionsList = (from d in _context.Options
                                                                              where d.QuestionId == c.ChildQuestionId
                                                                              select new OptionsViewModel
                                                                              {
                                                                                  OptionId = d.OptionId,
                                                                                  OptionText = d.OptionText,
                                                                              }).ToList(),
                                                           }).OrderBy(m => m.QuestionNumber).ToList(),
                                 }).OrderBy(m => m.QuestionNumber).ToList()

                };
                return questionnaire;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<QuestionResponseViewModel> ResponseQuestions(QuestionnaireViewModel Questionnaire)
        {

            QuestionResponseViewModel model = new QuestionResponseViewModel();
            try
            {
                var customer = _context.Customer.Where(m => m.UserId == Questionnaire.UserId).FirstOrDefault();
                if (customer == null)
                {
                    customer = _context.Customer.Where(m => m.CustomerId == Questionnaire.CustomerId).FirstOrDefault();
                }
                var resdata = _context.QuestionResponse.Where(m => m.QuestionnaireId == Questionnaire.QuestionnaireId && m.CustomerId == customer.CustomerId && m.ProjectId == Questionnaire.ProjectId).ToList();
                if (resdata.Count != 0)
                {
                    foreach (var Rsdata in resdata)
                    {
                        var employee = _context.QuestionResponse.Find(Rsdata.ResponseId);
                        _context.QuestionResponse.Remove(employee);
                        _context.SaveChanges();
                    }
                }


                foreach (var item in Questionnaire.Questions)
                {
                    if (item.ChildQuestionsList != null)
                    {
                        foreach (var item1 in item.ChildQuestionsList)
                        {
                            QuestionResponse data1 = new QuestionResponse();
                            data1.ResponseId = Guid.NewGuid();
                            data1.CustomerId = customer.CustomerId;
                            data1.QuestionId = item.QuestionId;
                            data1.ChildQuestionId = item1.ChildQuestionId;
                            data1.QuestionnaireId = item.QuestionnaireId;
                            if (item1.QuestionType == 0)
                            {
                                data1.OptionType = 0;
                            }
                            else if (item1.QuestionType == QuestionType.DateTimeType)
                            {
                                data1.OptionType = 1;
                            }
                            else
                            {
                                data1.OptionType = 2;
                            }
                            data1.Responsetext = item1.Responsetext;
                            data1.OptionId = item1.ResponseOptionId;
                            data1.OptionDate = item1.ResponseDateTime;
                            data1.ProjectId = Questionnaire.ProjectId;
                            _context.Add(data1);

                        }
                    }



                    model.SectionId = item.SectionId;
                    model.OptionId = item.ResponseOptionId;
                    model.QuestionnaireId = item.QuestionnaireId;
                    QuestionResponse data = new QuestionResponse();
                    data.ResponseId = Guid.NewGuid();
                    data.CustomerId = customer.CustomerId;
                    data.QuestionId = item.QuestionId;
                    data.QuestionnaireId = item.QuestionnaireId;
                    data.Responsetext = item.Responsetext;
                    if (item.QuestionType == 0)
                    {
                        data.OptionType = 0;
                    }
                    else if (item.QuestionType == QuestionType.DateTimeType)
                    {
                        data.OptionType = 1;
                    }
                    else
                    {
                        data.OptionType = 2;
                    }
                    data.OptionId = item.ResponseOptionId;
                    data.OptionDate = item.ResponseDateTime;
                    data.ProjectId = Questionnaire.ProjectId;
                    _context.Add(data);

                }
                //Methodology_Deliverables md = new Methodology_Deliverables();
                //md.Methodology_DeliverablesId = Guid.NewGuid();
                //md.CustomerId = customer.CustomerId;
                //md.CreatedBy = customer.CustomerId;
                //md.CreatedDate = DateTime.Now;
                //md.QuestionnaireId = model.QuestionnaireId;
                //md.ProjectId = Questionnaire.ProjectId;
                //md.ScopeTypeMethodolgy = Questionnaire.ScopeTypeMethodolgy;
                //md.ScopeTypeMethodolgy_FileName = Questionnaire.ScopeTypeMethodolgy_FileName;
                //md.ScopeTypeSampleReport = Questionnaire.ScopeTypeSampleReport;
                //md.ScopeTypeSampleReport_FileName = Questionnaire.ScopeTypeSampleReport_FileName;
                //_context.Add(md);
                model.CustomerId = customer.CustomerId;
                model.QuestionnaireId = Questionnaire.QuestionnaireId;
                model.ProjectId = Questionnaire.ProjectId;
                var Res = await _context.SaveChangesAsync();
                if (Res > 0)
                {
                    if (resdata.Count == 0)
                    {
                        CustomerQuestionnair data = new CustomerQuestionnair();
                        data.CustomerQuestionnairId = Guid.NewGuid();
                        data.CustomerId = customer.CustomerId;
                        data.CreatedBy = customer.CustomerId;
                        data.UpdatedDate = DateTime.Now;
                        data.CreatedDate = DateTime.Now;
                        data.QuestionnaireId = Questionnaire.QuestionnaireId;
                        data.IsQuestionnaire = true;
                        data.ProjectId = Questionnaire.ProjectId;
                        _context.Add(data);
                        await _context.SaveChangesAsync();
                    }

                }
            }
            catch (Exception ex) { throw; }
            return model;

        }

        public async Task<int> CustomerUpdate(QuestionnaireViewModel model)
        {
            var customer = _context.Customer.Where(m => m.UserId == model.UserId).FirstOrDefault();
            if (customer == null)
            {
                customer = _context.Customer.Where(m => m.CustomerId == model.CustomerId).FirstOrDefault();
            }
            var data = _context.Customer.Find(customer.CustomerId);
            data.Questionnaire = true;
            data.RepliedDate = DateTime.Now;
            _context.Update(data);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> ResponseQuestionsbySection(QuestionnaireViewModel model)
        {
            var customer = _context.Customer.Where(m => m.UserId == model.UserId).FirstOrDefault();
            QuestionResponse data = new QuestionResponse();
            data.ResponseId = Guid.NewGuid();
            data.CustomerId = customer.CustomerId;
            data.ProjectId = model.ProjectId;
            _context.Add(data);
            return await _context.SaveChangesAsync();
        }


        public QuestionnaireViewModel GetResponseQuestionsById(Guid? id, Guid QuestionnaireId, Guid ProjectId)
        {
            try
            {
                QuestionsViewModel model = new QuestionsViewModel();

                var questionnaire = new QuestionnaireViewModel()
                {
                    //  SectionId = id,
                    QuestionnaireData = (from q in _context.Questionnaire
                                         where q.QuestionnaireId == QuestionnaireId
                                         select new QuestionnaireViewModel
                                         {
                                             QuestionnaireId = q.QuestionnaireId,
                                             Name = q.Name
                                         }).FirstOrDefault(),
                    //Methodology_Deliverablesdata = (from m in _context.Methodology_Deliverables
                    //                                where m.ProjectId == ProjectId && m.QuestionnaireId == QuestionnaireId && m.CustomerId == id
                    //                                select new Methodology_DeliverablesViewModel
                    //                                {
                    //                                    CustomerId = m.CustomerId,
                    //                                    QuestionnaireId = m.QuestionnaireId,
                    //                                    ScopeTypeMethodolgy = m.ScopeTypeMethodolgy,
                    //                                    ScopeTypeMethodolgy_FileName = m.ScopeTypeMethodolgy_FileName,
                    //                                    ScopeTypeSampleReport = m.ScopeTypeSampleReport,
                    //                                    ScopeTypeSampleReport_FileName = m.ScopeTypeSampleReport_FileName,
                    //                                    Methodology_DeliverablesId = m.Methodology_DeliverablesId,
                    //                                    ProjectId = m.ProjectId,
                    //                                }).FirstOrDefault(),
                    Questions = (from a in _context.QuestionResponse
                                 where a.CustomerId == id && a.ChildQuestionId == Guid.Empty && a.QuestionnaireId == QuestionnaireId && a.ProjectId == ProjectId
                                 select new QuestionsViewModel
                                 {
                                     ResponseId = a.ResponseId,
                                     QuestionData = _context.Questions.Where(m => m.QuestionId == a.QuestionId).FirstOrDefault(),
                                     QuestionId = a.QuestionId,
                                     OptionType = a.OptionType,
                                     ResponseOptionId = a.OptionId,
                                     ResponseDateTime = a.OptionDate,
                                     Days = a.Days,
                                     Cost = a.Cost,
                                     Requirement = a.Requirement,
                                     OptionsList = (from b in _context.Options
                                                    where b.QuestionId == a.QuestionId
                                                    select new OptionsViewModel
                                                    {
                                                        OptionId = b.OptionId,
                                                        OptionText = b.OptionText,
                                                    }).ToList(),
                                     ChildQuestionsList = (from c in _context.QuestionResponse
                                                           where c.QuestionId == a.QuestionId && c.ChildQuestionId != Guid.Empty && a.QuestionnaireId == QuestionnaireId && c.ProjectId == ProjectId
                                                           select new ChildQuestionsViewModel
                                                           {
                                                               ResponseId = c.ResponseId,
                                                               QuestionData = _context.ChildQuestions.Where(m => m.ChildQuestionId == c.ChildQuestionId).FirstOrDefault(),
                                                               ChildQuestionId = c.ChildQuestionId,
                                                               OptionType = c.OptionType,
                                                               ResponseOptionId = c.OptionId,
                                                               ResponseDateTime = c.OptionDate,
                                                               Days = c.Days,
                                                               Cost = c.Cost,
                                                               Requirement = c.Requirement,
                                                               OptionsList = (from d in _context.Options
                                                                              where d.QuestionId == c.ChildQuestionId
                                                                              select new OptionsViewModel
                                                                              {
                                                                                  OptionId = d.OptionId,
                                                                                  OptionText = d.OptionText,
                                                                              }).ToList(),
                                                           }).OrderBy(m => m.QuestionData.QuestionNumber).ToList(),
                                 }).OrderBy(m => m.QuestionData.QuestionNumber).ToList()

                };
                return questionnaire;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public QuestionnaireViewModel ResponseDetailsByProjectId(Guid? id, Guid QuestionnaireId, Guid Projectid)
        {
            try
            {
                QuestionsViewModel model = new QuestionsViewModel();

                var questionnaire = new QuestionnaireViewModel()
                {
                    //  SectionId = id,
                    QuestionnaireData = (from q in _context.Questionnaire
                                         where q.QuestionnaireId == QuestionnaireId
                                         select new QuestionnaireViewModel
                                         {
                                             QuestionnaireId = q.QuestionnaireId,
                                             Name = q.Name
                                         }).FirstOrDefault(),
                    //Methodology_Deliverablesdata = (from m in _context.Methodology_Deliverables
                    //                                where m.ProjectId == Projectid && m.QuestionnaireId == QuestionnaireId && m.CustomerId == id
                    //                                select new Methodology_DeliverablesViewModel
                    //                                {
                    //                                    CustomerId = m.CustomerId,
                    //                                    QuestionnaireId = m.QuestionnaireId,
                    //                                    ScopeTypeMethodolgy = m.ScopeTypeMethodolgy,
                    //                                    ScopeTypeMethodolgy_FileName = m.ScopeTypeMethodolgy_FileName,
                    //                                    ScopeTypeSampleReport = m.ScopeTypeSampleReport,
                    //                                    ScopeTypeSampleReport_FileName = m.ScopeTypeSampleReport_FileName,
                    //                                    Methodology_DeliverablesId = m.Methodology_DeliverablesId,
                    //                                    ProjectId = m.ProjectId,
                    //                                }).FirstOrDefault(),
                    Questions = (from a in _context.QuestionResponse
                                 where a.CustomerId == id && a.ChildQuestionId == Guid.Empty && a.QuestionnaireId == QuestionnaireId && a.ProjectId == Projectid
                                 select new QuestionsViewModel
                                 {
                                     ResponseId = a.ResponseId,
                                     QuestionData = _context.Questions.Where(m => m.QuestionId == a.QuestionId).FirstOrDefault(),
                                     QuestionId = a.QuestionId,
                                     OptionType = a.OptionType,
                                     ResponseOptionId = a.OptionId,
                                     ResponseDateTime = a.OptionDate,
                                     Days = a.Days,
                                     Cost = a.Cost,
                                     Requirement = a.Requirement,
                                     Responsetext = a.Responsetext,
                                     OptionsList = (from b in _context.Options
                                                    where b.QuestionId == a.QuestionId
                                                    select new OptionsViewModel
                                                    {
                                                        OptionId = b.OptionId,
                                                        OptionText = b.OptionText,
                                                    }).ToList(),
                                     ChildQuestionsList = (from c in _context.QuestionResponse
                                                           where c.QuestionId == a.QuestionId && c.ChildQuestionId != Guid.Empty && a.QuestionnaireId == QuestionnaireId && c.ProjectId == Projectid
                                                           select new ChildQuestionsViewModel
                                                           {
                                                               ResponseId = c.ResponseId,
                                                               QuestionData = _context.ChildQuestions.Where(m => m.ChildQuestionId == c.ChildQuestionId).FirstOrDefault(),
                                                               ChildQuestionId = c.ChildQuestionId,
                                                               OptionType = c.OptionType,
                                                               ResponseOptionId = c.OptionId,
                                                               ResponseDateTime = c.OptionDate,
                                                               Days = c.Days,
                                                               Cost = c.Cost,
                                                               Requirement = c.Requirement,
                                                               Responsetext = c.Responsetext,
                                                               OptionsList = (from d in _context.Options
                                                                              where d.QuestionId == c.ChildQuestionId
                                                                              select new OptionsViewModel
                                                                              {
                                                                                  OptionId = d.OptionId,
                                                                                  OptionText = d.OptionText,
                                                                              }).ToList(),
                                                           }).OrderBy(m => m.QuestionData.QuestionNumber).ToList(),
                                 }).OrderBy(m => m.QuestionData.QuestionNumber).ToList()

                };
                return questionnaire;
            }
            catch (Exception)
            {
                throw;
            }
        }







        public QuestionnaireViewModel GetQuestionsByModule(Guid id)
        {
            try
            {
                QuestionsViewModel model = new QuestionsViewModel();

                var questionnaire = new QuestionnaireViewModel()
                {
                    QuestionnaireData = (from q in _context.Questionnaire
                                         where q.QuestionnaireId == id
                                         select new QuestionnaireViewModel
                                         {
                                             QuestionnaireId = q.QuestionnaireId,
                                             Name = q.Name
                                         }).FirstOrDefault(),
                    QuestionnaireId = id,
                    Questions = (from a in _context.Questions
                                 where a.QuestionnaireId == id
                                 select new QuestionsViewModel
                                 {
                                     Title = a.Title,
                                     SubTitle = a.SubTitle,
                                     QuestionId = a.QuestionId,
                                     SectionId = a.SectionId,
                                     QuestionNumber = a.QuestionNumber,
                                     QuestionType = a.questionType,
                                     QuestionText = a.QuestionText,
                                     RowNumber = a.RowNumber,
                                     QuestionnaireId = a.QuestionnaireId,
                                     InputType = a.InputType,
                                     isMandatory = a.isMandatory,
                                     Note = a.Note,
                                     Responsetext = a.QuestionValue,
                                     OptionsList = (from b in _context.Options
                                                    where b.QuestionId == a.QuestionId
                                                    select new OptionsViewModel
                                                    {
                                                        OptionId = b.OptionId,
                                                        OptionText = b.OptionText,
                                                        res = b.OptionValue,
                                                    }).ToList(),
                                     ChildQuestionsList = (from c in _context.ChildQuestions
                                                           where c.QuestionId == a.QuestionId
                                                           select new ChildQuestionsViewModel
                                                           {
                                                               ChildQuestionId = c.ChildQuestionId,
                                                               QuestionNumber = c.QuestionNumber,
                                                               SectionId = c.SectionId,
                                                               QuestionType = c.questionType,
                                                               QuestionText = c.QuestionText,
                                                               QuestionnaireId = c.QuestionnaireId,
                                                               Responsetext = c.QuestionValue,
                                                               isMandatory = c.isMandatory,
                                                               OptionsList = (from d in _context.Options
                                                                              where d.QuestionId == c.ChildQuestionId
                                                                              select new OptionsViewModel
                                                                              {
                                                                                  OptionId = d.OptionId,
                                                                                  OptionText = d.OptionText,
                                                                                  res = d.OptionValue,
                                                                              }).ToList(),
                                                           }).OrderBy(m => m.QuestionNumber).ToList(),
                                 }).OrderBy(m => m.RowNumber).ToList()

                };
                return questionnaire;
            }
            catch (Exception)
            {
                throw;
            }
        }





        public async Task<QuestionResponseViewModel> ResponseModuleWiseQuestions(QuestionnaireViewModel Questionnaire)
        {

            QuestionResponseViewModel model = new QuestionResponseViewModel();
            try
            {
                int serialNumber = 1;
                var customer = _context.Customer.Where(m => m.UserId == Questionnaire.UserId).FirstOrDefault();

                foreach (var item in Questionnaire.Questions)
                {
                    if (item.QuestionType == QuestionType.MultipleCheckType)
                    {
                        if (item.OptionsList != null)
                        {
                            foreach (var item2 in item.OptionsList)
                            {
                                QuestionResponse data1 = new QuestionResponse();
                                data1.IsChecked = item2.res;
                                data1.OptionId = item2.OptionId;
                                data1.CustomerId = customer.CustomerId;
                                data1.QuestionId = item.QuestionId;
                                data1.QuestionnaireId = Questionnaire.QuestionnaireId;
                                data1.OptionType = 4;
                                data1.ProjectId = Questionnaire.ProjectId;
                                _context.Add(data1);
                            }
                            serialNumber++;
                        }
                    }
                    else
                    {
                        model.SectionId = item.SectionId;
                        model.OptionId = item.ResponseOptionId;


                        QuestionResponse data = new QuestionResponse();
                        data.ResponseId = Guid.NewGuid();
                        data.CustomerId = customer.CustomerId;
                        data.QuestionId = item.QuestionId;
                        data.Responsetext = item.Responsetext;
                        data.QuestionnaireId = item.QuestionnaireId;
                        data.ProjectId = Questionnaire.ProjectId;
                        if (item.QuestionType == QuestionType.multipleResponseType)
                        {
                            data.OptionType = 0;
                        }
                        else if (item.QuestionType == QuestionType.DateTimeType)
                        {
                            data.OptionType = 1;
                        }
                        else if (item.QuestionType == QuestionType.TextType)
                        {
                            data.OptionType = 2;
                        }
                        else if (item.QuestionType == QuestionType.MultipleCheckType)
                        {


                        }
                        else if (item.QuestionType == QuestionType.Other)
                        {
                            data.OptionType = 5;

                        }

                        serialNumber++;
                        data.OptionId = item.ResponseOptionId;
                        data.OptionDate = item.ResponseDateTime;
                        _context.Add(data);
                    }




                    if (item.ChildQuestionsList != null)
                    {
                        foreach (var item1 in item.ChildQuestionsList)
                        {
                            if (item1.QuestionType == QuestionType.MultipleCheckType)
                            {
                                if (item1.OptionsList != null)
                                {
                                    foreach (var item2 in item1.OptionsList)
                                    {
                                        QuestionResponse data1 = new QuestionResponse();
                                        data1.IsChecked = item2.res;
                                        data1.OptionId = item2.OptionId;
                                        data1.CustomerId = customer.CustomerId;
                                        data1.QuestionId = item.QuestionId;
                                        data1.ChildQuestionId = item1.ChildQuestionId;
                                        data1.QuestionnaireId = Questionnaire.QuestionnaireId;
                                        data1.OptionType = 4;
                                        data1.ProjectId = Questionnaire.ProjectId;
                                        _context.Add(data1);
                                    }
                                    serialNumber++;
                                }
                            }
                            else
                            {
                                QuestionResponse data1 = new QuestionResponse();
                                data1.ResponseId = Guid.NewGuid();
                                data1.CustomerId = customer.CustomerId;
                                data1.QuestionId = item.QuestionId;
                                data1.ChildQuestionId = item1.ChildQuestionId;
                                data1.Responsetext = item1.Responsetext;
                                data1.QuestionnaireId = item1.QuestionnaireId;
                                data1.ProjectId = Questionnaire.ProjectId;
                                if (item1.QuestionType == QuestionType.multipleResponseType)
                                {
                                    data1.OptionType = 0;
                                }
                                else if (item1.QuestionType == QuestionType.DateTimeType)
                                {
                                    data1.OptionType = 1;
                                }
                                else if (item1.QuestionType == QuestionType.TextType)
                                {
                                    data1.OptionType = 2;
                                }
                                data1.OptionId = item1.ResponseOptionId;
                                data1.OptionDate = item1.ResponseDateTime;
                                _context.Add(data1);
                            }
                        }
                    }



                }

                var projectdata = _context.Projects.Find(Questionnaire.ProjectId);
                if (projectdata != null)
                {
                    projectdata.ProjectName = Questionnaire.ProjectName;
                    projectdata.ApplicationName = Questionnaire.ApplicationName;
                    projectdata.UpdatedDate = DateTime.Now;
                    _context.Update(projectdata);
                }

                //Methodology_Deliverables md = new Methodology_Deliverables();
                //md.Methodology_DeliverablesId = Guid.NewGuid();
                //md.CustomerId = customer.CustomerId;
                //md.CreatedBy = customer.CustomerId;
                //md.CreatedDate = DateTime.Now;
                //md.QuestionnaireId = Questionnaire.QuestionnaireId;
                //md.ProjectId = Questionnaire.ProjectId;
                //md.ScopeTypeMethodolgy = Questionnaire.ScopeTypeMethodolgy;
                //md.ScopeTypeMethodolgy_FileName = Questionnaire.ScopeTypeMethodolgy_FileName;
                //md.ScopeTypeSampleReport = Questionnaire.ScopeTypeSampleReport;
                //md.ScopeTypeSampleReport_FileName = Questionnaire.ScopeTypeSampleReport_FileName;
                //_context.Add(md);
                model.CustomerId = customer.CustomerId;
                model.QuestionnaireId = Questionnaire.QuestionnaireId;
                model.ProjectId = Questionnaire.ProjectId;
                var Res = await _context.SaveChangesAsync();
                if (Res > 0)
                {
                    var data1 = _context.Customer.Find(customer.CustomerId);
                    data1.Questionnaire = true;
                    data1.RepliedDate = DateTime.Now;
                    _context.Update(data1);
                    await _context.SaveChangesAsync();

                    CustomerQuestionnair data = new CustomerQuestionnair();
                    data.CustomerQuestionnairId = Guid.NewGuid();
                    data.CustomerId = customer.CustomerId;
                    data.CreatedBy = customer.CustomerId;
                    data.UpdatedDate = DateTime.Now;
                    data.CreatedDate = DateTime.Now;
                    data.QuestionnaireId = Questionnaire.QuestionnaireId;
                    data.IsQuestionnaire = true;
                    data.ProjectId = Questionnaire.ProjectId;
                    data.SectionId = model.SectionId;
                    _context.Add(data);
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception ex) { throw; }
            return model;

        }


        public QuestionnaireViewModel GetResponseQuestionsById1(Guid? id, Guid QuestionnaireId, Guid ProjectId)
        {
            try
            {
                QuestionsViewModel model = new QuestionsViewModel();

                var questionnaire = new QuestionnaireViewModel()
                {
                    //  SectionId = id,
                    QuestionnairDetailsViewdata = (from j in _context.QuestionnairDetails
                                                   where j.QuestionnaireId == QuestionnaireId && j.CustomerId == id
                                                   select new QuestionnairDetailsViewModel
                                                   {
                                                       QuestionnaireId = j.QuestionnaireId,
                                                       CustomerName = j.CustomerName,
                                                       ProjectName = j.ProjectName,
                                                       CustomerId = j.CustomerId,
                                                       ApplicationName = j.ApplicationName,
                                                       CreateDate = j.CreateDate,
                                                       QuestionnairDetailsId = j.QuestionnairDetailsId,
                                                   }).FirstOrDefault(),
                    //Methodology_Deliverablesdata = (from m in _context.Methodology_Deliverables
                    //                                where m.ProjectId == ProjectId && m.QuestionnaireId == QuestionnaireId && m.CustomerId == id
                    //                                select new Methodology_DeliverablesViewModel
                    //                                {
                    //                                    CustomerId = m.CustomerId,
                    //                                    QuestionnaireId = m.QuestionnaireId,
                    //                                    ScopeTypeMethodolgy = m.ScopeTypeMethodolgy,
                    //                                    ScopeTypeMethodolgy_FileName = m.ScopeTypeMethodolgy_FileName,
                    //                                    ScopeTypeSampleReport = m.ScopeTypeSampleReport,
                    //                                    ScopeTypeSampleReport_FileName = m.ScopeTypeSampleReport_FileName,
                    //                                    Methodology_DeliverablesId = m.Methodology_DeliverablesId,
                    //                                    ProjectId = m.ProjectId,                                                      
                    //                                }).FirstOrDefault(),

                    QuestionnaireData = (from q in _context.Questionnaire
                                         where q.QuestionnaireId == QuestionnaireId
                                         select new QuestionnaireViewModel
                                         {
                                             QuestionnaireId = q.QuestionnaireId,
                                             Name = q.Name
                                         }).FirstOrDefault(),

                    Questions = (from a in _context.QuestionResponse
                                 where a.CustomerId == id && a.ChildQuestionId == Guid.Empty && a.QuestionnaireId == QuestionnaireId && a.ProjectId == ProjectId
                                 select new QuestionsViewModel
                                 {
                                     ResponseId = a.ResponseId,
                                     QuestionData = _context.Questions.Where(m => m.QuestionId == a.QuestionId).FirstOrDefault(),
                                     QuestionId = a.QuestionId,
                                     OptionType = a.OptionType,
                                     ResponseOptionId = a.OptionId,
                                     ResponseDateTime = a.OptionDate,
                                     Responsetext = a.Responsetext,
                                     Chk = a.IsChecked,
                                     Title = model.Title,
                                     SubTitle = model.SubTitle,
                                     Cost = a.Cost,
                                     Days = a.Days,
                                     Requirement = a.Requirement,
                                     VariableName = a.VariableName,
                                     OptionsList = (from b in _context.Options
                                                    where b.QuestionId == a.QuestionId
                                                    select new OptionsViewModel
                                                    {

                                                        OptionId = b.OptionId,
                                                        OptionText = b.OptionText,
                                                    }).ToList(),
                                     ChildQuestionsList = (from c in _context.QuestionResponse
                                                           where c.QuestionId == a.QuestionId && c.ChildQuestionId != Guid.Empty && c.ProjectId == ProjectId
                                                           select new ChildQuestionsViewModel
                                                           {
                                                               ResponseId = c.ResponseId,
                                                               QuestionData = _context.ChildQuestions.Where(m => m.ChildQuestionId == c.ChildQuestionId).FirstOrDefault(),
                                                               ChildQuestionId = c.ChildQuestionId,
                                                               OptionType = c.OptionType,
                                                               ResponseOptionId = c.OptionId,
                                                               ResponseDateTime = c.OptionDate,
                                                               Responsetext = c.Responsetext,
                                                               Chk = c.IsChecked,
                                                               Cost = c.Cost,
                                                               Days = c.Days,
                                                               Requirement = c.Requirement,

                                                               OptionsList = (from d in _context.Options
                                                                              where d.QuestionId == c.ChildQuestionId
                                                                              select new OptionsViewModel
                                                                              {
                                                                                  OptionId = d.OptionId,
                                                                                  OptionText = d.OptionText,
                                                                              }).ToList(),
                                                           }).OrderBy(m => m.QuestionData.QuestionNumber).ToList(),
                                 }).OrderBy(m => m.QuestionData.RowNumber).ToList()





                };
                return questionnaire;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public QuestionnaireViewModel GetResponseQuestionsProjectById(Guid? id, Guid? Projectid, Guid QuestionnaireId)
        {
            try
            {
                QuestionsViewModel model = new QuestionsViewModel();

                var questionnaire = new QuestionnaireViewModel()
                {
                    //  SectionId = id,
                    ProjectsViewdata = (from j in _context.Projects
                                        where j.ProjectsId == Projectid
                                        select new ProjectsViewModel
                                        {
                                            ProjectsId = j.ProjectsId,
                                            CustomerName = j.CustomerName,
                                            ProjectName = j.ProjectName,
                                            CustomerId = j.CustomerId,
                                            ApplicationName = j.ApplicationName,
                                            CreateDate = j.CreateDate,
                                        }).FirstOrDefault(),
                    //Methodology_Deliverablesdata= (from m in _context.Methodology_Deliverables
                    //                               where m.ProjectId == Projectid && m.QuestionnaireId == QuestionnaireId&& m.CustomerId == id
                    //                               select new Methodology_DeliverablesViewModel
                    //                               {
                    //                                  CustomerId=m.CustomerId,
                    //                                  QuestionnaireId=m.QuestionnaireId,
                    //                                  ScopeTypeMethodolgy=m.ScopeTypeMethodolgy,
                    //                                  ScopeTypeMethodolgy_FileName=m.ScopeTypeMethodolgy_FileName,
                    //                                  ScopeTypeSampleReport=m.ScopeTypeSampleReport,
                    //                                  ScopeTypeSampleReport_FileName=m.ScopeTypeSampleReport_FileName,
                    //                                  Methodology_DeliverablesId=m.Methodology_DeliverablesId,
                    //                                  ProjectId=m.ProjectId
                    //                               }).FirstOrDefault(),

                    QuestionnaireData = (from q in _context.Questionnaire
                                         where q.QuestionnaireId == QuestionnaireId
                                         select new QuestionnaireViewModel
                                         {
                                             QuestionnaireId = q.QuestionnaireId,
                                             Name = q.Name
                                         }).FirstOrDefault(),

                    Questions = (from a in _context.QuestionResponse
                                 where a.CustomerId == id && a.ChildQuestionId == Guid.Empty && a.QuestionnaireId == QuestionnaireId && a.ProjectId == Projectid
                                 select new QuestionsViewModel
                                 {
                                     ResponseId = a.ResponseId,
                                     QuestionData = _context.Questions.Where(m => m.QuestionId == a.QuestionId).FirstOrDefault(),
                                     QuestionId = a.QuestionId,
                                     OptionType = a.OptionType,
                                     ResponseOptionId = a.OptionId,
                                     ResponseDateTime = a.OptionDate,
                                     Responsetext = a.Responsetext,
                                     Chk = a.IsChecked,
                                     Title = model.Title,
                                     SubTitle = model.SubTitle,
                                     Cost = a.Cost,
                                     Days = a.Days,
                                     Requirement = a.Requirement,
                                     VariableName = a.VariableName,
                                     OptionsList = (from b in _context.Options
                                                    where b.QuestionId == a.QuestionId
                                                    select new OptionsViewModel
                                                    {

                                                        OptionId = b.OptionId,
                                                        OptionText = b.OptionText,
                                                    }).ToList(),
                                     ChildQuestionsList = (from c in _context.QuestionResponse
                                                           where c.QuestionId == a.QuestionId && c.ChildQuestionId != Guid.Empty && c.ProjectId == Projectid
                                                           select new ChildQuestionsViewModel
                                                           {
                                                               ResponseId = c.ResponseId,
                                                               QuestionData = _context.ChildQuestions.Where(m => m.ChildQuestionId == c.ChildQuestionId).FirstOrDefault(),
                                                               ChildQuestionId = c.ChildQuestionId,
                                                               OptionType = c.OptionType,
                                                               ResponseOptionId = c.OptionId,
                                                               ResponseDateTime = c.OptionDate,
                                                               Responsetext = c.Responsetext,
                                                               Chk = c.IsChecked,
                                                               Cost = c.Cost,
                                                               Days = c.Days,
                                                               Requirement = c.Requirement,

                                                               OptionsList = (from d in _context.Options
                                                                              where d.QuestionId == c.ChildQuestionId
                                                                              select new OptionsViewModel
                                                                              {
                                                                                  OptionId = d.OptionId,
                                                                                  OptionText = d.OptionText,
                                                                              }).ToList(),
                                                           }).OrderBy(m => m.QuestionData.QuestionNumber).ToList(),
                                 }).OrderBy(m => m.QuestionData.RowNumber).ToList()





                };
                return questionnaire;
            }
            catch (Exception)
            {
                throw;
            }
        }






        public List<QuestionnaireViewModelAll> QuestionnaireAllList()
        {
            List<QuestionnaireViewModelAll> model = new List<QuestionnaireViewModelAll>();
            try
            {
                model = (from a in _context.Questionnaire
                         where a.IsDelete == false
                         select new QuestionnaireViewModelAll
                         {
                             QuestionnaireId = a.QuestionnaireId,
                             Name = a.Name,
                             IsHide=a.IsHide,
                             IsHideValue=a.IsHide.ToString()
                         }).OrderBy(m => m.Name).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public List<QuestionnaireReportDataModel> QuestionnaireAllListForReport()
        {
            List<QuestionnaireReportDataModel> model = new List<QuestionnaireReportDataModel>();
            try
            {
                model = (from z in _context.Questionnaire
                         select new QuestionnaireReportDataModel
                         {
                             QuestionnaireId = z.QuestionnaireId,
                             Name = z.Name,
                             Questions = (from a in _context.Questions
                                          where a.QuestionnaireId == z.QuestionnaireId
                                          select new QuestionsViewModel
                                          {
                                              Days = a.Days,
                                              Cost = a.Cost,
                                              QuestionId = a.QuestionId,
                                              QuestionnaireId = a.QuestionnaireId,
                                              ChildQuestionsList = (from c in _context.ChildQuestions
                                                                    where c.QuestionId == a.QuestionId
                                                                    select new ChildQuestionsViewModel
                                                                    {
                                                                        ChildQuestionId = c.ChildQuestionId,
                                                                        Days = c.Days,
                                                                        Cost = c.Cost,
                                                                        QuestionnaireId = c.QuestionnaireId,
                                                                    }).ToList(),
                                          }).ToList()
                         }).OrderBy(m => m.Name).ToList();

                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }





        public List<CustomerQuestionnairViewModel> CustomerQuestionnairList(Guid id)
        {
            List<CustomerQuestionnairViewModel> model = new List<CustomerQuestionnairViewModel>();
            try
            {
                // var customer = _context.Customer.Where(m => m.UserId == id.ToString()).FirstOrDefault();
                if (id != Guid.Empty)
                {

                    model = (from a in _context.CustomerQuestionnair
                             where a.CustomerId == id
                             select new CustomerQuestionnairViewModel
                             {
                                 QuestionnairDetailsViewdata = (from j in _context.QuestionnairDetails
                                                                where j.QuestionnaireId == a.QuestionnaireId && j.CustomerId == id
                                                                select new QuestionnairDetailsViewModel
                                                                {
                                                                    QuestionnaireId = j.QuestionnaireId,
                                                                    CustomerName = j.CustomerName,
                                                                    ProjectName = j.ProjectName,
                                                                    CustomerId = j.CustomerId,
                                                                    ApplicationName = j.ApplicationName,
                                                                    CreateDate = j.CreateDate,
                                                                    QuestionnairDetailsId = j.QuestionnairDetailsId,
                                                                }).FirstOrDefault(),
                                 QuestionnaireId = a.QuestionnaireId,
                                 CustomerQuestionnairId = a.CustomerQuestionnairId,
                                 CustomerId = a.CustomerId,
                                 CreatedBy = a.CreatedBy,
                                 CreatedDate = a.CreatedDate,
                                 IsQuestionnaire = a.IsQuestionnaire,
                                 UpdatedDate = a.UpdatedDate,
                                 ClientNmae = (from b in _context.Customer
                                               where b.CustomerId == a.CustomerId
                                               select new
                                               {
                                                   b.Client,
                                                   FirstName = "" + b.FirstName + " " + b.LastName + "",

                                               }).Single().Client,

                                 CustomerName = (from b in _context.Customer
                                                 where b.CustomerId == a.CustomerId
                                                 select new
                                                 {

                                                     FirstName = "" + b.FirstName + " " + b.LastName + "",

                                                 }).Single().FirstName,

                                 SalesNmae = (from z in _context.Customer
                                              where z.CustomerId == a.CustomerId
                                              from b in _context.Users
                                              where b.Id == z.CreatedBy.ToString()
                                              select new
                                              {
                                                  b.UserName,

                                              }).Single().UserName,
                                 Nmae = (from b in _context.Questionnaire
                                         where b.QuestionnaireId == a.QuestionnaireId
                                         select new
                                         {
                                             b.Name,
                                         }).Single().Name
                             }).ToList();
                }
                else
                {
                    model = (from a in _context.CustomerQuestionnair

                             select new CustomerQuestionnairViewModel
                             {
                                 QuestionnaireId = a.QuestionnaireId,
                                 CustomerQuestionnairId = a.CustomerQuestionnairId,
                                 CustomerId = a.CustomerId,
                                 CreatedBy = a.CreatedBy,
                                 CreatedDate = a.CreatedDate,
                                 IsQuestionnaire = a.IsQuestionnaire,
                                 UpdatedDate = a.UpdatedDate,
                                 ClientNmae = (from b in _context.Customer
                                               where b.CustomerId == a.CustomerId
                                               select new
                                               {
                                                   b.Client,
                                                   FirstName = "" + b.FirstName + " " + b.LastName + "",

                                               }).Single().Client,

                                 CustomerName = (from b in _context.Customer
                                                 where b.CustomerId == a.CustomerId
                                                 select new
                                                 {

                                                     FirstName = "" + b.FirstName + " " + b.LastName + "",

                                                 }).Single().FirstName,

                                 SalesNmae = (from z in _context.Customer
                                              where z.CustomerId == a.CustomerId
                                              from b in _context.Users
                                              where b.Id == z.CreatedBy.ToString()
                                              select new
                                              {
                                                  b.UserName,

                                              }).Single().UserName,
                                 Nmae = (from b in _context.Questionnaire
                                         where b.QuestionnaireId == a.QuestionnaireId
                                         select new
                                         {
                                             b.Name,
                                         }).Single().Name
                             }).ToList();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return model;
        }

        public async Task<CustomerProposal> CustomerReportDataSave(CustomerQuestionnairViewModel model)
        {
            var customer = _context.Customer.Where(m => m.CustomerId == model.CustomerId).FirstOrDefault();

            CustomerProposal cp = new CustomerProposal();
            cp.CustomerProposalId = Guid.NewGuid();
            cp.DocumentReference = model.ProposalViewModel.DocumentReference;
            cp.CreatedDate = DateTime.Now;
            cp.SalesId = customer.SalesId;
            cp.QuestionnaireId = model.QuestionnaireId;
            _context.Add(cp);
            await _context.SaveChangesAsync();
            return cp;
        }

        public CustomerProposalViewModel CustomerProposalAllData(Guid QuestionnaireId)
        {
            CustomerProposalViewModel model = new CustomerProposalViewModel();
            var customer = _context.CustomerProposal.Where(m => m.CustomerProposalId == QuestionnaireId).FirstOrDefault();
            model.CustomerProposalId = customer.CustomerProposalId;
            model.DocumentReference = customer.DocumentReference;
            model.CreatedDate = customer.CreatedDate;
            model.SalesId = customer.SalesId;
            model.QuestionnaireId = customer.QuestionnaireId;

            return model;
        }

        public CustomerViewModel CustomerAllData(Guid id)
        {
            CustomerViewModel model = new CustomerViewModel();
            var customer = _context.Customer.Where(m => m.CustomerId == id).FirstOrDefault();
            model = (from a in _context.Customer
                     where a.CustomerId == id
                     select new CustomerViewModel
                     {
                         CustomerId = a.CustomerId,
                         FirstName = a.FirstName,
                         LastName = a.LastName,
                         UserId = a.UserId,
                         Email = a.Email,
                         IsEmailSent = a.IsEmailSent,
                         Questionnaire = a.Questionnaire,
                         CreatedBy = a.CreatedBy,
                         RepliedDate = a.RepliedDate,
                         CurrentDateTime = DateTime.Now,
                         SendDate = a.SendDate,
                         Client = a.Client,
                         QuestionnaireId = a.QuestionnaireId,
                         SalesId = a.SalesId,
                         CreatedDate = a.CreatedDate,
                         UpdatedDate = a.UpdatedDate,
                         Address1 = a.Address1,
                         Address2 = a.Address2,
                         City = a.City,
                         State = a.State,
                         Country = a.Country,
                         MobileNumber = a.MobileNumber,
                         Zip = a.Zip,
                         UserName = (from b in _context.Users
                                     where b.Id == a.UserId
                                     select new
                                     {
                                         b.UserName,
                                     }).Single().UserName
                     }).FirstOrDefault();
            return model;
        }


        public SalesViewModel SalesAllData(Guid id)
        {
            SalesViewModel model = new SalesViewModel();
            var customer = _context.Customer.Where(m => m.CustomerId == id).FirstOrDefault();
            var User = _context.Users.Where(m => m.Id == customer.CreatedBy.ToString()).FirstOrDefault();
            var Sales = _context.Sales.Where(m => m.UserId == User.Id).FirstOrDefault();
            if (Sales != null)
            {
                model.Name = "" + Sales.FirstName + " " + Sales.LastName + "";
                model.FirstName = Sales.FirstName;
                model.LastName = Sales.LastName;
                model.Email = Sales.Email;
                model.UserId = Sales.UserId;
                model.MobileNumber = Sales.MobileNumber;
                model.CreatedDate = Sales.CreatedDate;
                model.IsEmailSent = Sales.IsEmailSent;
            }

            return model;
        }

        public QuestionnaireViewModel GetResponseQuestionsByProposalId(Guid id, Guid CustomerProposalId, Guid ProjectId)
        {

            var Customerproposal = _context.CustomerProposal.Where(m => m.CustomerProposalId == CustomerProposalId).FirstOrDefault();

            try
            {
                QuestionsViewModel model = new QuestionsViewModel();

                var questionnaire = new QuestionnaireViewModel()
                {
                    //  SectionId = id,
                    Questions = (from a in _context.QuestionResponse
                                 where a.CustomerId == id && a.ChildQuestionId == Guid.Empty && a.QuestionnaireId == Customerproposal.QuestionnaireId && a.ProjectId == ProjectId
                                 select new QuestionsViewModel
                                 {
                                     QuestionData = _context.Questions.Where(m => m.QuestionId == a.QuestionId).FirstOrDefault(),
                                     QuestionId = a.QuestionId,
                                     OptionType = a.OptionType,
                                     ResponseOptionId = a.OptionId,
                                     ResponseDateTime = a.OptionDate,
                                     Days = a.Days,
                                     Cost = a.Cost,
                                     Requirement = a.Requirement,
                                     OptionsList = (from b in _context.Options
                                                    where b.QuestionId == a.QuestionId
                                                    select new OptionsViewModel
                                                    {
                                                        OptionId = b.OptionId,
                                                        OptionText = b.OptionText,
                                                    }).ToList(),
                                     ChildQuestionsList = (from c in _context.QuestionResponse
                                                           where c.QuestionId == a.QuestionId && c.ChildQuestionId != Guid.Empty && a.QuestionnaireId == Customerproposal.QuestionnaireId && c.ProjectId == ProjectId
                                                           select new ChildQuestionsViewModel
                                                           {
                                                               QuestionData = _context.ChildQuestions.Where(m => m.ChildQuestionId == c.ChildQuestionId).FirstOrDefault(),
                                                               ChildQuestionId = c.ChildQuestionId,
                                                               OptionType = c.OptionType,
                                                               ResponseOptionId = c.OptionId,
                                                               ResponseDateTime = c.OptionDate,
                                                               Days = c.Days,
                                                               Cost = c.Cost,
                                                               Requirement = c.Requirement,
                                                               OptionsList = (from d in _context.Options
                                                                              where d.QuestionId == c.ChildQuestionId
                                                                              select new OptionsViewModel
                                                                              {
                                                                                  OptionId = d.OptionId,
                                                                                  OptionText = d.OptionText,
                                                                              }).ToList(),
                                                           }).OrderBy(m => m.QuestionData.QuestionNumber).ToList(),
                                 }).OrderBy(m => m.QuestionData.QuestionNumber).ToList()

                };
                return questionnaire;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<DbResponce> PCIQuestions(string rVSData, string ques2 = null, string ques3 = null, string ques4 = null, string ques5 = null, string UserId = null, string ques1 = null, string ProjectName = null, string CustomerName = null, string ProjectId = null, string Methodolgy = null, string Methodolgy_FileName = null, string SampleReport = null, string SampleReport_FileName = null)
        {
            DbResponce MSG = new DbResponce();
            try
            {

                List<QuestionResponseViewModel> Datal = new List<QuestionResponseViewModel>();
                Datal.Add(new QuestionResponseViewModel { QuestionId = new Guid("DD1327D2-6CC5-41D8-8C22-9C69FC582B4F") });
                Datal.Add(new QuestionResponseViewModel { QuestionId = new Guid("3819CE93-733B-4A37-B62F-7E34D787940F"), OptionId = new Guid(ques2) });
                Datal.Add(new QuestionResponseViewModel { QuestionId = new Guid("5064B1F5-8B79-4FA4-9446-367F7F308E45"), Responsetext = ques3 });
                Datal.Add(new QuestionResponseViewModel { QuestionId = new Guid("F56C6427-7004-4DC0-8C4A-0F6C4DB50930"), Responsetext = ques4 });
                Datal.Add(new QuestionResponseViewModel { QuestionId = new Guid("BA0BEFC0-5B93-4E22-B32B-8280F5C0DBA4"), Responsetext = ques5 });

                var customer = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();



                if (rVSData != null)
                {

                    var rv = rVSData.Split('|');
                    rv = rv.Skip(1).ToArray();

                    for (int i = 0; i < rv.Length; i += 4)
                    {
                        PCIQuestionnaire pc = new PCIQuestionnaire();
                        pc.PCIQuestionnaireId = Guid.NewGuid();
                        pc.IPAddress = rv[i];
                        pc.FQDN = rv[i + 1];
                        pc.Description = rv[i + 2];
                        pc.Ownership = rv[i + 3];
                        pc.CustomerId = customer.CustomerId;
                        pc.QuestionnaireId = new Guid("0A2DFC0B-1F17-447C-A7B8-067F9980325A");
                        pc.ProjectsId = new Guid(ProjectId);
                        _context.Add(pc);

                    }

                    foreach (var item1 in Datal)
                    {
                        QuestionResponse data1 = new QuestionResponse();
                        data1.ResponseId = Guid.NewGuid();
                        data1.CustomerId = customer.CustomerId;
                        data1.QuestionnaireId = new Guid("0A2DFC0B-1F17-447C-A7B8-067F9980325A");
                        data1.QuestionId = item1.QuestionId;
                        data1.OptionId = item1.OptionId;
                        data1.Responsetext = item1.Responsetext;
                        data1.ProjectId = new Guid(ProjectId);
                        if (item1.Responsetext == null)
                        {
                            data1.OptionType = 0;
                        }
                        else
                        {
                            data1.OptionType = 2;
                        }

                        _context.Add(data1);

                    }

                    //QuestionnairDetails qs = new QuestionnairDetails();
                    //qs.QuestionnairDetailsId = Guid.NewGuid();
                    //qs.CustomerName = CustomerName;
                    //qs.ProjectName = ProjectName;
                    //qs.CustomerId = customer.CustomerId;
                    //qs.CreateDate = DateTime.Now;
                    //qs.QuestionnaireId = new Guid("0A2DFC0B-1F17-447C-A7B8-067F9980325A");
                    //_context.Add(qs);


                    var projectdata = _context.Projects.Find(new Guid(ProjectId));
                    if (projectdata != null)
                    {
                        //  projectdata.ProjectName = ProjectName;
                        projectdata.CustomerName = CustomerName;
                        projectdata.UpdatedDate = DateTime.Now;
                        _context.Update(projectdata);
                    }

                    Methodology_Deliverables md = new Methodology_Deliverables();
                    md.Methodology_DeliverablesId = Guid.NewGuid();
                    md.CustomerId = customer.CustomerId;
                    md.CreatedBy = customer.CustomerId;
                    md.CreatedDate = DateTime.Now;
                    md.QuestionnaireId = new Guid("0A2DFC0B-1F17-447C-A7B8-067F9980325A");
                    md.ProjectId = new Guid(ProjectId);
                    md.ScopeTypeMethodolgy = Methodolgy;
                    md.ScopeTypeMethodolgy_FileName = Methodolgy_FileName;
                    md.ScopeTypeSampleReport = SampleReport;
                    md.ScopeTypeSampleReport_FileName = SampleReport_FileName;
                    _context.Add(md);

                    var res = await _context.SaveChangesAsync();
                    if (res > 0)
                    {
                        var data2 = _context.Customer.Find(customer.CustomerId);
                        data2.Questionnaire = true;
                        data2.RepliedDate = DateTime.Now;
                        _context.Update(data2);
                        await _context.SaveChangesAsync();

                        CustomerQuestionnair data3 = new CustomerQuestionnair();
                        data3.CustomerQuestionnairId = Guid.NewGuid();
                        data3.CustomerId = customer.CustomerId;
                        data3.CreatedBy = customer.CustomerId;
                        data3.UpdatedDate = DateTime.Now;
                        data3.CreatedDate = DateTime.Now;
                        data3.QuestionnaireId = new Guid("0A2DFC0B-1F17-447C-A7B8-067F9980325A");
                        data3.IsQuestionnaire = true;
                        data3.ProjectId = new Guid(ProjectId);
                        _context.Add(data3);
                        await _context.SaveChangesAsync();
                        MSG.msg = "Success";
                    }
                }
                return MSG;
            }
            catch (Exception ex) { throw; }


        }



        public QuestionnaireViewModel GetResponseQuestionsById3(Guid? id, Guid QuestionnaireId, Guid ProjectId)
        {
            try
            {
                QuestionsViewModel model = new QuestionsViewModel();

                var questionnaire = new QuestionnaireViewModel()
                {
                    //  SectionId = id,
                    QuestionnairDetailsViewdata = (from j in _context.QuestionnairDetails
                                                   where j.QuestionnaireId == QuestionnaireId && j.CustomerId == id
                                                   select new QuestionnairDetailsViewModel
                                                   {
                                                       QuestionnaireId = j.QuestionnaireId,
                                                       CustomerName = j.CustomerName,
                                                       ProjectName = j.ProjectName,
                                                       CustomerId = j.CustomerId,
                                                       ApplicationName = j.ApplicationName,
                                                       CreateDate = j.CreateDate,
                                                       QuestionnairDetailsId = j.QuestionnairDetailsId,
                                                   }).FirstOrDefault(),
                    //Methodology_Deliverablesdata = (from m in _context.Methodology_Deliverables
                    //                                where m.ProjectId == ProjectId && m.QuestionnaireId == QuestionnaireId && m.CustomerId == id
                    //                                select new Methodology_DeliverablesViewModel
                    //                                {
                    //                                    CustomerId = m.CustomerId,
                    //                                    QuestionnaireId = m.QuestionnaireId,
                    //                                    ScopeTypeMethodolgy = m.ScopeTypeMethodolgy,
                    //                                    ScopeTypeMethodolgy_FileName = m.ScopeTypeMethodolgy_FileName,
                    //                                    ScopeTypeSampleReport = m.ScopeTypeSampleReport,
                    //                                    ScopeTypeSampleReport_FileName = m.ScopeTypeSampleReport_FileName,
                    //                                    Methodology_DeliverablesId = m.Methodology_DeliverablesId,
                    //                                    ProjectId = m.ProjectId
                    //                                }).FirstOrDefault(),
                    QuestionnaireData = (from q in _context.Questionnaire
                                         where q.QuestionnaireId == QuestionnaireId
                                         select new QuestionnaireViewModel
                                         {
                                             QuestionnaireId = q.QuestionnaireId,
                                             Name = q.Name
                                         }).FirstOrDefault(),
                    Questions = (from a in _context.QuestionResponse
                                 where a.CustomerId == id && a.ChildQuestionId == Guid.Empty && a.QuestionnaireId == QuestionnaireId && a.ProjectId == ProjectId
                                 select new QuestionsViewModel
                                 {
                                     ResponseId = a.ResponseId,
                                     QuestionData = _context.Questions.Where(m => m.QuestionId == a.QuestionId).FirstOrDefault(),
                                     QuestionId = a.QuestionId,
                                     OptionType = a.OptionType,
                                     ResponseOptionId = a.OptionId,
                                     ResponseDateTime = a.OptionDate,
                                     Responsetext = a.Responsetext,
                                     Chk = a.IsChecked,
                                     Cost = a.Cost,
                                     Days = a.Days,
                                     VariableName = a.VariableName,
                                     Requirement = a.Requirement,
                                     Title = model.Title,
                                     SubTitle = model.SubTitle,
                                     OptionsList = (from b in _context.Options
                                                    where b.QuestionId == a.QuestionId
                                                    select new OptionsViewModel
                                                    {

                                                        OptionId = b.OptionId,
                                                        OptionText = b.OptionText,
                                                    }).ToList(),

                                 }).OrderBy(m => m.QuestionData.RowNumber).ToList(),
                    PCIQuestionnaireList = (from b in _context.PCIQuestionnaire
                                            where b.QuestionnaireId == QuestionnaireId && b.CustomerId == id && b.ProjectsId == ProjectId
                                            select new PCIQuestionnaireViewModel
                                            {

                                                IPAddress = b.IPAddress,
                                                Ownership = b.Ownership,
                                                QuestionnaireId = b.QuestionnaireId,
                                                PCIQuestionnaireId = b.PCIQuestionnaireId,
                                                Description = b.Description,
                                                FQDN = b.FQDN,
                                                Date = b.Date,
                                                CustomerId = b.CustomerId
                                            }).ToList(),
                };
                return questionnaire;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public QuestionnaireViewModel GetResponseQuestionsByProjectId(Guid? id, Guid Projectid, Guid QuestionnaireId)
        {
            try
            {
                QuestionsViewModel model = new QuestionsViewModel();

                var questionnaire = new QuestionnaireViewModel()
                {
                    //  SectionId = id,
                    ProjectsViewdata = (from j in _context.Projects
                                        where j.ProjectsId == Projectid
                                        select new ProjectsViewModel
                                        {
                                            ProjectsId = j.ProjectsId,
                                            CustomerName = j.CustomerName,
                                            ProjectName = j.ProjectName,
                                            CustomerId = j.CustomerId,
                                            ApplicationName = j.ApplicationName,
                                            CreateDate = j.CreateDate,
                                        }).FirstOrDefault(),
                    QuestionnaireData = (from q in _context.Questionnaire
                                         where q.QuestionnaireId == QuestionnaireId
                                         select new QuestionnaireViewModel
                                         {
                                             QuestionnaireId = q.QuestionnaireId,
                                             Name = q.Name
                                         }).FirstOrDefault(),
                    //Methodology_Deliverablesdata = (from m in _context.Methodology_Deliverables
                    //                                where m.ProjectId == Projectid && m.QuestionnaireId == QuestionnaireId && m.CustomerId == id
                    //                                select new Methodology_DeliverablesViewModel
                    //                                {
                    //                                    CustomerId = m.CustomerId,
                    //                                    QuestionnaireId = m.QuestionnaireId,
                    //                                    ScopeTypeMethodolgy = m.ScopeTypeMethodolgy,
                    //                                    ScopeTypeMethodolgy_FileName = m.ScopeTypeMethodolgy_FileName,
                    //                                    ScopeTypeSampleReport = m.ScopeTypeSampleReport,
                    //                                    ScopeTypeSampleReport_FileName = m.ScopeTypeSampleReport_FileName,
                    //                                    Methodology_DeliverablesId = m.Methodology_DeliverablesId,
                    //                                    ProjectId = m.ProjectId
                    //                                }).FirstOrDefault(),
                    Questions = (from a in _context.QuestionResponse
                                 where a.CustomerId == id && a.ChildQuestionId == Guid.Empty && a.QuestionnaireId == QuestionnaireId && a.ProjectId == Projectid
                                 select new QuestionsViewModel
                                 {
                                     ResponseId = a.ResponseId,
                                     QuestionData = _context.Questions.Where(m => m.QuestionId == a.QuestionId).FirstOrDefault(),
                                     QuestionId = a.QuestionId,
                                     OptionType = a.OptionType,
                                     ResponseOptionId = a.OptionId,
                                     ResponseDateTime = a.OptionDate,
                                     Responsetext = a.Responsetext,
                                     Chk = a.IsChecked,
                                     Cost = a.Cost,
                                     Days = a.Days,
                                     VariableName = a.VariableName,
                                     Requirement = a.Requirement,
                                     Title = model.Title,
                                     SubTitle = model.SubTitle,
                                     OptionsList = (from b in _context.Options
                                                    where b.QuestionId == a.QuestionId
                                                    select new OptionsViewModel
                                                    {

                                                        OptionId = b.OptionId,
                                                        OptionText = b.OptionText,
                                                    }).ToList(),

                                 }).OrderBy(m => m.QuestionData.RowNumber).ToList(),
                    PCIQuestionnaireList = (from b in _context.PCIQuestionnaire
                                            where b.QuestionnaireId == QuestionnaireId && b.CustomerId == id && b.ProjectsId == Projectid
                                            select new PCIQuestionnaireViewModel
                                            {

                                                IPAddress = b.IPAddress,
                                                Ownership = b.Ownership,
                                                QuestionnaireId = b.QuestionnaireId,
                                                PCIQuestionnaireId = b.PCIQuestionnaireId,
                                                Description = b.Description,
                                                FQDN = b.FQDN,
                                                Date = b.Date,
                                                CustomerId = b.CustomerId
                                            }).ToList(),
                };
                return questionnaire;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public List<CustomerViewModel> CustomerViewList()
        {
            List<CustomerViewModel> CustomerList = new List<CustomerViewModel>();

            CustomerList = (from b in _context.Customer
                            select new CustomerViewModel
                            {
                                FirstName = "" + b.FirstName + " " + b.LastName + "",
                                CustomerId = b.CustomerId,
                            }).ToList();

            return CustomerList;
        }

        public async Task<QuestionResponseViewModel> UpdateResponseModuleWiseQuestions(QuestionnaireViewModel Questionnaire)
        {

            QuestionResponseViewModel model = new QuestionResponseViewModel();
            try
            {

                foreach (var item in Questionnaire.Questions)
                {
                    if (item.ChildQuestionsList != null)
                    {
                        foreach (var item1 in item.ChildQuestionsList)
                        {
                            if (item1.OptionType == 4)
                            {
                                var data1 = _context.QuestionResponse.Find(item1.ResponseId);
                                data1.IsChecked = item1.Chk;
                                _context.Update(data1);
                            }
                            else
                            {
                                var data1 = _context.QuestionResponse.Find(item1.ResponseId);
                                data1.Responsetext = item1.Responsetext;
                                data1.OptionId = item1.ResponseOptionId;
                                data1.OptionDate = item1.ResponseDateTime;
                                _context.Update(data1);
                            }



                        }
                    }

                    if (item.OptionType == 4)
                    {
                        var data1 = _context.QuestionResponse.Find(item.ResponseId);

                        data1.IsChecked = item.Chk;
                        _context.Update(data1);
                    }
                    else
                    {
                        model.SectionId = item.SectionId;
                        model.OptionId = item.ResponseOptionId;
                        model.QuestionnaireId = Questionnaire.QuestionnaireId;

                        var data = _context.QuestionResponse.Find(item.ResponseId);
                        data.Responsetext = item.Responsetext;
                        data.OptionId = item.ResponseOptionId;
                        data.OptionDate = item.ResponseDateTime;
                        _context.Update(data);
                    }


                }
                //if (Questionnaire.Methodology_Deliverablesdata != null)
                //{
                //    var mddata = _context.Methodology_Deliverables.Find(Questionnaire.Methodology_Deliverablesdata.Methodology_DeliverablesId);
                //    mddata.ScopeTypeMethodolgy = Questionnaire.Methodology_Deliverablesdata.ScopeTypeMethodolgy;
                //    mddata.ScopeTypeMethodolgy_FileName = Questionnaire.Methodology_Deliverablesdata.ScopeTypeMethodolgy_FileName;
                //    mddata.ScopeTypeSampleReport = Questionnaire.Methodology_Deliverablesdata.ScopeTypeSampleReport;
                //    mddata.ScopeTypeSampleReport_FileName = Questionnaire.Methodology_Deliverablesdata.ScopeTypeSampleReport_FileName;
                //    mddata.UpdatedDate = DateTime.Now;
                //    _context.Update(mddata);
                //}

                model.CustomerId = Questionnaire.CustomerId;
                model.ProjectId = Questionnaire.ProjectId;
                model.QuestionnaireId = Questionnaire.QuestionnaireId;

                var Res = await _context.SaveChangesAsync();

            }
            catch (Exception ex) { throw; }
            return model;

        }



        public async Task<QuestionResponseViewModel> ChildQuestionUpdate(QuestionnaireViewModel Questionnaire)
        {

            QuestionResponseViewModel model = new QuestionResponseViewModel();
            try
            {
                var customer = _context.Customer.Where(m => m.UserId == Questionnaire.UserId).FirstOrDefault();
                if (customer == null)
                {
                    customer = _context.Customer.Where(m => m.CustomerId == Questionnaire.CustomerId).FirstOrDefault();
                }

                foreach (var item in Questionnaire.Questions)
                {
                    if (item.ChildQuestionsList != null)
                    {
                        foreach (var item1 in item.ChildQuestionsList)
                        {
                            var data1 = _context.QuestionResponse.Find(item1.ResponseId);

                            data1.OptionId = item1.ResponseOptionId;
                            data1.OptionDate = item1.ResponseDateTime;
                            _context.Update(data1);

                        }
                    }

                    var data = _context.QuestionResponse.Find(item.ResponseId);

                    data.OptionId = item.ResponseOptionId;
                    data.OptionDate = item.ResponseDateTime;
                    _context.Update(data);

                }
                //if (Questionnaire.Methodology_Deliverablesdata != null)
                //{
                //    var mddata = _context.Methodology_Deliverables.Find(Questionnaire.Methodology_Deliverablesdata.Methodology_DeliverablesId);
                //    mddata.ScopeTypeMethodolgy = Questionnaire.Methodology_Deliverablesdata.ScopeTypeMethodolgy;
                //    mddata.ScopeTypeMethodolgy_FileName = Questionnaire.Methodology_Deliverablesdata.ScopeTypeMethodolgy_FileName;
                //    mddata.ScopeTypeSampleReport = Questionnaire.Methodology_Deliverablesdata.ScopeTypeSampleReport;
                //    mddata.ScopeTypeSampleReport_FileName = Questionnaire.Methodology_Deliverablesdata.ScopeTypeSampleReport_FileName;
                //    mddata.UpdatedDate = DateTime.Now;
                //    _context.Update(mddata);
                //}



                var Res = await _context.SaveChangesAsync();
                if (Res > 0)
                {


                }
            }
            catch (Exception ex) { throw; }
            return model;

        }

        public async Task<int> DeletePCITableQuest(Guid? id)
        {
            try
            {
                _context.PCIQuestionnaire.Remove(await GetRowById(id));
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<PCIQuestionnaire> GetRowById(Guid? id)
        {
            try
            {
                return await _context.PCIQuestionnaire
                       .FirstOrDefaultAsync(m => m.PCIQuestionnaireId == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DbResponce> UpdateResponseModuleWiseQuestionsPCI(string rVSData, string ques2 = null, string ques3 = null, string ques4 = null, string ques5 = null, string UserId = null, string ques1 = null, string res1 = null, string res2 = null, string res3 = null, string res4 = null, string res5 = null, string CustomerId = null, string ProjectId = null, string Methodolgy = null, string Methodolgy_FileName = null, string SampleReport = null, string SampleReport_FileName = null)
        {
            DbResponce MSG = new DbResponce();
            try
            {


                List<QuestionResponseViewModel> Datal = new List<QuestionResponseViewModel>();
                Datal.Add(new QuestionResponseViewModel { QuestionId = new Guid("DD1327D2-6CC5-41D8-8C22-9C69FC582B4F"), ResponseId = new Guid(res1) });
                Datal.Add(new QuestionResponseViewModel { QuestionId = new Guid("3819CE93-733B-4A37-B62F-7E34D787940F"), OptionId = new Guid(ques2), ResponseId = new Guid(res2) });
                Datal.Add(new QuestionResponseViewModel { QuestionId = new Guid("5064B1F5-8B79-4FA4-9446-367F7F308E45"), Responsetext = ques3, ResponseId = new Guid(res3) });
                Datal.Add(new QuestionResponseViewModel { QuestionId = new Guid("F56C6427-7004-4DC0-8C4A-0F6C4DB50930"), Responsetext = ques4, ResponseId = new Guid(res4) });
                Datal.Add(new QuestionResponseViewModel { QuestionId = new Guid("BA0BEFC0-5B93-4E22-B32B-8280F5C0DBA4"), Responsetext = ques5, ResponseId = new Guid(res5) });

                var customer = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();
                if (customer == null)
                {
                    customer = _context.Customer.Where(m => m.CustomerId == new Guid(CustomerId)).FirstOrDefault();
                }
                if (rVSData != null)
                {

                    var rv = rVSData.Split('|');
                    rv = rv.Skip(1).ToArray();

                    for (int i = 0; i < rv.Length; i += 5)
                    {
                        if (rv[i] != "")
                        {
                            var pc1 = _context.PCIQuestionnaire.Find(new Guid(rv[i]));
                            pc1.PCIQuestionnaireId = new Guid(rv[i]);
                            pc1.IPAddress = rv[i + 1];
                            pc1.FQDN = rv[i + 2];
                            pc1.Description = rv[i + 3];
                            pc1.Ownership = rv[i + 4];
                            _context.Update(pc1);

                        }
                        else
                        {
                            PCIQuestionnaire pc = new PCIQuestionnaire();
                            pc.PCIQuestionnaireId = Guid.NewGuid();
                            pc.IPAddress = rv[i + 1];
                            pc.FQDN = rv[i + 2];
                            pc.Description = rv[i + 3];
                            pc.Ownership = rv[i + 4];
                            pc.CustomerId = customer.CustomerId;
                            pc.QuestionnaireId = new Guid("0A2DFC0B-1F17-447C-A7B8-067F9980325A");
                            pc.ProjectsId = new Guid(ProjectId);
                            _context.Add(pc);
                        }
                    }

                    foreach (var item1 in Datal)
                    {

                        var data1 = _context.QuestionResponse.Find(item1.ResponseId);

                        data1.OptionId = item1.OptionId;
                        data1.Responsetext = item1.Responsetext;
                        _context.Update(data1);
                    }
                    //var Methodology_Deliverablesdata = _context.Methodology_Deliverables.Where(m => m.QuestionnaireId == new Guid("0A2DFC0B-1F17-447C-A7B8-067F9980325A")&&m.ProjectId == new Guid(ProjectId)&& m.CustomerId== customer.CustomerId).FirstOrDefault();
                    //if (Methodology_Deliverablesdata != null)
                    //{
                    //    var mddata = _context.Methodology_Deliverables.Find(Methodology_Deliverablesdata.Methodology_DeliverablesId);
                    //    mddata.ScopeTypeMethodolgy = Methodolgy;
                    //    mddata.ScopeTypeMethodolgy_FileName = Methodolgy_FileName;
                    //    mddata.ScopeTypeSampleReport = SampleReport;
                    //    mddata.ScopeTypeSampleReport_FileName = SampleReport_FileName;
                    //    mddata.UpdatedDate = DateTime.Now;
                    //    _context.Update(mddata);
                    //}



                    var res = await _context.SaveChangesAsync();

                }
                return MSG;
            }

            catch (Exception ex) { throw; }


        }


        public List<IndustrySectorListModel> IndustrySectorListData()
        {

            List<IndustrySectorListModel> IndustrySectorListModeldata = new List<IndustrySectorListModel>();

            IndustrySectorListModeldata = (from c in _context.CyberEssentialsIndustrySectorList
                                           select new IndustrySectorListModel
                                           {
                                               Id = c.Id,
                                               ItemName = c.ItemName
                                           }).ToList();

            return IndustrySectorListModeldata;

        }

        public List<CyberEssentialsQuestionnaireSectionsViewModel> CEQSectionList()
        {
            List<CyberEssentialsQuestionnaireSectionsViewModel> Modeldata = new List<CyberEssentialsQuestionnaireSectionsViewModel>();

            Modeldata = (from c in _context.CyberEssentialsQuestionnaireSections
                         select new CyberEssentialsQuestionnaireSectionsViewModel
                         {
                             Id = c.Id,
                             Name = c.Name
                         }).ToList();

            return Modeldata;
        }

        public async Task<int> saveOrganisationDetails(CEQOrgDetailsViewModel CEQOrgDetailsView)
        {
            var customer = _context.Customer.Where(m => m.UserId == CEQOrgDetailsView.UserId).FirstOrDefault();
            if (customer == null)
            {
                customer = _context.Customer.Where(m => m.CustomerId == CEQOrgDetailsView.CustomerId).FirstOrDefault();
            }
            var resdata = _context.CEQOrgDetails.Where(m => m.QuestionnaireId == new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323") && m.CustomerId == customer.CustomerId && m.ProjectId == CEQOrgDetailsView.ProjectId).FirstOrDefault();
            if (resdata != null)
            {
                var employee = _context.CEQOrgDetails.Find(resdata.CEQOrgDetailsId);
                _context.CEQOrgDetails.Remove(employee);
                _context.SaveChanges();

            }
            CEQOrgDetails co = new CEQOrgDetails();
            co.CEQOrgDetailsId = Guid.NewGuid();
            co.Nameoforganisation = CEQOrgDetailsView.Nameoforganisation;
            co.RegisteredAddress = CEQOrgDetailsView.RegisteredAddress;
            co.CompanyCharityNumber = CEQOrgDetailsView.CompanyCharityNumber;
            co.Turnover = CEQOrgDetailsView.Turnover;
            co.Nameofmaincontact = CEQOrgDetailsView.Nameofmaincontact;
            co.ContactJobtitle = CEQOrgDetailsView.ContactJobtitle;
            co.ContactEmail = CEQOrgDetailsView.ContactEmail;
            co.ContactTelephone = CEQOrgDetailsView.ContactTelephone;
            co.CELevelDesired = CEQOrgDetailsView.CELevelDesired;
            co.Sector = CEQOrgDetailsView.Sector;
            co.Dateofresponse = CEQOrgDetailsView.Dateofresponse;
            co.NumberofEmployees = CEQOrgDetailsView.NumberofEmployees;
            co.CustomerId = customer.CustomerId;
            co.CanCRESTpubliciseyoursuccessfulcertification = CEQOrgDetailsView.CanCRESTpubliciseyoursuccessfulcertification;
            co.QuestionnaireId = new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323");
            co.ProjectId = CEQOrgDetailsView.ProjectId;
            _context.Add(co);
            return await _context.SaveChangesAsync();
        }

        public List<CEQQuestionsViewModel> CEQQuestionsList()
        {
            List<CEQQuestionsViewModel> CEQQuestionsViewList = new List<CEQQuestionsViewModel>();

            CEQQuestionsViewList = (from b in _context.CEQQuestions
                                    select new CEQQuestionsViewModel
                                    {
                                        CEQQuestionId = b.CEQQuestionId,
                                        QuestionnaireId = b.QuestionnaireId,
                                        QuestionNumber = b.QuestionNumber,
                                        QuestionText = b.QuestionText,
                                        QuestionType = b.QuestionType,
                                        RowNumber = b.RowNumber,
                                        Title = b.Title,
                                        Section = b.Section,
                                        VariableName = b.VariableName,
                                        Cost = b.Cost,
                                        Days = b.Days,
                                        Requirement = b.Requirement,
                                        StandardRecommendation = b.StandardRecommendation,
                                        CEQOptionsData = (from a in _context.CEQOptions
                                                          where a.QuestionId == b.CEQQuestionId
                                                          select new CEQOptionsViewModel
                                                          {
                                                              CEQOptionId = a.CEQOptionId,
                                                              OptionText = a.OptionText,

                                                          }).ToList(),
                                    }).OrderBy(m => m.RowNumber).ToList();

            return CEQQuestionsViewList;
        }


        public async Task<int> saveSignatureDetails(CEQSignatureViewModel CEQSignatureView)
        {
            var customer = _context.Customer.Where(m => m.UserId == CEQSignatureView.UserId).FirstOrDefault();
            if (customer == null)
            {
                customer = _context.Customer.Where(m => m.CustomerId == CEQSignatureView.CustomerId).FirstOrDefault();
            }
            var resdata = _context.CEQSignature.Where(m => m.QuestionnaireId == new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323") && m.CustomerId == customer.CustomerId && m.ProjectId == CEQSignatureView.ProjectId).FirstOrDefault();
            if (resdata != null)
            {
                var employee = _context.CEQSignature.Find(resdata.CEQSignatureId);
                _context.CEQSignature.Remove(employee);
                _context.SaveChanges();

            }
            CEQSignature co = new CEQSignature();
            co.CEQSignatureId = Guid.NewGuid();
            co.Nameoforganisation = CEQSignatureView.Nameoforganisation;
            co.FileName = CEQSignatureView.FileName;
            co.SignatureDate = CEQSignatureView.SignatureDate;
            co.CustomerId = customer.CustomerId;
            co.JobTitle = CEQSignatureView.JobTitle;
            co.Name = CEQSignatureView.Name;
            co.Nameofbusiness = CEQSignatureView.Nameofbusiness;
            co.CustomerId = customer.CustomerId;
            co.QuestionnaireId = new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323");
            co.ProjectId = CEQSignatureView.ProjectId;
            _context.Add(co);
            return await _context.SaveChangesAsync();
        }
        //CEQ -WAT
        public async Task<DbResponce> SaveWAT(string WATabledata, string UserId = null, string ProjectId = null, string orgcustId = null)
        {
            DbResponce MSG = new DbResponce();
            try
            {


                List<QuestionResponseViewModel> Datal = new List<QuestionResponseViewModel>();

                var customer = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();
                if (customer == null)
                {
                    customer = _context.Customer.Where(m => m.CustomerId == new Guid(orgcustId)).FirstOrDefault();
                }
                var resdata = _context.CEQWorkstationAssessment.Where(m => m.QuestionnaireId == new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323") && m.CustomerId == customer.CustomerId && m.ProjectId == new Guid(ProjectId)).ToList();
                if (resdata.Count != 0)
                {
                    foreach (var Rsdata in resdata)
                    {
                        var employee = _context.CEQWorkstationAssessment.Find(Rsdata.CEQWorkstationAssessmentId);
                        _context.CEQWorkstationAssessment.Remove(employee);
                        _context.SaveChanges();
                    }
                }
                if (WATabledata != null)
                {

                    var rv = WATabledata.Split('|');
                    rv = rv.Skip(1).ToArray();

                    for (int i = 0; i < rv.Length; i += 6)
                    {
                        CEQWorkstationAssessment ceqwa = new CEQWorkstationAssessment();
                        ceqwa.CEQWorkstationAssessmentId = Guid.NewGuid();
                        ceqwa.Descriptionofthedevice = rv[i];
                        ceqwa.OperatingSystem = rv[i + 1];
                        ceqwa.Usernameandpassword = rv[i + 2];
                        ceqwa.Confirmationthatthedevice = rv[i + 3];
                        ceqwa.Confirmationthatadminaccess = rv[i + 4];
                        ceqwa.TestLocation = rv[i + 5];
                        ceqwa.CustomerId = customer.CustomerId;
                        ceqwa.QuestionnaireId = new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323");
                        ceqwa.ProjectId = new Guid(ProjectId);
                        _context.Add(ceqwa);

                    }


                    var res = await _context.SaveChangesAsync();
                    if (res > 0)
                    {

                    }
                }
                return MSG;
            }
            catch (Exception ex) { throw; }


        }
        public async Task<DbResponce> SaveCloud(string CloudTabledata, string UserId = null, string ProjectId = null, string orgcustId = null)
        {
            DbResponce MSG = new DbResponce();
            try
            {

                List<QuestionResponseViewModel> Datal = new List<QuestionResponseViewModel>();
                var customer = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();
                if (customer == null)
                {
                    customer = _context.Customer.Where(m => m.CustomerId == new Guid(orgcustId)).FirstOrDefault();
                }
                var resdata = _context.CEQCloudSharedServicesAsses.Where(m => m.QuestionnaireId == new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323") && m.CustomerId == customer.CustomerId && m.ProjectId == new Guid(ProjectId)).ToList();
                if (resdata.Count != 0)
                {
                    foreach (var Rsdata in resdata)
                    {
                        var employee = _context.CEQCloudSharedServicesAsses.Find(Rsdata.CEQCloudSharedServicesAssesId);
                        _context.CEQCloudSharedServicesAsses.Remove(employee);
                        _context.SaveChanges();
                    }
                }

                if (CloudTabledata != null)
                {

                    var rv = CloudTabledata.Split('|');
                    rv = rv.Skip(1).ToArray();

                    for (int i = 0; i < rv.Length; i += 4)
                    {
                        CEQCloudSharedServicesAsses ceqcld = new CEQCloudSharedServicesAsses();
                        ceqcld.CEQCloudSharedServicesAssesId = Guid.NewGuid();
                        ceqcld.DescriptionOfService = rv[i];
                        ceqcld.Supplier = rv[i + 1];
                        ceqcld.IndependentAuditStandards = rv[i + 2];
                        ceqcld.EvidenceOfcertification = rv[i + 3];
                        ceqcld.CustomerId = customer.CustomerId;
                        ceqcld.QuestionnaireId = new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323");
                        ceqcld.ProjectId = new Guid(ProjectId);
                        _context.Add(ceqcld);

                    }


                    var res = await _context.SaveChangesAsync();
                    if (res > 0)
                    {

                    }
                }
                return MSG;
            }
            catch (Exception ex) { throw; }


        }

        public async Task<DbResponce> SaveRVSQuestions(string rVSData, string UserId = null, string[] options = null, string ProjectId = null, string orgcustId = null)
        {
            DbResponce MSG = new DbResponce();
            try
            {

                var customer = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();
                if (customer == null)
                {
                    customer = _context.Customer.Where(m => m.CustomerId == new Guid(orgcustId)).FirstOrDefault();
                }

                var resdata = _context.CEQRemoteVulnScan.Where(m => m.QuestionnaireId == new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323") && m.CustomerId == customer.CustomerId && m.ProjectId == new Guid(ProjectId)).ToList();
                if (resdata.Count != 0)
                {
                    foreach (var Rsdata in resdata)
                    {
                        var employee = _context.CEQRemoteVulnScan.Find(Rsdata.CEQRemoteVulnScanId);
                        _context.CEQRemoteVulnScan.Remove(employee);
                        _context.SaveChanges();
                    }
                }


                //
                if (rVSData != null)
                {

                    var rv = rVSData.Split('|');
                    //var selectedItem = options.Split(',');
                    rv = rv.Skip(1).ToArray();
                    int j = 0;

                    for (int i = 0; i < rv.Length; i += 4)
                    {
                        CEQRemoteVulnScan rvs = new CEQRemoteVulnScan();
                        rvs.CEQRemoteVulnScanId = Guid.NewGuid();
                        rvs.IPAddressv4andv6addresses = rv[i];
                        rvs.FullyQualifiedDomainName = rv[i + 1];
                        rvs.NatureandDescriptionofSystem = rv[i + 2];
                        rvs.SystemOwnershipandHosting = options[j];
                        rvs.Ifoutofscopepleaseciteareasonwhy = rv[i + 3];
                        rvs.CustomerId = customer.CustomerId;
                        rvs.QuestionnaireId = new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323");
                        rvs.ProjectId = new Guid(ProjectId);
                        _context.Add(rvs);
                        j++;
                    }


                    var res = await _context.SaveChangesAsync();
                    if (res > 0)
                    {

                    }
                }
                return MSG;
            }
            catch (Exception ex) { throw; }


        }


        public async Task<DbResponce> saveQuestionnaire(List<TestData> optionValue = null, string UserId = null, string ProjectId = null, string orgcustId = null)
        {
            DbResponce MSG = new DbResponce();
            try
            {

                var customer = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();
                if (customer == null)
                {
                    customer = _context.Customer.Where(m => m.CustomerId == new Guid(orgcustId)).FirstOrDefault();
                }

                var resdata = _context.CEQQuestionResponse.Where(m => m.QuestionnaireId == new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323") && m.CustomerId == customer.CustomerId && m.ProjectId == new Guid(ProjectId)).ToList();
                if (resdata.Count != 0)
                {
                    foreach (var Rsdata in resdata)
                    {
                        var employee = _context.CEQQuestionResponse.Find(Rsdata.CEQResponseId);
                        _context.CEQQuestionResponse.Remove(employee);
                        _context.SaveChanges();
                    }
                }


                if (optionValue != null)
                {
                    foreach (var item in optionValue)
                    {
                        var data1 = _context.CEQQuestions.Where(m => m.CEQQuestionId == new Guid(item.QuestionId)).FirstOrDefault();

                        CEQQuestionResponse rvs = new CEQQuestionResponse();
                        rvs.CEQResponseId = Guid.NewGuid();
                        rvs.QuestionId = new Guid(item.QuestionId);
                        rvs.Weighting = data1.Weighting;
                        if (item.Value == null)
                        {
                            rvs.OptionId = Guid.Empty;
                        }
                        else
                        {
                            var data = _context.CEQOptions.Where(m => m.CEQOptionId == new Guid(item.Value)).FirstOrDefault();
                            rvs.OptionId = new Guid(item.Value);
                            rvs.Response = data.Response;
                        }

                        rvs.CustomerId = customer.CustomerId;
                        rvs.QuestionnaireId = new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323");
                        rvs.ProjectId = new Guid(ProjectId);
                        _context.Add(rvs);
                    }

                    var res = await _context.SaveChangesAsync();
                    if (res > 0)
                    {

                    }
                }
                return MSG;
            }
            catch (Exception ex) { throw; }


        }

        public CEQOrgDetailsViewModel organisationData(Guid id, string UserId, Guid ProjectId)
        {
            CEQOrgDetailsViewModel model = new CEQOrgDetailsViewModel();

            var customer = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();
            if (customer == null)
            {
                customer = _context.Customer.Where(m => m.CustomerId == new Guid(UserId)).FirstOrDefault();
            }
            model = (from a in _context.CEQOrgDetails
                     where a.QuestionnaireId == id && a.CustomerId == customer.CustomerId && a.ProjectId == ProjectId
                     select new CEQOrgDetailsViewModel
                     {
                         QuestionnaireId = a.QuestionnaireId,
                         CanCRESTpubliciseyoursuccessfulcertification = a.CanCRESTpubliciseyoursuccessfulcertification,
                         CELevelDesired = a.CELevelDesired,
                         CEQOrgDetailsId = a.CEQOrgDetailsId,
                         CustomerId = a.CustomerId,
                         CompanyCharityNumber = a.CompanyCharityNumber,
                         ContactEmail = a.ContactEmail,
                         ContactJobtitle = a.ContactJobtitle,
                         ContactTelephone = a.ContactTelephone,
                         Cost = a.Cost,
                         Dateofresponse = a.Dateofresponse,
                         Days = a.Days,
                         Nameofmaincontact = a.Nameofmaincontact,
                         Nameoforganisation = a.Nameoforganisation,
                         NumberofEmployees = a.NumberofEmployees,
                         RegisteredAddress = a.RegisteredAddress,
                         Requirement = a.Requirement,
                         Sector = a.Sector,
                         Turnover = a.Turnover,
                     }).FirstOrDefault();
            return model;
        }


        public CEQSignatureViewModel cEQSignatureData(Guid id, string UserId, Guid ProjectId)
        {
            CEQSignatureViewModel model = new CEQSignatureViewModel();
            var customer = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();
            if (customer == null)
            {
                customer = _context.Customer.Where(m => m.CustomerId == new Guid(UserId)).FirstOrDefault();
            }
            model = (from a in _context.CEQSignature
                     where a.QuestionnaireId == id && a.CustomerId == customer.CustomerId && a.ProjectId == ProjectId
                     select new CEQSignatureViewModel
                     {
                         QuestionnaireId = a.QuestionnaireId,
                         CEQSignatureId = a.CEQSignatureId,
                         CustomerId = a.CustomerId,
                         FileName = a.FileName,
                         Cost = a.Cost,
                         JobTitle = a.JobTitle,
                         Days = a.Days,
                         Name = a.Name,
                         Nameoforganisation = a.Nameoforganisation,
                         Nameofbusiness = a.Nameofbusiness,
                         Requirement = a.Requirement,
                         SignatureDate = a.SignatureDate,

                     }).FirstOrDefault();
            return model;
        }
        public List<CEQCloudSharedServicesAssesViewModel> cEQCloudSharedServicesAssesList(Guid id, string UserId, Guid ProjectId)
        {
            List<CEQCloudSharedServicesAssesViewModel> model = new List<CEQCloudSharedServicesAssesViewModel>();
            var customer = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();
            if (customer == null)
            {
                customer = _context.Customer.Where(m => m.CustomerId == new Guid(UserId)).FirstOrDefault();
            }
            model = (from a in _context.CEQCloudSharedServicesAsses
                     where a.QuestionnaireId == id && a.CustomerId == customer.CustomerId && a.ProjectId == ProjectId
                     select new CEQCloudSharedServicesAssesViewModel
                     {
                         QuestionnaireId = a.QuestionnaireId,
                         CustomerId = a.CustomerId,
                         Cost = a.Cost,
                         Days = a.Days,
                         Requirement = a.Requirement,
                         CEQCloudSharedServicesAssesId = a.CEQCloudSharedServicesAssesId,
                         DescriptionOfService = a.DescriptionOfService,
                         EvidenceOfcertification = a.EvidenceOfcertification,
                         IndependentAuditStandards = a.IndependentAuditStandards,
                         Supplier = a.Supplier
                     }).ToList();
            return model;
        }



        public List<CEQQuestionResponseViewModel> cEQQuestionResponseViewModelList(Guid id, string UserId, Guid ProjectId)
        {
            List<CEQQuestionResponseViewModel> model = new List<CEQQuestionResponseViewModel>();
            var customer = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();
            if (customer == null)
            {
                customer = _context.Customer.Where(m => m.CustomerId == new Guid(UserId)).FirstOrDefault();
            }
            model = (from a in _context.CEQQuestionResponse
                     where a.QuestionnaireId == id && a.CustomerId == customer.CustomerId && a.ProjectId == ProjectId
                     select new CEQQuestionResponseViewModel
                     {
                         QuestionData = _context.CEQQuestions.Where(m => m.CEQQuestionId == a.QuestionId).FirstOrDefault(),
                         QuestionnaireId = a.QuestionnaireId,
                         CustomerId = a.CustomerId,
                         Cost = a.Cost,
                         Days = a.Days,
                         Requirement = a.Requirement,
                         CEQResponseId = a.CEQResponseId,
                         OptionDate = a.OptionDate,
                         OptionId = a.OptionId,
                         OptionType = a.OptionType,
                         QuestionId = a.QuestionId
                     }).ToList();

            return model;
        }


        public List<CEQRemoteVulnScanViewModel> RvsData(Guid id, string UserId, Guid ProjectId)
        {
            List<CEQRemoteVulnScanViewModel> model = new List<CEQRemoteVulnScanViewModel>();
            var customer = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();
            if (customer == null)
            {
                customer = _context.Customer.Where(m => m.CustomerId == new Guid(UserId)).FirstOrDefault();
            }
            model = (from a in _context.CEQRemoteVulnScan
                     where a.QuestionnaireId == id && a.CustomerId == customer.CustomerId && a.ProjectId == ProjectId
                     select new CEQRemoteVulnScanViewModel
                     {
                         QuestionnaireId = a.QuestionnaireId,
                         IPAddressv4andv6addresses = a.IPAddressv4andv6addresses,
                         FullyQualifiedDomainName = a.FullyQualifiedDomainName,
                         NatureandDescriptionofSystem = a.NatureandDescriptionofSystem,
                         CustomerId = a.CustomerId,
                         SystemOwnershipandHosting = a.SystemOwnershipandHosting,
                         Ifoutofscopepleaseciteareasonwhy = a.Ifoutofscopepleaseciteareasonwhy,
                         Cost = a.Cost,
                         Days = a.Days,
                         Requirement = a.Requirement,

                     }).ToList();
            return model;
        }
        public List<CEQWorkAssessViewModel> WaAssesData(Guid id, string UserId, Guid ProjectId)
        {
            List<CEQWorkAssessViewModel> model = new List<CEQWorkAssessViewModel>();
            var customer = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();
            if (customer == null)
            {
                customer = _context.Customer.Where(m => m.CustomerId == new Guid(UserId)).FirstOrDefault();
            }
            model = (from a in _context.CEQWorkstationAssessment
                     where a.QuestionnaireId == id && a.CustomerId == customer.CustomerId && a.ProjectId == ProjectId
                     select new CEQWorkAssessViewModel
                     {
                         QuestionnaireId = a.QuestionnaireId,
                         CustomerId = a.CustomerId,
                         Descriptionofthedevice = a.Descriptionofthedevice,
                         OperatingSystem = a.OperatingSystem,
                         Usernameandpassword = a.Usernameandpassword,
                         Confirmationthatthedevice = a.Confirmationthatthedevice,
                         Confirmationthatadminaccess = a.Confirmationthatadminaccess,
                         TestLocation = a.TestLocation,
                         Cost = a.Cost,
                         Days = a.Days,
                         Requirement = a.Requirement,

                     }).ToList();
            return model;
        }


        public List<CEQQuestionsViewModel> CEQQuestionsList2(Guid id, string UserId, Guid ProjectId)
        {
            List<CEQQuestionsViewModel> CEQQuestionsViewList = new List<CEQQuestionsViewModel>();
            var customer = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();
            if (customer == null)
            {
                customer = _context.Customer.Where(m => m.CustomerId == new Guid(UserId)).FirstOrDefault();
            }
            CEQQuestionsViewList = (from b in _context.CEQQuestions
                                    select new CEQQuestionsViewModel
                                    {
                                        CEQOResponceData = _context.CEQQuestionResponse.Where(m => m.QuestionId == b.CEQQuestionId && m.QuestionnaireId == b.QuestionnaireId && m.CustomerId == customer.CustomerId && m.ProjectId == ProjectId).FirstOrDefault(),
                                        CEQQuestionId = b.CEQQuestionId,
                                        QuestionnaireId = b.QuestionnaireId,
                                        QuestionNumber = b.QuestionNumber,
                                        QuestionText = b.QuestionText,
                                        QuestionType = b.QuestionType,
                                        RowNumber = b.RowNumber,
                                        Title = b.Title,
                                        Section = b.Section,
                                        Cost = b.Cost,
                                        Days = b.Days,
                                        Requirement = b.Requirement,
                                        VariableName = b.VariableName,
                                        StandardRecommendation = b.StandardRecommendation,
                                        CEQOptionsData = (from a in _context.CEQOptions
                                                          where a.QuestionId == b.CEQQuestionId
                                                          select new CEQOptionsViewModel
                                                          {
                                                              CEQOptionId = a.CEQOptionId,
                                                              OptionText = a.OptionText,
                                                              Response = a.Response
                                                          }).ToList(),
                                    }).OrderBy(m => m.RowNumber).ToList();

            return CEQQuestionsViewList;
        }


        public async Task<int> ECQFinalSubmit(Guid Questionnaire, Guid ProjectId, string UserId = null)
        {
            try
            {
                var customer = _context.Customer.Where(m => m.UserId == UserId).FirstOrDefault();
                if (customer != null)
                {
                    CustomerQuestionnair data = new CustomerQuestionnair();
                    data.CustomerQuestionnairId = Guid.NewGuid();
                    data.CustomerId = customer.CustomerId;
                    data.CreatedBy = customer.CustomerId;
                    data.UpdatedDate = DateTime.Now;
                    data.CreatedDate = DateTime.Now;
                    data.QuestionnaireId = Questionnaire;
                    data.IsQuestionnaire = true;
                    data.ProjectId = ProjectId;
                    _context.Add(data);
                }

            }
            catch (Exception ex)
            {

            }

            return await _context.SaveChangesAsync();
        }


        public QuestionnaireViewModel GetResponseQuestionsById4(Guid? id, Guid QuestionnaireId)
        {
            try
            {
                CEQQuestionsViewModel model = new CEQQuestionsViewModel();

                var questionnaire = new QuestionnaireViewModel()
                {
                    //  SectionId = id,
                    QuestionnaireData = (from q in _context.Questionnaire
                                         where q.QuestionnaireId == QuestionnaireId
                                         select new QuestionnaireViewModel
                                         {
                                             QuestionnaireId = q.QuestionnaireId,
                                             Name = q.Name
                                         }).FirstOrDefault(),
                    //CEQQuestions = (from a in _context.CEQQuestions
                    //             where  a.QuestionnaireId == QuestionnaireId
                    //             select new CEQQuestions
                    //             {
                    //                // CEQResponseId = a.CEQResponseId,
                    //                 QuestionData = _context.CEQQuestions.Where(m => m.CEQQuestionId == a.CEQQuestionId).FirstOrDefault(),
                    //                 CEQQuestionId = a.CEQQuestionId,
                    //                 OptionType = a.OptionType,
                    //                 ResponseOptionId = a.OptionId,
                    //                 ResponseDateTime = a.OptionDate,
                    //                 Responsetext = a.Responsetext,
                    //                 Chk = a.IsChecked,
                    //                 Cost = a.Cost,
                    //                 Days = a.Days,
                    //                 Requirement = a.Requirement,
                    //                 Title = model.Title,
                    //                 SubTitle = model.SubTitle,
                    //                 OptionsList = (from b in _context.Options
                    //                                where b.QuestionId == a.QuestionId
                    //                                select new OptionsViewModel
                    //                                {

                    //                                    OptionId = b.OptionId,
                    //                                    OptionText = b.OptionText,
                    //                                }).ToList(),

                    //             }).OrderBy(m => m.QuestionData.RowNumber).ToList(),
                    PCIQuestionnaireList = (from b in _context.PCIQuestionnaire
                                            where b.QuestionnaireId == QuestionnaireId && b.CustomerId == id
                                            select new PCIQuestionnaireViewModel
                                            {

                                                IPAddress = b.IPAddress,
                                                Ownership = b.Ownership,
                                                QuestionnaireId = b.QuestionnaireId,
                                                PCIQuestionnaireId = b.PCIQuestionnaireId,
                                                Description = b.Description,
                                                FQDN = b.FQDN,
                                                Date = b.Date,
                                                CustomerId = b.CustomerId
                                            }).ToList(),
                };
                return questionnaire;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> CyberEssentialsQuestionnaireUpdate(Guid Questionnaire, Guid UserId)
        {
            var data = _context.Customer.Find(UserId);
            data.UpdatedDate = DateTime.Now;
            _context.Update(data);
            return await _context.SaveChangesAsync();
        }
        public List<CEQQuestionResponseResult> cEQQuestionResponseResult(CyberEssentialsQuestionnaireModel model)
        {
            List<CyberEssentialsQuestionnaireSectionsViewModel> Modeldata = new List<CyberEssentialsQuestionnaireSectionsViewModel>();
            CEQQuestionResponseResult newdata1 = new CEQQuestionResponseResult();
            List<CEQQuestionResponseResult> newdata = new List<CEQQuestionResponseResult>();

            Modeldata = (from c in _context.CyberEssentialsQuestionnaireSections
                         select new CyberEssentialsQuestionnaireSectionsViewModel
                         {
                             Id = c.Id,
                             Name = c.Name
                         }).ToList();
            var z = 0;
            var x = 0;
            foreach (var a in Modeldata)
            {
                newdata1.sectionid = a.Id;
                newdata1.Name = a.Name;
                var QuestionList = model.CEQQuestionsViewModelList.Where(m => m.Section == a.Id).ToList();
                foreach (var item in QuestionList)
                {
                    if (item.CEQOResponceData.OptionId == new Guid("71E3188B-69B6-4970-BB21-EA1589BE6806"))
                    {
                        x = 1;
                    }
                    else if (item.CEQOResponceData.OptionId == new Guid("474E7C9B-D4E4-4A63-A459-523D36CBC3D6"))
                    {
                        x = 2;
                    }
                    else if (item.CEQOResponceData.OptionId == new Guid("C9456D84-080B-4C48-96DC-28F8F001D227"))
                    {
                        x = 3;
                    }
                    if (x == 1 && a.Id == 4)
                    {
                        if (item.QuestionNumber == "37" || item.QuestionNumber == "38" || item.QuestionNumber == "39" || item.QuestionNumber == "40")
                        {
                            newdata1.Responsedata += Convert.ToDecimal(item.CEQOResponceData.Response);
                            newdata1.Weightingdata += Convert.ToDecimal(item.CEQOResponceData.Weighting);
                        }
                    }
                    else if (x == 2 && a.Id == 4)
                    {
                        if (item.QuestionNumber == "41" || item.QuestionNumber == "42" || item.QuestionNumber == "43")
                        {
                            newdata1.Responsedata += Convert.ToDecimal(item.CEQOResponceData.Response);
                            newdata1.Weightingdata += Convert.ToDecimal(item.CEQOResponceData.Weighting);
                        }
                    }
                    else if (x == 3 && a.Id == 4)
                    {
                        if (item.QuestionNumber == "44")
                        {
                            newdata1.Responsedata += Convert.ToDecimal(item.CEQOResponceData.Response);
                            newdata1.Weightingdata += Convert.ToDecimal(item.CEQOResponceData.Weighting);
                        }

                    }
                    else
                    {
                        newdata1.Responsedata += Convert.ToDecimal(item.CEQOResponceData.Response);
                        newdata1.Weightingdata += Convert.ToDecimal(item.CEQOResponceData.Weighting);
                    }




                }
                newdata.Add(newdata1);
                newdata1 = new CEQQuestionResponseResult();

            }



            return newdata;
        }


        public QuestionnaireViewModel GetQuestionsById(Guid QuestionnaireId)
        {
            try
            {
                QuestionsViewModel model = new QuestionsViewModel();

                var questionnaire = new QuestionnaireViewModel()
                {
                    MethodologyeList = (from j in _context.Methodologye
                                        where j.Type == "Methodologies"
                                        select new MethodologyeViewModel
                                        {
                                            FileName = j.FileName,
                                            Type = j.Type,
                                            UpdatedDate = j.UpdatedDate,
                                            Id = j.Id
                                        }).ToList(),
                    SampleReportList = (from j in _context.Methodologye
                                        where j.Type == "SampleReport"
                                        select new MethodologyeViewModel
                                        {
                                            FileName = j.FileName,
                                            Type = j.Type,
                                            UpdatedDate = j.UpdatedDate,
                                            Id = j.Id,
                                        }).ToList(),
                    EquestionViewModelData = (from e in _context.Equestion
                                              where e.QuestionId == QuestionnaireId
                                              select new EquestionViewModel
                                              {
                                                  CostEquestion = e.CostEquestion,
                                                  DaysEquestion = e.DaysEquestion,
                                                  RequirmentEquestion = e.RequirmentEquestion
                                              }).FirstOrDefault(),

                    //  SectionId = id,
                    QuestionnaireData = (from q in _context.Questionnaire
                                         where q.QuestionnaireId == QuestionnaireId
                                         select new QuestionnaireViewModel
                                         {
                                             QuestionnaireId = q.QuestionnaireId,
                                             Name = q.Name
                                         }).FirstOrDefault(),
                    Methodology_Deliverablesdata = (from m in _context.Admin_Methodology_Deliverables
                                                    where m.QuestionnaireId == QuestionnaireId
                                                    select new Methodology_DeliverablesViewModel
                                                    {
                                                        QuestionnaireId = m.QuestionnaireId,
                                                        ScopeTypeMethodolgy = m.ScopeTypeMethodolgy,
                                                        ScopeTypeMethodolgy_FileName = (from b in _context.Methodologye
                                                                                        where b.Id == m.MethodologyId
                                                                                        select new
                                                                                        {
                                                                                            b.FileName,
                                                                                        }).Single().FileName,
                                                        ScopeTypeSampleReport = m.ScopeTypeSampleReport,
                                                        ScopeTypeSampleReport_FileName = (from b in _context.Methodologye
                                                                                          where b.Id == m.MethodologyReportId
                                                                                          select new
                                                                                          {
                                                                                              b.FileName,
                                                                                          }).Single().FileName,
                                                        Methodology_DeliverablesId = m.Methodology_DeliverablesId,
                                                        MethodologyId = m.MethodologyId,
                                                        MethodologyReportId = m.MethodologyReportId
                                                    }).FirstOrDefault(),

                    Questions = (from a in _context.Questions
                                 where a.QuestionnaireId == QuestionnaireId
                                 select new QuestionsViewModel
                                 {
                                     QuestionId = a.QuestionId,
                                     OptionType = Convert.ToInt32(a.questionType),
                                     Title = a.Title,
                                     SubTitle = a.SubTitle,
                                     Cost = a.Cost,
                                     Days = a.Days,
                                     Requirement = a.Requirement,
                                     VariableName = a.VariableName,
                                     QuestionNumber = a.QuestionNumber,
                                     RowNumber = a.RowNumber,
                                     QuestionText = a.QuestionText,
                                     QuestionType = a.questionType,
                                     QuestionValue = a.QuestionValue,
                                     isMandatory = a.isMandatory,
                                     OptionsList = (from b in _context.Options
                                                    where b.QuestionId == a.QuestionId
                                                    select new OptionsViewModel
                                                    {

                                                        OptionId = b.OptionId,
                                                        OptionText = b.OptionText,
                                                        OptionValue = b.OptionValue,
                                                    }).ToList(),
                                     ChildQuestionsList = (from c in _context.ChildQuestions
                                                           where c.QuestionId == a.QuestionId
                                                           select new ChildQuestionsViewModel
                                                           {

                                                               ChildQuestionId = c.ChildQuestionId,
                                                               OptionType = Convert.ToInt32(c.questionType),
                                                               Cost = c.Cost,
                                                               Days = c.Days,
                                                               Requirement = c.Requirement,
                                                               VariableName = c.VariableName,
                                                               QuestionNumber = c.QuestionNumber,
                                                               QuestionText = c.QuestionText,
                                                               QuestionType = c.questionType,
                                                               QuestionValue = c.QuestionValue,
                                                               isMandatory = c.isMandatory,
                                                               OptionsList = (from d in _context.Options
                                                                              where d.QuestionId == c.ChildQuestionId
                                                                              select new OptionsViewModel
                                                                              {
                                                                                  OptionId = d.OptionId,
                                                                                  OptionText = d.OptionText,
                                                                                  OptionValue = d.OptionValue,
                                                                              }).ToList(),
                                                           }).OrderBy(m => m.QuestionNumber).ToList(),
                                 }).OrderBy(m => m.RowNumber).ToList()

                };
                return questionnaire;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<QuestionResponseViewModel> UpdateAllQuestions(QuestionnaireViewModel Questionnaire)
        {

            QuestionResponseViewModel model = new QuestionResponseViewModel();
            try
            {

                foreach (var item in Questionnaire.Questions)
                {
                    if (item.ChildQuestionsList != null)
                    {
                        foreach (var item1 in item.ChildQuestionsList)
                        {
                            var data1 = _context.ChildQuestions.Find(item1.ChildQuestionId);
                            data1.Cost = item1.Cost;
                            data1.Days = item1.Days;
                            data1.Requirement = item1.Requirement;
                            if (data1.questionType == QuestionType.TextType)
                            {
                                data1.QuestionValue = item1.QuestionValue;
                            }
                            else if (data1.questionType == QuestionType.DateTimeType)
                            {
                                data1.QuestionValue = item1.QuestionValue;
                            }
                            _context.Update(data1);

                            if (data1.questionType == QuestionType.MultipleCheckType || data1.questionType == QuestionType.multipleResponseType)
                            {
                                if (item1.OptionsList.Count != 0)
                                {
                                    foreach (var Optionitem in item1.OptionsList)
                                    {
                                        var Optiondata = _context.Options.Find(Optionitem.OptionId);
                                        Optiondata.OptionValue = Optionitem.OptionValue;
                                        _context.Update(Optiondata);
                                    }
                                }

                            }

                        }
                    }
                    model.SectionId = item.SectionId;
                    model.OptionId = item.ResponseOptionId;
                    model.QuestionnaireId = Questionnaire.QuestionnaireId;

                    var data = _context.Questions.Find(item.QuestionId);
                    data.Cost = item.Cost;
                    data.Days = item.Days;
                    data.Requirement = item.Requirement;
                    if (data.questionType == QuestionType.TextType)
                    {
                        data.QuestionValue = item.QuestionValue;
                    }
                    _context.Update(data);

                    if (data.questionType == QuestionType.MultipleCheckType)
                    {
                        if (item.OptionsList.Count != 0)
                        {
                            foreach (var Optionitem in item.OptionsList)
                            {
                                var Optiondata = _context.Options.Find(Optionitem.OptionId);
                                Optiondata.OptionValue = Optionitem.OptionValue;
                                _context.Update(Optiondata);
                            }
                        }
                    }

                }
                if (Questionnaire.Methodology_Deliverablesdata != null)
                {
                    var Methodology_DeliverablesData = _context.Admin_Methodology_Deliverables.Where(m => m.Methodology_DeliverablesId == Questionnaire.Methodology_Deliverablesdata.Methodology_DeliverablesId).FirstOrDefault();
                    var Methodology_Data = _context.Methodologye.Where(m => m.Id == Questionnaire.Methodology_Deliverablesdata.MethodologyId).FirstOrDefault();
                    var Methodology_ReportData = _context.Methodologye.Where(m => m.Id == Questionnaire.Methodology_Deliverablesdata.MethodologyReportId).FirstOrDefault();

                    if (Methodology_DeliverablesData != null)
                    {
                        var mddata = _context.Admin_Methodology_Deliverables.Find(Questionnaire.Methodology_Deliverablesdata.Methodology_DeliverablesId);
                        mddata.ScopeTypeMethodolgy = Questionnaire.Methodology_Deliverablesdata.ScopeTypeMethodolgy;

                        mddata.ScopeTypeMethodolgy_FileName = Methodology_Data.FileName;
                        mddata.ScopeTypeSampleReport = Questionnaire.Methodology_Deliverablesdata.ScopeTypeSampleReport;
                        mddata.ScopeTypeSampleReport_FileName = Methodology_ReportData.FileName;
                        mddata.UpdatedDate = DateTime.Now;
                        mddata.MethodologyId = Questionnaire.Methodology_Deliverablesdata.MethodologyId;
                        mddata.MethodologyReportId = Questionnaire.Methodology_Deliverablesdata.MethodologyReportId;
                        _context.Update(mddata);
                    }
                    else
                    {

                        Admin_Methodology_Deliverables ad = new Admin_Methodology_Deliverables();
                        ad.Methodology_DeliverablesId = Guid.NewGuid();
                        ad.ScopeTypeMethodolgy = Questionnaire.Methodology_Deliverablesdata.ScopeTypeMethodolgy;
                        ad.ScopeTypeMethodolgy_FileName = Methodology_Data.FileName;
                        ad.ScopeTypeSampleReport = Questionnaire.Methodology_Deliverablesdata.ScopeTypeSampleReport;
                        ad.ScopeTypeSampleReport_FileName = Methodology_ReportData.FileName;
                        ad.UpdatedDate = DateTime.Now;
                        ad.QuestionnaireId = Questionnaire.QuestionnaireId;
                        ad.CreatedDate = DateTime.Now;
                        ad.MethodologyId = Questionnaire.Methodology_Deliverablesdata.MethodologyId;
                        ad.MethodologyReportId = Questionnaire.Methodology_Deliverablesdata.MethodologyReportId;
                        _context.Add(ad);

                    }

                }

                var Res = await _context.SaveChangesAsync();

            }
            catch (Exception ex) { throw; }
            return model;

        }



        public async Task<DbResponce> saveQuestionnaire1(List<TestData1> optionValue = null)
        {
            DbResponce MSG = new DbResponce();
            try
            {

                //  var resdata = _context.CEQQuestionResponse.Where(m => m.QuestionnaireId == new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323")).ToList();

                if (optionValue != null)
                {
                    foreach (var item in optionValue)
                    {
                        //var data1 = _context.CEQQuestions.Where(m => m.CEQQuestionId == new Guid(item.QuestionId)).FirstOrDefault();
                        var data1 = _context.CEQQuestions.Find(new Guid(item.QuestionId));
                        data1.Days = item.Days;
                        data1.Cost = item.Cost;
                        data1.Requirement = item.Requirement;

                        _context.Update(data1);
                    }

                    var res = await _context.SaveChangesAsync();
                    if (res > 0)
                    {
                        //var data2 = _context.Customer.Find(customer.CustomerId);
                        //data2.Questionnaire = true;
                        //data2.RepliedDate = DateTime.Now;
                        //_context.Update(data2);
                        //await _context.SaveChangesAsync();

                        //CustomerQuestionnair data3 = new CustomerQuestionnair();
                        //data3.CustomerQuestionnairId = Guid.NewGuid();
                        //data3.CustomerId = customer.CustomerId;
                        //data3.CreatedBy = customer.CustomerId;
                        //data3.UpdatedDate = DateTime.Now;
                        //data3.CreatedDate = DateTime.Now;
                        //data3.QuestionnaireId = new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323");
                        //data3.IsQuestionnaire = true;
                        //_context.Add(data3);
                        //await _context.SaveChangesAsync();
                        //MSG.msg = "Success";
                    }
                }
                return MSG;
            }
            catch (Exception ex) { throw; }


        }


        public async Task<DbResponce> saveMethodology_Deliverables(string ScopeTypeMethodolgy = null, string ScopeTypeMethodolgy_FileName = null, string ScopeTypeSampleReport = null, string ScopeTypeSampleReport_FileName = null, string Methodology_DeliverablesId = null)
        {
            DbResponce MSG = new DbResponce();
            try
            {

                //  var resdata = _context.CEQQuestionResponse.Where(m => m.QuestionnaireId == new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323")).ToList();

                if (Methodology_DeliverablesId != null)
                {

                    //var data1 = _context.CEQQuestions.Where(m => m.CEQQuestionId == new Guid(item.QuestionId)).FirstOrDefault();
                    var data1 = _context.Admin_Methodology_Deliverables.Find(new Guid(Methodology_DeliverablesId));
                    data1.ScopeTypeMethodolgy = ScopeTypeMethodolgy;
                    // data1.ScopeTypeMethodolgy_FileName = ScopeTypeMethodolgy_FileName;
                    data1.ScopeTypeSampleReport = ScopeTypeSampleReport;
                    // data1.ScopeTypeSampleReport_FileName = ScopeTypeSampleReport_FileName;
                    data1.MethodologyId = new Guid(ScopeTypeMethodolgy_FileName);
                    data1.MethodologyReportId = new Guid(ScopeTypeSampleReport_FileName);
                    _context.Update(data1);
                    var res = await _context.SaveChangesAsync();
                    if (res > 0)
                    {

                    }
                }
                return MSG;
            }
            catch (Exception ex) { throw; }


        }


        public async Task<QuestionResponseViewModel> ResponseQuestionsUpdate(QuestionnaireViewModel Questionnaire)
        {

            QuestionResponseViewModel model = new QuestionResponseViewModel();
            try
            {

                foreach (var item in Questionnaire.Questions)
                {
                    if (item.ChildQuestionsList != null)
                    {
                        foreach (var item1 in item.ChildQuestionsList)
                        {
                            var data1 = _context.ChildQuestions.Find(item1.ChildQuestionId);
                            data1.Cost = item1.Cost;
                            data1.Days = item1.Days;
                            data1.Requirement = item1.Requirement;
                            _context.Update(data1);

                        }
                    }

                    var data = _context.Questions.Find(item.QuestionId);
                    data.Cost = item.Cost;
                    data.Days = item.Days;
                    data.Requirement = item.Requirement;

                    _context.Update(data);

                }
                if (Questionnaire.Methodology_Deliverablesdata != null)
                {
                    var Methodology_DeliverablesData = _context.Admin_Methodology_Deliverables.Where(m => m.Methodology_DeliverablesId == Questionnaire.Methodology_Deliverablesdata.Methodology_DeliverablesId).FirstOrDefault();

                    if (Methodology_DeliverablesData != null)
                    {
                        var mddata = _context.Admin_Methodology_Deliverables.Find(Questionnaire.Methodology_Deliverablesdata.Methodology_DeliverablesId);
                        mddata.ScopeTypeMethodolgy = Questionnaire.Methodology_Deliverablesdata.ScopeTypeMethodolgy;
                        //  mddata.ScopeTypeMethodolgy_FileName = Questionnaire.Methodology_Deliverablesdata.ScopeTypeMethodolgy_FileName;
                        mddata.ScopeTypeSampleReport = Questionnaire.Methodology_Deliverablesdata.ScopeTypeSampleReport;
                        //  mddata.ScopeTypeSampleReport_FileName = Questionnaire.Methodology_Deliverablesdata.ScopeTypeSampleReport_FileName;
                        mddata.UpdatedDate = DateTime.Now;
                        mddata.MethodologyId = Questionnaire.Methodology_Deliverablesdata.MethodologyId;
                        mddata.MethodologyReportId = Questionnaire.Methodology_Deliverablesdata.MethodologyReportId;
                        _context.Update(mddata);
                    }

                }




                var Res = await _context.SaveChangesAsync();
                if (Res > 0)
                {

                }
            }
            catch (Exception ex) { throw; }
            return model;

        }

        public List<CustomerQuestionnairViewModel> ScopRequestCustomerQuestionnairList(Guid id)
        {
            List<CustomerQuestionnairViewModel> model = new List<CustomerQuestionnairViewModel>();
            try
            {
                var customer = _context.Customer.Where(m => m.UserId == id.ToString()).FirstOrDefault();
                if (customer != null)
                {


                    model = (from a in _context.CustomerQuestionnair
                             where a.CustomerId == customer.CustomerId
                             select new CustomerQuestionnairViewModel
                             {
                                 QuestionnaireId = a.QuestionnaireId,
                                 CustomerQuestionnairId = a.CustomerQuestionnairId,
                                 CustomerId = a.CustomerId,
                                 CreatedBy = a.CreatedBy,
                                 CreatedDate = a.CreatedDate,
                                 IsQuestionnaire = a.IsQuestionnaire,
                                 UpdatedDate = a.UpdatedDate,
                                 Nmae = (from b in _context.Questionnaire
                                         where b.QuestionnaireId == a.QuestionnaireId
                                         select new
                                         {
                                             b.Name,
                                         }).Single().Name
                             }).ToList();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return model;
        }

        public List<QuestionnaireReportDataModel> QuestionnaireListForReportWithDetails(Guid id, Guid Projectid)
        {
            List<QuestionnaireReportDataModel> model = new List<QuestionnaireReportDataModel>();
            try
            {
                if (Projectid != Guid.Empty)
                {
                    model = (from z in _context.CustomerQuestionnair
                             where z.CustomerId == id && z.ProjectId == Projectid
                             select new QuestionnaireReportDataModel
                             {
                                 ProjectsViewdata = (from j in _context.Projects
                                                     where j.ProjectsId == Projectid
                                                     select new ProjectsViewModel
                                                     {
                                                         ProjectsId = j.ProjectsId,
                                                         CustomerName = j.CustomerName,
                                                         ProjectName = j.ProjectName,
                                                         CustomerId = j.CustomerId,
                                                         ApplicationName = j.ApplicationName,
                                                         CreateDate = j.CreateDate,
                                                     }).FirstOrDefault(),
                                 QuestionnaireId = z.QuestionnaireId,
                                 Name = (from q in _context.Questionnaire
                                         where q.QuestionnaireId == z.QuestionnaireId
                                         select new
                                         {
                                             q.Name,
                                         }).Single().Name,
                                 Questions = (from a in _context.Questions
                                              where a.QuestionnaireId == z.QuestionnaireId
                                              select new QuestionsViewModel
                                              {
                                                  Days = a.Days,
                                                  Cost = a.Cost,
                                                  QuestionId = a.QuestionId,
                                                  QuestionnaireId = a.QuestionnaireId,
                                                  ChildQuestionsList = (from c in _context.ChildQuestions
                                                                        where c.QuestionId == a.QuestionId
                                                                        select new ChildQuestionsViewModel
                                                                        {
                                                                            ChildQuestionId = c.ChildQuestionId,
                                                                            Days = c.Days,
                                                                            Cost = c.Cost,
                                                                            QuestionnaireId = c.QuestionnaireId,
                                                                        }).ToList(),
                                              }).ToList()
                             }).OrderBy(m => m.Name).ToList();

                    return model;
                }
                else
                {
                    return model;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public List<ProjectsViewModel> ProjectsViewModelList(Guid id)
        {
            List<ProjectsViewModel> model = new List<ProjectsViewModel>();
            try
            {
                //var customer = _context.Customer.Where(m => m.UserId == id.ToString()).FirstOrDefault();
                if (id != Guid.Empty)
                {


                    model = (from v in _context.Projects
                             where v.CustomerId == id
                             select new ProjectsViewModel
                             {
                                 ProjectName = v.ProjectName,
                                 ProjectsId = v.ProjectsId,
                                 CustomerId = v.CustomerId,
                                 CreateDate = v.CreateDate,
                                 CreatedBy = v.CreatedBy,
                                 RequestStatus = v.RequestStatus,
                                 CustomerRequestsId = (from b in _context.CustomerRequests
                                                       where b.CustomerId == v.CustomerId && b.ProjectId == v.ProjectsId && b.IsAccept == false
                                                       select new
                                                       {
                                                           b.CustomerRequestsId,
                                                       }).Single().CustomerRequestsId,
                                 QDataList = (from a in _context.CustomerQuestionnair
                                              where a.CustomerId == id && a.ProjectId == v.ProjectsId
                                              select new CustomerQuestionnairViewModel
                                              {
                                                  ProjectsViewModeldata = (from j in _context.Projects
                                                                           where j.ProjectsId == v.ProjectsId
                                                                           select new ProjectsViewModel
                                                                           {
                                                                               ProjectsId = j.ProjectsId,
                                                                               CustomerName = j.CustomerName,
                                                                               ProjectName = j.ProjectName,
                                                                               CustomerId = j.CustomerId,
                                                                               ApplicationName = j.ApplicationName,
                                                                               CreateDate = j.CreateDate,
                                                                           }).FirstOrDefault(),
                                                  QuestionnaireId = a.QuestionnaireId,
                                                  CustomerQuestionnairId = a.CustomerQuestionnairId,
                                                  CustomerId = a.CustomerId,
                                                  CreatedBy = a.CreatedBy,
                                                  CreatedDate = a.CreatedDate,
                                                  IsQuestionnaire = a.IsQuestionnaire,
                                                  UpdatedDate = a.UpdatedDate,
                                                  ClientNmae = (from b in _context.Customer
                                                                where b.CustomerId == a.CustomerId
                                                                select new
                                                                {
                                                                    b.Client,
                                                                    FirstName = "" + b.FirstName + " " + b.LastName + "",

                                                                }).Single().Client,

                                                  CustomerName = (from b in _context.Customer
                                                                  where b.CustomerId == a.CustomerId
                                                                  select new
                                                                  {

                                                                      FirstName = "" + b.FirstName + " " + b.LastName + "",

                                                                  }).Single().FirstName,

                                                  SalesNmae = (from z in _context.Customer
                                                               where z.CustomerId == a.CustomerId
                                                               from b in _context.Users
                                                               where b.Id == z.CreatedBy.ToString()
                                                               select new
                                                               {
                                                                   b.UserName,

                                                               }).Single().UserName,
                                                  Nmae = (from b in _context.Questionnaire
                                                          where b.QuestionnaireId == a.QuestionnaireId
                                                          select new
                                                          {
                                                              b.Name,
                                                          }).Single().Name
                                              }).ToList(),

                                 UplodDocList = (from z in _context.UploadedDocuments
                                                 where z.ProjectId == v.ProjectsId && z.CustomerId == v.CustomerId
                                                 select new UploadedDocumentsViewModel
                                                 {
                                                     FileName = z.FileName,
                                                     FileContents = z.FileContents,
                                                     ProjectId = z.ProjectId,
                                                     CustomerId = z.CustomerId,
                                                     UploadedDocumentsId = z.UploadedDocumentsId,
                                                     FileModifiedTime = z.FileModifiedTime,

                                                 }).ToList(),
                                 UplodAdditionalDocList = (from z in _context.UploadFiles
                                                           where z.ProjectId == v.ProjectsId && z.CustomerId == v.CustomerId
                                                           select new UploadedDocumentsViewModel
                                                           {
                                                               FileName = z.FileName,
                                                               FileContents = z.FileContents,
                                                               ProjectId = z.ProjectId,
                                                               CustomerId = z.CustomerId,
                                                               UploadedDocumentsId = z.UploadedDocumentsId,
                                                               FileModifiedTime = z.FileModifiedTime,
                                                           }).ToList(),

                                 ClientNmae = (from b in _context.Customer
                                               where b.CustomerId == id
                                               select new
                                               {
                                                   b.Client,
                                                   FirstName = "" + b.FirstName + " " + b.LastName + "",

                                               }).Single().Client,

                                 CustomerName = (from b in _context.Customer
                                                 where b.CustomerId == id
                                                 select new
                                                 {

                                                     FirstName = "" + b.FirstName + " " + b.LastName + "",

                                                 }).Single().FirstName,

                                 SalesNmae = (from z in _context.Customer
                                              where z.CustomerId == id
                                              from b in _context.Users
                                              where b.Id == z.CreatedBy.ToString()
                                              select new
                                              {
                                                  b.UserName,

                                              }).Single().UserName,
                             }).ToList();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return model;
        }


        public List<ProjectsViewModel> UploadedDocumentsViewModel(Guid id)
        {
            List<ProjectsViewModel> model = new List<ProjectsViewModel>();
            try
            {
                if (id != Guid.Empty)
                {
                    model = (from v in _context.Projects
                             where v.CustomerId == id
                             select new ProjectsViewModel
                             {
                                 CustomerId = v.CustomerId,
                                 ProjectsId = v.ProjectsId,
                                 UplodDocList = (from z in _context.UploadedDocuments
                                                 where z.ProjectId == v.ProjectsId && z.CustomerId == v.CustomerId
                                                 select new UploadedDocumentsViewModel
                                                 {
                                                     FileName = z.FileName,
                                                     FileContents = z.FileContents,
                                                     ProjectId = z.ProjectId,
                                                     CustomerId = z.CustomerId,
                                                     UploadedDocumentsId = z.UploadedDocumentsId,
                                                     FileModifiedTime = z.FileModifiedTime,

                                                 }).ToList(),
                             }).ToList();

                }

            }
            catch (Exception)
            {

                throw;
            }
            return model;


        }
        public List<ProjectsViewModel> ProjectsViewModelListque(Guid id)
        {
            List<ProjectsViewModel> model = new List<ProjectsViewModel>();
            try
            {
                var customer = _context.Customer.Where(m => m.UserId == id.ToString()).FirstOrDefault();
                if (customer != null)
                {


                    model = (from v in _context.Projects
                             where v.CustomerId == customer.CustomerId
                             select new ProjectsViewModel
                             {
                                 ProjectName = v.ProjectName,
                                 ProjectsId = v.ProjectsId,
                                 CustomerId = v.CustomerId,
                                 CreateDate = v.CreateDate,
                                 CreatedBy = v.CreatedBy,
                                 QDataList = (from a in _context.CustomerQuestionnair
                                              where a.CustomerId == customer.CustomerId && a.ProjectId == v.ProjectsId
                                              select new CustomerQuestionnairViewModel
                                              {
                                                  ProjectsViewModeldata = (from j in _context.Projects
                                                                           where j.ProjectsId == v.ProjectsId
                                                                           select new ProjectsViewModel
                                                                           {
                                                                               ProjectsId = j.ProjectsId,
                                                                               CustomerName = j.CustomerName,
                                                                               ProjectName = j.ProjectName,
                                                                               CustomerId = j.CustomerId,
                                                                               ApplicationName = j.ApplicationName,
                                                                               CreateDate = j.CreateDate,
                                                                           }).FirstOrDefault(),
                                                  QuestionnaireId = a.QuestionnaireId,
                                                  CustomerQuestionnairId = a.CustomerQuestionnairId,
                                                  CustomerId = a.CustomerId,
                                                  CreatedBy = a.CreatedBy,
                                                  CreatedDate = a.CreatedDate,
                                                  IsQuestionnaire = a.IsQuestionnaire,
                                                  UpdatedDate = a.UpdatedDate,
                                                  ClientNmae = (from b in _context.Customer
                                                                where b.CustomerId == a.CustomerId
                                                                select new
                                                                {
                                                                    b.Client,
                                                                    FirstName = "" + b.FirstName + " " + b.LastName + "",

                                                                }).Single().Client,

                                                  CustomerName = (from b in _context.Customer
                                                                  where b.CustomerId == a.CustomerId
                                                                  select new
                                                                  {

                                                                      FirstName = "" + b.FirstName + " " + b.LastName + "",

                                                                  }).Single().FirstName,

                                                  SalesNmae = (from z in _context.Customer
                                                               where z.CustomerId == a.CustomerId
                                                               from b in _context.Users
                                                               where b.Id == z.CreatedBy.ToString()
                                                               select new
                                                               {
                                                                   b.UserName,

                                                               }).Single().UserName,
                                                  Nmae = (from b in _context.Questionnaire
                                                          where b.QuestionnaireId == a.QuestionnaireId
                                                          select new
                                                          {
                                                              b.Name,
                                                          }).Single().Name
                                              }).ToList(),


                             }).ToList();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return model;
        }

        public ProjectsViewModel GetProjectById(Guid id)
        {
            ProjectsViewModel model = new ProjectsViewModel();
            try
            {
                model = (from j in _context.Projects
                         where j.ProjectsId == id
                         select new ProjectsViewModel
                         {
                             ProjectsId = j.ProjectsId,
                             CustomerName = j.CustomerName,
                             ProjectName = j.ProjectName,
                             CustomerId = j.CustomerId,
                             ApplicationName = j.ApplicationName,
                             CreateDate = j.CreateDate,
                         }).FirstOrDefault();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int DeleteCustomerQuestionnairsByProjectId(Guid? id, Guid QuestionnaireId, Guid ProjectId)
        {
            try
            {

                var data1 = (from a in _context.QuestionResponse
                             where a.CustomerId == id && a.ChildQuestionId == Guid.Empty && a.QuestionnaireId == QuestionnaireId && a.ProjectId == ProjectId
                             select new QuestionsViewModel
                             {
                                 ResponseId = a.ResponseId,
                                 QuestionId = a.QuestionId,
                                 ChildQuestionsList = (from c in _context.QuestionResponse
                                                       where c.QuestionId == a.QuestionId && c.ChildQuestionId != Guid.Empty && a.QuestionnaireId == QuestionnaireId && c.ProjectId == ProjectId
                                                       select new ChildQuestionsViewModel
                                                       {
                                                           ChildQuestionId = c.ChildQuestionId,
                                                           ResponseId = c.ResponseId,
                                                       }).ToList(),
                             }).ToList();

                if (data1.Count != 0)
                {
                    foreach (var Rsdata in data1)
                    {
                        if (Rsdata.ChildQuestionsList.Count != 0)
                        {
                            foreach (var ChieldRsdata in Rsdata.ChildQuestionsList)
                            {
                                var ChieldRsdataremove = _context.QuestionResponse.Find(ChieldRsdata.ResponseId);
                                _context.QuestionResponse.Remove(ChieldRsdataremove);
                                _context.SaveChanges();
                            }
                        }
                        var employee = _context.QuestionResponse.Find(Rsdata.ResponseId);
                        _context.QuestionResponse.Remove(employee);
                        _context.SaveChanges();
                    }
                }





                //var resdata = _context.QuestionResponse.Where(m => m.QuestionnaireId == QuestionnaireId && m.CustomerId == id && m.ProjectId == ProjectId).ToList();
                //if (resdata.Count != 0)
                //{
                //    foreach (var Rsdata in resdata)
                //    {
                //        var employee = _context.QuestionResponse.Find(Rsdata.ResponseId);
                //        _context.QuestionResponse.Remove(employee);
                //        _context.SaveChanges();
                //    }
                //}
                //var Methodology_DeliverablesData = _context.Methodology_Deliverables.Where(m => m.QuestionnaireId == QuestionnaireId && m.CustomerId == id && m.ProjectId == ProjectId).FirstOrDefault();
                //if (Methodology_DeliverablesData != null)
                //{
                //    var data = _context.Methodology_Deliverables.Find(Methodology_DeliverablesData.Methodology_DeliverablesId);
                //    _context.Methodology_Deliverables.Remove(data);
                //    _context.SaveChanges();
                //}


                var CustomerQuestionnaireData = _context.CustomerQuestionnair.Where(m => m.QuestionnaireId == QuestionnaireId && m.CustomerId == id && m.ProjectId == ProjectId).FirstOrDefault();
                if (CustomerQuestionnaireData != null)
                {
                    var data = _context.CustomerQuestionnair.Find(CustomerQuestionnaireData.CustomerQuestionnairId);
                    _context.CustomerQuestionnair.Remove(data);
                    _context.SaveChanges();
                }



                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public int DeleteCustomerPciQuestionnairsByProjectId(Guid? id, Guid QuestionnaireId, Guid ProjectId)
        {
            try
            {
                var resdata = _context.QuestionResponse.Where(m => m.QuestionnaireId == QuestionnaireId && m.CustomerId == id && m.ProjectId == ProjectId).ToList();
                if (resdata.Count != 0)
                {
                    foreach (var Rsdata in resdata)
                    {
                        var employee = _context.QuestionResponse.Find(Rsdata.ResponseId);
                        _context.QuestionResponse.Remove(employee);
                        _context.SaveChanges();
                    }
                }
                var pcidata = _context.PCIQuestionnaire.Where(m => m.QuestionnaireId == QuestionnaireId && m.CustomerId == id && m.ProjectsId == ProjectId).ToList();
                if (pcidata.Count != 0)
                {
                    foreach (var Rsdata in pcidata)
                    {
                        var employee = _context.PCIQuestionnaire.Find(Rsdata.PCIQuestionnaireId);
                        _context.PCIQuestionnaire.Remove(employee);
                        _context.SaveChanges();
                    }
                }

                var CustomerQuestionnaireData = _context.CustomerQuestionnair.Where(m => m.QuestionnaireId == QuestionnaireId && m.CustomerId == id && m.ProjectId == ProjectId).FirstOrDefault();
                if (CustomerQuestionnaireData != null)
                {
                    var data = _context.CustomerQuestionnair.Find(CustomerQuestionnaireData.CustomerQuestionnairId);
                    _context.CustomerQuestionnair.Remove(data);
                    _context.SaveChanges();
                }
                var mdQuestionnaireData = _context.Methodology_Deliverables.Where(m => m.QuestionnaireId == QuestionnaireId && m.CustomerId == id && m.ProjectId == ProjectId).FirstOrDefault();
                if (mdQuestionnaireData != null)
                {
                    var data = _context.Methodology_Deliverables.Find(mdQuestionnaireData.Methodology_DeliverablesId);
                    _context.Methodology_Deliverables.Remove(data);
                    _context.SaveChanges();
                }



                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public int DeleteCustomerCeqQuestionnairsByProjectId(Guid? id, Guid QuestionnaireId, Guid ProjectId)
        {
            try
            {
                var resdata = _context.CEQSignature.Where(m => m.QuestionnaireId == new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323") && m.CustomerId == id && m.ProjectId == ProjectId).FirstOrDefault();
                if (resdata != null)
                {
                    var employee = _context.CEQSignature.Find(resdata.CEQSignatureId);
                    _context.CEQSignature.Remove(employee);
                    _context.SaveChanges();

                }

                var CEQOrgdata = _context.CEQOrgDetails.Where(m => m.QuestionnaireId == new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323") && m.CustomerId == id && m.ProjectId == ProjectId).FirstOrDefault();
                if (CEQOrgdata != null)
                {
                    var employee = _context.CEQOrgDetails.Find(CEQOrgdata.CEQOrgDetailsId);
                    _context.CEQOrgDetails.Remove(employee);
                    _context.SaveChanges();

                }

                var CEQQuestionResponsedata = _context.CEQQuestionResponse.Where(m => m.QuestionnaireId == new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323") && m.CustomerId == id && m.ProjectId == ProjectId).ToList();
                if (CEQQuestionResponsedata.Count != 0)
                {
                    foreach (var Rsdata in CEQQuestionResponsedata)
                    {
                        var employee = _context.CEQQuestionResponse.Find(Rsdata.CEQResponseId);
                        _context.CEQQuestionResponse.Remove(employee);
                        _context.SaveChanges();
                    }
                }
                var CEQCloudShareddata = _context.CEQCloudSharedServicesAsses.Where(m => m.QuestionnaireId == new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323") && m.CustomerId == id && m.ProjectId == ProjectId).ToList();
                if (CEQCloudShareddata.Count != 0)
                {
                    foreach (var Rsdata in CEQCloudShareddata)
                    {
                        var employee = _context.CEQCloudSharedServicesAsses.Find(Rsdata.CEQCloudSharedServicesAssesId);
                        _context.CEQCloudSharedServicesAsses.Remove(employee);
                        _context.SaveChanges();
                    }
                }

                var CEQWorkstationdata = _context.CEQWorkstationAssessment.Where(m => m.QuestionnaireId == new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323") && m.CustomerId == id && m.ProjectId == ProjectId).ToList();
                if (CEQWorkstationdata.Count != 0)
                {
                    foreach (var Rsdata in CEQWorkstationdata)
                    {
                        var employee = _context.CEQWorkstationAssessment.Find(Rsdata.CEQWorkstationAssessmentId);
                        _context.CEQWorkstationAssessment.Remove(employee);
                        _context.SaveChanges();
                    }
                }
                var CEQRemotedata = _context.CEQRemoteVulnScan.Where(m => m.QuestionnaireId == new Guid("54515C61-E5C1-43D5-87CA-AF8102A06323") && m.CustomerId == id && m.ProjectId == ProjectId).ToList();
                if (CEQRemotedata.Count != 0)
                {
                    foreach (var Rsdata in CEQRemotedata)
                    {
                        var employee = _context.CEQRemoteVulnScan.Find(Rsdata.CEQRemoteVulnScanId);
                        _context.CEQRemoteVulnScan.Remove(employee);
                        _context.SaveChanges();
                    }
                }

                var CustomerQuestionnaireData = _context.CustomerQuestionnair.Where(m => m.QuestionnaireId == QuestionnaireId && m.CustomerId == id && m.ProjectId == ProjectId).FirstOrDefault();
                if (CustomerQuestionnaireData != null)
                {
                    var data = _context.CustomerQuestionnair.Find(CustomerQuestionnaireData.CustomerQuestionnairId);
                    _context.CustomerQuestionnair.Remove(data);
                    _context.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public Methodology_DeliverablesViewModel Methodology_DeliverablesdataProjectById(Guid? id, Guid? Projectid, Guid QuestionnaireId)
        {
            try
            {
                Methodology_DeliverablesViewModel model = new Methodology_DeliverablesViewModel();


                model = (from m in _context.Methodology_Deliverables
                         where m.ProjectId == Projectid && m.QuestionnaireId == QuestionnaireId && m.CustomerId == id
                         select new Methodology_DeliverablesViewModel
                         {
                             CustomerId = m.CustomerId,
                             QuestionnaireId = m.QuestionnaireId,
                             ScopeTypeMethodolgy = m.ScopeTypeMethodolgy,
                             ScopeTypeMethodolgy_FileName = m.ScopeTypeMethodolgy_FileName,
                             ScopeTypeSampleReport = m.ScopeTypeSampleReport,
                             ScopeTypeSampleReport_FileName = m.ScopeTypeSampleReport_FileName,
                             Methodology_DeliverablesId = m.Methodology_DeliverablesId,
                             ProjectId = m.ProjectId
                         }).FirstOrDefault();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AllProjectsViewModel AllProjectsData(Guid id, Guid ProjectId)
        {
            AllProjectsViewModel model = new AllProjectsViewModel();
            try
            {
                model = (from m in _context.Projects
                         where m.CustomerId == id && m.ProjectsId == ProjectId
                         select new AllProjectsViewModel
                         {
                             ProjectsId = m.ProjectsId,
                             ProjectName = m.ProjectName,
                             CustomerId = m.CustomerId,
                             QDataList = (from n in _context.CustomerQuestionnair
                                          where n.CustomerId == id && n.ProjectId == m.ProjectsId
                                          select new ProjectCustomerQuestionnairModel
                                          {
                                              CustomerQuestionnairId = n.CustomerQuestionnairId,
                                              QuestionnaireId = n.QuestionnaireId,
                                              CustomerId = n.CustomerId,
                                              ProjectId = n.ProjectId,
                                              Name = (from q in _context.Questionnaire
                                                      where q.QuestionnaireId == n.QuestionnaireId
                                                      select new
                                                      {
                                                          q.Name,
                                                      }).Single().Name,
                                              Questions = (from a in _context.QuestionResponse
                                                           where a.CustomerId == id && a.ChildQuestionId == Guid.Empty && a.QuestionnaireId == n.QuestionnaireId && a.ProjectId == n.ProjectId
                                                           select new QuestionsViewModel
                                                           {
                                                               ResponseId = a.ResponseId,
                                                               QuestionData = _context.Questions.Where(m => m.QuestionId == a.QuestionId).FirstOrDefault(),
                                                               QuestionId = a.QuestionId,
                                                               OptionType = a.OptionType,
                                                               ResponseOptionId = a.OptionId,
                                                               ResponseDateTime = a.OptionDate,
                                                               Responsetext = a.Responsetext,
                                                               Chk = a.IsChecked,
                                                               Cost = a.Cost,
                                                               Days = a.Days,
                                                               Requirement = a.Requirement,
                                                               VariableName = a.VariableName,
                                                               ChildQuestionsList = (from c in _context.QuestionResponse
                                                                                     where c.QuestionId == a.QuestionId && c.ChildQuestionId != Guid.Empty && c.ProjectId == n.ProjectId
                                                                                     select new ChildQuestionsViewModel
                                                                                     {
                                                                                         ResponseId = c.ResponseId,
                                                                                         QuestionData = _context.ChildQuestions.Where(m => m.ChildQuestionId == c.ChildQuestionId).FirstOrDefault(),
                                                                                         ChildQuestionId = c.ChildQuestionId,
                                                                                         OptionType = c.OptionType,
                                                                                         ResponseOptionId = c.OptionId,
                                                                                         ResponseDateTime = c.OptionDate,
                                                                                         Responsetext = c.Responsetext,
                                                                                         Cost = c.Cost,
                                                                                         Days = c.Days,
                                                                                         Requirement = c.Requirement,
                                                                                     }).OrderBy(m => m.QuestionData.QuestionNumber).ToList(),
                                                           }).OrderBy(m => m.QuestionData.RowNumber).ToList(),
                                              QuestionsData = (from v in _context.Questions
                                                               where v.QuestionnaireId == n.QuestionnaireId && v.SectionId == n.SectionId
                                                               select new QuestionsViewModel
                                                               {
                                                                   Days = v.Days,
                                                                   Cost = v.Cost,
                                                                   QuestionId = v.QuestionId,
                                                                   QuestionnaireId = v.QuestionnaireId,
                                                                   VariableName = v.VariableName,
                                                                   ChildQuestionsList = (from f in _context.ChildQuestions
                                                                                         where f.QuestionId == v.QuestionId
                                                                                         select new ChildQuestionsViewModel
                                                                                         {
                                                                                             ChildQuestionId = f.ChildQuestionId,
                                                                                             Days = f.Days,
                                                                                             Cost = f.Cost,
                                                                                             QuestionnaireId = f.QuestionnaireId,
                                                                                             VariableName = f.VariableName,
                                                                                         }).ToList(),
                                                               }).ToList()
                                          }).ToList(),
                         }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }

            return model;
        }


        public ProjectsViewModel projectsdata(Guid id, Guid Projectid)
        {
            try
            {
                ProjectsViewModel model = new ProjectsViewModel();


                model = (from m in _context.Projects
                         where m.ProjectsId == Projectid && m.CustomerId == id
                         select new ProjectsViewModel
                         {
                             ProjectsId = m.ProjectsId,
                             ProjectName = m.ProjectName,
                             CustomerId = m.CustomerId,
                             ApplicationName = m.ApplicationName,
                             CreateDate = m.CreateDate,
                             CreatedBy = m.CreatedBy,
                             CustomerName = m.CustomerName,
                             UpdatedDate = m.UpdatedDate
                         }).FirstOrDefault();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Methodology_DeliverablesViewModel> Methodology_DeliverablesdataProjectById1(Guid? id, Guid? Projectid)
        {
            try
            {
                List<Methodology_DeliverablesViewModel> model = new List<Methodology_DeliverablesViewModel>();


                model = (from m in _context.Methodology_Deliverables
                         where m.ProjectId == Projectid && m.CustomerId == id
                         select new Methodology_DeliverablesViewModel
                         {
                             CustomerId = m.CustomerId,
                             QuestionnaireId = m.QuestionnaireId,
                             ScopeTypeMethodolgy = m.ScopeTypeMethodolgy,
                             ScopeTypeMethodolgy_FileName = m.ScopeTypeMethodolgy_FileName,
                             ScopeTypeSampleReport = m.ScopeTypeSampleReport,
                             ScopeTypeSampleReport_FileName = m.ScopeTypeSampleReport_FileName,
                             Methodology_DeliverablesId = m.Methodology_DeliverablesId,
                             ProjectId = m.ProjectId
                         }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Methodology_DeliverablesViewModel AdminMethodology_Deliverablesdata(Guid id)
        {
            try
            {
                Methodology_DeliverablesViewModel model = new Methodology_DeliverablesViewModel();


                model = (from m in _context.Admin_Methodology_Deliverables
                         where m.QuestionnaireId == id
                         select new Methodology_DeliverablesViewModel
                         {
                             Methodology_DeliverablesId = m.Methodology_DeliverablesId,
                             ScopeTypeMethodolgy = m.ScopeTypeMethodolgy,
                             ScopeTypeMethodolgy_FileName = (from b in _context.Methodologye
                                                             where b.Id == m.MethodologyId
                                                             select new
                                                             {
                                                                 b.FileName,
                                                             }).Single().FileName,
                             ScopeTypeSampleReport = m.ScopeTypeSampleReport,
                             ScopeTypeSampleReport_FileName = (from b in _context.Methodologye
                                                               where b.Id == m.MethodologyReportId
                                                               select new
                                                               {
                                                                   b.FileName,
                                                               }).Single().FileName,
                             QuestionnaireId = m.QuestionnaireId,
                             MethodologyId = m.MethodologyId,
                             MethodologyReportId = m.MethodologyReportId
                         }).FirstOrDefault();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<QuestionnaireViewModel> AdminMethodology_List_BYQuestionnaire(Guid? id, Guid? Projectid)
        {
            List<QuestionnaireViewModel> model = new List<QuestionnaireViewModel>();

            model = (from a in _context.CustomerQuestionnair
                     where a.CustomerId == id && a.ProjectId == Projectid
                     select new QuestionnaireViewModel
                     {
                         Methodology_Deliverablesdata = (from j in _context.Admin_Methodology_Deliverables
                                                         where j.QuestionnaireId == a.QuestionnaireId
                                                         select new Methodology_DeliverablesViewModel
                                                         {
                                                             ScopeTypeMethodolgy = j.ScopeTypeMethodolgy,
                                                             ScopeTypeMethodolgy_FileName = (from b in _context.Methodologye
                                                                                             where b.Id == j.MethodologyId
                                                                                             select new
                                                                                             {
                                                                                                 b.FileName,
                                                                                             }).Single().FileName,
                                                             ScopeTypeSampleReport = j.ScopeTypeSampleReport,
                                                             ScopeTypeSampleReport_FileName = (from b in _context.Methodologye
                                                                                               where b.Id == j.MethodologyReportId
                                                                                               select new
                                                                                               {
                                                                                                   b.FileName,
                                                                                               }).Single().FileName,
                                                             Methodology_DeliverablesId = j.Methodology_DeliverablesId,
                                                             MethodologyId = j.MethodologyId,
                                                             MethodologyReportId = j.MethodologyReportId
                                                         }).FirstOrDefault(),
                     }).ToList();

            return model;
        }


        public async Task<int> UpdateSingleQuestions(QuestionnaireViewModel model)
        {
            var Res = 0;
            try
            {
                if (model.QuestionUpdateData != null)
                {
                    if (model.QuestionUpdateData.ChieldQuestionType == "ChieldQuestion")
                    {
                        var Data = _context.ChildQuestions.Where(m => m.ChildQuestionId == model.QuestionUpdateData.QuestionId).FirstOrDefault();

                        if (Data != null)
                        {
                            var mddata = _context.ChildQuestions.Find(Data.ChildQuestionId);
                            mddata.QuestionText = model.QuestionUpdateData.Name;
                            mddata.isMandatory = model.QuestionUpdateData.isMandatory;
                            _context.Update(mddata);
                        }
                    }
                    else
                    {
                        var Data = _context.Questions.Where(m => m.QuestionId == model.QuestionUpdateData.QuestionId && m.QuestionnaireId == model.QuestionUpdateData.QuestionnaireId).FirstOrDefault();

                        if (Data != null)
                        {
                            var mddata = _context.Questions.Find(Data.QuestionId);
                            mddata.QuestionText = model.QuestionUpdateData.Name;
                            mddata.isMandatory = model.QuestionUpdateData.isMandatory;
                            _context.Update(mddata);
                        }
                    }


                }

                Res = await _context.SaveChangesAsync();

            }
            catch (Exception ex) { throw; }
            return Res;

        }
        public async Task<int> DeleteSingleQuestions(Guid id, Guid QuestionnaireId)
        {
            var Res = 0;
            try
            {
                if (id != Guid.Empty)
                {
                    var QuestionResponseData = _context.QuestionResponse.Where(m => m.QuestionId == id && m.QuestionnaireId == QuestionnaireId).FirstOrDefault();
                    var OptionData = _context.Options.Where(m => m.QuestionId == id).ToList();

                    var Data = _context.Questions.Where(m => m.QuestionId == id && m.QuestionnaireId == QuestionnaireId).FirstOrDefault();

                    if (Data != null)
                    {
                        var mddata = _context.Questions.Find(Data.QuestionId);
                        _context.Questions.Remove(mddata);
                        if (OptionData.Count != 0)
                        {
                            foreach (var it in OptionData)
                            {
                                var Optiondata = _context.Options.Find(it.OptionId);
                                _context.Options.Remove(Optiondata);
                            }
                        }
                    }

                    if (QuestionResponseData != null)
                    {
                        var mddata1 = _context.QuestionResponse.Find(QuestionResponseData.ResponseId);
                        _context.QuestionResponse.Remove(mddata1);
                    }

                }

                Res = await _context.SaveChangesAsync();

            }
            catch (Exception ex) { throw; }
            return Res;
        }

        public async Task<int> DeleteSingleChieldQuestions(Guid id, Guid QuestionnaireId)
        {
            var Res = 0;
            try
            {
                if (id != Guid.Empty)
                {
                    var QuestionResponseData = _context.QuestionResponse.Where(m => m.ChildQuestionId == id && m.QuestionnaireId == QuestionnaireId).FirstOrDefault();
                    var OptionData = _context.Options.Where(m => m.QuestionId == id).ToList();

                    var Data = _context.ChildQuestions.Where(m => m.ChildQuestionId == id).FirstOrDefault();

                    if (Data != null)
                    {
                        var mddata = _context.ChildQuestions.Find(Data.ChildQuestionId);
                        _context.ChildQuestions.Remove(mddata);

                        if (OptionData.Count != 0)
                        {
                            foreach (var it in OptionData)
                            {
                                var Optiondata = _context.Options.Find(it.OptionId);
                                _context.Options.Remove(Optiondata);
                            }
                        }
                    }

                    if (QuestionResponseData != null)
                    {
                        var mddata1 = _context.QuestionResponse.Find(QuestionResponseData.ResponseId);
                        _context.QuestionResponse.Remove(mddata1);
                    }

                }

                Res = await _context.SaveChangesAsync();

            }
            catch (Exception ex) { throw; }
            return Res;
        }

        public async Task<int> UpdateSingleCEQQuestions(CyberEssentialsQuestionnaireModel model)
        {
            var Res = 0;
            try
            {
                if (model.CEQQuetUpdateData != null)
                {
                    var Data = _context.CEQQuestions.Where(m => m.CEQQuestionId == model.CEQQuetUpdateData.QuestionId).FirstOrDefault();

                    if (Data != null)
                    {
                        var mddata = _context.CEQQuestions.Find(Data.CEQQuestionId);
                        mddata.QuestionText = model.CEQQuetUpdateData.Name;
                        _context.Update(mddata);
                    }

                }

                Res = await _context.SaveChangesAsync();

            }
            catch (Exception ex) { throw; }
            return Res;

        }
        public async Task<int> DeleteSingleCEQQuestions(Guid id, Guid QuestionnaireId)
        {
            var Res = 0;
            try
            {
                if (id != Guid.Empty)
                {
                    var CEQQuesRespData = _context.CEQQuestionResponse.Where(m => m.QuestionId == id && m.QuestionnaireId == QuestionnaireId).FirstOrDefault();

                    var ResData = _context.CEQQuestions.Where(m => m.CEQQuestionId == id && m.QuestionnaireId == QuestionnaireId).FirstOrDefault();

                    if (ResData != null)
                    {
                        var ResQdata = _context.CEQQuestions.Find(ResData.CEQQuestionId);
                        _context.CEQQuestions.Remove(ResQdata);
                    }
                    if (CEQQuesRespData != null)
                    {
                        var ResponQdata = _context.CEQQuestionResponse.Find(CEQQuesRespData.CEQResponseId);
                        _context.CEQQuestionResponse.Remove(ResponQdata);
                    }
                }

                Res = await _context.SaveChangesAsync();

            }
            catch (Exception ex) { throw; }
            return Res;
        }
        public async Task<int> AddNewSingleQuestions(QuestionnaireViewModel model)
        {
            var Res = 0;
            var vData = 0;
            var QuestionNum = 0;
            var DataRowNum = 0;
            var Data = 0;
            try
            {
                if (model.QuestionUpdateData != null)
                {
                    var dataList = _context.Questions.Where(m => m.QuestionnaireId == model.QuestionUpdateData.QuestionnaireId).ToList();
                    if (dataList.Count != 0)
                    {
                        Data = dataList.Select(a => a.RowNumber).Max();
                    }
                    // Data = _context.Questions.Where(m => m.QuestionnaireId == model.QuestionUpdateData.QuestionnaireId).ToList().Select(a => a.RowNumber).Max();
                    if (Data != 0)
                    {
                        DataRowNum = Data;
                        var VarData = _context.Questions.Where(m => m.QuestionnaireId == model.QuestionUpdateData.QuestionnaireId && m.RowNumber == Data).FirstOrDefault();
                        var vName = VarData.VariableName.Split('A');
                        if (vName[1] != null)
                        {
                            vData = Convert.ToInt32(vName[1]) + 1;
                        }
                        if (VarData.QuestionNumber.Contains("."))
                        {
                            var qNum = VarData.QuestionNumber.Split('.');
                            QuestionNum = Convert.ToInt32(qNum[0]) + 1;
                        }
                        else
                        {
                            QuestionNum = Convert.ToInt32(VarData.QuestionNumber) + 1;
                        }
                    }
                    else
                    {
                        Data = 0;
                        QuestionNum = 1;
                        vData = 1;
                    }


                    Questions que = new Questions();
                    que.QuestionId = Guid.NewGuid();
                    que.QuestionnaireId = model.QuestionUpdateData.QuestionnaireId;
                    que.RowNumber = Data + 1;
                    que.QuestionText = model.QuestionUpdateData.Name;
                    que.isMandatory = model.QuestionUpdateData.isMandatory;
                    que.SectionId = 0;
                    if (model.QuestionUpdateData.CheckBoxCheck == true)
                    {
                        que.questionType = QuestionType.MultipleCheckType;
                    }
                    else
                    {
                        que.questionType = QuestionType.TextType;
                    }

                    // que.questionType = QuestionType.TextType;
                    que.VariableName = "A" + vData;
                    que.QuestionNumber = QuestionNum.ToString();
                    _context.Add(que);
                    Res = await _context.SaveChangesAsync();
                    if (model.QuestionUpdateData.CheckBoxCheck == true)
                    {
                        Options optio = new Options();
                        foreach (var it in model.QuestionUpdateData.CheckboxText)
                        {
                            if (it != null && it != "")
                            {
                                optio.OptionText = it;
                                optio.QuestionId = que.QuestionId;
                                optio.OptionId = Guid.NewGuid();
                                _context.Add(optio);
                                await _context.SaveChangesAsync();
                            }

                        }
                    }
                }


            }
            catch (Exception ex) { throw; }
            return Res;

        }




        public async Task<int> AddNewSecurityScopingSingleQuestions(QuestionnaireViewModel model)
        {
            var Res = 0;
            var vData = 0;
            char QuestionNumberAlphabet = 'a';
            char ch = 'a';
            try
            {
                if (model.QuestionUpdateData != null)
                {
                    var Data = _context.Questions.Where(m => m.QuestionnaireId == model.QuestionUpdateData.QuestionnaireId && m.SectionId == Convert.ToInt32(model.QuestionUpdateData.SectionId)).ToList().Select(a => a.QuestionNumber).Max();
                    var VarData = _context.Questions.Where(m => m.QuestionnaireId == model.QuestionUpdateData.QuestionnaireId && m.QuestionNumber == Data).FirstOrDefault();
                    var vName = VarData.VariableName.Split('A');
                    if (vName[1] != null)
                    {
                        vData = Convert.ToInt32(vName[1]) + 1;
                    }

                    var newalphabate = VarData.QuestionNumber.ToCharArray();
                    if (newalphabate[1] > 0)
                    {
                        ch = newalphabate[1];
                        QuestionNumberAlphabet = ch++;
                    }

                    Questions que = new Questions();
                    que.QuestionId = Guid.NewGuid();
                    que.QuestionnaireId = model.QuestionUpdateData.QuestionnaireId;
                    que.QuestionText = model.QuestionUpdateData.Name;
                    que.SectionId = Convert.ToInt32(model.QuestionUpdateData.SectionId);
                    que.questionType = QuestionType.TextType;
                    que.VariableName = "A" + vData;
                    que.QuestionNumber = "" + newalphabate[0] + ch;
                    _context.Add(que);
                }

                Res = await _context.SaveChangesAsync();

            }
            catch (Exception ex) { }
            return Res;

        }


        //public List<UploadedDocumentsViewModel> UploadedDocumentsViewModel(Guid id)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<Equestion> EquestionViewModel(EquestionViewModel model)
        {

            try
            {
                Equestion EqModel = new Equestion();
                if (model != null)
                {
                    var ResData = _context.Equestion.Where(m => m.QuestionId == model.QuestionnaireId).FirstOrDefault();
                    if (ResData != null)
                    {
                        if (model.EquestionType == "Cost")
                        {
                            ResData.CostEquestion = model.CreatedFormula;
                        }
                        else if (model.EquestionType == "Days")
                        {
                            ResData.DaysEquestion = model.CreatedFormula;
                        }
                        ResData.RequirmentEquestion = model.RequirmentEquestion;
                        ResData.UpdatedDate = DateTime.Now;
                        _context.Update(ResData);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        if (model.EquestionType == "Cost")
                        {
                            EqModel.CostEquestion = model.CreatedFormula;
                        }
                        else if (model.EquestionType == "Days")
                        {
                            EqModel.DaysEquestion = model.CreatedFormula;
                        }
                        // EqModel.DaysEquestion = model.DaysEquestion;
                        // EqModel.CostEquestion = model.CostEquestion;
                        EqModel.RequirmentEquestion = model.RequirmentEquestion;
                        EqModel.QuestionId = model.QuestionnaireId;
                        EqModel.CreatedDate = DateTime.Now;
                        _context.Add(EqModel);
                        await _context.SaveChangesAsync();
                    }

                }
                return EqModel;

            }
            catch (Exception ex) { throw; }


        }

        public EquestionViewModel EquationData(Guid id)
        {
            try
            {
                EquestionViewModel model = new EquestionViewModel();


                model = (from m in _context.Equestion
                         where m.QuestionId == id
                         select new EquestionViewModel
                         {
                             DaysEquestion = m.DaysEquestion,
                             CostEquestion = m.CostEquestion,
                             RequirmentEquestion = m.RequirmentEquestion,
                             QuestionnaireId = m.QuestionId

                         }).FirstOrDefault();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<int> AddNewQuestionnaireService(AddNewQuestionnair model)
        {
            var Res = 0;
            try
            {
                if (model.Name != null)
                {
                    Questionnaire que = new Questionnaire();
                    que.QuestionnaireId = Guid.NewGuid();
                    que.Name = model.Name;
                    _context.Add(que);
                }
                Res = await _context.SaveChangesAsync();
            }
            catch (Exception ex) { throw; }
            return Res;

        }


        public async Task<int> _DeleteSingleQuestionnaire(Guid id)
        {
            var Res = 0;
            try
            {
                if (id != Guid.Empty)
                {
                    var Data = _context.Questionnaire.Find(id);
                    if (Data != null)
                    {
                        Data.IsDelete = true;
                        _context.Update(Data);
                    }
                }

                Res = await _context.SaveChangesAsync();

            }
            catch (Exception ex) { throw; }
            return Res;
        }
        public QuestionnaireViewModel AddNeQuestions(List<QuestOptions> options, string questionName, string questionType, string questioneerId)
        {

            try
            {
                QuestionnaireViewModel model = new QuestionnaireViewModel();

                var vData = 0;
                var NewNumber = 0;
                char QuestionNumberAlphabet = 'a';
                char ch = 'a';

                if (questioneerId != null)
                {
                    var ResQuesData = _context.Questions.Where(m => m.QuestionnaireId == new Guid(questioneerId)).ToList().Select(a => a.RowNumber).Max();
                    var VarData = _context.Questions.Where(m => m.QuestionnaireId == new Guid(questioneerId) && m.RowNumber == ResQuesData).FirstOrDefault();
                    var vName = VarData.VariableName.Split('A');

                    var Rowno = VarData.RowNumber;

                    if (vName[1] != null)
                    {
                        vData = Convert.ToInt32(vName[1]) + 1;
                    }

                    var newalphabate = VarData.QuestionNumber;

                    if (newalphabate != null)
                    {
                        NewNumber = Convert.ToInt32(newalphabate) + 1;
                    }

                    Questions Ques = new Questions();
                    if (questionType == "0")
                    {
                        Ques.questionType = QuestionType.multipleResponseType;
                    }
                    else if (questionType == "2")
                    {
                        Ques.questionType = QuestionType.TextType;
                    }
                    Ques.QuestionnaireId = new Guid(questioneerId);
                    Ques.QuestionText = questionName;
                    Ques.QuestionId = Guid.NewGuid();
                    Ques.VariableName = "A" + vData;
                    //Ques.QuestionNumber = "" + newalphabate[0] + ch;
                    Ques.QuestionNumber = NewNumber.ToString();
                    Ques.SectionId = 0;
                    Ques.RowNumber = Rowno + 1;
                    _context.Add(Ques);
                    _context.SaveChanges();

                    var ResData = _context.Questions.Where(m => m.QuestionnaireId == new Guid(questioneerId) && m.QuestionId == Ques.QuestionId).FirstOrDefault();

                    if (questionType == "0")
                    {
                        if (ResData.QuestionId != null)
                        {
                            Options optio = new Options();
                            foreach (var item in options)
                            {
                                var Questname = item.name[0];
                                optio.OptionText = Questname.ToString();
                                optio.QuestionId = ResData.QuestionId;
                                optio.OptionId = Guid.NewGuid();
                                _context.Add(optio);
                                _context.SaveChanges();
                            }

                        }
                    }
                    model.QuestionnaireId = ResData.QuestionnaireId;
                }
                return (model);

            }
            catch (Exception ex) { throw; }

        }


        public List<MethodologyeViewModel> Methodologyelist()
        {
            List<MethodologyeViewModel> MethodologyeList = new List<MethodologyeViewModel>();
            try
            {
                MethodologyeList = (from j in _context.Methodologye
                                    where j.Type == "Methodologies"
                                    select new MethodologyeViewModel
                                    {
                                        FileName = j.FileName,
                                        Type = j.Type,
                                        UpdatedDate = j.UpdatedDate,
                                        Id = j.Id,
                                    }).ToList();
            }
            catch (Exception ex)
            {

            }
            return MethodologyeList;
        }

        public List<MethodologyeViewModel> SampleReportlist()
        {
            List<MethodologyeViewModel> SampleReportList = new List<MethodologyeViewModel>();
            try
            {
                SampleReportList = (from j in _context.Methodologye
                                    where j.Type == "SampleReport"
                                    select new MethodologyeViewModel
                                    {
                                        FileName = j.FileName,
                                        Type = j.Type,
                                        UpdatedDate = j.UpdatedDate,
                                        Id = j.Id,
                                    }).ToList();
            }
            catch (Exception ex)
            {

            }
            return SampleReportList;
        }

        public async Task<QuestionnaireViewModel> AddNewSecuirtyQuestions(List<QuestOptions> options, string questionName, string questionType, string questioneerId, string sectionId)
        {
            var Res = "";
            var vData = 0;
            var QuestionNum = 0;
            char QuestionNumberAlphabet = 'a';
            char ch = 'a';

            try
            {
                QuestionnaireViewModel model = new QuestionnaireViewModel();

                if (questioneerId != null)
                {

                    var Data = _context.Questions.Where(m => m.QuestionnaireId == new Guid(questioneerId) && m.SectionId == Convert.ToInt32(sectionId)).ToList().Select(a => a.QuestionNumber).Max();
                    var chielData = _context.ChildQuestions.Where(m => m.QuestionnaireId == new Guid(questioneerId) && m.QuestionNumber == Data).FirstOrDefault();
                    if (chielData != null)
                    {
                        var vName1 = chielData.VariableName.Split('A');
                        if (vName1[1] != null)
                        {
                            vData = Convert.ToInt32(vName1[1]) + 1;
                        }
                        var newalphabate = chielData.QuestionNumber.ToCharArray();
                        if (newalphabate[1] != null)
                        {
                            ch = newalphabate[1];
                            QuestionNumberAlphabet = ch++;
                        }
                        Questions que = new Questions();
                        que.QuestionId = Guid.NewGuid();
                        que.QuestionnaireId = new Guid(questioneerId);
                        que.QuestionText = questionName;
                        que.SectionId = Convert.ToInt32(sectionId);
                        if (questionType == "0")
                        {
                            que.questionType = QuestionType.multipleResponseType;
                        }
                        else if (questionType == "2")
                        {
                            que.questionType = QuestionType.TextType;
                        }
                        else if (questionType == "1")
                        {
                            que.questionType = QuestionType.DateTimeType;
                        }
                        que.VariableName = "A" + vData;
                        que.QuestionNumber = "" + newalphabate[0] + ch;
                        _context.Add(que);
                        _context.SaveChanges();

                        var ResData = _context.Questions.Where(m => m.QuestionnaireId == new Guid(questioneerId) && m.QuestionId == que.QuestionId).FirstOrDefault();

                        if (questionType == "0")
                        {
                            if (ResData.QuestionId != null)
                            {
                                Options optio = new Options();
                                foreach (var item in options)
                                {
                                    var Questname = item.name[0];
                                    optio.OptionText = Questname.ToString();
                                    optio.QuestionId = ResData.QuestionId;
                                    optio.OptionId = Guid.NewGuid();
                                    _context.Add(optio);
                                    _context.SaveChanges();
                                }

                            }
                        }
                        model.QuestionnaireId = que.QuestionnaireId;
                    }
                    else
                    {
                        var VarData = _context.Questions.Where(m => m.QuestionnaireId == new Guid(questioneerId) && m.QuestionNumber == Data).FirstOrDefault();
                        var vName = VarData.VariableName.Split('A');
                        if (vName[1] != null)
                        {
                            vData = Convert.ToInt32(vName[1]) + 1;
                        }
                        var newalphabate = VarData.QuestionNumber.ToCharArray();
                        if (newalphabate[1] != null)
                        {
                            ch = newalphabate[1];
                            QuestionNumberAlphabet = ch++;
                        }
                        Questions que = new Questions();
                        que.QuestionId = Guid.NewGuid();
                        que.QuestionnaireId = new Guid(questioneerId);
                        que.QuestionText = questionName;
                        que.SectionId = Convert.ToInt32(sectionId);
                        if (questionType == "0")
                        {
                            que.questionType = QuestionType.multipleResponseType;
                        }
                        else if (questionType == "2")
                        {
                            que.questionType = QuestionType.TextType;
                        }
                        else if (questionType == "1")
                        {
                            que.questionType = QuestionType.DateTimeType;
                        }
                        que.VariableName = "A" + vData;
                        que.QuestionNumber = "" + newalphabate[0] + ch;
                        _context.Add(que);
                        _context.SaveChanges();

                        var ResData = _context.Questions.Where(m => m.QuestionnaireId == new Guid(questioneerId) && m.QuestionId == que.QuestionId).FirstOrDefault();

                        if (questionType == "0")
                        {
                            if (ResData.QuestionId != null)
                            {
                                Options optio = new Options();
                                foreach (var item in options)
                                {
                                    var Questname = item.name[0];
                                    optio.OptionText = Questname.ToString();
                                    optio.QuestionId = ResData.QuestionId;
                                    optio.OptionId = Guid.NewGuid();
                                    _context.Add(optio);
                                    _context.SaveChanges();
                                }

                            }
                        }
                        model.QuestionnaireId = que.QuestionnaireId;
                    }

                }
                return (model);
            }
            catch (Exception ex) { throw; }


        }



        public async Task<int> AddNewSecuirtyQuestions2(List<QuestOptions> options, string questionName, string questionType, string questioneerId, string sectionId)
        {
            var Res = 0;
            var vData = 0;
            var QuestionNum = 0;
            char QuestionNumberAlphabet = 'a';
            char ch = 'a';

            try
            {
                QuestionnaireViewModel model = new QuestionnaireViewModel();

                if (questioneerId != null)
                {

                    var Data = _context.Questions.Where(m => m.QuestionnaireId == new Guid(questioneerId) && m.SectionId == Convert.ToInt32(sectionId)).ToList().Select(a => a.QuestionNumber).Max();
                    var chielData = _context.ChildQuestions.Where(m => m.QuestionnaireId == new Guid(questioneerId) && m.QuestionNumber == Data).FirstOrDefault();
                    if (chielData != null)
                    {
                        var vName1 = chielData.VariableName.Split('A');
                        if (vName1[1] != null)
                        {
                            vData = Convert.ToInt32(vName1[1]) + 1;
                        }
                        var newalphabate = chielData.QuestionNumber.ToCharArray();
                        if (newalphabate[1] != null)
                        {
                            ch = newalphabate[1];
                            QuestionNumberAlphabet = ch++;
                        }
                        Questions que = new Questions();
                        que.QuestionId = Guid.NewGuid();
                        que.QuestionnaireId = new Guid(questioneerId);
                        que.QuestionText = questionName;
                        que.SectionId = Convert.ToInt32(sectionId);
                        if (questionType == "0")
                        {
                            que.questionType = QuestionType.multipleResponseType;
                        }
                        else if (questionType == "2")
                        {
                            que.questionType = QuestionType.TextType;
                        }
                        else if (questionType == "1")
                        {
                            que.questionType = QuestionType.DateTimeType;
                        }
                        que.VariableName = "A" + vData;
                        que.QuestionNumber = "" + newalphabate[0] + ch;
                        _context.Add(que);
                        Res = await _context.SaveChangesAsync();
                        //var ResData = _context.Questions.Where(m => m.QuestionnaireId == new Guid(questioneerId) && m.QuestionId == que.QuestionId).FirstOrDefault();

                        if (questionType == "0")
                        {
                            if (que.QuestionId != Guid.Empty)
                            {
                                Options optio = new Options();
                                foreach (var item in options)
                                {
                                    var Questname = item.name[0];
                                    optio.OptionText = Questname.ToString();
                                    optio.QuestionId = que.QuestionId;
                                    optio.OptionId = Guid.NewGuid();
                                    _context.Add(optio);
                                    Res = await _context.SaveChangesAsync();

                                }

                            }
                        }
                        model.QuestionnaireId = que.QuestionnaireId;



                    }
                    else
                    {
                        var VarData = _context.Questions.Where(m => m.QuestionnaireId == new Guid(questioneerId) && m.QuestionNumber == Data).FirstOrDefault();
                        var vName = VarData.VariableName.Split('A');
                        if (vName[1] != null)
                        {
                            vData = Convert.ToInt32(vName[1]) + 1;
                        }
                        var newalphabate = VarData.QuestionNumber.ToCharArray();
                        if (newalphabate[1] != null)
                        {
                            ch = newalphabate[1];
                            QuestionNumberAlphabet = ch++;
                        }
                        Questions que = new Questions();
                        que.QuestionId = Guid.NewGuid();
                        que.QuestionnaireId = new Guid(questioneerId);
                        que.QuestionText = questionName;
                        que.SectionId = Convert.ToInt32(sectionId);
                        if (questionType == "0")
                        {
                            que.questionType = QuestionType.multipleResponseType;
                        }
                        else if (questionType == "2")
                        {
                            que.questionType = QuestionType.TextType;
                        }
                        else if (questionType == "1")
                        {
                            que.questionType = QuestionType.DateTimeType;
                        }
                        que.VariableName = "A" + vData;
                        que.QuestionNumber = "" + newalphabate[0] + ch;
                        _context.Add(que);
                        Res = await _context.SaveChangesAsync();
                        // var ResData = _context.Questions.Where(m => m.QuestionnaireId == new Guid(questioneerId) && m.QuestionId == que.QuestionId).FirstOrDefault();

                        if (questionType == "0")
                        {
                            if (que.QuestionId != Guid.Empty)
                            {
                                Options optio = new Options();
                                foreach (var item in options)
                                {
                                    var Questname = item.name[0];
                                    optio.OptionText = Questname.ToString();
                                    optio.QuestionId = que.QuestionId;
                                    optio.OptionId = Guid.NewGuid();
                                    _context.Add(optio);
                                    Res = await _context.SaveChangesAsync();
                                }

                            }
                        }
                        model.QuestionnaireId = que.QuestionnaireId;

                    }

                }
                return (Res);
            }
            catch (Exception ex) { throw; }


        }


        public async Task<DbResponce> UpdateProjectStatus(string Status, string CustomerId = null, string ProjectsId = null)
        {
            DbResponce MSG = new DbResponce();
            try
            {
                var projectsId = new Guid(ProjectsId);
                var customerId = new Guid(CustomerId);
                if (projectsId != Guid.Empty && customerId != Guid.Empty)
                {
                    var data = _context.Projects.Where(m => m.ProjectsId == projectsId && m.CustomerId == customerId).FirstOrDefault();
                    if (data != null)
                    {
                        var ps = _context.Projects.Find(data.ProjectsId);
                        ps.RequestStatus = Status;
                    }
                }
                var res = await _context.SaveChangesAsync();

                MSG.msg = res.ToString();
                return MSG;
            }

            catch (Exception ex) { throw; }


        }

        public async Task<int> AddNewSingleSubQuestions(QuestionnaireViewModel model)
        {
            var Res = 0;
            string vData = null;
            string QuestionNum = null;
            var DataRowNum = 0;
            var Data = 0;
            try
            {
                if (model.QuestionUpdateData != null)
                {
                    var Questiondata = _context.Questions.Where(m => m.QuestionId == model.QuestionUpdateData.QuestionId).FirstOrDefault();
                    if (Questiondata != null)
                    {
                        var ChildQuestionsdata = _context.ChildQuestions.Where(m => m.QuestionId == model.QuestionUpdateData.QuestionId).FirstOrDefault();
                        if (ChildQuestionsdata != null)
                        {
                            var ChildQuestiondata = _context.ChildQuestions.Where(m => m.QuestionId == model.QuestionUpdateData.QuestionId).ToList().Select(a => a.VariableName).Max();


                            DataRowNum = Data;
                            var VarData = _context.ChildQuestions.Where(m => m.QuestionId == model.QuestionUpdateData.QuestionId && m.VariableName == ChildQuestiondata).FirstOrDefault();
                            var vName = VarData.VariableName.Split('A');
                            //if (vName[1] != null)
                            //{
                            //    if(vName[1].Contains("."))
                            //    {
                            //        var qNum = VarData.VariableName.Split('.');
                            //        vData = (""+qNum[0]+"." + (Convert.ToDecimal(qNum[1]) + 1)).ToString();

                            //    }
                            //    else
                            //    {
                            //        vData = ("A" + (Convert.ToInt32(vName[1]) + 1)).ToString();

                            //    }
                            //}
                            if (VarData.QuestionNumber.Contains("."))
                            {
                                var qNum = VarData.QuestionNumber.Split('.');
                                // QuestionNum = Convert.ToInt32(qNum[0]) + 1;
                            }
                            else
                            {

                                char a = VarData.QuestionNumber[VarData.QuestionNumber.Length - 1];
                                a++;
                                string result = VarData.QuestionNumber.Remove(VarData.QuestionNumber.Length - 1);
                                QuestionNum = "" + result + "" + a + "";
                            }
                        }
                        else
                        {
                            Data = 0;
                            QuestionNum = "" + Questiondata.QuestionNumber + "a";
                            // vData = ""+Questiondata.VariableName+".1";
                        }
                    }

                    ChildQuestions que = new ChildQuestions();
                    que.ChildQuestionId = Guid.NewGuid();
                    que.QuestionnaireId = model.QuestionUpdateData.QuestionnaireId;
                    que.QuestionId = model.QuestionUpdateData.QuestionId;
                    que.QuestionText = model.QuestionUpdateData.Name;
                    que.isMandatory = model.QuestionUpdateData.isMandatory;
                    que.SectionId = 0;
                    if (model.QuestionUpdateData.CheckBoxCheck == true)
                    {
                        que.questionType = QuestionType.MultipleCheckType;
                    }
                    else
                    {
                        que.questionType = QuestionType.TextType;
                    }
                    que.VariableName = vData;
                    que.QuestionNumber = QuestionNum.ToString();
                    _context.Add(que);

                    Res = await _context.SaveChangesAsync();
                    if (model.QuestionUpdateData.CheckBoxCheck == true)
                    {
                        Options optio = new Options();
                        foreach (var it in model.QuestionUpdateData.CheckboxText)
                        {
                            if (it != null && it != "")
                            {
                                optio.OptionText = it;
                                optio.QuestionId = que.ChildQuestionId;
                                optio.OptionId = Guid.NewGuid();
                                _context.Add(optio);
                                await _context.SaveChangesAsync();
                            }

                        }
                    }
                }
            }
            catch (Exception ex) { throw; }
            return Res;

        }

        public async Task<int> UpdateAllVar(QuestionnaireViewModel model)
        {
            var Res = 0;
            try
            {
                if (model.QuestionnaireId != Guid.Empty)
                {
                    var Questiondata = _context.Questions.Where(m => m.QuestionnaireId == model.QuestionnaireId).OrderBy(m => m.QuestionNumber).ToList();
                    if (Questiondata != null)
                    {
                        var a = 1;
                        foreach (var item in Questiondata)
                        {
                            var ps = _context.Questions.Find(item.QuestionId);
                            ps.VariableName = "A" + a;
                            a++;
                            //  Res = await _context.SaveChangesAsync();
                            var CQuestiondata = _context.ChildQuestions.Where(m => m.QuestionId == item.QuestionId).OrderBy(m => m.QuestionNumber).ToList();
                            if (CQuestiondata != null)
                            {
                                foreach (var item1 in CQuestiondata)
                                {
                                    var qs = _context.ChildQuestions.Find(item1.ChildQuestionId);
                                    qs.VariableName = "A" + a;
                                    a++;
                                }
                            }

                        }

                    }

                }
                Res = await _context.SaveChangesAsync();
            }
            catch (Exception ex) { throw; }
            return Res;

        }

        public async Task<int> DragDropQuestion(Guid draggId, Guid DroppedId, Guid questioneerId)
        {
            var Res = 0;
            string QuestionNumber = null;
            var RowNumber = 0;
            var draggQuestiondata = _context.Questions.Where(m => m.QuestionId == draggId).FirstOrDefault();
            QuestionNumber = draggQuestiondata.QuestionNumber;
            RowNumber = draggQuestiondata.RowNumber;
            var DroppQuestiondata = _context.Questions.Where(m => m.QuestionId == DroppedId).FirstOrDefault();
            if (draggQuestiondata != null)
            {
                var ps = _context.Questions.Find(draggQuestiondata.QuestionId);
                ps.QuestionNumber = DroppQuestiondata.QuestionNumber;
                ps.RowNumber = DroppQuestiondata.RowNumber;
                Res = await _context.SaveChangesAsync();
                var CQuestiondata = _context.ChildQuestions.Where(m => m.QuestionId == draggQuestiondata.QuestionId).OrderBy(m => m.QuestionNumber).ToList();
                if (CQuestiondata != null)
                {
                    foreach (var item1 in CQuestiondata)
                    {
                        var qs = _context.ChildQuestions.Find(item1.ChildQuestionId);
                        char a = qs.QuestionNumber[qs.QuestionNumber.Length - 1];
                        qs.QuestionNumber = "" + ps.QuestionNumber + "" + a + "";
                        await _context.SaveChangesAsync();
                    }
                }

            }
            if (DroppQuestiondata != null)
            {
                var ps = _context.Questions.Find(DroppQuestiondata.QuestionId);
                ps.QuestionNumber = QuestionNumber;
                ps.RowNumber = RowNumber;
                Res = await _context.SaveChangesAsync();
                var CQuestiondata = _context.ChildQuestions.Where(m => m.QuestionId == DroppQuestiondata.QuestionId).OrderBy(m => m.QuestionNumber).ToList();
                if (CQuestiondata != null)
                {
                    foreach (var item1 in CQuestiondata)
                    {
                        var qs = _context.ChildQuestions.Find(item1.ChildQuestionId);
                        char a = qs.QuestionNumber[qs.QuestionNumber.Length - 1];
                        qs.QuestionNumber = "" + ps.QuestionNumber + "" + a + "";
                        await _context.SaveChangesAsync();
                    }
                }


            }
            return Res;
        }
        public QuestionUpdate GetAllMultipleCheckBoxList(Guid QuestionId, Guid QuestioneerId)
        {
            QuestionUpdate model = new QuestionUpdate();
            if (QuestionId != Guid.Empty)
            {
                model.OptionList = (from j in _context.Options
                                    where j.QuestionId == QuestionId
                                    select new OptionsViewModel
                                    {
                                        OptionId = j.OptionId,
                                        OptionText = j.OptionText
                                    }).ToList();
            }
            return model;
        }

        public async Task<DbResponce> UpdateOptions(List<OptionUpdate> optionValue = null, string QuestionId = null, string QuestionName = null,string Type=null,bool isMandatory=false)
        {
            DbResponce MSG = new DbResponce();
            try
            {
                var questionId = new Guid(QuestionId);
                if (questionId != Guid.Empty)
                {
                    if(Type== "ChieldQuestion")
                    {
                        var data1 = _context.ChildQuestions.Find(questionId);
                        data1.QuestionText = QuestionName;
                        data1.isMandatory = isMandatory;
                        _context.Update(data1);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var data1 = _context.Questions.Find(questionId);
                        data1.QuestionText = QuestionName;
                        data1.isMandatory = isMandatory;
                        _context.Update(data1);
                        _context.SaveChanges();
                    }
                   
                }
                  var resdata = _context.Options.Where(m => m.QuestionId == questionId).ToList();
                if (resdata != null)
                {
                    foreach (var item in resdata)
                    {
                        var d = optionValue.Where(m => m.OptionId == item.OptionId.ToString()).FirstOrDefault();
                        if(d==null)
                        {
                            var employee = _context.Options.Find(item.OptionId);
                            _context.Options.Remove(employee);
                            _context.SaveChanges();
                        }
                    }

                    }
                if (optionValue != null)
                {
                    foreach (var item in optionValue)
                    {
                        if (item.OptionId != null && item.OptionId != "")
                        {
                            var da = _context.Options.Find(new Guid(item.OptionId));
                            da.OptionText = item.OptionText;
                            _context.Update(da);
                        }
                        else
                        {
                            if (item.OptionText != "" && item.OptionText != null)
                            {
                                Options data = new Options();
                                data.OptionId = Guid.NewGuid();
                                data.OptionText = item.OptionText;
                                data.QuestionId = questionId;
                                _context.Add(data);
                               // _context.SaveChangesAsync();
                            }
                        }
                    }

                    var res = await _context.SaveChangesAsync();
                   // MSG.msg = res.ToString();

                }
                return MSG;
            }
            catch (Exception ex) { throw; }


        }

        public async Task<int> EditQuestionnaireService(AddNewQuestionnair model)
        {
            var Res = 0;
            try
            {
                if (model.QuestionnaireId != Guid.Empty)
                {
                    var da = _context.Questionnaire.Find(model.QuestionnaireId);
                    da.Name = model.Name;
                    _context.Update(da);
                }
                Res = await _context.SaveChangesAsync();
            }
            catch (Exception ex) { throw; }
            return Res;

        }

        public async Task<DbResponce> HideUnHideUpdate(string Toggle, Guid questionnaireId)
        {
            DbResponce MSG = new DbResponce();
            try
            {
                if (questionnaireId != Guid.Empty)
                {
                    var data = _context.Questionnaire.Where(m => m.QuestionnaireId == questionnaireId).FirstOrDefault();
                    if (data != null)
                    {
                        var ps = _context.Questionnaire.Find(data.QuestionnaireId);
                        ps.IsHide = Convert.ToBoolean(Toggle);
                        _context.Update(ps);
                    }
                }
                var res = await _context.SaveChangesAsync();
                MSG.msg = res.ToString();
                return MSG;
            }
            catch (Exception ex) { throw; }
        }
    }

}

using Microsoft.AspNetCore.Http;
using OnlineScoping.Migrations;
using OnlineScoping.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class CustomerQuestionnairViewModel
    {
        public Guid CustomerQuestionnairId { get; set; }
        public Guid QuestionnaireId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsQuestionnaire { get; set; }
        public string Nmae { get; set; }
        public string ClientNmae { get; set; }
        public string SalesNmae { get; set; }
        public string CustomerName { get; set; }
        public string FileName { get; set; }
        public List<QuestionnaireViewModelAll> QuestionnaireList { get; set; }
        public List<CustomerQuestionnairViewModel> CustomerQuestionnairList { get; set; }
        public List<CustomerViewModel> CustomerViewList { get; set; }
        public List<ProjectsViewModel> ProjectsViewList { get; set; }
        public CustomerProposalViewModel ProposalViewModel { get; set; }

        public QuestionnairDetailsViewModel QuestionnairDetailsViewdata { get; set; }
        public ProjectsViewModel ProjectsViewModeldata { get; set; }
        public QuestionnaireViewModel QuestionnaireData { get; set; }
        public UploadedDocumentsViewModel UploadedDocumentsData { get; set; }
        public Methodology_DeliverablesViewModel Methodology_DeliverablesViewModelData { get; set; }
        public AddNewQuestionnair AddNewQuestionnairData { get; set; }
        public List<ProjectsViewModel> UploadedDocViewList { get; set; }
        public CustomerQuestionnairViewModel() { }

        public CustomerQuestionnairViewModel(CustomerQuestionnair CustomerQuestionnair)
        {
            CustomerId = CustomerQuestionnair.CustomerId;
            CustomerQuestionnairId = CustomerQuestionnair.CustomerQuestionnairId;
            QuestionnaireId = CustomerQuestionnair.QuestionnaireId;
            CreatedBy = CustomerQuestionnair.CreatedBy;
            CreatedDate = CustomerQuestionnair.CreatedDate;
            UpdatedDate = CustomerQuestionnair.UpdatedDate;
            IsQuestionnaire = CustomerQuestionnair.IsQuestionnaire;
        }

        public List<Status> StatusList = new List<Status>{new Status{
            Id="1",Name="Request Awaiting Review"},
        new Status{Id="2",Name="Request Under Review"},
        new Status{Id="3",Name="Scope Finalised/Pre Req’s required"},
        new Status{Id="4",Name="Booking Confirmed"},      
        };

    }
    public class Status
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class ProjectCustomerQuestionnairModel
    {
        public Guid CustomerQuestionnairId { get; set; }
        public Guid QuestionnaireId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public decimal DaysTesting { get; set; }
        public decimal DaysReporting { get; set; }
        public decimal DaysSum { get; set; }
        public decimal CastSum { get; set; }
        public string RequirementSum { get; set; }
        public ProjectsViewModel ProjectsViewModeldata { get; set; }
        public QuestionnaireViewModel QuestionnaireData { get; set; }
        public List<QuestionsViewModel> Questions { get; set; }
        public List<QuestionsViewModel> QuestionsData { get; set; }
        public Methodology_DeliverablesViewModel Methodology_DeliverablesData { get; set; }
    }
    public class AddNewQuestionnair
    {
  
        public Guid QuestionnaireId { get; set; }
        [Required]
        [Display(Name = "Questionnaire Name")]
        public string Name { get; set; }
       
    }
}

using OnlineScoping.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class QuestionnaireViewModelAll
    {
        
        public Guid QuestionnaireId { get; set; }
        public string Name { get; set; }
        public bool IsHide { get; set; }
        public string IsHideValue { get; set; }
        public QuestionnaireViewModelAll() { }
        public QuestionnaireViewModelAll(Questionnaire Questionnaire)
        {
            QuestionnaireId = Questionnaire.QuestionnaireId;
           Name = Questionnaire.Name;
            IsHide = Questionnaire.IsHide;
        }
    }


    public class QuestionnaireReportDataModel
    {

        public Guid QuestionnaireId { get; set; }
        public string Name { get; set; }
        public decimal QuantityDays { get; set; }
        public decimal CastSum { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DaysTesting { get; set; }
        public decimal DaysReporting { get; set; }
        public decimal Total { get; set; }
        public string PenTestTime { get; set; }
        public decimal TotalDaysSum { get; set; }
        public List<QuestionsViewModel> Questions { get; set; }
        public QuestionnairDetailsViewModel QuestionnairDetailsViewdata { get; set; }
        public ProjectsViewModel ProjectsViewdata { get; set; }

    }

}

using OnlineScoping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class QuestionnairDetailsViewModel
    {
        public Guid QuestionnairDetailsId { get; set; }
        public Guid QuestionnaireId { get; set; }
        public Guid CustomerId { get; set; }
        public string ProjectName { get; set; }
        public string CustomerName { get; set; }
        public string ApplicationName { get; set; }
        public DateTime CreateDate { get; set; }

        public QuestionnairDetailsViewModel(){}
        public QuestionnairDetailsViewModel(QuestionnairDetails QuestionnairDetails)
        {
            QuestionnairDetailsId = QuestionnairDetails.QuestionnairDetailsId;
            QuestionnaireId = QuestionnairDetails.QuestionnaireId;
            CustomerId = QuestionnairDetails.CustomerId;
            ProjectName = QuestionnairDetails.ProjectName;
            CustomerName = QuestionnairDetails.CustomerName;
            ApplicationName = QuestionnairDetails.ApplicationName;
            CreateDate = QuestionnairDetails.CreateDate;
        }
    }
}

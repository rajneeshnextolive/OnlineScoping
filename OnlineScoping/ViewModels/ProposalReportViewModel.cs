using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class ProposalReportViewModel
    {
        public CustomerProposalViewModel CustomerProposalViewModel { get; set; }
        public CustomerViewModel CustomerViewModel { get; set; }
        public SalesViewModel SalesViewModel { get; set; }
        public QuestionnaireViewModel QuestionnaireViewModel { get; set; }
        public QuestionResponseViewModel QuestionResponseViewModel { get; set; }
        public QuestionsViewModel QuestionsViewModel { get; set; }
        public Methodology_DeliverablesViewModel Methodology_Deliverablesdata { get; set; }
        public List<Methodology_DeliverablesViewModel> Methodology_DeliverablesdataList { get; set; }
        public List<QuestionnaireReportDataModel> QuestionnaireList { get; set; }
        public List<AllProjectsViewModel> AllProjectsList { get; set; }
        public List<QuestionnaireViewModel> QuestionnaireMethodology_DeliverablesList { get; set; }
        public AllProjectsViewModel ProjectsAllData { get; set; }
        public ProjectsViewModel Projectsdata { get; set; }
        public decimal TotalExcludingVAT { get; set; }
        public decimal TotalExcludingDay { get; set; }
        public string QName { get; set; }

        public decimal TotalDaysSum { get; set; }
        public decimal TotalCastSum { get; set; }
        public string TotalRequirementSum { get; set; }
        public Guid CustomerId { get; set; }
    }
}

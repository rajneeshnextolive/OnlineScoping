using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class QuestionnaireViewModel
    {
        public Guid CustomerId { get; set; }
        public Guid QuestionnaireId { get; set; }
        public Guid ProjectId { get; set; }
        public int? SectionId { get; set; }
        public string UserId { get; set; }
        public string Pages { get; set; }
        public string Name { get; set; }
      //  public bool isMandatory { get; set; }

        public List<QuestionsViewModel> Questions { get; set; }
        public QuestionsViewModel QuestionsViewModel { get; set; }
        public QuestionnaireViewModel QuestionnaireData { get; set; }
      
        public List<PCIQuestionnaireViewModel> PCIQuestionnaireList { get; set; }
        public decimal DaysSum { get; set; }
        public decimal CastSum { get; set; }
        public string RequirementSum { get; set; }
        [Required]
        public string ProjectName { get; set; }

        public string CustomerName { get; set; }
        [Required]
        public string ApplicationName { get; set; }
        public string ScopeTypeMethodolgy { get; set; }
        public string ScopeTypeMethodolgy_FileName { get; set; }
        public string ScopeTypeSampleReport { get; set; }
        public string ScopeTypeSampleReport_FileName { get; set; }

        public CEQQuestionsViewModel CEQQuestionsViewModel { get; set; }
        public QuestionnairDetailsViewModel QuestionnairDetailsViewdata { get; set; }
        public ProjectsViewModel ProjectsViewdata { get; set; }
        public Methodology_DeliverablesViewModel Methodology_Deliverablesdata { get; set; }
        public List<CEQQuestionsViewModel> CEQQuestions { get; set; }     
        public QuestionUpdate QuestionUpdateData { get; set; }
        public QuestOptions QuestiQuestionData { get; set; }
        public EquestionViewModel EquestionViewModelData { get; set; }     
        public List<MethodologyeViewModel> MethodologyeList { get; set; }
        public List<MethodologyeViewModel> SampleReportList { get; set; }
    }

    public class CyberEssentialsQuestionnaireModel
    {

        public CustomerProposalViewModel CustomerProposalViewModel { get; set; }
        public CustomerViewModel CustomerViewModel { get; set; }
        public SalesViewModel SalesViewModel { get; set; }
        public QuestionnaireViewModel QuestionnaireViewModel { get; set; }

        public QuestionnaireDataModel QuestionnaireData { get; set; }
        public CEQOrgDetailsViewModel OrganisationData { get; set; }
        public CEQSignatureViewModel CEQSignatureData { get; set; }
        public List<CEQRemoteVulnScanViewModel> RvsData { get; set; }
        public List<CEQWorkAssessViewModel> WaAssesData { get; set; }

        public List<IndustrySectorListModel> IndustrySectorList { get; set; }
        public List<CEQQuestionsViewModel> CEQQuestionsViewModelList { get; set; }
        public List<CEQCloudSharedServicesAssesViewModel> CEQCloudSharedServicesAssesList { get; set; }
        public List<CEQQuestionResponseViewModel> CEQQuestionResponseViewModelList { get; set; }
        public List<CEQQuestionResponseResult> CEQQuestionResponseResultList { get; set; }
        public Guid CustomerId { get; set; }
        public Guid QuestionnaireId { get; set; }
        public Guid ProjectId { get; set; }
        //  public List<QuestionsModel> QuestionList { get; set; }
        public List<CyberEssentialsQuestionnaireSectionsViewModel> SectionList { get; set; }
        // public List<QuestionnaireDataModel> QuestionnaireDataList { get; set; }
        public Methodology_DeliverablesViewModel Methodology_Deliverablesdata { get; set; }
        public string Organisation { get; set; }
        public string Remote { get; set; }
        public string Workstation { get; set; }
        public string Cloud { get; set; }
        public string Security { get; set; }
        public string Signature { get; set; }
        public string Pages { get; set; }
        public CEQQuestUpdate CEQQuetUpdateData { get; set; }
        public List<MethodologyeViewModel> MethodologyeList { get; set; }
        public List<MethodologyeViewModel> SampleReportList { get; set; }


    }


    public class QuestionnaireDataModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
        public int CustomerId { get; set; }
        public string SelectedOptions { get; set; }
        public string Status { get; set; }
        public string OrganisationName { get; set; }
        public string RegisteredAddress { get; set; }
        public string Company_CharityNumber { get; set; }
        public string Sector { get; set; }
        public string Turnover { get; set; }
        public string NumberOfEmployees { get; set; }
        public string MainContactName { get; set; }
        public string ContactJobTitle { get; set; }
        public DateTime? DateOfResponse { get; set; }
        public string ContactEmail { get; set; }
        public string ContactTelephone { get; set; }
        public string CELevelDesired { get; set; }
        public string CustomerName { get; set; }
        public string SignatureFile { get; set; }
        public DateTime? DateOfSignature { get; set; }
        public string PrintName { get; set; }
        public string JobTitle { get; set; }
        public string BusinessUnitName { get; set; }
        public bool QuestionnaireStatus { get; set; }
        public string SignatureOrganisationName { get; set; }
        public string TotalMarks { get; set; }
        public string TotalWeight { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<QuestionnaireDataModel> QuestionnaireDataModelList { get; set; }
        public List<QuestionnaireDataModel> CompleteReportList { get; set; }
        public List<QuestionnaireDataModel> QAApprovedList { get; set; }
        public List<QuestionnaireDataModel> MinorUpdatesRequiredList { get; set; }
        public List<QuestionnaireDataModel> RequiresFurtherWorkList { get; set; }
        public List<QuestionnaireDataModel> QARejectedList { get; set; }
       
        public List<Status1> StatusList = new List<Status1>{
        new Status1{Id=1,status="QA Approved "},
        new Status1{Id=2,status="Minor Updates Required "},
        new Status1{Id=3,status="Requires Further Work "},
        new Status1{Id=4,status="Rejected"}
        };
        public string UserName { get; set; }
        public string strDateOfResponse { get; set; }

       // public List<RemoteVulnerabilityScanModel> RemoteVulnerabilityScanList { get; set; }
    }


    public class Status1
    {
        public int Id { get; set; }

        public string status { get; set; }
    }

    public class IndustrySectorListModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
    }

    public class TestData
    {
        public string Value { get; set; }
        public string QuestionId { get; set; }
    }
    public class TestData1
    {
        public int Days { get; set; }  
        public int Cost { get; set; }
        public string Requirement { get; set; }

        public string QuestionId { get; set; }
    }
    public class OptionUpdate
    {   
        public string OptionId { get; set; }
        public string OptionText { get; set; }
    }

    public class QuestionUpdate
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string ChieldQuestionType { get; set; }
        public string SectionId { get; set; }
        public bool isMandatory { get; set; }
        public Guid QuestionId { get; set; }
        public Guid QuestionnaireId { get; set; }
        [Display(Name = "Checkbox Name")]
        public string[] CheckboxText { get; set; }
        public bool CheckBoxCheck { get; set; }
        public List<OptionsViewModel> OptionList { get; set; }
        public List<AddNewOption> AddNewOptionList { get; set; }
    }
    public class CEQQuestUpdate
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string CEQuestionType { get; set; }
        public string SectionId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid QuestionnaireId { get; set; }
    }


    public class AddNewOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class QuestOptions
    {
        public string[] name { get; set; }
      
    }
}

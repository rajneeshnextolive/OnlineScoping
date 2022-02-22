using OnlineScoping.Models;
using OnlineScoping.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Services
{
   public interface IQuestionnaireService
    {   
        public QuestionnaireViewModel GetQuestionsById(int? id,Guid QuestionnaireId);
        public Task<QuestionResponseViewModel> ResponseQuestions(QuestionnaireViewModel QuestionnaireView);
        public Task<QuestionResponseViewModel> ResponseQuestionsUpdate(QuestionnaireViewModel QuestionnaireView);
        public Task<int> CustomerUpdate(QuestionnaireViewModel QuestionnaireView);
        public Task<int> ResponseQuestionsbySection(QuestionnaireViewModel QuestionnaireView);

        public QuestionnaireViewModel GetResponseQuestionsById(Guid? id, Guid QuestionnaireId,Guid ProjectId);
        public QuestionnaireViewModel ResponseDetailsByProjectId(Guid? id, Guid QuestionnaireId,Guid Projectid);
        public QuestionnaireViewModel GetQuestionsByModule(Guid id);

        public Task<QuestionResponseViewModel> ResponseModuleWiseQuestions(QuestionnaireViewModel QuestionnaireView);
        public QuestionnaireViewModel GetResponseQuestionsById1(Guid? id, Guid QuestionnaireId,Guid ProjectId);
        public QuestionnaireViewModel GetResponseQuestionsProjectById(Guid? id, Guid? Projectid, Guid QuestionnaireId);
        public Methodology_DeliverablesViewModel Methodology_DeliverablesdataProjectById(Guid? id, Guid? Projectid, Guid QuestionnaireId);
        public QuestionnaireViewModel GetQuestionsById(Guid QuestionnaireId);


        List<QuestionnaireViewModelAll> QuestionnaireAllList();
        List<QuestionnaireReportDataModel> QuestionnaireAllListForReport();
        List<QuestionnaireReportDataModel> QuestionnaireListForReportWithDetails(Guid id,Guid Projectid);

        List<CustomerQuestionnairViewModel> CustomerQuestionnairList(Guid id);
        List<CustomerQuestionnairViewModel> ScopRequestCustomerQuestionnairList(Guid id);
        public Task<CustomerProposal> CustomerReportDataSave(CustomerQuestionnairViewModel model);

         public CustomerProposalViewModel CustomerProposalAllData(Guid QuestionnaireId);
         public CustomerViewModel CustomerAllData(Guid id);
         public SalesViewModel SalesAllData(Guid id);
         public QuestionnaireViewModel GetResponseQuestionsByProposalId(Guid id,Guid CustomerProposalId,Guid ProjectId);
        public Task<DbResponce> PCIQuestions(string rVSData, string ques2 = null, string ques3 = null, string ques4 = null, string ques5 = null,string UserId=null,string ques1=null, string ProjectName = null, string CustomerName = null,string ProjectId=null, string Methodolgy = null, string Methodolgy_FileName = null, string SampleReport = null, string SampleReport_FileName = null);


        public QuestionnaireViewModel GetResponseQuestionsById3(Guid? id, Guid QuestionnaireId,Guid ProjectId);
        public QuestionnaireViewModel GetResponseQuestionsByProjectId(Guid? id,Guid Projectid, Guid QuestionnaireId);
        public List<CustomerViewModel> CustomerViewList();
      

        public Task<QuestionResponseViewModel> UpdateResponseModuleWiseQuestions(QuestionnaireViewModel QuestionnaireView);
        public Task<QuestionResponseViewModel> ChildQuestionUpdate(QuestionnaireViewModel Questionnaire);
        public Task<int> DeletePCITableQuest(Guid? id);
        public Task<DbResponce> UpdateResponseModuleWiseQuestionsPCI(string rVSData, string ques2 = null, string ques3 = null, string ques4 = null, string ques5 = null, string UserId = null, string ques1 = null, string res1 = null, string res2 = null, string res3 = null, string res4 = null, string res5 = null, string CustomerId = null, string ProjectId = null, string Methodolgy = null, string Methodolgy_FileName = null, string SampleReport = null, string SampleReport_FileName = null);
        public List<IndustrySectorListModel> IndustrySectorListData();
        public List<CyberEssentialsQuestionnaireSectionsViewModel> CEQSectionList();
        public Task<int> saveOrganisationDetails(CEQOrgDetailsViewModel CEQOrgDetailsView);
        public Task<int> saveSignatureDetails(CEQSignatureViewModel CEQSignatureView);
        public List<CEQQuestionsViewModel> CEQQuestionsList();
        //CEQ-WAT
        public Task<DbResponce> SaveWAT(string WATabledata, string UserId = null,string ProjectId=null,string orgcustId=null );
        public Task<DbResponce> SaveCloud(string CloudTabledata, string UserId = null,string ProjectId=null, string orgcustId = null);
        public Task<DbResponce> SaveRVSQuestions(string rVSData, string UserId = null, string[] options = null,string ProjectId=null , string orgcustId=null );
         public Task<DbResponce> saveQuestionnaire(List<TestData> optionValue = null, string UserId = null, string ProjectId = null, string orgcustId = null);
         public Task<DbResponce> saveQuestionnaire1(List<TestData1> optionValue = null);
         public Task<DbResponce> saveMethodology_Deliverables(string ScopeTypeMethodolgy = null, string ScopeTypeMethodolgy_FileName = null, string ScopeTypeSampleReport = null, string ScopeTypeSampleReport_FileName = null, string Methodology_DeliverablesId = null);

        public CEQOrgDetailsViewModel organisationData(Guid id, string UserId,Guid ProjectId);
        public CEQSignatureViewModel cEQSignatureData(Guid id, string UserId, Guid ProjectId);
        public List<CEQRemoteVulnScanViewModel> RvsData(Guid id, string UserId, Guid ProjectId);  
        public List<CEQWorkAssessViewModel> WaAssesData(Guid id, string UserId, Guid ProjectId);
        public List<CEQCloudSharedServicesAssesViewModel> cEQCloudSharedServicesAssesList(Guid id, string UserId, Guid ProjectId);
        public List<CEQQuestionResponseViewModel> cEQQuestionResponseViewModelList(Guid id, string UserId, Guid ProjectId);
        public List<CEQQuestionsViewModel> CEQQuestionsList2(Guid id, string UserId, Guid ProjectId);
        public Task<int> ECQFinalSubmit(Guid Questionnaire,Guid ProjectId, string UserId = null);
        public Task<int> CyberEssentialsQuestionnaireUpdate(Guid Questionnaire, Guid UserId);
        public List<CEQQuestionResponseResult> cEQQuestionResponseResult(CyberEssentialsQuestionnaireModel model);
        public Task<QuestionResponseViewModel> UpdateAllQuestions(QuestionnaireViewModel QuestionnaireView);
        List<ProjectsViewModel> ProjectsViewModelList(Guid id);
        //List<ProjectsViewModel> ProjectsViewModelList1(string name);
        List<ProjectsViewModel> UploadedDocumentsViewModel(Guid id);
        List<ProjectsViewModel> ProjectsViewModelListque(Guid id);
        public ProjectsViewModel GetProjectById(Guid id);
        public int DeleteCustomerQuestionnairsByProjectId(Guid? id, Guid QuestionnaireId, Guid ProjectId);
        public int DeleteCustomerPciQuestionnairsByProjectId(Guid? id, Guid QuestionnaireId, Guid ProjectId);
        public int DeleteCustomerCeqQuestionnairsByProjectId(Guid? id, Guid QuestionnaireId, Guid ProjectId);
        //public int DeleteCustomerSecurityQuestionnairsByProjectId(Guid? id, Guid QuestionnaireId, Guid ProjectId);
        public AllProjectsViewModel AllProjectsData(Guid id, Guid ProjectId);
        public ProjectsViewModel projectsdata(Guid id, Guid ProjectId);
        public Methodology_DeliverablesViewModel AdminMethodology_Deliverablesdata(Guid id);
        public List<Methodology_DeliverablesViewModel> Methodology_DeliverablesdataProjectById1(Guid? id, Guid? Projectid);
        public List<QuestionnaireViewModel> AdminMethodology_List_BYQuestionnaire(Guid? id, Guid? Projectid);
        public Task<int> UpdateSingleQuestions(QuestionnaireViewModel QuestionnaireView);
        public Task<int> AddNewSingleQuestions(QuestionnaireViewModel QuestionnaireView);
        public Task<int> DeleteSingleQuestions(Guid id, Guid QuestionnaireId);
        public Task<int> DeleteSingleChieldQuestions(Guid id, Guid QuestionnaireId);
        public Task<int> AddNewSecurityScopingSingleQuestions(QuestionnaireViewModel QuestionnaireView);
        public Task<Equestion> EquestionViewModel(EquestionViewModel EquestionViewMod);
        public EquestionViewModel EquationData(Guid id);
        public Task<int> UpdateSingleCEQQuestions(CyberEssentialsQuestionnaireModel QuestionnaireView);
        public Task<int> DeleteSingleCEQQuestions(Guid id, Guid QuestionnaireId);
        public Task<int> _DeleteSingleQuestionnaire(Guid id);
        public Task<int> AddNewQuestionnaireService(AddNewQuestionnair model);
        public List<MethodologyeViewModel> Methodologyelist();
        public List<MethodologyeViewModel> SampleReportlist();
        public QuestionnaireViewModel AddNeQuestions(List<QuestOptions> options,string questionName, string questionType, string questioneerId);
        public Task<QuestionnaireViewModel> AddNewSecuirtyQuestions(List<QuestOptions> options,string questionName, string questionType, string questioneerId,string sectionId);
        public Task<int> AddNewSecuirtyQuestions2(List<QuestOptions> options,string questionName, string questionType, string questioneerId,string sectionId);

        public Task<DbResponce> UpdateProjectStatus(string Status, string CustomerId = null, string ProjectsId = null);

        public Task<int> AddNewSingleSubQuestions(QuestionnaireViewModel QuestionnaireView);
        public Task<int> UpdateAllVar(QuestionnaireViewModel QuestionnaireView);
        public Task<int> DragDropQuestion(Guid draggId, Guid DroppedId,Guid questioneerId);
        public QuestionUpdate GetAllMultipleCheckBoxList(Guid QuestionId,Guid QuestioneerId);

        public Task<DbResponce> UpdateOptions(List<OptionUpdate> optionValue = null, string QuestionId = null, string QuestionName=null,string Type=null,bool isMandatory=false);
        public Task<int> EditQuestionnaireService(AddNewQuestionnair model);
        public Task<DbResponce> HideUnHideUpdate(string Toggle, Guid questionnaireId);
    
    }
}

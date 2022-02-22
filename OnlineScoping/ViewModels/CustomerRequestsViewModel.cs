using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class CustomerRequestsViewModel
    {
        public Guid CustomerRequestsId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid SalesId { get; set; }
        public bool IsAccept { get; set; }
        public bool IsSeen { get; set; }
        public string ProjectName { get; set; }
        public string CustomerName { get; set; }
        public string Image { get; set; }
        public string FileName { get; set; }
        public List<UploadedDocumentsViewModel> UploadedDocumentList { get; set; }
        public List<CustomerQuestionnairViewModel> QDataList { get; set; }
        public List<MethodologyeViewModel> MethodologyeViewModelList { get; set; }
        public ProjectsViewModel ProjectsData { get; set; }
        public List<ProjectCustomerQuestionnairModel> ProjectCustomerQuestionnairList { get; set; }


    }
}

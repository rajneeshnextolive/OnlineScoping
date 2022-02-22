using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class ProjectsViewModel
    {
        public Guid ProjectsId { get; set; }
        public Guid CustomerId { get; set; }
        [Required(ErrorMessage = "Project Name is required")]
        public string ProjectName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public string CustomerName { get; set; }
        public string ApplicationName { get; set; }
        public string Nmae { get; set; }
        public string ClientNmae { get; set; }
        public string SalesNmae { get; set; }
        public string RequestStatus { get; set; }
        public List<CustomerQuestionnairViewModel> QDataList { get; set; }
        public Guid CustomerRequestsId { get; set; }
        public List<UploadedDocumentsViewModel> UplodDocList { get; set; }
        public List<UploadedDocumentsViewModel> UplodAdditionalDocList { get; set; }
        public List<UploadedDocumentsViewModel> ProchekupUploadList { get; set; }
    }


    public class AllProjectsViewModel
    {
        public Guid ProjectsId { get; set; }
        public Guid CustomerId { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public string CustomerName { get; set; }
        public string ApplicationName { get; set; }
        public List<ProjectCustomerQuestionnairModel> QDataList { get; set; }
    }
}

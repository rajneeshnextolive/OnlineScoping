using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class CEQCloudSharedServicesAssesViewModel
    {
        public Guid CEQCloudSharedServicesAssesId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid QuestionnaireId { get; set; }
        public string DescriptionOfService { get; set; }
        public string Supplier { get; set; }
        public string IndependentAuditStandards { get; set; }
        public string EvidenceOfcertification { get; set; }
        public int Cost { get; set; }
        public int Days { get; set; }
        public string Requirement { get; set; }
    }
}

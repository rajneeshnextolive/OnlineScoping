using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Models
{
    public class CEQCloudSharedServicesAsses
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CEQCloudSharedServicesAssesId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid QuestionnaireId { get; set; }
        public Guid ProjectId { get; set; }
        public string DescriptionOfService { get; set; }
        public string Supplier { get; set; }
        public string IndependentAuditStandards { get; set; }
        public string EvidenceOfcertification { get; set; }
        public int Cost { get; set; }
        public int Days { get; set; }
        public string Requirement { get; set; }
    }
}

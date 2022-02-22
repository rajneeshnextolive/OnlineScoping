using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Models
{
    public class CEQRemoteVulnScan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CEQRemoteVulnScanId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid QuestionnaireId { get; set; }
        public Guid ProjectId { get; set; }
        public string IPAddressv4andv6addresses { get; set; }
        public string FullyQualifiedDomainName { get; set; }
        public string NatureandDescriptionofSystem { get; set; }
        public string SystemOwnershipandHosting { get; set; }
        public string Ifoutofscopepleaseciteareasonwhy { get; set; }
        public int Cost { get; set; }
        public int Days { get; set; }
        public string Requirement { get; set; }
    }
}

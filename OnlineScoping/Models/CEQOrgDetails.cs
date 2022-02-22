using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Models
{
    public class CEQOrgDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CEQOrgDetailsId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid QuestionnaireId { get; set; }
        public Guid ProjectId { get; set; }
        public string Nameoforganisation { get; set; }
        public string RegisteredAddress { get; set; }
        public string CompanyCharityNumber { get; set; }
        public string Sector { get; set; }
        public string Turnover { get; set; }
        public string NumberofEmployees { get; set; }
        public string Nameofmaincontact { get; set; }
        public string ContactJobtitle { get; set; }
        public string Dateofresponse { get; set; }
        public string ContactEmail { get; set; }
        public string ContactTelephone { get; set; }
        public string CELevelDesired { get; set; }
        public string CanCRESTpubliciseyoursuccessfulcertification { get; set; }
        public int Cost { get; set; }
        public int Days { get; set; }
        public string Requirement { get; set; }
    }
}

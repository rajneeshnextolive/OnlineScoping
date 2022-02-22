using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Models
{
    public class Admin_Methodology_Deliverables
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Methodology_DeliverablesId { get; set; }
        public Guid QuestionnaireId { get; set; }
        public Guid MethodologyId { get; set; }
        public Guid MethodologyReportId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ScopeTypeMethodolgy { get; set; }
        public string ScopeTypeMethodolgy_FileName { get; set; }
        public string ScopeTypeSampleReport { get; set; }
        public string ScopeTypeSampleReport_FileName { get; set; }
    }
}

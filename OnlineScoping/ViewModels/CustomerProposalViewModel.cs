using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class CustomerProposalViewModel
    {

        public Guid CustomerProposalId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Document Reference")]
        public string DocumentReference { get; set; }
        public Guid SalesId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid QuestionnaireId { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}

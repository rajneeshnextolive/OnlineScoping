using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class EquestionViewModel
    {
        public Guid Id { get; set; }
        public string DaysEquestion { get; set; }
        public string CostEquestion { get; set; }
        public string RequirmentEquestion { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public Guid QuestionnaireId { get; set; }

        [Display(Name = "Equestion")]
        [Required(ErrorMessage = "Equestion is required")]
        //[RegularExpression(@"^([A^B-Z])$",
        //ErrorMessage = "Equestion is not valid")]
        public string CreatedFormula { get; set; }
        public string EquestionType { get; set; }
        public EquestionViewModel EquestionViewModelData { get; set; }

    }
}

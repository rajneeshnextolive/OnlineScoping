using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Models
{
    public class Options
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OptionId { get; set; }
        public Guid QuestionId { get; set; }  
        public string OptionText { get; set; }
        public bool res { get; set; }
         public bool OptionValue { get; set; }
    }
}

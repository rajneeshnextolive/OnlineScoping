using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Models
{
    public class CEQOptions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CEQOptionId { get; set; }
        public Guid QuestionId { get; set; }
        public string OptionText { get; set; }
        public string Response { get; set; }
        public bool res { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class CEQOptionsViewModel
    {
        public Guid CEQOptionId { get; set; }
        public Guid QuestionId { get; set; }
        public string OptionText { get; set; }
        public bool res { get; set; }
        public string Response { get; set; }
    }
}

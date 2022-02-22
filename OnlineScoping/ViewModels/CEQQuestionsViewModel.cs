using OnlineScoping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class CEQQuestionsViewModel
    {
        public Guid CEQQuestionId { get; set; }
        public string QuestionNumber { get; set; }
        public int QuestionType { get; set; }
        public string QuestionText { get; set; }
        public string StandardRecommendation { get; set; }
        public Guid QuestionnaireId { get; set; }
        public string Title { get; set; }
        public int RowNumber { get; set; }
        public int Section { get; set; }
        public string ResponseSum { get; set; }

        public string VariableName { get; set; }
        public int Cost { get; set; }
        public int Days { get; set; }
        public string Requirement { get; set; }
        public List<CEQOptionsViewModel> CEQOptionsData { get; set; }
        public CEQQuestionResponse CEQOResponceData { get; set; }
    }
}

using OnlineScoping.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class ChildQuestionsViewModel
    {
        public Guid QuestionId { get; set; }
        public Guid ChildQuestionId { get; set; }
        public int SectionId { get; set; }
        public string QuestionNumber { get; set; }
        public QuestionType QuestionType { get; set; }
        public int OptionType { get; set; }
        public ChildQuestions QuestionData { get; set; }
        public string QuestionText { get; set; }
        public string UserName { get; set; }
        public List<OptionsViewModel> OptionsList { get; set; }
        public List<Options> OptionsList1 { get; set; }
        public Guid ResponseOptionId { get; set; }
        public int ResponseOptionId1 { get; set; }
        public DateTime ResponseDateTime { get; set; }
        public Guid QuestionnaireId { get; set; }
        //[Required(ErrorMessage = "The Question field is required")]
        public string Responsetext { get; set; }
        public Guid ResponseId { get; set; }
        public bool Chk { get; set; }

        public int Days { get; set; }
        public int Cost { get; set; }
        public string Requirement { get; set; }
        public string QuestionValue { get; set; }
        public bool isMandatory { get; set; }

        public string VariableName { get; set; }
        public int InputType { get; set; }
        public ChildQuestionsViewModel() { }

   

    }
}

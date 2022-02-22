using OnlineScoping.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class QuestionsViewModel
    {

        public Guid QuestionId { get; set; }
        public string QuestionId2 { get; set; }
        public int SectionId { get; set; }
        public string QuestionNumber { get; set; }
        public QuestionType QuestionType { get; set; }
        public Questions QuestionData { get; set; }
        public int OptionType { get; set; }
        public string QuestionText { get; set; }
        public string UserName { get; set; }
        public List<OptionsViewModel> OptionsList { get; set; }
        public List<OptionsViewModel> Equastionlis { get; set; }
        public List<ChildQuestionsViewModel> ChildQuestionsList { get; set; }
        public List<PCIQuestionnaireViewModel> PCIQuestionnaireList { get; set; }
        public List<Options> OptionsList1 { get; set; }
        public Guid ResponseOptionId { get; set; }
        public int ResponseOptionId1 { get; set; }
        public DateTime ResponseDateTime { get; set; }
        public Guid QuestionnaireId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public int RowNumber { get; set; }
        //[Required(ErrorMessage = "The Question field is required")]    
        public string Responsetext { get; set; }
        public QuestionsViewModel() { }

        public bool Chk { get; set; }
        public Guid chk1 { get; set; }

        public Guid ResponseId { get; set; }

        public int Days { get; set; }
        public int Cost { get; set; }
        public string Requirement { get; set; }
        public string VariableName { get; set; }
        public int InputType { get; set; }
        public string Note { get; set; }
        public string QuestionValue { get; set; }
        public bool isMandatory { get; set; }


    }








}

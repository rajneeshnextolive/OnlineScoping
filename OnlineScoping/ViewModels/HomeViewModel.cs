using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class HomeViewModel
    {

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Questionnaire { get; set; }
        public string ConfirmPassword { get; set; }
        public string RoleName { get; set; }
        public Guid Id { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UserId { get; set; }
        public Guid CustomerId { get; set; }
        public string FileUpload { get; set; }
        public IFormFile MyImage { set; get; }
        public List<CustomerViewModel> CustomerViewList { get; set; }
        public List<ProjectsViewModel> ProjectsViewList { get; set; }
        public CustomerViewModel CustomerViewData { get; set; }
        public SalesViewModel SalesViewData { get; set; }
        public ProjectsViewModel ProjectsViewData { get; set; }
        public List<QuestionnaireViewModelAll> QuestionnaireList { get; set; }
        public List<CustomerQuestionnairViewModel> CustomerQuestionnairList { get; set; }
        public UploadedDocumentsViewModel UploadedDocumentsData { get; set; }

        public HomeViewModel() { }


        public HomeViewModel GetAdmin()
        {
            return new HomeViewModel
            {
                Id = Id,
                UserName = UserName,
                Password = Password,
                RoleName = RoleName,
                Email = Email,

            };
        }

        }
}

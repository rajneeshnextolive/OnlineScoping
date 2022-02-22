using Microsoft.AspNetCore.Http;
using OnlineScoping.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineScoping.ViewModels
{
    public class CustomerViewModel
    {
        public Guid CustomerId { get; set; }

   
        [Required(ErrorMessage = "First Name is required")]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string UserId { get; set; }

      
        [DataType(DataType.Text)]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]

        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",
        ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Display(Name = "Is Email Sent")]
        public bool IsEmailSent { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
        public bool Questionnaire { get; set; }
        public Guid SalesId { get; set; }
        public string QuestionnaireId { get; set; }
        public DateTime RepliedDate { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime CurrentDateTime { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Client Name")]
        public string Client { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        [RegularExpression(@"^(((\+44)\s\s?|\([0-9]{1,2}\)\s\s?|\(\+[0-9]{1,2}\)\s\s?|(\+[0-9]{1,3)\s\s?|0)[\d]{3}\s\s?[\d]{3}\s\s?[\d]{4})|(((\+[0-9]{1,3})\s\s?|0)[\d]{3}\s?[\d]{3}\s?[\d]{4})|\((\d{3})\)[-\s]?\d{3}[-\s]?\d{4}|((0)[\d]{4}\s?[\d]{3}\s?[\d]{3})$", ErrorMessage = "Enter valid Phone Number")]

        public string MobileNumber { get; set; }

        [RegularExpression(@"^([a-zA-Z]{1,2}[0-9R][0-9a-zA-Z]? [0-9][a-zA-z0-9]{2})|([a-zA-Z]{1,2}[0-9R][0-9a-zA-Z][0-9][a-zA-Z0-9]{2})$", ErrorMessage = "Please enter valid zip.")]

        public string Zip { get; set; }
        public IFormFile MyImage { set; get; }
        public Guid CreatedBy { get; set; }
        public string? UserName { get; set; }
        [Display(Name = "User Name")]
        [Required]
        public string TempUserName { get; set; }
        public string CustomerUserName { get; set; }
        public int CustomerQuestionnaircount { get; set; }
        public int ProjectsViewList { get; set; }
        public List<SalesViewModel> SalseList { get; set; }
        [Display(Name = "Sales Manager")]
        public string SalesManager { get; set; }
        public List<QuestionnaireViewModelAll> QuestionnaireList { get; set; }
        public List<CustomerViewModel> CustomerViewModelList { get; set; }
        public CustomerViewModel() { }
        public string TempData { get; set; }
        public bool msg { get; set; }
        public bool Auth { get; set; }


        public AccountModel Accountmodel { get; set; }
       public AuthModel authmodel { get; set; }

        public CustomerViewModel(Customer customer)
        {
            CustomerId = customer.CustomerId;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            UserId = customer.UserId;
            Email = customer.Email;
            IsEmailSent = customer.IsEmailSent;
            Questionnaire = customer.Questionnaire;
            CreatedBy = customer.CreatedBy;
            RepliedDate = customer.RepliedDate;
            SendDate = customer.SendDate;
            Client = customer.Client;
            QuestionnaireId = customer.QuestionnaireId;
            SalesId = customer.SalesId;
            CreatedDate = customer.CreatedDate;
            UpdatedDate = customer.UpdatedDate;
            CurrentDateTime = DateTime.Now;
            Address1 = Address1;
            Address2 = Address2;
            City = City;
            State = State;
            Country = Country;
            MobileNumber = MobileNumber;
            Zip = Zip;
            TempUserName = TempUserName;
        }

        public Customer GetCustomer()
        {
            return new Customer
            {
                CustomerId = CustomerId,
                FirstName = FirstName,
                LastName = LastName,
                UserId = UserId,
                Email = Email,
                IsEmailSent = IsEmailSent,
                Questionnaire = Questionnaire,
                CreatedBy = CreatedBy,
                RepliedDate = RepliedDate,
                SendDate = SendDate,
                Client = Client,
                QuestionnaireId = QuestionnaireId,
                SalesId = SalesId,
                CreatedDate = CreatedDate,
                UpdatedDate = UpdatedDate,
                Address1 = Address1,
                Address2 = Address2,
                City = City,
                State = State,
                Country = Country,
                MobileNumber = MobileNumber,
                Zip= Zip,
                TempUserName= TempUserName,
            };
        }
    }


    public class AuthModel
    {
        public Guid Id { get; set; }
        public bool Auth { get; set; }

    }

}

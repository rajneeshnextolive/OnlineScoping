using Microsoft.AspNetCore.Http;
using OnlineScoping.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class SalesViewModel
    {
        public Guid CustomerId { get; set; }

        [Required]
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

        [Display(Name = "User Name")]
        [Required]
        public string TempUserName { get; set; }


        [Display(Name = "Is Email Sent")]
        public bool IsEmailSent { get; set; }

        public DateTime CreatedDate { get; set; }
        [RegularExpression(@"^(((\+44)\s\s?|\([0-9]{1,2}\)\s\s?|\(\+[0-9]{1,2}\)\s\s?|(\+[0-9]{1,3)\s\s?|0)[\d]{3}\s\s?[\d]{3}\s\s?[\d]{4})|(((\+[0-9]{1,3})\s\s?|0)[\d]{3}\s?[\d]{3}\s?[\d]{4})|\((\d{3})\)[-\s]?\d{3}[-\s]?\d{4}|((0)[\d]{4}\s?[\d]{3}\s?[\d]{3})$", ErrorMessage = "Enter valid Phone Number")]

        public string MobileNumber { get; set; }
        public string UserName { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Name { get; set; }
        public IFormFile MyImage { set; get; }
        public SalesViewModel() {}
        public bool msg { get; set; }
        public bool Auth { get; set; }

        public AccountModel Accountmodel { get; set; }
        public AuthModel authmodel { get; set; }

        public List<SalesViewModel> Salesviewmodel { get; set; }
        public SalesViewModel(Sales sale)
        {
            CustomerId = sale.CustomerId;
            FirstName = sale.FirstName;
            LastName = sale.LastName;
            UserId = sale.UserId;
            Email = sale.Email;
            IsEmailSent = sale.IsEmailSent;
            CreatedDate = sale.CreatedDate;
            UpdatedDate = sale.UpdatedDate;
            MobileNumber = sale.MobileNumber;
            TempUserName = sale.TempUserName;


        }


        public Sales GetSales()
        {
            return new Sales
            {
                CustomerId = CustomerId,
                FirstName = FirstName,
                LastName = LastName,
                UserId = UserId,
                Email = Email,
                IsEmailSent = IsEmailSent,
                CreatedDate = CreatedDate,
                UpdatedDate = UpdatedDate,
                MobileNumber= MobileNumber,
                TempUserName= TempUserName,
            };
        }


    }

    public class AuthSAlesModel
    {
        public Guid Id { get; set; }
        public bool Auth { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Email")]
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
        public string Client { get; set; }
        public string TempUserName { get; set; }
        public Guid CreatedBy { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string MobileNumber { get; set; }
        public string Zip { get; set; }
        public bool IsCustomerUpdate { get; set; }
    }
}

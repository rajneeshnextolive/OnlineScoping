using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FileName { get; set; }
        public bool Is2Fa { get; set; }
        public string OTP { get; set; }
    }
}

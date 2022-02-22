using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OnlineScoping.Data;
using OnlineScoping.Models;

namespace OnlineScoping.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OnlineScopingContext _context;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger,
            UserManager<ApplicationUser> userManager,
            OnlineScopingContext context)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            var UserId = _userManager.GetUserId(User); // Get user id:
            if(UserId!=null)
            {
                var data = _context.Users.Where(m => m.Id == UserId).FirstOrDefault();
                if(data!=null)
                {
                    if(data.Is2Fa==true)
                    {
                        returnUrl= Url.Content("~/AzureAD/Account/SignOut");
                    }else
                    {

                    }
                }
            }
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
               // return LocalRedirect(callurl);
            }
            else
            {
                return LocalRedirect("/");
                //  return RedirectToPage();
            }
        }
    }
}

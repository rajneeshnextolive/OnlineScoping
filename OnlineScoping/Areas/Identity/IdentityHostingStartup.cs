using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineScoping.Data;
using OnlineScoping.Models;

[assembly: HostingStartup(typeof(OnlineScoping.Areas.Identity.IdentityHostingStartup))]
namespace OnlineScoping.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {

                //services.AddDefaultIdentity<ApplicationUser>(options => {
                //    options.Password.RequireLowercase = false;
                //    options.Password.RequireUppercase = false;
                //    options.SignIn.RequireConfirmedAccount = false;
                //})
                //.AddEntityFrameworkStores<OnlineScopingContext>();

            });

            

        }
    }
}
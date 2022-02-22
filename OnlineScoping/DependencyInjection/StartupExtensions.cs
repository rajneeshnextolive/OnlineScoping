using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineScoping.Services;
using OnlineScoping.Services.imp;
using OnlineScoping.Utilities.EmailService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddServiceCollections(this IServiceCollection services)
        {
            services.AddScoped<ICustomersService, CustomersService>();
            services.AddScoped<ISalesService, SalesService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IQuestionnaireService, QuestionnaireService>();
            services.AddScoped<Ihome, HomeService>();
            return services;
        }
    }
}

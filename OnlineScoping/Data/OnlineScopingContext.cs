using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineScoping.Models;

namespace OnlineScoping.Data
{
    public class OnlineScopingContext : IdentityDbContext<ApplicationUser>
    {
        public OnlineScopingContext (DbContextOptions<OnlineScopingContext> options)
            : base(options)
        {
        }

        public DbSet<Sales> Sales { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Options> Options { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<QuestionResponse> QuestionResponse { get; set; }
        public DbSet<ChildQuestions> ChildQuestions { get; set; }
        public DbSet<Questionnaire> Questionnaire { get; set; }
        public DbSet<CustomerQuestionnair> CustomerQuestionnair { get; set; }
        public DbSet<CustomerProposal> CustomerProposal { get; set; }
        public DbSet<PCIQuestionnaire> PCIQuestionnaire { get; set; }
        public DbSet<CyberEssentialsIndustrySectorList> CyberEssentialsIndustrySectorList { get; set; }
        public DbSet<CEQCloudSharedServicesAsses> CEQCloudSharedServicesAsses { get; set; }
        public DbSet<CEQOptions> CEQOptions { get; set; }
        public DbSet<CEQOrgDetails> CEQOrgDetails { get; set; }
        public DbSet<CEQQuestionResponse> CEQQuestionResponse { get; set; }
        public DbSet<CEQQuestions> CEQQuestions { get; set; }
        public DbSet<CEQRemoteVulnScan> CEQRemoteVulnScan { get; set; }
        public DbSet<CEQWorkstationAssessment> CEQWorkstationAssessment { get; set; }
        public DbSet<CyberEssentialsQuestionnaireSections> CyberEssentialsQuestionnaireSections { get; set; }
        public DbSet<CEQSignature> CEQSignature { get; set; }
        public DbSet<QuestionnairDetails> QuestionnairDetails { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<Methodology_Deliverables> Methodology_Deliverables { get; set; }
        public DbSet<Admin_Methodology_Deliverables> Admin_Methodology_Deliverables { get; set; }
        public DbSet<CustomerRequests> CustomerRequests { get; set; }
        public DbSet<UploadedDocuments> UploadedDocuments { get; set; }

        public DbSet<Methodologye> Methodologye { get; set; }
        public DbSet<Equestion> Equestion { get; set; }
        public DbSet<UploadFiles> UploadFiles { get; set; }
        public DbSet<Permissions> Permissions { get; set; }






    }
}

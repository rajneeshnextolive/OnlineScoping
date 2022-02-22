using OnlineScoping.Models;
using OnlineScoping.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Services
{
    public interface ICustomersService
    {
        Task<Guid> CreateCustomer(Customer customer);
        Task<Guid> AddNewProject(ProjectsViewModel model);
        public Guid AddNewProjectNewFunction(ProjectsViewModel model);
        Task<Guid> UpdateProject(ProjectsViewModel model);
        public Guid UpdateProjectNewFunction(ProjectsViewModel model);
        Task<int> DeleteNewProject(Guid? id);
        bool CustomerExists(Guid id);
        Task<int> DeleteCustomerById(Guid? id);
        Task<Customer> GetCustomerById(Guid? id);
        Task<CustomerViewModel> NewGetCustomerById(Guid? id);
        Task<List<Customer>> GetCustomers();
        List<CustomerViewModel> GetCustomersAdmin();
        List<CustomerViewModel> NewGetCustomers(Guid UserId);
        Task<int> UpdateCustomer(Customer customer);
        Task<int> UpdateCustomerIsEmailSent(Customer customer);
        Task<int> UpdateCustomerUserId(Customer customer, string id);
        bool EmailExists(string email);

        List<CustomerViewModel> GetCustomersEmailVerified();
        List<CustomerViewModel> GetCustomersReplied();


        List<CustomerViewModel> NewGetCustomersEmailVerified(Guid UserId);
        List<CustomerViewModel> NewGetCustomersReplied(Guid UserId);
        List<SalesViewModel> GetSales();

        List<QuestionnaireViewModelAll> QuestionnaireAllList();

        List<CustomerQuestionnairViewModel> CustomerQuestionnairList(Guid id);
        List<ProjectsViewModel> ProjectsViewModelList(Guid id);
        List<ProjectsViewModel> ProjectsViewModelListWithFile(Guid id);


        ApplicationUser imagedata(string name);

        bool UserEmailExists(string Email);
        bool UsernameExists(string Username);
 
        public Task<AccountModel> ChangePassword(CustomerViewModel model);


        List<CustomerRequestsViewModel> CustomerRequestList(string name);

        List<UploadedDocumentsViewModel> CustomerDocUploList(string name);
        List<CustomerRequestsViewModel> CustomerRequestListByCustomerId(Guid id);
        List<CustomerRequestsViewModel> CustomerRequestProposalListByCustomerId(Guid id);
        List<CustomerRequestsViewModel> CustomerRequestProposalAllListByCustomerId(Guid id);

        List<CustomerRequestsViewModel> SalesAcceptList(string name);
        public int TotalProjectCount(string UserName);
        Task<int> AuthUpdate(string UserId,bool IsAuth);
        List<PermissionsViewModel> PermissionsAllList();
        public Task<DbResponce> PermissionsUpdate(string Toggle, Guid permissionsId);
        List<QuestionnaireViewModelAll> QuestionnaireAllClientList();

    }
}

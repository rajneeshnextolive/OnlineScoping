using Microsoft.EntityFrameworkCore;
using OnlineScoping.Data;
using OnlineScoping.Models;
using OnlineScoping.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace OnlineScoping.Services.imp
{
    public class CustomersService : ICustomersService
    {
        private readonly OnlineScopingContext _context;

        public CustomersService(OnlineScopingContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetCustomers()
        {
            try
            {
                return await _context.Customer.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CustomerViewModel> GetCustomersAdmin()
        {
            List<CustomerViewModel> model = new List<CustomerViewModel>();
            try
            {
                model = (from a in _context.Customer
                         select new CustomerViewModel
                         {
                             CustomerId = a.CustomerId,
                             FirstName = a.FirstName,
                             LastName = a.LastName,
                             UserId = a.UserId,
                             Email = a.Email,
                             IsEmailSent = a.IsEmailSent,
                             Questionnaire = a.Questionnaire,
                             CreatedBy = a.CreatedBy,
                             RepliedDate = a.RepliedDate,
                             CurrentDateTime = DateTime.Now,
                             SendDate = a.SendDate,
                             Client = a.Client,
                             QuestionnaireId = a.QuestionnaireId,
                             SalesId = a.SalesId,
                             CreatedDate = a.CreatedDate,
                             UpdatedDate = a.UpdatedDate,
                             Address1 = a.Address1,
                             Address2 = a.Address2,
                             City = a.City,
                             State = a.State,
                             Country = a.Country,
                             MobileNumber = a.MobileNumber,
                             Zip = a.Zip,
                             TempUserName = a.TempUserName,
                             CustomerQuestionnaircount = _context.CustomerQuestionnair.Where(t => t.CustomerId == a.CustomerId).Count(),
                             ProjectsViewList = _context.Projects.Where(t => t.CustomerId == a.CustomerId).Count(),
                             UserName = (from b in _context.Users
                                         where b.Id == a.CreatedBy.ToString()
                                         select new
                                         {
                                             b.UserName,
                                         }).Single().UserName,
                             CustomerUserName = (from b in _context.Users
                                                 where b.Id == a.UserId
                                                 select new
                                                 {
                                                     b.UserName,
                                                 }).Single().UserName,
                             Auth = (from b in _context.Users
                                     where b.Id == a.UserId
                                     select new
                                     {
                                         b.Is2Fa,
                                     }).Single().Is2Fa,
                         }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<CustomerViewModel> NewGetCustomers(Guid UserId)
        {
            List<CustomerViewModel> model = new List<CustomerViewModel>();

            try
            {
                model = (from a in _context.Customer
                         where a.CreatedBy == UserId
                         select new CustomerViewModel
                         {
                             CustomerId = a.CustomerId,
                             FirstName = a.FirstName,
                             LastName = a.LastName,
                             UserId = a.UserId,
                             Email = a.Email,
                             IsEmailSent = a.IsEmailSent,
                             Questionnaire = a.Questionnaire,
                             CreatedBy = a.CreatedBy,
                             RepliedDate = a.RepliedDate,
                             CurrentDateTime = DateTime.Now,
                             SendDate = a.SendDate,
                             Client = a.Client,
                             QuestionnaireId = a.QuestionnaireId,
                             SalesId = a.SalesId,
                             CreatedDate = a.CreatedDate,
                             UpdatedDate = a.UpdatedDate,
                             Address1 = a.Address1,
                             Address2 = a.Address2,
                             City = a.City,
                             State = a.State,
                             Country = a.Country,
                             MobileNumber = a.MobileNumber,
                             Zip = a.Zip,
                             TempUserName = a.TempUserName,
                             CustomerQuestionnaircount = _context.CustomerQuestionnair.Where(t => t.CustomerId == a.CustomerId).Count(),
                             ProjectsViewList = _context.Projects.Where(t => t.CustomerId == a.CustomerId).Count(),
                             UserName = (from b in _context.Users
                                         where b.Id == UserId.ToString()
                                         select new
                                         {
                                             b.UserName,
                                         }).Single().UserName,
                             CustomerUserName = (from b in _context.Users
                                                 where b.Id == a.UserId
                                                 select new
                                                 {
                                                     b.UserName,
                                                 }).Single().UserName,
                             Auth = (from b in _context.Users
                                     where b.Id == a.UserId
                                     select new
                                     {
                                         b.Is2Fa,
                                     }).Single().Is2Fa,
                         }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<Customer> GetCustomerById(Guid? id)
        {
            try
            {
                return await _context.Customer
                       .FirstOrDefaultAsync(m => m.CustomerId == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Guid> CreateCustomer(Customer customer)
        {
            try
            {
                customer.CustomerId = Guid.NewGuid();
                customer.CreatedDate = DateTime.Now;
                customer.UpdatedDate = DateTime.Now;
                _context.Add(customer);
                var aa = await _context.SaveChangesAsync();
                return customer.CustomerId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> UpdateCustomer(Customer customer)
        {

            try
            {
                var data = _context.Customer.Find(customer.CustomerId);
                data.UpdatedDate = DateTime.Now;
                data.FirstName = customer.FirstName;
                data.LastName = customer.LastName;
                data.Client = customer.Client;
                data.Address1 = customer.Address1;
                data.Address2 = customer.Address2;
                data.City = customer.City;
                data.MobileNumber = customer.MobileNumber;
                data.State = customer.State;
                data.Country = customer.Country;
                data.Zip = customer.Zip;
                data.IsCustomerUpdate = true;
                if (customer.CreatedBy != Guid.Empty)
                {
                    data.CreatedBy = customer.CreatedBy;
                }
                _context.Update(data);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> DeleteCustomerById(Guid? id)
        {
            try
            {
                var data1 = _context.Customer.Where(m => m.CustomerId == id).FirstOrDefault();
                var data2 = _context.Users.Where(m => m.Id == data1.UserId).FirstOrDefault();
                _context.Customer.Remove(await GetCustomerById(id));
                if (data2 != null)
                {
                    var data = _context.Users.Find(data2.Id);
                    if (data != null)
                    {
                        _context.Users.Remove(data);
                    }
                }
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CustomerExists(Guid id)
        {
            return _context.Customer.Any(e => e.CustomerId == id);
        }

        public async Task<int> UpdateCustomerIsEmailSent(Customer customer)
        {
            try
            {
                customer.UpdatedDate = DateTime.Now;
                customer.IsEmailSent = true;
                customer.SendDate = DateTime.Now;
                _context.Update(customer);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateCustomerUserId(Customer customer, string id)
        {
            try
            {
                customer.UpdatedDate = DateTime.Now;
                customer.UserId = id;
                _context.Update(customer);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool EmailExists(string Email)
        {
            return _context.Customer.Any(e => e.Email == Email);
        }


        public List<CustomerViewModel> GetCustomersEmailVerified()
        {
            List<CustomerViewModel> model = new List<CustomerViewModel>();
            try
            {
                model = (from a in _context.Customer
                         where a.IsEmailSent == true
                         select new CustomerViewModel
                         {
                             CustomerId = a.CustomerId,
                             FirstName = a.FirstName,
                             LastName = a.LastName,
                             UserId = a.UserId,
                             Email = a.Email,
                             IsEmailSent = a.IsEmailSent,
                             Questionnaire = a.Questionnaire,
                             CreatedBy = a.CreatedBy,
                             RepliedDate = a.RepliedDate,
                             CurrentDateTime = DateTime.Now,
                             SendDate = a.SendDate,
                             Client = a.Client,
                             QuestionnaireId = a.QuestionnaireId,
                             SalesId = a.SalesId,
                             CreatedDate = a.CreatedDate,
                             UpdatedDate = a.UpdatedDate,
                             CustomerQuestionnaircount = _context.CustomerQuestionnair.Where(t => t.CustomerId == a.CustomerId).Count(),
                             UserName = (from b in _context.Users
                                         where b.Id == a.CreatedBy.ToString()
                                         select new
                                         {
                                             b.UserName,
                                         }).Single().UserName,
                             CustomerUserName = (from b in _context.Users
                                                 where b.Id == a.UserId
                                                 select new
                                                 {
                                                     b.UserName,
                                                 }).Single().UserName,
                             Auth = (from b in _context.Users
                                     where b.Id == a.UserId
                                     select new
                                     {
                                         b.Is2Fa,
                                     }).Single().Is2Fa,

                         }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<CustomerViewModel> GetCustomersReplied()
        {
            List<CustomerViewModel> model = new List<CustomerViewModel>();
            try
            {
                model = (from a in _context.Customer
                         where a.Questionnaire == true
                         select new CustomerViewModel
                         {
                             CustomerId = a.CustomerId,
                             FirstName = a.FirstName,
                             LastName = a.LastName,
                             UserId = a.UserId,
                             Email = a.Email,
                             IsEmailSent = a.IsEmailSent,
                             Questionnaire = a.Questionnaire,
                             CreatedBy = a.CreatedBy,
                             RepliedDate = a.RepliedDate,
                             CurrentDateTime = DateTime.Now,
                             SendDate = a.SendDate,
                             Client = a.Client,
                             QuestionnaireId = a.QuestionnaireId,
                             SalesId = a.SalesId,
                             CreatedDate = a.CreatedDate,
                             UpdatedDate = a.UpdatedDate,
                             TempUserName = a.TempUserName,
                             CustomerQuestionnaircount = _context.CustomerQuestionnair.Where(t => t.CustomerId == a.CustomerId).Count(),
                             UserName = (from b in _context.Users
                                         where b.Id == a.CreatedBy.ToString()
                                         select new
                                         {
                                             b.UserName,
                                         }).Single().UserName,
                             CustomerUserName = (from b in _context.Users
                                                 where b.Id == a.UserId
                                                 select new
                                                 {
                                                     b.UserName,
                                                 }).Single().UserName,
                             Auth = (from b in _context.Users
                                     where b.Id == a.UserId
                                     select new
                                     {
                                         b.Is2Fa,
                                     }).Single().Is2Fa,
                         }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<CustomerViewModel> NewGetCustomersEmailVerified(Guid UserId)
        {
            List<CustomerViewModel> model = new List<CustomerViewModel>();

            try
            {
                model = (from a in _context.Customer
                         where a.CreatedBy == UserId && a.IsEmailSent == true
                         select new CustomerViewModel
                         {
                             CustomerId = a.CustomerId,
                             FirstName = a.FirstName,
                             LastName = a.LastName,
                             UserId = a.UserId,
                             Email = a.Email,
                             IsEmailSent = a.IsEmailSent,
                             Questionnaire = a.Questionnaire,
                             CreatedBy = a.CreatedBy,
                             RepliedDate = a.RepliedDate,
                             CurrentDateTime = DateTime.Now,
                             SendDate = a.SendDate,
                             Client = a.Client,
                             QuestionnaireId = a.QuestionnaireId,
                             SalesId = a.SalesId,
                             CreatedDate = a.CreatedDate,
                             UpdatedDate = a.UpdatedDate,
                             TempUserName = a.TempUserName,
                             CustomerQuestionnaircount = _context.CustomerQuestionnair.Where(t => t.CustomerId == a.CustomerId).Count(),
                             UserName = (from b in _context.Users
                                         where b.Id == a.CreatedBy.ToString()
                                         select new
                                         {
                                             b.UserName,
                                         }).Single().UserName,
                             CustomerUserName = (from b in _context.Users
                                                 where b.Id == a.UserId
                                                 select new
                                                 {
                                                     b.UserName,
                                                 }).Single().UserName,
                             Auth = (from b in _context.Users
                                     where b.Id == a.UserId
                                     select new
                                     {
                                         b.Is2Fa,
                                     }).Single().Is2Fa,
                         }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CustomerViewModel> NewGetCustomersReplied(Guid UserId)
        {
            List<CustomerViewModel> model = new List<CustomerViewModel>();

            try
            {
                model = (from a in _context.Customer
                         where a.CreatedBy == UserId && a.Questionnaire == true
                         select new CustomerViewModel
                         {
                             CustomerId = a.CustomerId,
                             FirstName = a.FirstName,
                             LastName = a.LastName,
                             UserId = a.UserId,
                             Email = a.Email,
                             IsEmailSent = a.IsEmailSent,
                             Questionnaire = a.Questionnaire,
                             CreatedBy = a.CreatedBy,
                             RepliedDate = a.RepliedDate,
                             CurrentDateTime = DateTime.Now,
                             SendDate = a.SendDate,
                             Client = a.Client,
                             QuestionnaireId = a.QuestionnaireId,
                             SalesId = a.SalesId,
                             CreatedDate = a.CreatedDate,
                             UpdatedDate = a.UpdatedDate,
                             TempUserName = a.TempUserName,
                             CustomerQuestionnaircount = _context.CustomerQuestionnair.Where(t => t.CustomerId == a.CustomerId).Count(),
                             UserName = (from b in _context.Users
                                         where b.Id == UserId.ToString()
                                         select new
                                         {
                                             b.UserName,
                                         }).Single().UserName,
                             CustomerUserName = (from b in _context.Users
                                                 where b.Id == a.UserId
                                                 select new
                                                 {
                                                     b.UserName,
                                                 }).Single().UserName,
                             Auth = (from b in _context.Users
                                     where b.Id == a.UserId
                                     select new
                                     {
                                         b.Is2Fa,
                                     }).Single().Is2Fa,
                         }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<SalesViewModel> GetSales()
        {
            List<SalesViewModel> model = new List<SalesViewModel>();
            try
            {
                model = (from a in _context.Sales
                         where a.UserId != null
                         select new SalesViewModel
                         {
                             CustomerId = new Guid(a.UserId),
                             Name = "" + a.FirstName + " " + a.LastName + "",

                         }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CustomerViewModel> NewGetCustomerById(Guid? id)
        {
            CustomerViewModel model = new CustomerViewModel();

            try
            {
                var a = await _context.Customer
                           .FirstOrDefaultAsync(m => m.CustomerId == id);

                model.Address1 = a.Address1;
                model.Address2 = a.Address2;
                model.City = a.City;
                model.Client = a.Client;
                model.Country = a.Country;
                model.CreatedBy = a.CreatedBy;
                model.CreatedDate = a.CreatedDate;
                model.CustomerId = a.CustomerId;
                model.Email = a.Email;
                model.FirstName = a.FirstName;
                model.LastName = a.LastName;
                model.MobileNumber = a.MobileNumber;
                model.Questionnaire = a.Questionnaire;
                model.QuestionnaireId = a.QuestionnaireId;
                model.RepliedDate = a.RepliedDate;
                model.SalesId = a.SalesId;
                model.SendDate = a.SendDate;
                model.State = a.State;
                model.UpdatedDate = a.UpdatedDate;
                model.UserId = a.UserId;
                model.Zip = a.Zip;
                model.TempUserName = a.TempUserName;
                var dataName = _context.Users.Where(m => m.Id == a.CreatedBy.ToString()).FirstOrDefault();

                if (dataName != null)
                {
                    model.UserName = dataName.UserName;
                }
                var dataName1 = _context.Users.Where(m => m.Id == a.UserId).FirstOrDefault();

                if (dataName1 != null)
                {
                    model.CustomerUserName = dataName1.UserName;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }

        public List<QuestionnaireViewModelAll> QuestionnaireAllList()
        {
            List<QuestionnaireViewModelAll> model = new List<QuestionnaireViewModelAll>();
            try
            {
                model = (from a in _context.Questionnaire where a.IsDelete!=true
                         select new QuestionnaireViewModelAll
                         {
                             QuestionnaireId = a.QuestionnaireId,
                             Name = a.Name
                         }).OrderBy(m => m.Name).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<QuestionnaireViewModelAll> QuestionnaireAllClientList()
        {
            List<QuestionnaireViewModelAll> model = new List<QuestionnaireViewModelAll>();
            try
            {
                model = (from a in _context.Questionnaire
                         where a.IsDelete != true &&a.IsHide==false
                         select new QuestionnaireViewModelAll
                         {
                             QuestionnaireId = a.QuestionnaireId,
                             Name = a.Name
                         }).OrderBy(m => m.Name).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CustomerQuestionnairViewModel> CustomerQuestionnairList(Guid id)
        {
            List<CustomerQuestionnairViewModel> model = new List<CustomerQuestionnairViewModel>();
            try
            {
                var customer = _context.Customer.Where(m => m.UserId == id.ToString()).FirstOrDefault();
                if (customer != null)
                {


                    model = (from a in _context.CustomerQuestionnair
                             where a.CustomerId == customer.CustomerId
                             select new CustomerQuestionnairViewModel
                             {
                                 QuestionnaireId = a.QuestionnaireId,
                                 CustomerQuestionnairId = a.CustomerQuestionnairId,
                                 CustomerId = a.CustomerId,
                                 CreatedBy = a.CreatedBy,
                                 CreatedDate = a.CreatedDate,
                                 IsQuestionnaire = a.IsQuestionnaire,
                                 UpdatedDate = a.UpdatedDate,
                                 Nmae = (from b in _context.Questionnaire
                                         where b.QuestionnaireId == a.QuestionnaireId
                                         select new
                                         {
                                             b.Name,
                                         }).Single().Name
                             }).ToList();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return model;
        }


        public ApplicationUser imagedata(string name)
        {
            ApplicationUser model = new ApplicationUser();
            var data = _context.Users.Where(m => m.UserName == name).FirstOrDefault();
            if (data != null)
            {
                model.FileName = data.FileName;
            }

            return model;
        }
        public bool UsernameExists(string Username)
        {
            return _context.Users.Any(e => e.UserName == Username);
        }
        public bool UserEmailExists(string Email)
        {
            return _context.Users.Any(e => e.Email == Email);

        }

        public async Task<AccountModel> ChangePassword(CustomerViewModel model)
        {
            AccountModel MSG = new AccountModel();
            try
            {
                var customer = _context.Customer.Where(m => m.CustomerId == model.Accountmodel.Id).FirstOrDefault();
                var id = customer.UserId;
                if (id != null)
                {
                    var data = _context.Users.Find(id);
                    data.PasswordHash = model.Accountmodel.Password;
                    _context.Update(data);
                    var res = await _context.SaveChangesAsync();
                }
                return MSG;
            }
            catch (Exception ex) { throw; }

        }

        public Guid AddNewProjectNewFunction(ProjectsViewModel projects)
        {
            try
            {
                var PermissionsData = _context.Permissions.Where(m => m.Tyepe == "Project").FirstOrDefault();

                Projects ps = new Projects();
                ps.ProjectsId = Guid.NewGuid();
                ps.CreateDate = DateTime.Now;
                if (PermissionsData.Toggle == true)
                {
                    var newdate = DateTime.Today;

                    ps.ProjectName = "KPMG" + DateTime.Now.ToString("dd") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("yy") + "MAH-" + projects.ProjectName;
                }
                else
                {
                    ps.ProjectName = projects.ProjectName;
                }
                ps.CustomerId = projects.CustomerId;
                ps.CreatedBy = projects.CustomerId;
                ps.RequestStatus = projects.RequestStatus;
                _context.Add(ps);
                _context.SaveChangesAsync();
                return ps.ProjectsId;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<Guid> AddNewProject(ProjectsViewModel projects)
        {
            var res = new Guid();
            try
            {
                var PermissionsData = _context.Permissions.Where(m => m.Tyepe == "Project").FirstOrDefault();

                Projects ps = new Projects();
                ps.ProjectsId = Guid.NewGuid();
                ps.CreateDate = DateTime.Now;
                if (PermissionsData.Toggle == true)
                {
                    var newdate = DateTime.Today;

                    ps.ProjectName = "KPMG" + DateTime.Now.ToString("dd") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("yy") + "MAH-" + projects.ProjectName;
                }
                else
                {
                    ps.ProjectName = projects.ProjectName;
                }
                ps.CustomerId = projects.CustomerId;
                ps.CreatedBy = projects.CustomerId;
                ps.RequestStatus = projects.RequestStatus;       
                _context.Add(ps);
              var Res =  await _context.SaveChangesAsync();
                if (Res > 0)
                {
                    res= ps.ProjectsId;
                }                  
            }
            catch (Exception)
            {
                throw;
            }

            return res;
        }


        public Guid UpdateProjectNewFunction(ProjectsViewModel projects)
        {
            try
            {
                var ps = _context.Projects.Find(projects.ProjectsId);
                if (ps != null)
                {
                    ps.UpdatedDate = DateTime.Now;
                    ps.ProjectName = projects.ProjectName;
                    _context.Update(ps);
                     _context.SaveChangesAsync();
                }
                return ps.ProjectsId;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<Guid> UpdateProject(ProjectsViewModel projects)
        {
            try
            {
                var ps = _context.Projects.Find(projects.ProjectsId);
                if (ps != null)
                {
                    ps.UpdatedDate = DateTime.Now;
                    ps.ProjectName = projects.ProjectName;
                    _context.Update(ps);
                    await _context.SaveChangesAsync();
                }
                return ps.ProjectsId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteNewProject(Guid? id)
        {
            try
            {
                var ps = _context.Projects.Find(id);
                if (ps != null)
                {
                    _context.Projects.Remove(ps);
                    await _context.SaveChangesAsync();
                }
                var data = _context.CustomerRequests.Where(m => m.ProjectId == id).FirstOrDefault();
                if (data != null)
                {
                    var CRdata = _context.CustomerRequests.Find(data.CustomerRequestsId);
                    if (CRdata != null)
                    {
                        _context.CustomerRequests.Remove(CRdata);
                        await _context.SaveChangesAsync();
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public List<ProjectsViewModel> ProjectsViewModelList(Guid id)
        {
            List<ProjectsViewModel> model = new List<ProjectsViewModel>();
            try
            {
                var customer = _context.Customer.Where(m => m.UserId == id.ToString()).FirstOrDefault();
                if (customer != null)
                {


                    model = (from v in _context.Projects
                             where v.CustomerId == customer.CustomerId
                             select new ProjectsViewModel
                             {
                                 ProjectName = v.ProjectName,
                                 ProjectsId = v.ProjectsId,
                                 CustomerId = v.CustomerId,
                                 CreateDate = v.CreateDate,
                                 CreatedBy = v.CreatedBy,
                                 QDataList = (from a in _context.CustomerQuestionnair
                                              where a.CustomerId == customer.CustomerId && a.ProjectId == v.ProjectsId
                                              select new CustomerQuestionnairViewModel
                                              {
                                                  QuestionnairDetailsViewdata = (from j in _context.QuestionnairDetails
                                                                                 where j.QuestionnaireId == a.QuestionnaireId && j.CustomerId == customer.CustomerId
                                                                                 select new QuestionnairDetailsViewModel
                                                                                 {
                                                                                     QuestionnaireId = j.QuestionnaireId,
                                                                                     CustomerName = j.CustomerName,
                                                                                     ProjectName = j.ProjectName,
                                                                                     CustomerId = j.CustomerId,
                                                                                     ApplicationName = j.ApplicationName,
                                                                                     CreateDate = j.CreateDate,
                                                                                     QuestionnairDetailsId = j.QuestionnairDetailsId,
                                                                                 }).FirstOrDefault(),
                                                  QuestionnaireId = a.QuestionnaireId,
                                                  CustomerQuestionnairId = a.CustomerQuestionnairId,
                                                  CustomerId = a.CustomerId,
                                                  CreatedBy = a.CreatedBy,
                                                  CreatedDate = a.CreatedDate,
                                                  IsQuestionnaire = a.IsQuestionnaire,
                                                  UpdatedDate = a.UpdatedDate,
                                                  ClientNmae = (from b in _context.Customer
                                                                where b.CustomerId == a.CustomerId
                                                                select new
                                                                {
                                                                    b.Client,
                                                                    FirstName = "" + b.FirstName + " " + b.LastName + "",

                                                                }).Single().Client,

                                                  CustomerName = (from b in _context.Customer
                                                                  where b.CustomerId == a.CustomerId
                                                                  select new
                                                                  {

                                                                      FirstName = "" + b.FirstName + " " + b.LastName + "",

                                                                  }).Single().FirstName,

                                                  SalesNmae = (from z in _context.Customer
                                                               where z.CustomerId == a.CustomerId
                                                               from b in _context.Users
                                                               where b.Id == z.CreatedBy.ToString()
                                                               select new
                                                               {
                                                                   b.UserName,

                                                               }).Single().UserName,
                                                  Nmae = (from b in _context.Questionnaire
                                                          where b.QuestionnaireId == a.QuestionnaireId
                                                          select new
                                                          {
                                                              b.Name,
                                                          }).Single().Name
                                              }).ToList(),


                             }).ToList();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return model;
        }

        public List<ProjectsViewModel> ProjectsViewModelListWithFile(Guid id)
        {
            List<ProjectsViewModel> model = new List<ProjectsViewModel>();
            try
            {
                var customer = _context.Customer.Where(m => m.UserId == id.ToString()).FirstOrDefault();
                if (customer != null)
                {
                    model = (from v in _context.Projects
                             where v.CustomerId == customer.CustomerId
                             select new ProjectsViewModel
                             {
                                 ProjectName = v.ProjectName,
                                 ProjectsId = v.ProjectsId,
                                 CustomerId = v.CustomerId,
                                 CreateDate = v.CreateDate,
                                 CreatedBy = v.CreatedBy,
                                 RequestStatus = v.RequestStatus,
                                 QDataList = (from a in _context.CustomerQuestionnair
                                              where a.CustomerId == customer.CustomerId && a.ProjectId == v.ProjectsId
                                              select new CustomerQuestionnairViewModel
                                              {
                                                  QuestionnairDetailsViewdata = (from j in _context.QuestionnairDetails
                                                                                 where j.QuestionnaireId == a.QuestionnaireId && j.CustomerId == customer.CustomerId
                                                                                 select new QuestionnairDetailsViewModel
                                                                                 {
                                                                                     QuestionnaireId = j.QuestionnaireId,
                                                                                     CustomerName = j.CustomerName,
                                                                                     ProjectName = j.ProjectName,
                                                                                     CustomerId = j.CustomerId,
                                                                                     ApplicationName = j.ApplicationName,
                                                                                     CreateDate = j.CreateDate,
                                                                                     QuestionnairDetailsId = j.QuestionnairDetailsId,
                                                                                 }).FirstOrDefault(),
                                                  QuestionnaireId = a.QuestionnaireId,
                                                  CustomerQuestionnairId = a.CustomerQuestionnairId,
                                                  CustomerId = a.CustomerId,
                                                  CreatedBy = a.CreatedBy,
                                                  CreatedDate = a.CreatedDate,
                                                  IsQuestionnaire = a.IsQuestionnaire,
                                                  UpdatedDate = a.UpdatedDate,
                                                  ClientNmae = (from b in _context.Customer
                                                                where b.CustomerId == a.CustomerId
                                                                select new
                                                                {
                                                                    b.Client,
                                                                    FirstName = "" + b.FirstName + " " + b.LastName + "",
                                                                }).Single().Client,
                                                  CustomerName = (from b in _context.Customer
                                                                  where b.CustomerId == a.CustomerId
                                                                  select new
                                                                  {
                                                                      FirstName = "" + b.FirstName + " " + b.LastName + "",
                                                                  }).Single().FirstName,
                                                  SalesNmae = (from z in _context.Customer
                                                               where z.CustomerId == a.CustomerId
                                                               from b in _context.Users
                                                               where b.Id == z.CreatedBy.ToString()
                                                               select new
                                                               {
                                                                   b.UserName,
                                                               }).Single().UserName,
                                                  Nmae = (from b in _context.Questionnaire
                                                          where b.QuestionnaireId == a.QuestionnaireId
                                                          select new
                                                          {
                                                              b.Name,
                                                          }).Single().Name
                                              }).ToList(),
                                 UplodDocList = (from z in _context.UploadFiles
                                                 where z.ProjectId == v.ProjectsId && z.CustomerId == v.CustomerId
                                                 select new UploadedDocumentsViewModel
                                                 {
                                                     FileName = z.FileName,
                                                     FileContents = z.FileContents,
                                                     ProjectId = z.ProjectId,
                                                     CustomerId = z.CustomerId,
                                                     UploadedDocumentsId = z.UploadedDocumentsId,
                                                     FileModifiedTime = z.FileModifiedTime,
                                                 }).ToList(),
                                 ProchekupUploadList = (from z in _context.UploadedDocuments
                                                        where z.ProjectId == v.ProjectsId && z.CustomerId == v.CustomerId
                                                        select new UploadedDocumentsViewModel
                                                 {
                                                     FileName = z.FileName,
                                                     FileContents = z.FileContents,
                                                     ProjectId = z.ProjectId,
                                                     CustomerId = z.CustomerId,
                                                     UploadedDocumentsId = z.UploadedDocumentsId,
                                                     FileModifiedTime = z.FileModifiedTime,
                                                 }).ToList(),
                             }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }

        public List<CustomerRequestsViewModel> CustomerRequestList(string name)
        {
            List<CustomerRequestsViewModel> model = new List<CustomerRequestsViewModel>();
            var data = _context.Users.Where(m => m.UserName == name).FirstOrDefault();
            if (data != null)
            {
                model = (from v in _context.CustomerRequests
                         where v.SalesId == new Guid(data.Id) && v.IsAccept == false
                         select new CustomerRequestsViewModel
                         {
                             CustomerId = v.CustomerId,
                             ProjectId = v.ProjectId,
                             CustomerRequestsId = v.CustomerRequestsId,
                             CreateDate = v.CreateDate,
                             ProjectName = (from b in _context.Projects
                                            where b.ProjectsId == v.ProjectId
                                            select new
                                            {
                                                b.ProjectName,
                                            }).Single().ProjectName,
                             CustomerName = (from b in _context.Customer
                                             where b.CustomerId == v.CustomerId
                                             select new
                                             {
                                                 FirstName = "" + b.FirstName + " " + b.LastName + "",
                                             }).Single().FirstName,
                             Image = (from b in _context.Users
                                      where b.Id == v.CreatedBy.ToString()
                                      select new
                                      {
                                          FileName = b.FileName,
                                      }).Single().FileName,
                             FileName = (from f in _context.UploadedDocuments
                                         where f.CustomerId == v.CustomerId
                                         select new
                                         {
                                             f.FileName,
                                         }).Single().FileName
                         }).ToList();
            }
            return model;
        }

        public List<UploadedDocumentsViewModel> CustomerDocUploList(string name)
        {
            List<UploadedDocumentsViewModel> model = new List<UploadedDocumentsViewModel>();
            try
            {
                var data = _context.Users.Where(m => m.UserName == name).FirstOrDefault();
                if (data != null)
                {
                    var Cusid = _context.Customer.Where(m => m.UserId == data.Id).FirstOrDefault();

                    if (Cusid != null)
                    {
                        model = (from v in _context.UploadedDocuments
                                 where v.CustomerId == Cusid.CustomerId && v.IsSeen == false
                                 select new UploadedDocumentsViewModel
                                 {
                                     CustomerId = v.CustomerId,
                                     ProjectId = v.ProjectId,
                                     UploadedDocumentsId = v.UploadedDocumentsId,
                                     CreatedDate = Convert.ToDateTime(v.FileModifiedTime),
                                     FileName = (from f in _context.UploadedDocuments
                                                 where f.CustomerId == v.CustomerId
                                                 select new
                                                 {
                                                     f.FileName,
                                                 }).Single().FileName,
                                     ProjectName = (from b in _context.Projects
                                                    where b.ProjectsId == v.ProjectId
                                                    select new
                                                    {
                                                        b.ProjectName,
                                                    }).Single().ProjectName,
                                 }).ToList();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }

        public List<CustomerRequestsViewModel> SalesAcceptList(string name)
        {
            List<CustomerRequestsViewModel> model = new List<CustomerRequestsViewModel>();
            var data = _context.Users.Where(m => m.UserName == name).FirstOrDefault();
            var cusdata = _context.Customer.Where(m => m.UserId == data.Id).FirstOrDefault();
            if (cusdata != null)
            {
                model = (from v in _context.CustomerRequests
                         where v.CustomerId == cusdata.CustomerId && v.IsAccept == true && v.IsSeen == true
                         select new CustomerRequestsViewModel
                         {
                             CustomerId = v.CustomerId,
                             ProjectId = v.ProjectId,
                             CustomerRequestsId = v.CustomerRequestsId,
                             CreateDate = v.CreateDate,
                             ProjectName = (from b in _context.Projects
                                            where b.ProjectsId == v.ProjectId
                                            select new
                                            {
                                                b.ProjectName,
                                            }).Single().ProjectName,
                             CustomerName = (from b in _context.Customer
                                             where b.CustomerId == v.CustomerId
                                             select new
                                             {
                                                 FirstName = "" + b.FirstName + " " + b.LastName + "",
                                             }).Single().FirstName,
                             Image = (from b in _context.Users
                                      where b.Id == v.CreatedBy.ToString()
                                      select new
                                      {
                                          FileName = b.FileName,
                                      }).Single().FileName,
                         }).ToList();
            }
            return model;
        }
      
        public List<CustomerRequestsViewModel> CustomerRequestListByCustomerId(Guid id)
        {
            List<CustomerRequestsViewModel> model = new List<CustomerRequestsViewModel>();

            if (id != Guid.Empty)
            {
                //new code start

                model = (from v in _context.CustomerRequests
                         join a in _context.Projects on v.ProjectId equals a.ProjectsId
                         where v.CustomerId == id && v.IsAccept == true
                         select new CustomerRequestsViewModel
                         {
                             CustomerId = v.CustomerId,
                             ProjectId = v.ProjectId,
                             CustomerRequestsId = v.CustomerRequestsId,
                             CreateDate = v.CreateDate,
                             IsAccept = v.IsAccept,
                             IsSeen = v.IsSeen,
                             CreatedBy = v.CreatedBy,
                             SalesId = v.SalesId,
                             UpdatedDate = v.UpdatedDate,
                             ProjectName = (from b in _context.Projects
                                            where b.ProjectsId == v.ProjectId
                                            select new
                                            {
                                                b.ProjectName,
                                            }).Single().ProjectName,
                             CustomerName = (from b in _context.Customer
                                             where b.CustomerId == v.CustomerId
                                             select new
                                             {
                                                 FirstName = "" + b.FirstName + " " + b.LastName + "",
                                             }).Single().FirstName,
                             Image = (from b in _context.Users
                                      where b.Id == v.CreatedBy.ToString()
                                      select new
                                      {
                                          FileName = b.FileName,
                                      }).Single().FileName,
                             UploadedDocumentList = (from z in _context.UploadedDocuments
                                                     where z.ProjectId == v.ProjectId && z.CustomerId == v.CustomerId
                                                     select new UploadedDocumentsViewModel
                                                     {
                                                         FileName = z.FileName,
                                                         FileContents = z.FileContents,
                                                         ProjectId = z.ProjectId,
                                                         CustomerId = z.CustomerId,
                                                         UploadedDocumentsId = z.UploadedDocumentsId,
                                                         FileModifiedTime = z.FileModifiedTime,

                                                     }).ToList(),

                             ProjectCustomerQuestionnairList = (from n in _context.CustomerQuestionnair
                                                                where n.CustomerId == v.CustomerId && n.ProjectId == v.ProjectId
                                                                select new ProjectCustomerQuestionnairModel
                                                                {
                                                                    CustomerQuestionnairId = n.CustomerQuestionnairId,
                                                                    QuestionnaireId = n.QuestionnaireId,
                                                                    CustomerId = n.CustomerId,
                                                                    ProjectId = n.ProjectId,

                                                                    Name = (from q in _context.Questionnaire
                                                                            where q.QuestionnaireId == n.QuestionnaireId
                                                                            select new
                                                                            {
                                                                                q.Name,
                                                                            }).Single().Name,
                                                                    Methodology_DeliverablesData = (from f in _context.Admin_Methodology_Deliverables
                                                                                                    where f.QuestionnaireId == n.QuestionnaireId
                                                                                                    select new Methodology_DeliverablesViewModel
                                                                                                    {
                                                                                                        Methodology_DeliverablesId = f.Methodology_DeliverablesId,
                                                                                                        ScopeTypeMethodolgy = f.ScopeTypeMethodolgy,
                                                                                                        ScopeTypeSampleReport = f.ScopeTypeSampleReport,
                                                                                                        QuestionnaireId = f.QuestionnaireId,
                                                                                                        MethodologyId = f.MethodologyId,
                                                                                                        MethodologyReportId = f.MethodologyReportId,
                                                                                                        MethodologyeData = (from b in _context.Methodologye
                                                                                                                            where b.Id == f.MethodologyId
                                                                                                                            select new MethodologyeViewModel
                                                                                                                            {
                                                                                                                                FileContents = b.FileContents,
                                                                                                                                Type = b.Type,
                                                                                                                                FileName = b.FileName,
                                                                                                                                Id = b.Id,
                                                                                                                                CreatedDate = b.CreatedDate

                                                                                                                            }).FirstOrDefault(),
                                                                                                        MethodologyeReportData = (from b in _context.Methodologye
                                                                                                                                  where b.Id == f.MethodologyReportId
                                                                                                                                  select new MethodologyeViewModel
                                                                                                                                  {
                                                                                                                                      FileContents = b.FileContents,
                                                                                                                                      Type = b.Type,
                                                                                                                                      FileName = b.FileName,
                                                                                                                                      Id = b.Id,
                                                                                                                                      CreatedDate = b.CreatedDate
                                                                                                                                  }).FirstOrDefault(),
                                                                                                    }).FirstOrDefault()
                                                                }).ToList(),
                             ProjectsData = (from za in _context.Projects
                                             where za.ProjectsId == v.ProjectId && za.CustomerId == v.CustomerId
                                             select new ProjectsViewModel
                                             {
                                                 CustomerId = za.CustomerId,
                                                 ProjectName = za.ProjectName,
                                                 ProjectsId = za.ProjectsId,
                                                 // ProjectCustomerQuestionnairList = (from n in _context.CustomerQuestionnair
                                                 //  where n.CustomerId == v.CustomerId && n.ProjectId == za.ProjectsId
                                                 //  select new ProjectCustomerQuestionnairModel
                                                 //  {
                                                 // QuestionnaireId = aa.QuestionnaireId,
                                                 //Nmae = aa.Name,
                                                 //Methodology_DeliverablesViewModelData = (from f in _context.Admin_Methodology_Deliverables
                                                 //                                         where f.QuestionnaireId == a.QuestionnaireId
                                                 //                                         select new Methodology_DeliverablesViewModel
                                                 //                                         {
                                                 //                                             Methodology_DeliverablesId = f.Methodology_DeliverablesId,
                                                 //                                             ScopeTypeMethodolgy = f.ScopeTypeMethodolgy,
                                                 //                                             ScopeTypeSampleReport = f.ScopeTypeSampleReport,
                                                 //                                             QuestionnaireId = f.QuestionnaireId,
                                                 //                                             MethodologyId = f.MethodologyId,
                                                 //                                             MethodologyReportId = f.MethodologyReportId,
                                                 //                                             MethodologyeData = (from b in _context.Methodologye
                                                 //                                                                 where b.Id == f.MethodologyId
                                                 //                                                                 select new MethodologyeViewModel
                                                 //                                                                 {

                                                 //                                                                 }).FirstOrDefault(),
                                                 //                                             MethodologyeReportData = (from b in _context.Methodologye
                                                 //                                                                       where b.Id == f.MethodologyReportId
                                                 //                                                                       select new MethodologyeViewModel
                                                 //                                                                       {

                                                 //                                                                       }).FirstOrDefault(),
                                                 //                                         }).FirstOrDefault()
                                                 // }).ToList(),
                                             }).FirstOrDefault(),
                         }).ToList();

                //new code stop
            }

            return model;
        }

        public List<CustomerRequestsViewModel> CustomerRequestProposalListByCustomerId(Guid id)
        {
            List<CustomerRequestsViewModel> model = new List<CustomerRequestsViewModel>();

            if (id != Guid.Empty)
            {
                model = (from v in _context.CustomerRequests
                         where v.CustomerId == id && v.IsAccept == false
                         select new CustomerRequestsViewModel
                         {
                             CustomerId = v.CustomerId,
                             ProjectId = v.ProjectId,
                             CustomerRequestsId = v.CustomerRequestsId,
                             CreateDate = v.CreateDate,
                             IsAccept = v.IsAccept,
                             IsSeen = v.IsSeen,
                             CreatedBy = v.CreatedBy,
                             SalesId = v.SalesId,
                             UpdatedDate = v.UpdatedDate,
                             ProjectName = (from b in _context.Projects
                                            where b.ProjectsId == v.ProjectId
                                            select new
                                            {
                                                b.ProjectName,
                                            }).Single().ProjectName,
                             CustomerName = (from b in _context.Customer
                                             where b.CustomerId == v.CustomerId
                                             select new
                                             {
                                                 FirstName = "" + b.FirstName + " " + b.LastName + "",
                                             }).Single().FirstName,
                             Image = (from b in _context.Users
                                      where b.Id == v.CreatedBy.ToString()
                                      select new
                                      {
                                          FileName = b.FileName,
                                      }).Single().FileName,
                         }).ToList();
            }
            return model;
        }

        public List<CustomerRequestsViewModel> CustomerRequestProposalAllListByCustomerId(Guid id)
        {
            List<CustomerRequestsViewModel> model = new List<CustomerRequestsViewModel>();
            if (id != Guid.Empty)
            {
                model = (from v in _context.CustomerRequests
                         where v.CustomerId == id
                         select new CustomerRequestsViewModel
                         {
                             CustomerId = v.CustomerId,
                             ProjectId = v.ProjectId,
                             CustomerRequestsId = v.CustomerRequestsId,
                             CreateDate = v.CreateDate,
                             IsAccept = v.IsAccept,
                             IsSeen = v.IsSeen,
                             CreatedBy = v.CreatedBy,
                             SalesId = v.SalesId,
                             UpdatedDate = v.UpdatedDate,
                             ProjectName = (from b in _context.Projects
                                            where b.ProjectsId == v.ProjectId
                                            select new
                                            {
                                                b.ProjectName,
                                            }).Single().ProjectName,
                             CustomerName = (from b in _context.Customer
                                             where b.CustomerId == v.CustomerId
                                             select new
                                             {
                                                 FirstName = "" + b.FirstName + " " + b.LastName + "",
                                             }).Single().FirstName,
                             Image = (from b in _context.Users
                                      where b.Id == v.CreatedBy.ToString()
                                      select new
                                      {
                                          FileName = b.FileName,
                                      }).Single().FileName,
                         }).ToList();
            }
            return model;
        }

        public int TotalProjectCount(string name)
        {
            int TotalProject = 0;
            var data = _context.Users.Where(m => m.UserName == name).FirstOrDefault();
            if (data != null)
            {
                var Customerdata = _context.Customer.Where(m => m.UserId == data.Id).FirstOrDefault();
                if (Customerdata != null)
                {
                    TotalProject = _context.Projects.Where(m => m.CustomerId == Customerdata.CustomerId).Count();
                }
            }
            return TotalProject;
        }

        public async Task<int> AuthUpdate(string UserId, bool IsAuth)
        {
            try
            {
                var data = _context.Users.Find(UserId);
                if (data != null)
                {
                    data.Is2Fa = IsAuth;
                    data.TwoFactorEnabled = IsAuth;
                    _context.Update(data);
                }
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PermissionsViewModel> PermissionsAllList()
        {
            List<PermissionsViewModel> model = new List<PermissionsViewModel>();
            try
            {
                model = (from v in _context.Permissions
                         select new PermissionsViewModel
                         {
                             Name = v.Name,
                             PermissionsId = v.PermissionsId,
                             Toggle = v.Toggle,
                             Tyepe = v.Tyepe,
                             Value = v.Toggle.ToString()
                         }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }

        public async Task<DbResponce> PermissionsUpdate(string Toggle, Guid permissionsId)
        {
            DbResponce MSG = new DbResponce();
            try
            {
                if (permissionsId != Guid.Empty)
                {
                    var data = _context.Permissions.Where(m => m.PermissionsId == permissionsId).FirstOrDefault();
                    if (data != null)
                    {
                        var ps = _context.Permissions.Find(data.PermissionsId);
                        ps.Toggle = Convert.ToBoolean(Toggle);
                    }
                }
                var res = await _context.SaveChangesAsync();
                MSG.msg = res.ToString();
                return MSG;
            }
            catch (Exception ex) { throw; }
        }



    }
}

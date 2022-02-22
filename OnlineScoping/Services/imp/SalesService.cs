using Microsoft.EntityFrameworkCore;
using OnlineScoping.Data;
using OnlineScoping.Models;
using OnlineScoping.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Services.imp
{
    public class SalesService: ISalesService
    {
        private readonly OnlineScopingContext _context;

        public SalesService(OnlineScopingContext context)
        {
            _context = context;
        }

        public async Task<List<Sales>> GetSales()
        {
            try
            {
                return await _context.Sales.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<SalesViewModel> GetSalesAdmin()
        {
            List<SalesViewModel> model = new List<SalesViewModel>();
            try
            {
                model = (from a in _context.Sales
                         select new SalesViewModel
                         {
                             CustomerId = a.CustomerId,
                             FirstName = a.FirstName,
                             LastName = a.LastName,
                             UserId = a.UserId,
                             Email = a.Email,
                             IsEmailSent = a.IsEmailSent,                         
                             CreatedDate = a.CreatedDate,
                             UpdatedDate = a.UpdatedDate,                        
                             MobileNumber = a.MobileNumber,    
                            TempUserName=a.TempUserName,
                             UserName = (from b in _context.Users
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
                                                      }).Single().Is2Fa

                         }).ToList();




                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }




        public async Task<Sales> GetSaleById(Guid? id)
        {
            try
            {
                return await _context.Sales.FirstOrDefaultAsync(m => m.CustomerId == id);
            }
            catch (Exception)
            {
                throw;
            }
        }
     
        public bool SalesExistsInUser(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        public async Task<Guid> CreateSales(Sales sales)
        {
            try
            {
                sales.CustomerId = Guid.NewGuid();
                sales.CreatedDate = DateTime.Now;
                sales.UpdatedDate = DateTime.Now;
                _context.Add(sales);
                 await _context.SaveChangesAsync();
                return sales.CustomerId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateSales(Sales sales)
        {
            try
            {
                var data = _context.Sales.Find(sales.CustomerId);
                data.UpdatedDate = DateTime.Now;
                data.FirstName = sales.FirstName;
                data.LastName = sales.LastName;
                data.MobileNumber = sales.MobileNumber;
                data.UpdatedDate = DateTime.Now;
                _context.Update(data);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteSalesById(Guid? id)
        {
            try
            {
               var data1= _context.Sales.Where(m => m.CustomerId == id).FirstOrDefault();
               var data2= _context.Users.Where(m => m.Id == data1.UserId).FirstOrDefault();
                _context.Sales.Remove(await GetSaleById(id));
                if(data2!=null)
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

        public async Task<int> UpdateSalesIsEmailSent(Sales sales)
        {
            try
            {
                sales.UpdatedDate = DateTime.Now;
                sales.IsEmailSent = true;
                _context.Update(sales);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateSalesUserId(Sales sales, string id)
        {
            try
            {
                sales.UpdatedDate = DateTime.Now;
                sales.UserId = id;
                _context.Update(sales);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SalesExists(Guid id)
        {
            return _context.Sales.Any(e => e.CustomerId == id);
        }

        public bool EmailExists(string Email)
        {
            return _context.Sales.Any(e => e.Email == Email);
        }
        public bool UsernameExists(string Username)
        {
            return _context.Users.Any(e => e.UserName == Username);
        }
        public bool UserEmailExists(string Email)
        {
            return _context.Users.Any(e => e.Email == Email);
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

    }
}

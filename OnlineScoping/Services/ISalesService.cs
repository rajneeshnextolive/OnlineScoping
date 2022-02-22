using OnlineScoping.Models;
using OnlineScoping.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Services
{
    public interface ISalesService
    {
        Task<List<Sales>> GetSales();
       List<SalesViewModel> GetSalesAdmin();
        Task<Sales> GetSaleById(Guid? id);
        Task<Guid> CreateSales(Sales sales);
        Task<int> UpdateSales(Sales sales);
        Task<int> DeleteSalesById(Guid? id);
        Task<int> UpdateSalesIsEmailSent(Sales sales);
        Task<int> UpdateSalesUserId(Sales sales, string id);
        bool SalesExists(Guid id);
        bool EmailExists(string Email); 
        bool UserEmailExists(string Email);
        bool UsernameExists(string Username);
        Task<int> AuthUpdate(string UserId, bool IsAuth);
    }
}

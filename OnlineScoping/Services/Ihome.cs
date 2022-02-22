using OnlineScoping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Services
{
   public interface Ihome
    {
        Task<int> UpdateAdminDetails(AccountModel model);
        public Task<int> RequestPraposalAdd(Guid CustomerId,Guid ProjectId,Guid CreatedBy,string UserId);
        public Task<int> AcceptProposalRequest(Guid CustomerId,Guid ProjectId);

        public Task<int> CustomerAcceptProposalRequest(Guid CustomerId, Guid ProjectId);
        public Task<int> CustomerDocUplod(Guid CustomerId, Guid ProjectId);
    }
}

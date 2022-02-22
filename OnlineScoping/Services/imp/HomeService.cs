using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineScoping.Data;
using OnlineScoping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Services.imp
{
    public class HomeService : Ihome
    {
        private readonly OnlineScopingContext _context;

        public HomeService(OnlineScopingContext context)
        {
            _context = context;
        }
        public async Task<int> UpdateAdminDetails(AccountModel model)
        {
            try
            {
                model.UpdatedDate = DateTime.Now;
                _context.Update(model);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<int> RequestPraposalAdd(Guid CustomerId,Guid ProjectId,Guid CreatedBy,string UserId)
        {
            try
            {
              if(CustomerId!=Guid.Empty&&ProjectId!=Guid.Empty&& CreatedBy!=Guid.Empty)
                {
                    var data = _context.CustomerRequests.Where(m => m.CustomerId == CustomerId && m.ProjectId == ProjectId).FirstOrDefault();
                    if(data!=null)
                    {
                        var RequestData = _context.CustomerRequests.Find(data.CustomerRequestsId);
                        RequestData.CreateDate = DateTime.Now;
                        RequestData.IsAccept = false;
                        RequestData.IsSeen = false;
                        RequestData.UpdatedDate = DateTime.Now;
                        RequestData.CreatedBy = new Guid(UserId);
                        _context.Update(RequestData);
                        

                    }else
                    {
                        CustomerRequests cr = new CustomerRequests();
                        cr.CustomerRequestsId = Guid.NewGuid();
                        cr.CustomerId = CustomerId;
                        cr.ProjectId = ProjectId;
                        cr.CreateDate = DateTime.Now;
                        cr.CreatedBy =new Guid(UserId);
                        cr.SalesId = CreatedBy;
                        _context.Add(cr);
                    }                                
                }
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        
   public async Task<int> AcceptProposalRequest(Guid CustomerId, Guid ProjectId)
        {
            try
            {
                if (CustomerId != Guid.Empty && ProjectId != Guid.Empty)
                {
                    var data = _context.CustomerRequests.Where(m => m.CustomerId == CustomerId && m.ProjectId == ProjectId).FirstOrDefault();
                    if (data != null)
                    {
                        var RequestData = _context.CustomerRequests.Find(data.CustomerRequestsId);                
                        RequestData.IsAccept = true;
                        RequestData.IsSeen = true;
                        _context.Update(RequestData);
                    }                 
                }
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> CustomerAcceptProposalRequest(Guid CustomerId, Guid ProjectId)
        {
            try
            {
                if (CustomerId != Guid.Empty && ProjectId != Guid.Empty)
                {
                    var data = _context.CustomerRequests.Where(m => m.CustomerId == CustomerId && m.ProjectId == ProjectId).FirstOrDefault();
                    if (data != null)
                    {
                        var RequestData = _context.CustomerRequests.Find(data.CustomerRequestsId);
                        RequestData.IsAccept = true;
                        RequestData.IsSeen = false;
                        _context.Update(RequestData);
                    }
                }
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> CustomerDocUplod(Guid CustomerId, Guid ProjectId)
        {
            try
            {
                if (CustomerId != Guid.Empty && ProjectId != Guid.Empty)
                {
                    var data = _context.UploadedDocuments.Where(m => m.CustomerId == CustomerId && m.UploadedDocumentsId == ProjectId).FirstOrDefault();
                    if (data != null)
                    {
                        var UploadDoc = _context.UploadedDocuments.Find(data.UploadedDocumentsId);

                        UploadDoc.IsSeen = true;
                        _context.Update(UploadDoc);
                    }
                }
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

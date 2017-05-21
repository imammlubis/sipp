using Sipp.Data.Entity.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Service.Organization
{
    public interface ICompanyHistoryRepository
    {
        IQueryable<CompanyHistory> Get();
        CompanyHistory Add(CompanyHistory CompanyHistory);
        Task<CompanyHistory> AddAsync(CompanyHistory CompanyHistory);
        CompanyHistory Update(CompanyHistory CompanyHistory);
        Task<CompanyHistory> UpdateAsync(CompanyHistory CompanyHistory);
        int Remove(CompanyHistory CompanyHistory);
        Task<int> RemoveAsync(CompanyHistory CompanyHistory);
        CompanyHistory Find(string id);
        Task<CompanyHistory> FindAsync(string id);
    }
}

using Sipp.Data.Entity.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Service.Organization
{
    public interface ICompanyEmailRepository
    {
        IQueryable<CompanyEmail> Get();
        CompanyEmail Add(CompanyEmail CompanyEmail);
        Task<CompanyEmail> AddAsync(CompanyEmail CompanyEmail);
        CompanyEmail Update(CompanyEmail CompanyEmail);
        Task<CompanyEmail> UpdateAsync(CompanyEmail CompanyEmail);
        int Remove(CompanyEmail CompanyEmail);
        Task<int> RemoveAsync(CompanyEmail CompanyEmail);
        CompanyEmail Find(string id);
        Task<CompanyEmail> FindAsync(string id);
    }
}

using Sipp.Data.Entity.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Service.Organization
{
    public interface ICompanyRepository
    {
        IQueryable<Company> Get();
        Company Add(Company Company);
        Task<Company> AddAsync(Company Company);
        Company Update(Company Company);
        Task<Company> UpdateAsync(Company Company);
        int Remove(Company Company);
        Task<int> RemoveAsync(Company Company);
        Company Find(string id);
        Task<Company> FindAsync(string id);
    }
}

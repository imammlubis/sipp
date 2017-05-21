using Sipp.Core.Data.GenericRepository;
using Sipp.Data.Entity.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Service.Organization
{
    public class CompanyRepository : ICompanyRepository
    {
        private GetRepository<Company> getRepository = new GetRepository<Company>();
        private AddRepository<Company> addRepository = new AddRepository<Company>();
        private FindRepository<Company> findRepository = new FindRepository<Company>();
        private UpdateRepository<Company> updateRepository = new UpdateRepository<Company>();
        private RemoveRepository<Company> removeRepository = new RemoveRepository<Company>();

        public IQueryable<Company> Get()
        {
            return getRepository.Get;
        }
        public Company Add(Company Company)
        {
            return addRepository.Add(Company);
        }
        public async Task<Company> AddAsync(Company Company)
        {
            return await addRepository.AddAsync(Company);
        }
        public Company Update(Company Company)
        {
            return updateRepository.Update(Company);
        }
        public async Task<Company> UpdateAsync(Company Company)
        {
            return await updateRepository.UpdateAsync(Company);
        }
        public int Remove(Company Company)
        {
            try
            {
                removeRepository.Remove(Company);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<int> RemoveAsync(Company Company)
        {
            try
            {
                await removeRepository.RemoveAsync(Company);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public Company Find(string id)
        {
            return findRepository.Find(id);
        }
        public async Task<Company> FindAsync(string id)
        {
            return await findRepository.FindAsync(id);
        }
    }
}

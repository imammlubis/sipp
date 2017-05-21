using Sipp.Core.Data.GenericRepository;
using Sipp.Data.Entity.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Service.Organization
{
    public class CompanyHistoryRepository : ICompanyHistoryRepository
    {
        private GetRepository<CompanyHistory> getRepository = new GetRepository<CompanyHistory>();
        private AddRepository<CompanyHistory> addRepository = new AddRepository<CompanyHistory>();
        private FindRepository<CompanyHistory> findRepository = new FindRepository<CompanyHistory>();
        private UpdateRepository<CompanyHistory> updateRepository = new UpdateRepository<CompanyHistory>();
        private RemoveRepository<CompanyHistory> removeRepository = new RemoveRepository<CompanyHistory>();

        public IQueryable<CompanyHistory> Get()
        {
            return getRepository.Get;
        }
        public CompanyHistory Add(CompanyHistory CompanyHistory)
        {
            return addRepository.Add(CompanyHistory);
        }
        public async Task<CompanyHistory> AddAsync(CompanyHistory CompanyHistory)
        {
            return await addRepository.AddAsync(CompanyHistory);
        }
        public CompanyHistory Update(CompanyHistory CompanyHistory)
        {
            return updateRepository.Update(CompanyHistory);
        }
        public async Task<CompanyHistory> UpdateAsync(CompanyHistory CompanyHistory)
        {
            return await updateRepository.UpdateAsync(CompanyHistory);
        }
        public int Remove(CompanyHistory CompanyHistory)
        {
            try
            {
                removeRepository.Remove(CompanyHistory);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<int> RemoveAsync(CompanyHistory CompanyHistory)
        {
            try
            {
                await removeRepository.RemoveAsync(CompanyHistory);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public CompanyHistory Find(string id)
        {
            return findRepository.Find(id);
        }
        public async Task<CompanyHistory> FindAsync(string id)
        {
            return await findRepository.FindAsync(id);
        }
    }
}

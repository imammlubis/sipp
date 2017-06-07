using Sipp.Core.Data.GenericRepository;
using Sipp.Data.Entity.Organization;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Service.Organization
{
    public class CompanyEmailRepository : ICompanyEmailRepository
    {
        private GetRepository<CompanyEmail> getRepository = new GetRepository<CompanyEmail>();
        private AddRepository<CompanyEmail> addRepository = new AddRepository<CompanyEmail>();
        private FindRepository<CompanyEmail> findRepository = new FindRepository<CompanyEmail>();
        private UpdateRepository<CompanyEmail> updateRepository = new UpdateRepository<CompanyEmail>();
        private RemoveRepository<CompanyEmail> removeRepository = new RemoveRepository<CompanyEmail>();

        public IQueryable<CompanyEmail> Get()
        {
            return getRepository.Get;
        }
        public CompanyEmail Add(CompanyEmail CompanyEmail)
        {
            return addRepository.Add(CompanyEmail);
        }
        public async Task<CompanyEmail> AddAsync(CompanyEmail CompanyEmail)
        {
            return await addRepository.AddAsync(CompanyEmail);
        }
        public CompanyEmail Update(CompanyEmail CompanyEmail)
        {
            return updateRepository.Update(CompanyEmail);
        }
        public async Task<CompanyEmail> UpdateAsync(CompanyEmail CompanyEmail)
        {
            return await updateRepository.UpdateAsync(CompanyEmail);
        }
        public int Remove(CompanyEmail CompanyEmail)
        {
            try
            {
                removeRepository.Remove(CompanyEmail);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<int> RemoveAsync(CompanyEmail CompanyEmail)
        {
            try
            {
                await removeRepository.RemoveAsync(CompanyEmail);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public CompanyEmail Find(string id)
        {
            return findRepository.Find(id);
        }
        public async Task<CompanyEmail> FindAsync(string id)
        {
            return await findRepository.FindAsync(id);
        }
    }
}

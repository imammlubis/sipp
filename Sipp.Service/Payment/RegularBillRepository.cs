using Sipp.Core.Data.GenericRepository;
using Sipp.Data.Entity.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Service.Payment
{
    public class RegularBillRepository : IRegularBillRepository
    {

        private GetRepository<RegularBill> getRepository = new GetRepository<RegularBill>();
        private AddRepository<RegularBill> addRepository = new AddRepository<RegularBill>();
        private FindRepository<RegularBill> findRepository = new FindRepository<RegularBill>();
        private UpdateRepository<RegularBill> updateRepository = new UpdateRepository<RegularBill>();
        private RemoveRepository<RegularBill> removeRepository = new RemoveRepository<RegularBill>();

        public IQueryable<RegularBill> Get()
        {
            return getRepository.Get;
        }
        public RegularBill Add(RegularBill RegularBill)
        {
            return addRepository.Add(RegularBill);
        }
        public async Task<RegularBill> AddAsync(RegularBill RegularBill)
        {
            return await addRepository.AddAsync(RegularBill);
        }
        public RegularBill Update(RegularBill RegularBill)
        {
            return updateRepository.Update(RegularBill);
        }
        public async Task<RegularBill> UpdateAsync(RegularBill RegularBill)
        {
            return await updateRepository.UpdateAsync(RegularBill);
        }
        public int Remove(RegularBill RegularBill)
        {
            try
            {
                removeRepository.Remove(RegularBill);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<int> RemoveAsync(RegularBill RegularBill)
        {
            try
            {
                await removeRepository.RemoveAsync(RegularBill);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public RegularBill Find(string id)
        {
            return findRepository.Find(id);
        }
        public async Task<RegularBill> FindAsync(string id)
        {
            return await findRepository.FindAsync(id);
        }
    }
}

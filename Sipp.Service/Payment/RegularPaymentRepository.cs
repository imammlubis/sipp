using Sipp.Core.Data.GenericRepository;
using Sipp.Data.Entity.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Service.Payment
{
    public class RegularPaymentRepository : IRegularPaymentRepository
    {
        private GetRepository<RegularPayment> getRepository = new GetRepository<RegularPayment>();
        private AddRepository<RegularPayment> addRepository = new AddRepository<RegularPayment>();
        private FindRepository<RegularPayment> findRepository = new FindRepository<RegularPayment>();
        private UpdateRepository<RegularPayment> updateRepository = new UpdateRepository<RegularPayment>();
        private RemoveRepository<RegularPayment> removeRepository = new RemoveRepository<RegularPayment>();

        public IQueryable<RegularPayment> Get()
        {
            return getRepository.Get;
        }
        public RegularPayment Add(RegularPayment RegularPayment)
        {
            return addRepository.Add(RegularPayment);
        }
        public async Task<RegularPayment> AddAsync(RegularPayment RegularPayment)
        {
            return await addRepository.AddAsync(RegularPayment);
        }
        public RegularPayment Update(RegularPayment RegularPayment)
        {
            return updateRepository.Update(RegularPayment);
        }
        public async Task<RegularPayment> UpdateAsync(RegularPayment RegularPayment)
        {
            return await updateRepository.UpdateAsync(RegularPayment);
        }
        public int Remove(RegularPayment RegularPayment)
        {
            try
            {
                removeRepository.Remove(RegularPayment);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<int> RemoveAsync(RegularPayment RegularPayment)
        {
            try
            {
                await removeRepository.RemoveAsync(RegularPayment);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public RegularPayment Find(string id)
        {
            return findRepository.Find(id);
        }
        public async Task<RegularPayment> FindAsync(string id)
        {
            return await findRepository.FindAsync(id);
        }
    }
}

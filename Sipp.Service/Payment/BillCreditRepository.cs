using Sipp.Core.Data.GenericRepository;
using Sipp.Data.Entity.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Service.Payment
{
    public class BillCreditRepository : IBillCreditRepository
    {
        private GetRepository<BillCredit> getRepository = new GetRepository<BillCredit>();
        private AddRepository<BillCredit> addRepository = new AddRepository<BillCredit>();
        private FindRepository<BillCredit> findRepository = new FindRepository<BillCredit>();
        private UpdateRepository<BillCredit> updateRepository = new UpdateRepository<BillCredit>();
        private RemoveRepository<BillCredit> removeRepository = new RemoveRepository<BillCredit>();

        public IQueryable<BillCredit> Get()
        {
            return getRepository.Get;
        }
        public BillCredit Add(BillCredit BillCredit)
        {
            return addRepository.Add(BillCredit);
        }
        public async Task<BillCredit> AddAsync(BillCredit BillCredit)
        {
            return await addRepository.AddAsync(BillCredit);
        }
        public BillCredit Update(BillCredit BillCredit)
        {
            return updateRepository.Update(BillCredit);
        }
        public async Task<BillCredit> UpdateAsync(BillCredit BillCredit)
        {
            return await updateRepository.UpdateAsync(BillCredit);
        }
        public int Remove(BillCredit BillCredit)
        {
            try
            {
                removeRepository.Remove(BillCredit);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<int> RemoveAsync(BillCredit BillCredit)
        {
            try
            {
                await removeRepository.RemoveAsync(BillCredit);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public BillCredit Find(string id)
        {
            return findRepository.Find(id);
        }
        public async Task<BillCredit> FindAsync(string id)
        {
            return await findRepository.FindAsync(id);
        }
    }
}

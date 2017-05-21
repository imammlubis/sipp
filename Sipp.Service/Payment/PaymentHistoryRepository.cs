using Sipp.Core.Data.GenericRepository;
using Sipp.Data.Entity.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Service.Payment
{
    public class PaymentHistoryRepository : IPaymentHistoryRepository
    {
        private GetRepository<PaymentHistory> getRepository = new GetRepository<PaymentHistory>();
        private AddRepository<PaymentHistory> addRepository = new AddRepository<PaymentHistory>();
        private FindRepository<PaymentHistory> findRepository = new FindRepository<PaymentHistory>();
        private UpdateRepository<PaymentHistory> updateRepository = new UpdateRepository<PaymentHistory>();
        private RemoveRepository<PaymentHistory> removeRepository = new RemoveRepository<PaymentHistory>();

        public IQueryable<PaymentHistory> Get()
        {
            return getRepository.Get;
        }
        public PaymentHistory Add(PaymentHistory PaymentHistory)
        {
            return addRepository.Add(PaymentHistory);
        }
        public async Task<PaymentHistory> AddAsync(PaymentHistory PaymentHistory)
        {
            return await addRepository.AddAsync(PaymentHistory);
        }
        public PaymentHistory Update(PaymentHistory PaymentHistory)
        {
            return updateRepository.Update(PaymentHistory);
        }
        public async Task<PaymentHistory> UpdateAsync(PaymentHistory PaymentHistory)
        {
            return await updateRepository.UpdateAsync(PaymentHistory);
        }
        public int Remove(PaymentHistory PaymentHistory)
        {
            try
            {
                removeRepository.Remove(PaymentHistory);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<int> RemoveAsync(PaymentHistory PaymentHistory)
        {
            try
            {
                await removeRepository.RemoveAsync(PaymentHistory);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public PaymentHistory Find(string id)
        {
            return findRepository.Find(id);
        }
        public async Task<PaymentHistory> FindAsync(string id)
        {
            return await findRepository.FindAsync(id);
        }
    }
}

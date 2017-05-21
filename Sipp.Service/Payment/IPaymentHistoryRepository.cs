using Sipp.Data.Entity.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Service.Payment
{
    public interface IPaymentHistoryRepository
    {
        IQueryable<PaymentHistory> Get();
        PaymentHistory Add(PaymentHistory IPaymentHistory);
        Task<PaymentHistory> AddAsync(PaymentHistory IPaymentHistory);
        PaymentHistory Update(PaymentHistory PaymentHistory);
        Task<PaymentHistory> UpdateAsync(PaymentHistory IPaymentHistory);
        int Remove(PaymentHistory IPaymentHistory);
        Task<int> RemoveAsync(PaymentHistory IPaymentHistory);
        PaymentHistory Find(string id);
        Task<PaymentHistory> FindAsync(string id);
    }
}

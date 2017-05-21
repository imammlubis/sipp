using Sipp.Data.Entity.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Service.Payment
{
    public interface IRegularPaymentRepository
    {
        IQueryable<RegularPayment> Get();
        RegularPayment Add(RegularPayment RegularPayment);
        Task<RegularPayment> AddAsync(RegularPayment RegularPayment);
        RegularPayment Update(RegularPayment RegularPayment);
        Task<RegularPayment> UpdateAsync(RegularPayment RegularPayment);
        int Remove(RegularPayment RegularPayment);
        Task<int> RemoveAsync(RegularPayment RegularPayment);
        RegularPayment Find(string id);
        Task<RegularPayment> FindAsync(string id);
    }
}

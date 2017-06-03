using Sipp.Data.Entity.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Service.Payment
{
    public interface IRegularBillRepository
    {
        IQueryable<RegularBill> Get();
        RegularBill Add(RegularBill IRegularBill);
        Task<RegularBill> AddAsync(RegularBill IRegularBill);
        RegularBill Update(RegularBill RegularBill);
        Task<RegularBill> UpdateAsync(RegularBill IRegularBill);
        int Remove(RegularBill IRegularBill);
        Task<int> RemoveAsync(RegularBill IRegularBill);
        RegularBill Find(string id);
        Task<RegularBill> FindAsync(string id);
    }
}

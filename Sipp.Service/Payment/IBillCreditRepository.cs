using Sipp.Data.Entity.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Service.Payment
{
    public interface IBillCreditRepository
    {
        IQueryable<BillCredit> Get();
        BillCredit Add(BillCredit IBillCredit);
        Task<BillCredit> AddAsync(BillCredit IBillCredit);
        BillCredit Update(BillCredit BillCredit);
        Task<BillCredit> UpdateAsync(BillCredit IBillCredit);
        int Remove(BillCredit IBillCredit);
        Task<int> RemoveAsync(BillCredit IBillCredit);
        BillCredit Find(string id);
        Task<BillCredit> FindAsync(string id);
    }
}

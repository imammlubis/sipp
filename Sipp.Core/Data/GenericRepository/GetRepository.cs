using Sipp.Core.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Core.Data.GenericRepository
{
    public class GetRepository<T> where T : class
    {
        readonly ApplicationDbContext _context = new ApplicationDbContext();

        public void Dispose()
        {
            if (_context == null)
                return;

            _context.Dispose();
        }

        public IQueryable<T> Get
        {
            get
            {
                return
                _context.Set<T>();
            }
        }
    }
}

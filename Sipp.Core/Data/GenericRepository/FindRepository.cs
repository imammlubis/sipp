using Sipp.Core.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Core.Data.GenericRepository
{
    public class FindRepository<T> where T : class
    {
        readonly ApplicationDbContext _context = new ApplicationDbContext();

        public void Dispose()
        {
            if (_context == null)
                return;

            _context.Dispose();
        }

        public T Find(object[] keyValues)
        {
            return _context.Set<T>().Find(keyValues);
        }

        public T Find(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public T Find(string id)
        {
            return _context.Set<T>().Find(id);
        }

        //public async Task<T> FindAsync(int id)
        //{
        //    return await _context.Set<T>().FindAsync(id);
        //}

        public async Task<T> FindAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

    }
}

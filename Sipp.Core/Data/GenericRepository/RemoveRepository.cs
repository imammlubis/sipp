using Sipp.Core.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Core.Data.GenericRepository
{
    public class RemoveRepository<T> where T : class
    {
        readonly ApplicationDbContext _context = new ApplicationDbContext();

        public void Dispose()
        {
            if (_context == null)
                return;

            _context.Dispose();
        }
        public void Remove(T entity)
        {

            _context.Set<T>().Attach(entity);
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public async Task RemoveAsync(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}

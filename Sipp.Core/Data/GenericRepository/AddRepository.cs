using Sipp.Core.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Core.Data.GenericRepository
{
    public class AddRepository<T> where T : class
    {
        readonly ApplicationDbContext _context = new ApplicationDbContext();
        public void Dispose()
        {
            if (_context == null)
                return;

            _context.Dispose();
        }

        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
            _context.Entry(entity).GetDatabaseValues();
            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            await _context.Entry(entity).GetDatabaseValuesAsync();
            return entity;

        }


    }
}

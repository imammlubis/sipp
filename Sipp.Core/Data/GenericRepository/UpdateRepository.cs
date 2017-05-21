using Sipp.Core.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Core.Data.GenericRepository
{
    public class UpdateRepository<T> where T : class
    {
        readonly ApplicationDbContext _context = new ApplicationDbContext();

        public void Dispose()
        {
            if (_context == null)
                return;

            _context.Dispose();
        }

        public T Update(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
                entry = _context.Entry(entity);
            }
            entry.State = EntityState.Modified;
            _context.SaveChanges();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
                entry = _context.Entry(entity);
            }
            entry.State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}

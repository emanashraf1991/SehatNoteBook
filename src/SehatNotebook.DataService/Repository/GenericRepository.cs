using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SehatNotebook.DataService.Data;
using SehatNotebook.DataService.IConfiguration;
using SehatNotebook.DataService.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SehatNotebook.DataService.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected AppDBContext _appDbContext ;
        protected readonly ILogger _logger;
        internal DbSet<T> dbset;
        public GenericRepository(AppDBContext appDbContext,ILogger logger)
        {
            _appDbContext= appDbContext;
            dbset= appDbContext.Set<T>();
            _logger=logger;
        }
        public virtual async Task<bool> Add(T entity)
        {
            await  dbset.AddAsync(entity);
            return true;
        }

        public Task<bool> Delete(Guid Id, string UserId)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbset.ToListAsync() ;
        }

        public virtual async Task<T> GetById(Guid Id)
        {
            return await dbset.FindAsync(Id);
        }

        public Task<bool> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehatNotebook.DataService.IConfiguration
{
    public interface IGenericRepository<T> where T : class  
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetById(Guid Id);
Task<bool> Add(T entity);
Task<bool> Update(T entity);
Task<bool> Delete(Guid Id, string UserId);

    }
}
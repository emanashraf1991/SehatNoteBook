using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SehatNotebook.DataService.IConfiguration;
using SehatNotebook.Entities.DBSet;

namespace SehatNotebook.DataService.IRepository
{
    public interface IUserRepository : IGenericRepository<User> 
    {
        Task<bool> UpdateUserProfile (User user);
        Task<User> GetIdentityById(Guid UserId);
    }
}
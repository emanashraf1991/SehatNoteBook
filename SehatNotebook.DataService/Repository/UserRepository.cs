using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SehatNotebook.DataService.Data;
using SehatNotebook.DataService.IRepository;
using SehatNotebook.Entities.DBSet;

namespace SehatNotebook.DataService.Repository
{
    public class UserRepository:GenericRepository<User>,IUserRepository
    {
        public UserRepository(AppDBContext appDBContext,ILogger logger ):base(appDBContext,logger)
        {
            
        }
          public override async Task<IEnumerable<User>> GetAllAsync()
        {
            try{
            return await dbset.Where(i=>i.Status==1).AsNoTracking() .ToListAsync() ;

             }catch(Exception ex){
            _logger.LogError(ex,"{Repo} has generate an error", typeof(UserRepository));
            return Enumerable.Empty<User>();
        }
        }
    }
}
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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDBContext appDBContext,ILogger logger ):base(appDBContext,logger)
        {
            
        }
        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            try{
                 return await dbset.Where(i=>i.Status==1).AsNoTracking() .ToListAsync();
             }
             catch(Exception ex){
                _logger.LogError(ex,"{Repo} has generate an error", typeof(UserRepository));
                return Enumerable.Empty<User>();
            }
        }

        public async Task<User> GetIdentityById(Guid IdentityId)
        {
            try{
                 return  await dbset.Where(i=>i.Status == 1 && i.IdentityId == IdentityId).FirstOrDefaultAsync() ;
           }
             catch(Exception ex){
                _logger.LogError(ex,"{Repo} GetIdentityById has generate an error", typeof(UserRepository));
                return null;
            }
        }

        public async Task<bool> UpdateUserProfile(User user)
        {
             try{
                 var userToUpdate= await dbset.Where(i=>i.Status == 1 && i.Id == user.Id).FirstOrDefaultAsync() ;
                 if(userToUpdate==null) return false; 
                 userToUpdate.FirstName = user.FirstName;
                 userToUpdate.LastName = user.LastName;
                 userToUpdate.Address = user.Address;
                 userToUpdate.Sex = user.Sex;
                 userToUpdate.UpdateDate = DateTime.UtcNow;
                 userToUpdate.Phone = user.Phone;
                 userToUpdate.MobileNumber = user.MobileNumber;
                 return true;
             }
             catch(Exception ex){
                _logger.LogError(ex,"{Repo} UpdateUserProfile has generate an error", typeof(UserRepository));
                return false;
            }
        }
    }
}
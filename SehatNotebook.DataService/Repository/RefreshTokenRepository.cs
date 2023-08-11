using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SehatNotebook.DataService.Data;
using SehatNotebook.DataService.IConfiguration;
using SehatNotebook.DataService.IRepository;
using SehatNotebook.DataService.Repository;
using SehatNotebook.Entities.DBSet;


namespace SehatNotebook.DataService
{   
    public class RefreshTokenRepository:GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AppDBContext appDBContext,ILogger logger ) : base(appDBContext,logger)
        {
            
        }

        

        public override async Task<IEnumerable<RefreshToken>> GetAllAsync()
        {
            try{
                return await dbset.Where(i=>i.Status==1).AsNoTracking() .ToListAsync() ;

             } catch(Exception ex){
                _logger.LogError(ex,"{Repo} has generate an error", typeof(RefreshTokenRepository));
                return Enumerable.Empty<RefreshToken>();
             }
        }

        public async Task<RefreshToken> GetByRefreshToken(string refreshToken)
        {
            try{
                return await dbset.Where(i=>i.Token.ToLower().Equals (refreshToken.ToLower())).AsNoTracking().FirstOrDefaultAsync();
            }
            catch(Exception ex){
                 _logger.LogError(ex,"{Repo} GetByRefreshToken has generate an error", typeof(RefreshTokenRepository));
                return null;
            }
        }

        public async Task<bool> MarkrefreshTokenAsUsed(RefreshToken refreshToken)
        {
            refreshToken.IsReused=true;
              try{
               var token= await dbset.Where(i=>i.Token.ToLower().Equals (refreshToken.Token.ToLower())).AsNoTracking().FirstOrDefaultAsync();
                if(token == null) return false;
                token.IsReused = refreshToken.IsReused;
                return true;
            }
            catch(Exception ex){
                 _logger.LogError(ex,"{Repo} GetByRefreshToken has generate an error", typeof(RefreshTokenRepository));
                return false;
            }
        }
    }
}

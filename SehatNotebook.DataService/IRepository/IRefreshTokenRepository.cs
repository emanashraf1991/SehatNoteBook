using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SehatNotebook.DataService.IConfiguration;
using SehatNotebook.Entities.DBSet;

namespace SehatNotebook.DataService
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken> 
    {
        Task<RefreshToken> GetByRefreshToken (string refreshToken);
        Task<bool> MarkrefreshTokenAsUsed (RefreshToken refreshToken);
    }
}

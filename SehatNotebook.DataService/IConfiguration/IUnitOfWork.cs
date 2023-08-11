using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SehatNotebook.DataService.IRepository;

namespace SehatNotebook.DataService.IConfiguration
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        Task CompleteAsync();
    }
}
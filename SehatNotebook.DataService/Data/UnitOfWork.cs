using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SehatNotebook.DataService.IConfiguration;
using SehatNotebook.DataService.IRepository;
using SehatNotebook.DataService.Repository;

namespace SehatNotebook.DataService.Data
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly AppDBContext _appDBContext;
        private readonly ILogger _logger;
        public IUserRepository Users {get; private set;}

      
        public UnitOfWork(AppDBContext appDBContext, ILoggerFactory loggerFactory)
        {
            _appDBContext=appDBContext;
            _logger=loggerFactory.CreateLogger("db_logs");
            Users=new UserRepository(appDBContext,_logger);
        }

        public async Task CompleteAsync()
        {
            await _appDBContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _appDBContext.Dispose();
        }
    }
}
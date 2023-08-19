using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SehatNotebook.DataService.IConfiguration;
using SehatNotebook.Entities.DBSet;

namespace SehatNotebook.DataService.IRepository
{
    public interface IHealthDataRepository : IGenericRepository<HealthData> 
    {
       
    }
}
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
    public class HealthDataRepository : GenericRepository<HealthData>, IHealthDataRepository
    {
        public HealthDataRepository(AppDBContext appDBContext, ILogger logger ):base(appDBContext,logger)
        {
            
        } 
        public override async Task<IEnumerable<HealthData>> GetAllAsync()
        {
            try{
                 return await dbset.Where(i=>i.Status==1).AsNoTracking() .ToListAsync();
             }
             catch(Exception ex){
                _logger.LogError(ex,"{Repo} has generate an error", typeof(HealthDataRepository));
                return Enumerable.Empty<HealthData>();
            }
        }

        // public async Task<HealthData> GetHealthDataById(Guid Id)
        // {
        //     try{
        //          return  await dbset.Where(i=>i.Status == 1 && i.Id == Id).FirstOrDefaultAsync() ;
        //    }
        //      catch(Exception ex){
        //         _logger.LogError(ex,"{Repo} GetIdentityById has generate an error", typeof(HealthDataRepository));
        //         return null;
        //     }
        // }

        public async Task<bool> UpdateHealthData(HealthData healthData)
        {
             try{
                 var healthDataToUpdate= await dbset.Where(i=>i.Status == 1 && i.Id == healthData.Id).FirstOrDefaultAsync() ;
                 if(healthDataToUpdate==null) 
                     return false; 
                 healthDataToUpdate.BloodType = healthData.BloodType;
                 healthDataToUpdate.Height = healthData.Height;
                 healthDataToUpdate.Weight = healthData.Weight;
                 healthDataToUpdate.UseGlasses = healthData.UseGlasses;
                 healthDataToUpdate.UpdateDate = DateTime.UtcNow; 
                 return true;
             }
             catch(Exception ex){
                _logger.LogError(ex,"{Repo} UpdateHealthDataProfile has generate an error", typeof(HealthDataRepository));
                return false;
            }
        }
    }
}
 
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SehatNotebook.Entities.DBSet;
using System.Data;
namespace SehatNotebook.DataService.Data{
    public class AppDBContext:IdentityDbContext{
        public virtual DbSet<User> Users {get;set;}
        public virtual DbSet<RefreshToken> RefreshTokens {get;set;}
        public virtual DbSet<HealthData> HealthData {get;set;}

        public AppDBContext(DbContextOptions<AppDBContext> options):base(options)
        {
            
        }
    }
}
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SehatNotebook.Entities.DBSet{
  public class HealthData : BaseEntity{
        public decimal Height { get; set; } 
        public decimal Weight { get; set; }
        public string BloodType { get; set; }  //should use enum    
        public bool UseGlasses { get; set; } 
     }
} 
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SehatNotebook.Entities.DBSet{
  public class RefreshToken : BaseEntity{
        public string UserId { get; set; }//The logged user Id
        public string Token { get; set; }
        public string JwtId { get; set; } //The Id generated when jwt Id requested
        public bool IsUsed { get; set; } //To make sure that it used once
        public bool IsRevoked { get; set; } //make sure it still valid
        public DateTime ExpiryDate { get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }   
     }
}
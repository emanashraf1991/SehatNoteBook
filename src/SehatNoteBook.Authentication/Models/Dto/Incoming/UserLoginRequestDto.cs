using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehatNoteBook.Authentication.Models.Dto.Incoming
{
    public class UserLoginRequestDto
    {    
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
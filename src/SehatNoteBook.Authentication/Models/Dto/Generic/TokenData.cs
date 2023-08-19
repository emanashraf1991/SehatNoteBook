using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehatNoteBook.Authentication
{
    //used for internal requests
    public class TokenData
    {
         public string JwtToken { get; set; }
         public string RefreshToken { get; set; }
    }
}

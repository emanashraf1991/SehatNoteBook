using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehatNoteBook.Entities.Dtos.Incoming
{
    public class UserDto
    {
          public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }
    }
}
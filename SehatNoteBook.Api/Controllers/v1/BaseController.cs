using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SehatNotebook.DataService.Data;
using SehatNoteBook.Entities.Dtos.Incoming;
using SehatNotebook.Entities.DBSet;
 using  SehatNotebook.DataService.IConfiguration;
using Microsoft.AspNetCore.Identity;

namespace SehatNoteBook.Api.Controllers.v1
{
     [ApiController]
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiVersion("1.0")]
    public class BaseController : ControllerBase
    {
         public IUnitOfWork _unitOFWork;
         public UserManager<IdentityUser> _userManager;
        public BaseController(IUnitOfWork unitOfWork,UserManager<IdentityUser> userManager)
        {
           _unitOFWork=unitOfWork;
           _userManager=userManager;
        }
       
    }
}
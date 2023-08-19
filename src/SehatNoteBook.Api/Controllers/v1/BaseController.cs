using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SehatNotebook.DataService.Data; 
using SehatNotebook.Entities.DBSet;
 using  SehatNotebook.DataService.IConfiguration;
using Microsoft.AspNetCore.Identity;
using SehatNoteBook.Authentication;
using AutoMapper;

namespace SehatNoteBook.Api.Controllers.v1
{
     [ApiController]
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiVersion("1.0")]
    public class BaseController : ControllerBase
    {
        public IUnitOfWork _unitOFWork;
        public UserManager<IdentityUser> _userManager;
        public readonly IMapper _mapper;
        public BaseController(IUnitOfWork unitOfWork,UserManager<IdentityUser> userManager, IMapper mapper)
        {
           _unitOFWork=unitOfWork;
           _userManager=userManager;
           _mapper=mapper;
        }
       internal Error PopulateError(string code,string msg,string type){
        return new Error {
            Code=code,
            Message=msg,
            Type=type
        };
       }
    }
}
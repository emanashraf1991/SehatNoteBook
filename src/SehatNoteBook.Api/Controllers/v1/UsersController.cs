using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SehatNotebook.DataService.Data; 
using SehatNotebook.Entities.DBSet;
 using  SehatNotebook.DataService.IConfiguration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SehatNoteBook.Authentication;
using SehatNotebook.Configuration.Messages;
using AutoMapper;
using SehatNoteBook.Entities.Dtos.Incoming;

namespace SehatNoteBook.Api.Controllers.v1
{    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : BaseController
    {
        public UsersController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager,IMapper mapper)
        :base(unitOfWork, userManager, mapper)
        {
            
        }
        [HttpGet]
        [Route("GetUsers",Name ="GetUsers")]
        public async Task< IActionResult> GetUsers(){
            var result=new PagedResult<User>();
            var users=await _unitOFWork.Users.GetAllAsync();
            result.Content=users.ToList();
            result.ResultsCount= users.Count();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetUserById",Name ="GetUserById")]
        public async Task< IActionResult> GetUserById(Guid Id){
            var result = new Result<User>();
            var user=await _unitOFWork.Users.GetById(Id);
            if(user!=null){
                result.Content=user;
                return Ok(result);
            }
           result.Error =PopulateError("404", ErrorMessages.Profile.UserNotFound, ErrorMessages.Generic.UnableToProcess); 
           return BadRequest(result); 
        }  
        [HttpPost]
        [Route("AddUser",Name ="AddUser")]
        public async Task< IActionResult> AddUser( UserDto userDto){
            var _mappedUser= _mapper.Map<User>(userDto);
            await _unitOFWork.Users.Add(_mappedUser);
            await _unitOFWork.CompleteAsync();

            var result = new Result<UserDto>();
            result.Content = userDto;

            return CreatedAtRoute("GetUserById",new {Id= _mappedUser.Id}, result);
        }

    }
}
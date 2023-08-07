using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SehatNotebook.DataService.Data;
using SehatNoteBook.Entities.Dtos.Incoming;
using SehatNotebook.Entities.DBSet;
 using  SehatNotebook.DataService.IConfiguration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace SehatNoteBook.Api.Controllers.v1
{    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : BaseController
    {
        public UsersController(IUnitOfWork unitOfWork):base(unitOfWork)
        {
            
        }
        [HttpGet]
        [Route("GetUsers",Name ="GetUsers")]
        public async Task< IActionResult> GetUsers(){
            var users=await _unitOFWork.Users.GetAllAsync();
            return Ok(users);
        }
         [HttpGet]
        [Route("GetUserById",Name ="GetUserById")]
        public async Task< IActionResult> GetUserById(Guid Id){
            var user=await _unitOFWork.Users.GetById(Id);
            return Ok(user);
        }
       // [HttpPost]
        //[Route("AddUser")]
        //public async Task< IActionResult> AddUser(UserDto userDto)
        //{
          
          //  return CreatedAtRoute("GetUsers",new {Id =user.Id},user);
        //}
        

    }}
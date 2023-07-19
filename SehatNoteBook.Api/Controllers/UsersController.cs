using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SehatNotebook.DataService.Data;
using SehatNoteBook.Entities.Dtos.Incoming;
using SehatNotebook.Entities.DBSet;
 
namespace SehatNoteBook.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private AppDBContext _appDBContext;
        public UsersController(AppDBContext appDBContext)
        {
            _appDBContext=appDBContext;
        }
        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers(){
            var users= _appDBContext.Users.Where(i=>i.Status==1).ToList();
            return Ok(users);
        }
        [HttpPost]
        [Route("AddUser")]
        public IActionResult AddUser(UserDto userDto)
        {
            var user =new User();
            user.Status= 1;
            user.FirstName= userDto.FirstName;
            user.LastName= userDto.LastName;
            user.Phone= userDto.Phone;
            user.Country= userDto.Country;
            user.Address= userDto.Address;
            user.DateOfBirth=Convert.ToDateTime( userDto.DateOfBirth);

            _appDBContext.Users.Add(user);
            _appDBContext.SaveChanges();
            return Ok();
        }
        

    }}
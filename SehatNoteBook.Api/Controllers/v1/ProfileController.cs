using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SehatNotebook.DataService.Data;
using SehatNoteBook.Entities.Dtos;
using SehatNotebook.Entities.DBSet;
 using  SehatNotebook.DataService.IConfiguration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SehatNoteBook.Authentication;

namespace SehatNoteBook.Api.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileController : BaseController
    {
        public ProfileController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        :base(unitOfWork, userManager)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile(){
            var loggedUser =await _userManager.GetUserAsync(HttpContext.User);
            if(loggedUser == null)
             return BadRequest("User not found!"); 
            var IdentityId=new Guid( loggedUser.Id);
            var profile=await _unitOFWork.Users.GetIdentityById(IdentityId);
              if(profile == null)
             return BadRequest("profile not found!");
            //should create dto, will update later
            return Ok(profile);
        }
        [HttpPut]
        [Route("PostProfile")]
        public async Task<IActionResult> PostProfile([FromBody] UpdateProfileDto profileDto){
            if(!ModelState.IsValid)
            {
                return BadRequest("Invalid Payload");
            }
            var loggedUser= await _userManager.GetUserAsync(HttpContext.User);
            if(loggedUser == null)
                return BadRequest("User not found!"); 
            var IdentityId=new Guid( loggedUser.Id );
            var profile=await _unitOFWork.Users.GetIdentityById(IdentityId);
            if( profile == null )
                return BadRequest("profile not found!");

            profile.Sex = profileDto.Sex;
            profile.MobileNumber = profileDto.MobileNumber;
            profile.Address = profileDto.Address;
            profile.Country = profileDto.Country;

            var isUpdated = await _unitOFWork.Users.UpdateUserProfile(profile);
            if(isUpdated){
                await _unitOFWork.CompleteAsync();
                return Ok(profile);
            }
            else return BadRequest ("Error, profile not updated");
        }
    }
}

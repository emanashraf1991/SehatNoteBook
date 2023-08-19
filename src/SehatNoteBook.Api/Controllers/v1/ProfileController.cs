using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SehatNotebook.DataService.Data;  
using SehatNotebook.DataService.IConfiguration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SehatNoteBook.Authentication; 
using SehatNotebook.Configuration.Messages;
using SehatNotebook.Entities.DBSet;
using AutoMapper;
using SehatNoteBook.Entities.Dtos.Outgoing;

namespace SehatNoteBook.Api.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileController : BaseController
    {
        public ProfileController( IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, IMapper mapper)
        :base( unitOfWork, userManager, mapper)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var loggedUser =await _userManager.GetUserAsync(HttpContext.User);
            var result = new Result<ProfileDto>();
           
            if(loggedUser == null){
                result.Error =PopulateError("400", ErrorMessages.Profile.UserNotFound, ErrorMessages.Generic.TypeBadRequest); 

                return BadRequest(result); 
            }
             
            var IdentityId=new Guid( loggedUser.Id);
            var profile=await _unitOFWork.Users.GetIdentityById(IdentityId);
            if(profile == null)
                {
                    result.Error =PopulateError("400", ErrorMessages.Profile.UserNotFound, ErrorMessages.Generic.TypeBadRequest); 

                    return BadRequest(result); 
                }
              var _mappedProfile= _mapper.Map<ProfileDto>(profile);

            result.Content = _mappedProfile;
            return Ok(result);
        }
        [HttpPut]
        [Route("PostProfile")]
        public async Task<IActionResult> PostProfile([FromBody] UpdateProfileDto profileDto){
           var result=new Result<ProfileDto>();
            if(!ModelState.IsValid)
            {
                 result.Error =PopulateError( "400",ErrorMessages.Generic.InvalidPayload, ErrorMessages.Generic.TypeBadRequest);
                    
                return BadRequest(result);                
            }
            var loggedUser= await _userManager.GetUserAsync(HttpContext.User);
            if(loggedUser == null)
            {
                result.Error =PopulateError("400",ErrorMessages.Profile.UserNotFound, ErrorMessages.Generic.TypeBadRequest); 
                return BadRequest(result);     
            } 
            var IdentityId=new Guid( loggedUser.Id );
            var profile=await _unitOFWork.Users.GetIdentityById(IdentityId);
            if( profile == null )
            {
                result.Error =PopulateError("400",ErrorMessages.Profile.ProfileNotFound,ErrorMessages.Generic.TypeBadRequest);               
                 
                return BadRequest(result); 
            } 

            profile.Address= profileDto.Address;
            profile.Sex= profileDto.Sex;
            profile.MobileNumber= profileDto.MobileNumber;
            profile.Country= profileDto.Country;

            var isUpdated = await _unitOFWork.Users.UpdateUserProfile(profile);
            if(isUpdated){
                await _unitOFWork.CompleteAsync();
                var _mappedProfile= _mapper.Map<ProfileDto>(profileDto);
                result.Content = _mappedProfile;
                return Ok(result);
            }
            else{
                result.Error = PopulateError("400", ErrorMessages.Generic.SomethingWentWrong, ErrorMessages.Generic.UnableToProcess);               
                return BadRequest(result); 
            }  
        }
    }
}

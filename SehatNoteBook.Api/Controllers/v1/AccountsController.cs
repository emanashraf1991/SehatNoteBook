using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SehatNotebook.DataService.Data;
using SehatNoteBook.Entities.Dtos.Incoming;
using SehatNotebook.Entities.DBSet;
using SehatNotebook.DataService.IConfiguration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options ;
using SehatNoteBook.Authentication.Configuration;
using SehatNoteBook.Authentication.Models.Dto.Incoming;
using SehatNoteBook.Authentication.Models.Dto.Outgoing;
using Microsoft.AspNetCore;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace SehatNoteBook.Api.Controllers.v1
{ 
    public class AccountsController : BaseController
    {
         private readonly UserManager<IdentityUser> _userManager;
         private readonly JwtConfig _jwtConfig;

         public AccountsController(
            IUnitOfWork unitOfWork,
            UserManager<IdentityUser> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor):base(unitOfWork)
        {
            _userManager=userManager;
            _jwtConfig=optionsMonitor.CurrentValue;
        }


  [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLoginRequestDto loginDto){
                
                if(ModelState.IsValid){
                    var userExist = await _userManager.FindByEmailAsync(loginDto.Email);
                    
                    if(userExist == null){
                        return BadRequest( new UserLoginResponseDto{
                                Success=false,
                                Errors=new List<string>{
                                    "Invalid Authentication Request " 
                                }
                            }
                        );
                    }

                    var isValidPass= await _userManager.CheckPasswordAsync(userExist,loginDto.Password);
           if(isValidPass){
                var token=GenerateJwtToken(userExist);
                return Ok(new UserLoginResponseDto{
                            Success=true,
                            Token= token
                        });
           }
           else{
            return BadRequest( new UserLoginResponseDto{
                        Success=false,
                        Errors=new List<string>{
                           "Invalid Authentication Request " 
                         }
                      }
                    );
           }
            }
            else
                {
                    return BadRequest( new UserLoginResponseDto{
                        Success=false,
                        Errors=new List<string>{
                            "Invalid Payload"
                        }
                      }
                    );
                }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserRegisterationRequestDto registerDto){
                
                if(ModelState.IsValid){
                    var userExist = await _userManager.FindByEmailAsync(registerDto.Email);
                    
                    if(userExist != null){
                        return BadRequest( new UserRegisterationResponseDto{
                                Success=false,
                                Errors=new List<string>{
                                    "Email already in use "+userExist.ToString()
                                }
                            }
                        );
                    }

                    var newUser= new IdentityUser{
                        Email= registerDto.Email,
                        UserName=registerDto.Email,
                        EmailConfirmed=true //I'll modify it later
                    };

                    var isCreated = await  _userManager.CreateAsync(newUser,registerDto.Password);

                    if(!isCreated.Succeeded)
                    {
                          return BadRequest( new UserRegisterationResponseDto{
                            Success=isCreated.Succeeded,
                            Errors=isCreated.Errors.Select(e=>e.Description).ToList()
                          }
                       );  
                    }

                    var token = GenerateJwtToken(newUser);

                    var user =new User();
                    user.IdentityId =new Guid( newUser.Id);
                    user.Status= 1;
                    user.Email= registerDto.Email;
                    user.FirstName= registerDto.FirstName;
                    user.LastName= registerDto.LastName;
                    user.Phone="";// userDto.Phone;
                    user.Country="";// userDto.Country;
                    user.Address= "";//userDto.Address;
                    user.DateOfBirth=DateTime.UtcNow;//Convert.ToDateTime( userDto.DateOfBirth);

                    await _unitOFWork.Users.Add(user);
                    await _unitOFWork.CompleteAsync();

                    return Ok(new UserRegisterationResponseDto{
                        Success=true,
                        Token= token
                    });
                }
                else
                {
                    return BadRequest( new UserRegisterationResponseDto{
                        Success=false,
                        Errors=new List<string>{
                            "Invalid Payload"
                        }
                      }
                    );
                }
        }

        private string GenerateJwtToken(IdentityUser user){
            var jwtHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor =  new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(new []{
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires= DateTime.UtcNow.AddHours(3),//Update later to 5 min
                SigningCredentials=new SigningCredentials(
                    new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = jwtHandler.CreateToken(tokenDescriptor);
            var jwtToken= jwtHandler.WriteToken(token);
            return jwtToken;
        }
    }
}
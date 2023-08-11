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
using System.Security.Cryptography;
using SehatNoteBook.Authentication;

namespace SehatNoteBook.Api.Controllers.v1
{ 
    public class AccountsController : BaseController
    {
         private readonly UserManager<IdentityUser> _userManager;
         private readonly TokenValidationParameters _tokenValidationParameters;             
         private readonly JwtConfig _jwtConfig;

         public AccountsController(
            IUnitOfWork unitOfWork,
            UserManager<IdentityUser> userManager,
            TokenValidationParameters tokenValidationParameters,
            IOptionsMonitor<JwtConfig> optionsMonitor):base(unitOfWork)
        {
            _userManager=userManager;
            _tokenValidationParameters = tokenValidationParameters; 
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
                        var token= await GenerateJwtToken(userExist);
                        return Ok( 
                            new UserLoginResponseDto{
                                Success=true,
                                Token= token.JwtToken,
                                RefreshToken= token.RefreshToken,
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

                    var token =await GenerateJwtToken(newUser);

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
                        Token= token.JwtToken,
                        RefreshToken= token.RefreshToken,
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

        private async Task<TokenData> GenerateJwtToken(IdentityUser user){
           
            var jwtHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor =  new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(new []{
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires= DateTime.UtcNow.Add(_jwtConfig.ExpiryTimeFrame),//Update later to 5 min
                SigningCredentials=new SigningCredentials(
                    new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = jwtHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtHandler.WriteToken(token);

            //Generate refresh token
            var refreshToken = new RefreshToken{
                AddedDate=DateTime.UtcNow,
                Token = GenerateRefreshToken(),
                UserId = user.Id,
                IsReused=false,
                IsRevoked = false,
                JwtId = token.Id,
                Status = 1,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            await _unitOFWork.RefreshTokens.Add(refreshToken);
            await _unitOFWork.CompleteAsync();

            var tokenData = new TokenData{
                JwtToken= jwtToken,
                RefreshToken = refreshToken.Token
            };

            return tokenData;
        }
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDto tokenDto){
            if(ModelState.IsValid){
                //verify token
                var result = await VerifyToken(tokenDto);
                if(result == null){
                    return BadRequest( new UserRegisterationResponseDto{
                        Success=false,
                        Errors=new List<string>{
                            "Token Validation failed!"
                        }
                        }
                    );
                }
                return Ok(result);
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
        private async Task<AuthResult> VerifyToken(TokenRequestDto token){
            var tokenHandler = new JwtSecurityTokenHandler();
            try{
                //We need to check the validity of the token
               var principle =  tokenHandler.ValidateToken(token.Token,_tokenValidationParameters,out var validatedToken);

                if(validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase);
                    if(!result)
                        return null;
                }
                //We need to check the expiry date
                var utcExpiryDate = long.Parse(principle.Claims.FirstOrDefault(x=>x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expDate = UnixTimeStampToDateTime(utcExpiryDate);

                if(expDate > DateTime.Now){
                    return new AuthResult{
                        Success = false,
                        Errors = new List<string>{"JwtToken has not expired"}
                    };
                }

                var refreshTokenExist = await _unitOFWork.RefreshTokens.GetByRefreshToken(token.RefreshToken);
                if(refreshTokenExist==null){
                    return new AuthResult{
                        Success = false,
                        Errors = new List<string>{"Invalid RefreshToken "}
                    };
                }

                if(DateTime.Now > refreshTokenExist.ExpiryDate){
                     return new AuthResult{
                        Success = false,
                        Errors = new List<string>{"Refresh Token has expired, It cannot be used"}
                    };
                }
                  if(refreshTokenExist.IsRevoked){
                     return new AuthResult{
                        Success = false,
                        Errors = new List<string>{"Refresh Token has been revoked, It cannot be used"}
                    };
                }
                
                var jti = principle.Claims.FirstOrDefault(x=>x.Type == JwtRegisteredClaimNames.Jti).Value;
                if(refreshTokenExist.JwtId != jti){
                     return new AuthResult{
                        Success = false,
                        Errors = new List<string>{"Refresh Token reference does not match the jwt token"}
                    };
                }
                //Start Processing and get new token
                refreshTokenExist.IsReused =true;
              var updateResult=  await _unitOFWork.RefreshTokens.MarkrefreshTokenAsUsed(refreshTokenExist);
            if(updateResult){
                await _unitOFWork.CompleteAsync();
                var dbUser= await _userManager.FindByIdAsync(refreshTokenExist.UserId);
                if(dbUser == null)
                {  
                     return new AuthResult{
                        Success = false,
                        Errors = new List<string>{"Error processing request."}
                    };
                }
                var accessToken= await GenerateJwtToken(dbUser);

                return new AuthResult{
                    Token = accessToken.JwtToken,
                    RefreshToken = accessToken.RefreshToken,
                    Success = true
                };
            }
            else{
                  return new AuthResult{
                        Success = false,
                        Errors = new List<string>{"Error processing request."}
                    };
            }
            }
            catch(Exception){
                      return new AuthResult{
                        Success = false,
                        Errors = new List<string>{"Error processing request."}
                    };
            }
        }

       public static DateTime UnixTimeStampToDateTime( double unixTimeStamp )
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
            return dateTime;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
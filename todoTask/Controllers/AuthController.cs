using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using todoTask.Models.DTO;
using todoTask.Repositories.Tokens;

/*
 * For Reader
 UserName: user@example.com
 Password: string
 */
namespace todoTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;


        // User Manager class used to create a new User
        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            this._userManager = userManager;
            this._tokenRepository = tokenRepository;
        } 

        // Post: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            //CreateAsync is inside of the user manager class
            var identityResult =  await _userManager.CreateAsync(identityUser,registerRequestDto.Password);
            
            if (identityResult.Succeeded)
            {
                // Add Roles to this User
                if(registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                   identityResult = await _userManager.AddToRolesAsync(identityUser,registerRequestDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        //Creating Jwt Token
                        return Ok("Message: User was Register! Please Login");
                    }
                }
            }

            return BadRequest("Message: Something Went Wrong!");
        
        }

        // Post :/api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var uservalidate =  await _userManager.FindByEmailAsync(loginRequestDto.UserName);

            if (uservalidate != null)
            {
                var checkPassword = await _userManager.CheckPasswordAsync(uservalidate, loginRequestDto.Password);

                if (checkPassword)
                {
                    //Get roles for this user
                    var roles = await _userManager.GetRolesAsync(uservalidate);

                    if (roles !=null)
                    {
                        // Create Token
                        var jwtToken = _tokenRepository.CreateJwtToken(uservalidate, roles.ToList());

                        //Assign Jwttoken to LoginResponseDto
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken,
                        };

                        return Ok(response);
                    }

                    return Ok("Login successfully!");
                }
            }

            return BadRequest("UserName or Password incorrect!");
        }

    }
}

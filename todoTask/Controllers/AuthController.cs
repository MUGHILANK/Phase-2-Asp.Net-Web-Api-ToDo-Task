using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using todoTask.Models.DTO;
using todoTask.Services.JWT;

namespace todoTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtValidateServices _jwtValidateServices;

        // User Manager class used to create a new User
        public AuthController(UserManager<IdentityUser> userManager,IJwtValidateServices jwtValidateServices)
        {
            this._userManager = userManager;
            this._jwtValidateServices = jwtValidateServices;
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

            var identityResult =  await _userManager.CreateAsync(identityUser,registerRequestDto.Password);
            
            if (identityResult.Succeeded)
            {
                // Add Roles to this User
                if(registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                   identityResult = await _userManager.AddToRolesAsync(identityUser,registerRequestDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("Message: User was Register! Please Login");
                    }
                }
            }

            return BadRequest("Message: Something Went Wrong!");
        
        }



    }
}

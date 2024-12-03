using AuthApi.Application.DTOs;
using AuthApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Responses;

namespace AuthApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IUser userInterface) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<Response>> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await userInterface.Login(loginDTO);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetUserDTO>> GetUser(int id)
        {
            if(id <= 0)
            {
                return BadRequest("invalid user ID");
            }
            var user = await userInterface.GetUser(id);
            return user.ID > 0 ? Ok(user) : BadRequest(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<Response>> Register(AppUserDTO appUserDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await userInterface.Register(appUserDTO);
            return result.Flag ? Ok(result) : BadRequest(result);
        }
    }
}

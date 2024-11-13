using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MShop.Application.Constants;
using MShop.Application.DTOs;
using MShop.Application.Enums;
using MShop.Application.Interfaces;
using MShop.Application.SharedFunct;

namespace MShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(SystemRegisterUserDTO userDTO)
        {
            try
            {
                var result = await _authService.RegisterSystemUser(userDTO);

                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                var errorResponse = Utils.ActionCatch<IEnumerable<SystemRegisterUserDTO>>(EnumErrCode.Error, ApiMessage.ErrorMsg, ex.Message);
                return BadRequest(errorResponse);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login (SystemSignInUserDTO user)
        {
            try
            {
                var result = await _authService.LoginSystemUser(user);

                if (result.Success)
                {
                    return Ok(result);
                }
                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                var errorResponse = Utils.ActionCatch<IEnumerable<SystemRegisterUserDTO>>(EnumErrCode.Error, ApiMessage.ErrorMsg, ex.Message);
                return BadRequest(errorResponse);
            }
        }

    }
}

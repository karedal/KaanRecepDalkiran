using System;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Sum.Api.Models;
using Sum.Api.Service;

namespace Sum.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private static readonly ILog _log = LogManager.GetLogger(typeof(AuthController));
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost(nameof(Register))]
        public async Task<ActionResult<AuthenticationResult>> Register([FromBody] RegisterDto request)
        {
            try
            {
                var result = await _userService.RegisterAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _log.Error($"UserController Register method - {ex.Message}", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(nameof(Login))]
        public async Task<ActionResult<AuthenticationResult>> Login([FromBody] LoginDto request)
        {
            try
            {
                var result = await _userService.LoginAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _log.Error($"UserController Login method - {ex.Message}", ex);
                return BadRequest(ex.Message);
            }
        }

    }
}
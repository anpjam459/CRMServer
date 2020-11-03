using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMServer.Api.Services;
using CRMServer.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRMServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        //api/auth/register
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model);
                if (result.IsSusscess) return Ok(result);

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid"); //status code 400
        }

        //api/auth/login
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(model);
                if (result.IsSusscess) return Ok(result);

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid"); //status code 400
        }
    }
}

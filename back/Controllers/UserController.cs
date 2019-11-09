using Backend.Entities;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }
        [Authorize(Policy = "admin")]
        [HttpGet]
        [Route("User")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(_service.GetUsers());
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("User/Login")]
        public ActionResult<User> Login(User user)
        {
            var returned = _service.ValidateLogin(user.Email, user.Password);
            if (!String.IsNullOrEmpty(returned.Token))
            {
                return Ok(returned);
            }
            else
            {
                return Unauthorized(returned);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("User")]
        public ActionResult<User> CreateUser(User user)
        { 
            try
            {
                return Created("User", _service.Register(user));
            }
            catch(Exception e)
            {
                return BadRequest(new { Message = e.Message});
            }
        }
        private JwtSecurityToken GetTokenFromRequest(HttpRequest request)
        {
            var jwt = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            return token;
        }
    }
}

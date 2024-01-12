using EzBooking.Dtto;
using EzBooking.Models;
using EzBooking.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Cryptography;

namespace EzBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly UserRepo _userRepo;
        

        public AuthController(UserRepo userRepo) 
        {
            _userRepo = userRepo;
            
        }


        [HttpPost("login")]
        [ProducesResponseType(400)]
        public async Task<ActionResult<string>> Login([FromBody] LoginDto login)
        {
            var getuser = _userRepo.GetUserByEmail(login.email);

            if(getuser==null || !BCrypt.Net.BCrypt.Verify(login.password, getuser.password))
            {
                return BadRequest("Dados inválidos");
            }
            else
            {
                string token = _userRepo.CreateToken(getuser);
                getuser.token = token;
                _userRepo.UpdateUser(getuser);
                return Ok(token);
            }
            
        }


    }

}

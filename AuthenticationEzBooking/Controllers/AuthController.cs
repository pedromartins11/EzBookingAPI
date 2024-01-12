using Microsoft.AspNetCore.Mvc;
using AuthenticationEzBooking.Repository;
using AuthenticationEzBooking.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using AuthenticationEzBooking.Dto;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;

namespace AuthenticationEzBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly AuthRepo _authRepo;

        public AuthController(AuthRepo authRepo)
        {
            _authRepo = authRepo;
        }


        [HttpPost("login")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<string>> Login([FromBody] AuthDto login)
        {
            var getuser = _authRepo.GetUserByEmail(login.email);

            if (getuser == null || !BCrypt.Net.BCrypt.Verify(login.password, getuser.password))
            {
                return BadRequest("Dados inválidos");
            }
            else
            {
                int userId = _authRepo.GetUserIdbyEmail(getuser.email);
                int getUserType = _authRepo.GetUserTypeById(userId);
                string token = _authRepo.CreateToken(userId, getUserType);
                _authRepo.UpdateUserToken(userId, token);

                //var refreshToken = _authRepo.GenerateRefreshToken();
                //_authRepo.SetRefreshToken(refreshToken, Response);
                //_authRepo.UpdateUserRefreshToken(userId, refreshToken.token);
                //_authRepo.UpdateUserTokenCreated(userId, refreshToken.Created);
                //_authRepo.UpdateUserTokenExpires(userId, refreshToken.Expires);


                return Ok(token);
            }

        }

        /*
        [HttpPost("logout")]
        [AuthAuthorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (_authRepo.IsTokenValid(token))
            {
                _authRepo.RevokeToken(token);
            }
            else
                return BadRequest("Acesso não autorizado: Token inválido");

            return Ok("Logout bem sucedido");

        }

        //[HttpPost("refresh-token")]
        //public async Task<ActionResult<string>> RefreshToken()
        //{
        //    var refreshToken = Request.Cookies["refreshToken"];
        //    var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    int.TryParse(userId, out int userIdInt);

        //    int getUserType = _authRepo.GetUserTypeById(userIdInt);
        //    string refreshTokenUser = _authRepo.GetRefreshTokenById(userIdInt);
        //    DateTime tokenExpiresUser = _authRepo.GetTokenExpiresById(userIdInt);
        //    if (refreshTokenUser != refreshToken)
        //    {
        //        return BadRequest("Refresh Token inválido");
        //    }
        //    else if(tokenExpiresUser < DateTime.Now)
        //    {
        //        return Unauthorized("Token expirou!");
        //    }

        //    string token = _authRepo.CreateToken(userIdInt, getUserType);
        //    var newRefreshToken = _authRepo.GenerateRefreshToken();
        //    _authRepo.UpdateUserRefreshToken(userIdInt, newRefreshToken.token);
        //    _authRepo.SetRefreshToken(newRefreshToken, Response);

        //    return Ok(token);
        //}

    }

}

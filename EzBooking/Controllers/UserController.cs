using EzBooking.Models;
using EzBooking.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Text;
using EzBooking.Dtto;
using System.Security.Claims;

namespace EzBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserRepo _userRepo;
        private readonly UserTypesRepo _userTypesRepo;

        public UserController(UserRepo userRepo, UserTypesRepo userTypeRepo)
        {

            _userRepo = userRepo;
            _userTypesRepo = userTypeRepo;
        }

        /// <summary>
        /// Obtém todos os utilizadores.
        /// </summary>
        /// <returns>Uma lista de Utilizadores.</returns>
        [HttpGet]
        [AuthAuthorize]
        [AdminAuthorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userRepo.GetUsers();

            if (users == null || users.Count == 0)
            {
                return NotFound("Nenhum utilizador encontrado."); //404
            }

            return Ok(users);
        }


        /// <summary>
        /// Obtém um utilizador
        /// </summary>
        /// <param name="userId" example ="1">O ID do Utilizador</param>
        /// <returns>Um utilizador</returns>
        //[AuthAuthorize]
        [HttpGet("{userId}")]
        [ProducesResponseType(200)] 
        [ProducesResponseType(404)]
        public ActionResult<User> GetUser(int userId)
        {
            var user = _userRepo.GetUser(userId);

            if (user == null)
            {
                return NotFound("User not found"); //404
            }

            return Ok(user);
        }

        /// <summary>
        /// Obtém Casas do Utilizador
        /// </summary>
        /// <param name="id" example ="1">O ID do Utilizador</param>
        /// <returns>Um Utilizador.</returns>
        [HttpGet("Houses/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<User>> GetHousesFromUser(int id)
        {
            var user = await _userRepo.GetHousesFromUser(id);

            if (user == null)
            {
                return NotFound("Utilizador não encontrado."); // Código 404
            }

            if (user.Houses == null || !user.Houses.Any())
            {
                return NotFound("Utilizador não tem casas."); // Código 404
            }

            if (id <= 0)
            {
                return BadRequest("ID inválido."); // Código 400 se o ID for inválido.
            }

            return Ok(user);
        }

        /// <summary>
        /// Obtém Reservas do Utilizador
        /// </summary>
        /// <param name="id" example ="1">O ID do Utilizador</param>
        /// <returns>Um Utilizador.</returns>
        [HttpGet("Reservations/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<User>> GetReservationsFromUser(int id)
        {
            var user = await _userRepo.GetReservationsFromUser(id);

            if (user == null)
            {
                return NotFound("Utilizador não tem reservas."); // Código 404
            }


            if (id <= 0)
            {
                return BadRequest("ID inválido."); // Código 400 se o ID for inválido.
            }

            return Ok(user);
        }

        /// <summary>
        /// Cria um utilizador
        /// </summary>
        /// <param name="userCreate"></param>
        /// <returns>Cria um utilizador</returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] User userCreate)
        {
            if (userCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var checkEmail = _userRepo.CheckEmail(userCreate.email);
            if (checkEmail == true)
            {
                ModelState.AddModelError("", "Email already exists");
                return StatusCode(422, ModelState);
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userCreate.password);

            userCreate.password = hashedPassword;
            userCreate.status = true;

            UserTypes userType = _userTypesRepo.GetUserType(1);
            userCreate.userType = userType;

            bool created = _userRepo.CreateUser(userCreate);

            if (created)
            {
                return Ok("Successfully created");
            }
            else
            {
                ModelState.AddModelError("Database", "Failed to save the user");
                return BadRequest(ModelState);
            }  
        }

        /// <summary>
        /// Altera avatar do utilizador
        /// </summary>
        [HttpPut("avatar")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateAvatar([FromForm] AvatarDto viewModel)
        {
            if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                
                var uniqueFileName = $"{userId}_{Path.GetFileName(viewModel.ImageFile.FileName)}.jpg";
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Users", uniqueFileName);

                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    viewModel.ImageFile.CopyToAsync(fileStream);
                }


                var userLogged = _userRepo.GetUser(int.Parse(userId));
                userLogged.image = uniqueFileName;

                // Atualizar o usuário apenas se ele não for nulo
                bool updated = _userRepo.UpdateUser(userLogged);

                if (updated)
                {
                    return Ok("Successfully updated");
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong updating owner");
                    return StatusCode(500, ModelState);
                }
            }
            else
            {
                ModelState.AddModelError("", "Image file is missing");
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Atualiza um utilizador
        /// </summary>
        /// <param name="userId" example="1">ID do utilizador</param>
        /// <returns>Atualiza um utilizador</returns>
        [HttpPut("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(int userId,
           [FromBody] User updatedUser)
        {

            var existingUser = _userRepo.GetUser(userId);

            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.name = updatedUser.name;
            existingUser.email = updatedUser.email;
            existingUser.password = updatedUser.password;
            existingUser.phone = updatedUser.phone;
            existingUser.token = updatedUser.token;
            existingUser.status = updatedUser.status;


            bool updated = _userRepo.UpdateUser(existingUser);

            if (updated)
            {
                return Ok("Successfully updated");
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }
        }

        /// <summary>
        /// Apaga um utilizador
        /// </summary>
        /// <param name="userId" example="1">ID do utilizador</param>
        /// <returns>Exclui um utilizador</returns>
        [HttpDelete("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int userId)
        {
            if (!_userRepo.UserExists(userId))
            {
                return NotFound();
            }

            var userToDelete = _userRepo.GetUser(userId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepo.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return NoContent();
        }

        /// <summary>
        /// Altera estado do utilizador
        /// </summary>
        /// <param name="userId" example="1">ID do utilizador</param>
        /// <returns></returns>
        [HttpPut("{userId}/Deactivate")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult SoftDeleteUser(int userId)
        {

            var existingUser = _userRepo.GetUser(userId);

            if (existingUser == null)
            {
                return NotFound();
            }

            if (existingUser.status == false)
                existingUser.status = true;
            else
                existingUser.status = false;



            bool updated = _userRepo.UpdateUser(existingUser);

            if (updated)
            {
                return Ok("Estado do utilizador atualizado com sucesso!");
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }
        }
    }
}

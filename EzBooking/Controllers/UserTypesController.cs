using EzBooking.Models;
using EzBooking.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Text;

namespace EzBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTypesController : Controller
    {
        private readonly UserTypesRepo _userTypesRepo;

        public UserTypesController(UserTypesRepo userTypesRepo)
        {
            _userTypesRepo = userTypesRepo;
        }

        /// <summary>
        /// Obtém todos os tipos de utilizadores.
        /// </summary>
        /// <returns>Uma lista de Utilizadores.</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<UserTypes>> GetUserTypes()
        {
            var userTypes = _userTypesRepo.GetUserTypes();

            if (userTypes == null || userTypes.Count == 0)
            {
                return NotFound("Nenhum tipo de utilizador encontrado."); //404
            }

            return Ok(userTypes);
        }

        /// <summary>
        /// Obtém um tipo de utilizador
        /// </summary>
        /// <param name="userTypeId" example ="1">O ID do tipo de utilizador</param>
        /// <returns>Um utilizador</returns>
        [HttpGet("{userTypeId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<UserTypes> GetUserTypes(int userTypeId)
        {
            var userType = _userTypesRepo.GetUserType(userTypeId);

            if (userType == null)
            {
                return NotFound("User Type not found"); //404
            }

            return Ok(userType);
        }

        /// <summary>
        /// Cria um tipo de utilizador
        /// </summary>
        /// <param name="userTypeCreate"></param>
        /// <returns>Cria um utilizador</returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUserType([FromBody] UserTypes userTypeCreate)
        {
            if (userTypeCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var checkType = _userTypesRepo.CheckType(userTypeCreate.type);

            if (checkType == true)
            {
                ModelState.AddModelError("", "Type already exists");
                return StatusCode(422, ModelState);
            }


            bool created = _userTypesRepo.CreateUserTypes(userTypeCreate);

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
        /// Altera dados do tipo de utilizador
        /// </summary>
        /// <param name="userTypeId" example ="1">ID do tipo de utilizador</param>
        /// <param name="updatedUserType"></param>
        /// <returns></returns>
        [HttpPut("{userTypeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUserType(int userTypeId,
           [FromBody] UserTypes updatedUserType)
        {

            var existingUserType = _userTypesRepo.GetUserType(userTypeId);

            if (existingUserType == null)
            {
                return NotFound();
            }

            var checkType = _userTypesRepo.CheckType(updatedUserType.type);
            if (checkType == true)
            {
                ModelState.AddModelError("", "Type already exists");
                return StatusCode(422, ModelState);
            }

            existingUserType.type = updatedUserType.type;


            bool updated = _userTypesRepo.UpdateUserType(existingUserType);

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
        /// Apaga um tipo de utilizador
        /// </summary>
        /// <param name="userTypeId" example="1">ID do tipo de utilizador</param>
        /// <returns>Exclui um tipo de utilizador</returns>
        [HttpDelete("{userTypeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUserType(int userTypeId)
        {
            if (!_userTypesRepo.UserTypeExists(userTypeId))
            {
                return NotFound();
            }

            var userTypeToDelete = _userTypesRepo.GetUserType(userTypeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userTypesRepo.DeleteUserType(userTypeToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return NoContent();
        }
    }
}

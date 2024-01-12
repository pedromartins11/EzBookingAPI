using EzBooking.Models;
using EzBooking.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EzBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusHouseController : Controller
    {
        private readonly StatusHouseRepo _statusHouseRepo;

        public StatusHouseController(StatusHouseRepo statusHouseRepo)
        {
            _statusHouseRepo = statusHouseRepo;
        }

        //GETS
        /// <summary>
        /// Obtém todos os Estados Das Casas.
        /// </summary>
        /// <returns>Uma lista de Estados de Casas.</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<StatusHouse>> GetStatusHouses()
        {
            var statushouses = _statusHouseRepo.GetStatusHouses();

            if (statushouses == null)
            {
                return NotFound("Nenhum Estado para Casa encontrado."); //404
            }

            return Ok(statushouses);
        }

        /// <summary>
        /// Obtém um unico Estado ID.
        /// </summary>
        /// <param name="id" example ="1">O ID do estado do alojamento</param>
        /// <returns>Um Estado de alojamento.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<StatusHouse> GetStatusHouseCodeById(int id)
        {
            var statushouses = _statusHouseRepo.GetStatusHouseById(id);

            if (statushouses == null)
            {
                return NotFound("Estado da Casa não encontrada."); 
            }

            if (id <= 0)
            {
                return BadRequest("ID inválido."); // Código 400 se o ID for inválido.
            }

            return Ok(statushouses);
        }

        //CREATES
        /// <summary>
        /// Cria um estado de Casa.
        /// </summary>
        /// <returns>Um Estado de Alojamento.</returns>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(409)]
        public IActionResult CreateStatusHouse([FromBody] StatusHouse statusHouse)
        {
            if (statusHouse == null)
            {
                return BadRequest("Dados inválidos");
            }

            if (_statusHouseRepo.StatusHouseExists(statusHouse.name))
                return StatusCode(409, "O Estado da House Já Existe");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _statusHouseRepo.CreateStatusHouse(statusHouse);

            return CreatedAtAction("CreateStatusHouse", new { id = statusHouse.id }, statusHouse);
        }

    }
}

using EzBooking.Models;
using EzBooking.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EzBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationStatesController : Controller
    {
        private readonly ReservationStatesRepo _reservationStatesRepo;

        public ReservationStatesController(ReservationStatesRepo reservationStatesRepo)
        {
            _reservationStatesRepo = reservationStatesRepo;
        }

        //GETS

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<ReservationStates>> GetReservationStates()
        {
            var reservationstates = _reservationStatesRepo.GetReservationStates();

            if (reservationstates == null)
            {
                return NotFound("Nenhum Estado para Reserva encontrado."); //404
            }

            return Ok(reservationstates);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<ReservationStates> GetReservationStatesCodeById(int id)
        {
            var reservationstates = _reservationStatesRepo.GetReservationStatesById(id);

            if (reservationstates == null)
            {
                return NotFound("Estado da Reserva não encontrado.");
            }

            if (id <= 0)
            {
                return BadRequest("ID inválido."); // Código 400 se o ID for inválido.
            }

            return Ok(reservationstates);
        }

        //CREATES
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(409)]
        public IActionResult CreateReservationStates([FromBody] ReservationStates reservationStates)
        {
            if (reservationStates == null)
            {
                return BadRequest("Dados inválidos");
            }

            if (_reservationStatesRepo.ReservationStatesExists(reservationStates.state))
                return StatusCode(409, "O Estado da Reserva Já Existe");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _reservationStatesRepo.CreateReservationStates(reservationStates);

            return CreatedAtAction("CreateReservationStates", new { id = reservationStates.id }, reservationStates);
        }

    }
}

using EzBooking.Models;
using EzBooking.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace EzBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : Controller
    {
        private readonly ReservationRepo _reservationRepo;
        private readonly ReservationStatesRepo _reservationStatesRepo;
        private readonly HouseRepo _houseRepo;
        private readonly UserRepo _userRepo;
        private readonly PaymentRepo _paymentRepo;
        private readonly PaymentStateRepo _paymentStateRepo;

        //private readonly FeedbackRepo _feedbackRepo;

        public ReservationController(ReservationRepo reservationRepo, ReservationStatesRepo reservationStatesRepo, HouseRepo houseRepo, UserRepo userRepo, PaymentRepo paymentRepo, PaymentStateRepo paymentStateRepo)
        {
            _reservationRepo = reservationRepo;
            _reservationStatesRepo = reservationStatesRepo;
            _houseRepo = houseRepo;
            _userRepo = userRepo;
            _paymentRepo = paymentRepo;
            _paymentStateRepo = paymentStateRepo;

            //_feedbackRepo = feedbackRepo;
            //_paymentRepo = paymentRepo;
        }

        //GETS

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<House>>> GetReservations()
        {
            var reservations = await _reservationRepo.GetReservations(); // Use um método que já inclui as propriedades associadas

            if (reservations == null || reservations.Count == 0)
            {
                return NotFound("Nenhuma reserva encontrada."); //404
            }

            return Ok(reservations);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<House>> GetReservationById(int id)
        {
            var reservation = await _reservationRepo.GetReservationById(id);

            if (reservation == null)
            {
                return NotFound("Reserva não encontrada."); // Código 404 se a casa não for encontrada.
            }

            if (id <= 0)
            {
                return BadRequest("ID inválido."); // Código 400 se o ID for inválido.
            }

            return Ok(reservation);
        }

        //CREATES
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> CreateReservation([FromBody] Reservation reservation,int houseId, int userId)
        {
            if (reservation == null)
            {
                return BadRequest("Dados inválidos");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User existingUser = _userRepo.GetUser(userId);

            if (existingUser == null)
            {
                return BadRequest("User não existe");
            }

            House existingHouse = await _houseRepo.GetHouseById(houseId);

            if (existingHouse == null)
            {
                return BadRequest("Casa não existe");
            }


            // Valida Numero de Hospedes
            if (reservation.guestsNumber > existingHouse.guestsNumber)
            {
                return BadRequest("Numero de Hospedes da reserva excede o numero de Hospedes do Alojamento");
            }


            // Verificar DATAS
            if (reservation.init_date < DateTime.Now || reservation.end_date < DateTime.Now)
            {
                return BadRequest("Datas Invalidas!");
            }

            if (reservation.init_date > reservation.end_date)
            {
                return BadRequest("Datas Invalidas!");
            }


            reservation.House = existingHouse;
            reservation.User = existingUser;


            // ValidateReservationDate
            bool resDateCheck = await _reservationRepo.ValidateReservationDate(reservation);

            if (resDateCheck == false)
            {
                return BadRequest("Datas de Reserva Invalidas!");
            }

            // CalculateTotalPrice
            double? valor = await _reservationRepo.CalculateTotalPrice(reservation);


            ReservationStates status = _reservationStatesRepo.GetReservationStatesById(2);

            if (status == null)
            {
                return BadRequest("Estado não existe");
            }

            reservation.ReservationStates = status;


            // Valida SharedRoom
            if (existingHouse.sharedRoom == true)
            {
                if (existingHouse.rooms > 0)
                {
                    existingHouse.rooms = existingHouse.rooms - reservation.guestsNumber;
                }
                else
                {
                    return BadRequest("Número de Quartos Insuficiente!");
                }
            }

            // Criar Reserva
            await _reservationRepo.CreateReservation(reservation);


            // Payment Create
            Payment newPayment = new Payment
            {
                    Reservation = reservation,
                    creationDate = DateTime.Now,
                    paymentDate = null,
                    paymentMethod = null,
                    paymentValue = (float)valor,
                    state = _paymentStateRepo.GetPaymentState(1),
            };
            _paymentRepo.CreatePayment(newPayment);
            


            return CreatedAtAction("CreateReservation", new { id = reservation.id_reservation }, reservation);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateReservation(int id, [FromBody] Reservation reservation)
        {
            if (reservation == null)
                return BadRequest(ModelState);

            if (!_reservationRepo.ReservationExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            ReservationStates status = _reservationStatesRepo.GetReservationStatesById(2);
            var nreservation = await _reservationRepo.GetReservationById(id);
            nreservation.init_date = reservation.init_date;
            nreservation.end_date = reservation.end_date;
            nreservation.guestsNumber = reservation.guestsNumber;
            nreservation.ReservationStates = status;

            _reservationRepo.UpdateReservation(nreservation);

            return Ok();
        }

        //DELETE
        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            if (!_reservationRepo.ReservationExists(id))
            {
                return NotFound();
            }

            var reservationToDelete = await _reservationRepo.GetReservationById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reservationRepo.DeleteReservation(reservationToDelete))
            {
                ModelState.AddModelError("", "Erro ao eliminar a Reserva");
            }

            var responseMessage = $"A reserva com o ID {id} foi eliminada com sucesso.";
            return Ok(responseMessage);
        }

    }
}

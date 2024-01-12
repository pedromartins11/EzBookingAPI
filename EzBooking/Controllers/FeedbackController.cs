using EzBooking.Models;
using EzBooking.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : Controller
    {
        private readonly FeedbackRepo _feedbackRepo;
        private readonly ReservationRepo _reservationRepo;

        public FeedbackController(FeedbackRepo feedbackRepo, ReservationRepo reservationRepo)
        {
            _feedbackRepo = feedbackRepo;
            _reservationRepo = reservationRepo;
        }

        /// <summary>
        /// Obtém todos os feedbacks.
        /// </summary>
        /// <returns>Uma lista de feedbacks.</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<Feedback>> GetFeedbacks()
        {
            var feedbacks = _feedbackRepo.GetFeedbacks();

            if (feedbacks == null || feedbacks.Count == 0)
            {
                return NotFound("Nenhum feedback encontrado."); //404
            }

            return Ok(feedbacks);
        }

        /// <summary>
        /// Cria um feedback
        /// </summary>
        /// <param name="feedbackCreate"></param>
        /// <returns>Cria um feedback</returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateFeedback([FromBody] Feedback feedbackCreate, int reservationId)
        {
            if (feedbackCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Reservation getReservation = await _reservationRepo.GetReservationById(reservationId);

            if (getReservation == null)
            {
                return NotFound("A reserva não existe.");
            }

            feedbackCreate.Reservation = getReservation;

            bool created = _feedbackRepo.CreateFeedback(feedbackCreate);

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
        /// Obtém um feedback
        /// </summary>
        /// <param name="feedbackId" example ="1">O ID do feedback</param>
        /// <returns>Um feedbaack</returns>
        [HttpGet("{feedbackId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<Feedback> GetFeedback(int feedbackId)
        {
            var feedback = _feedbackRepo.GetFeedback(feedbackId);

            if (feedback == null)
            {
                return NotFound("Feedback not found"); //404
            }

            return Ok(feedback);
        }

        /// <summary>
        /// Atualiza um feedback
        /// </summary>
        /// <param name="feedbackId" example="1">ID do feedback</param>
        /// <returns>Atualiza um feedback</returns>
        [HttpPut("{feedbackId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateFeedback(int feedbackId,
           [FromBody] Feedback updatedFeedback)
        {

            var existingFeedback = _feedbackRepo.GetFeedback(feedbackId);

            if (existingFeedback == null)
            {
                return NotFound();
            }

            existingFeedback.classification = updatedFeedback.classification;
            existingFeedback.comment = updatedFeedback.comment;

            bool updated = _feedbackRepo.UpdateFeedback(existingFeedback);

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
        /// Apaga um feedback
        /// </summary>
        /// <param name="feedbackId" example="1">ID do feedback</param>
        /// <returns>Exclui um feedback</returns>
        [HttpDelete("{feedbackId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteFeedback(int feedbackId)
        {
            var feedbackToDelete = _feedbackRepo.GetFeedback(feedbackId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_feedbackRepo.DeleteFeedback(feedbackToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return NoContent();
        }

    }
}

using EzBooking.Models;
using EzBooking.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentStatesController : Controller
    {
        private readonly PaymentStateRepo _paymentStateRepo;

        public PaymentStatesController(PaymentStateRepo paymentStateRepo)
        {
            _paymentStateRepo = paymentStateRepo;
        }

        /// <summary>
        /// Obtém todos os estados de pagamento.
        /// </summary>
        /// <returns>Uma lista de estados de pagamento.</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<PaymentStates>> GetPaymentStates()
        {
            var paymentStates = _paymentStateRepo.GetPaymentStates();

            if (paymentStates == null || paymentStates.Count == 0)
            {
                return NotFound("Nenhum estado de pagamento encontrado."); //404
            }

            return Ok(paymentStates);
        }


        /// <summary>
        /// Obtém um estado de pagamento
        /// </summary>
        /// <param name="paymentStateId" example ="1">O ID do estado de pagamento</param>
        /// <returns>Um estado de pagamento</returns>
        [HttpGet("{paymentStateId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<Payment> GetPaymentState(int paymentStateId)
        {
            var paymentState = _paymentStateRepo.GetPaymentState(paymentStateId);

            if (paymentState == null)
            {
                return NotFound("Payment not found"); //404
            }

            return Ok(paymentState);
        }

        /// <summary>
        /// Cria um estado de pagamento
        /// </summary>
        /// <param name="paymentStateCreate"></param>
        /// <returns>Cria um estado de pagamento</returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePaymentStates([FromBody] PaymentStates paymentStateCreate)
        {
            if (paymentStateCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var checkState = _paymentStateRepo.CheckState(paymentStateCreate.state);
            if (checkState == true)
            {
                ModelState.AddModelError("", "State already exists");
                return StatusCode(422, ModelState);
            }

            bool created = _paymentStateRepo.CreatePaymentStates(paymentStateCreate);

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
        /// Atualiza um estado de pagamento
        /// </summary>
        /// <param name="paymentStateId" example="1">ID do estado de pagamento</param>
        /// <returns>Atualiza um estado de pagamento</returns>
        [HttpPut("{paymentStateId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePaymentState(int paymentStateId,
           [FromBody] PaymentStates updatedPaymentState)
        {

            var existingPaymentState = _paymentStateRepo.GetPaymentState(paymentStateId);

            if (existingPaymentState == null)
            {
                return NotFound();
            }

            var checkState = _paymentStateRepo.CheckState(updatedPaymentState.state);
            if (checkState == true)
            {
                ModelState.AddModelError("", "State already exists");
                return StatusCode(422, ModelState);
            }

            existingPaymentState.state = updatedPaymentState.state;

            bool updated = _paymentStateRepo.UpdatePaymentState(existingPaymentState);

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
        /// Apaga um estado de pagamento
        /// </summary>
        /// <param name="paymentStatesId" example="1">ID do estado de pagamento</param>
        /// <returns>Exclui um estado de pagamento</returns>
        [HttpDelete("{paymentStatesId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePaymentStates(int paymentStatesId)
        {
            var paymentStatesToDelete = _paymentStateRepo.GetPaymentState(paymentStatesId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_paymentStateRepo.DeletePaymentStates(paymentStatesToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return NoContent();
        }

    }
}

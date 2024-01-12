using EzBooking.Models;
using EzBooking.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EzBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostalCodeController : Controller
    {
        private readonly PostalCodeRepo _postalCodeRepo;

        public PostalCodeController(PostalCodeRepo postalCodeRepo)
        {
            _postalCodeRepo = postalCodeRepo;
        }

        //GETS

        /// <summary>
        /// Obtém todos os Codigos Postais.
        /// </summary>
        /// <returns>Uma lista de Codigos Postais.</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<PostalCode>> GetPostalCodes()
        {
            var postalcodes = _postalCodeRepo.GetPostalCodes();

            if (postalcodes == null || postalcodes.Count == 0)
            {
                return NotFound("Nenhum Codigo Postal encontrado."); //404
            }

            return Ok(postalcodes);
        }

        /// <summary>
        /// Obtém um Codigo Postal.
        /// </summary>
        /// <param name="id" example ="4750">O ID do Codigo Postal</param>
        /// <returns>Um Codigo Postal.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<PostalCode> GetPostalCodeById(int id)
        {
            var postalcode = _postalCodeRepo.GetPostalCodeById(id);

            if (postalcode == null)
            {
                return NotFound("Codigo Postal não encontrada."); // Código 404 se a casa não for encontrada.
            }

            if (id <= 0)
            {
                return BadRequest("ID inválido."); // Código 400 se o ID for inválido.
            }

            return Ok(postalcode);
        }

        //CREATES
        /// <summary>
        /// Cria uma Codigo Postal.
        /// </summary>
        /// <returns>Um Codigo Postal.</returns>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(409)]
        public IActionResult CreatePostalCode([FromBody] PostalCode postalCode)
        {
            if (postalCode == null)
            {
                return BadRequest("Dados inválidos");
            }

            if (_postalCodeRepo.PostalCodeExists(postalCode.postalCode))
                return StatusCode(409, "O Codigo Postal Já Existe");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _postalCodeRepo.CreatePostalCode(postalCode);

            return CreatedAtAction("CreatePostalCode", new { id = postalCode.postalCode }, postalCode);
        }

    }
}

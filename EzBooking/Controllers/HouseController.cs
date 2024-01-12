using EzBooking.Models;
using EzBooking.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace EzBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseController : Controller
    {
        private readonly HouseRepo _houseRepo;
        private readonly StatusHouseRepo _statusHouseRepo;
        private readonly PostalCodeRepo _postalCodeRepo;
        private readonly ImageRepo _imageRepo;
        private readonly UserRepo _userRepo;

        public HouseController(HouseRepo houseRepo, StatusHouseRepo statusHouseRepo, PostalCodeRepo postalCodeRepo, ImageRepo imageRepo, UserRepo userRepo)
        {
            _houseRepo = houseRepo;
            _statusHouseRepo = statusHouseRepo;
            _postalCodeRepo = postalCodeRepo;
            _imageRepo = imageRepo;
            _userRepo = userRepo;
        }

        //GETS

        /// <summary>
        /// Obtém Casas com Filtro.
        /// </summary>
        /// <param name="location" example ="Porto">A localização da Casa (Distrito)</param>
        /// <param name="guestsNumber" example ="3">O Nº de Pessoas</param>
        /// <param name="startDate" example ="2023-12-09">Data Check-in</param>
        /// <param name="endDate" example ="2023-12-10">Data Check-out</param>
        /// <returns>Uma lista de Casas Filtradas.</returns>
        /// <response code="200">Retorna uma lista de Casas com Filtro.</response>
        [HttpGet("Filtered")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<House>>> GetHousesFiltered(string location, int guestsNumber, DateTime startDate, DateTime endDate)
        {
            var houses = await _houseRepo.AvailableHouses(location, guestsNumber, startDate, endDate);

            if (houses == null || houses.Count == 0)
            {
                return NotFound("Nenhuma casa encontrada."); //404
            }

            return Ok(houses);
        }

        /// <summary>
        /// Obtém todas as Casas Ativas.
        /// </summary>
        /// <returns>Uma lista de Casas.</returns>
        /// <response code="200">Retorna uma lista de Casas.</response>
        [HttpGet]
        //[Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<House>>> GetHouses()
        {
            var houses = await _houseRepo.GetHouses();

            if (houses == null || houses.Count == 0)
            {
                return NotFound("Nenhuma casa encontrada."); //404
            }

            return Ok(houses);
        }

        /// <summary>
        /// Obtém Uma Casa
        /// </summary>
        /// <param name="id" example ="1">O ID da Casa</param>
        /// <returns>Uma Casa.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<House>> GetHouseById(int id)
        {
            var house = await _houseRepo.GetHouseByIdDetail(id);

            if (house == null)
            {
                return NotFound("Casa não encontrada."); // Código 404 se a casa não for encontrada.
            }

            if (id <= 0)
            {
                return BadRequest("ID inválido."); // Código 400 se o ID for inválido.
            }

            return Ok(house);
        }

        /// <summary>
        /// Obtém Casas Suspensas
        /// </summary>
        /// <returns>Uma Lista de Casas Suspensas.</returns>
        [HttpGet("susp")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<House>> GetHousesSusp()
        {
            var houses = _houseRepo.GetHousesSusp();

            if (houses == null || houses.Count == 0)
            {
                return NotFound("Nenhuma casa encontrada."); //404
            }

            return Ok(houses);
        }

        /// <summary>
        /// Obtém Reservas de uma Casa
        /// </summary>
        /// <param name="id" example ="1">O ID da Casa</param>
        /// <returns>Uma Casa.</returns>
        [HttpGet("Reservations/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<House>> GetReservationsFromHouseById(int id)
        {
            var house = await _houseRepo.GetReservationFromHouseById(id);

            if (house == null)
            {
                return NotFound("Casa não encontrada."); // Código 404 se a casa não for encontrada.
            }

            if (id <= 0)
            {
                return BadRequest("ID inválido."); // Código 400 se o ID for inválido.
            }

            return Ok(house);
        }

        //CREATES
        /// <summary>
        /// Cria uma Casa
        /// </summary>
        /// <returns>Cria uma Casa.</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(201)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> CreateHouse([FromBody] House house)
        {
            if (house == null)
            {
                return BadRequest("Dados inválidos");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (house.price != null && house.priceyear != null)
                return BadRequest("A casa so pode ter preço mensal ou anual");

            PostalCode existingPostalCode = _postalCodeRepo.GetPostalCodeById(house.PostalCode.postalCode);

            if (existingPostalCode != null && _houseRepo.PostalCodePropertyExists(existingPostalCode.postalCode, house.propertyAssessment))
                return StatusCode(409, "Já Existe uma casa com esse artigo matricial nesse codigo postal");

            if (existingPostalCode == null)
            {
                existingPostalCode = new PostalCode
                {
                    postalCode = house.PostalCode.postalCode,
                    concelho = house.PostalCode.concelho,
                    district = house.PostalCode.district,
                };
                _postalCodeRepo.CreatePostalCode(existingPostalCode);
            }

            house.PostalCode = existingPostalCode;

            StatusHouse status = _statusHouseRepo.GetStatusHouseById(1);
            house.StatusHouse = status;

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            User userLogged = _userRepo.GetUser(int.Parse(userId));
            house.User = userLogged;
            

            await _houseRepo.CreateHouse(house);

            return CreatedAtAction("CreateHouse", new { id = house.id_house }, house);
        }

        /// <summary>
        /// Altera Estado da casa para Aprovado.
        /// </summary>
        /// <param name="id" example ="1">O ID da Casa</param>
        /// <returns>Uma Casa com estado atualizado.</returns>
        [HttpPut("state/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateStateHouse(int id)
        {

            if (!_houseRepo.HouseExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            StatusHouse status = _statusHouseRepo.GetStatusHouseById(2);
            var rhouse = await _houseRepo.GetHouseById(id);
            rhouse.StatusHouse = status;
            _houseRepo.UpdateHouse(rhouse);

            return Ok(rhouse);
        }

        /// <summary>
        /// Altera Estado da casa para Apagada.
        /// </summary>
        /// <param name="id" example ="1">O ID da Casa</param>
        /// <returns>Uma Casa com estado Apagado.</returns>
        [HttpPut("stateDelete/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateStateDelHouse(int id)
        {

            if (!_houseRepo.HouseExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            StatusHouse status = _statusHouseRepo.GetStatusHouseById(3);
            var rhouse = await _houseRepo.GetHouseById(id);
            rhouse.StatusHouse = status;
            bool tf = _houseRepo.UpdateHouse(rhouse);
            if(!tf) return NotFound();
            return Ok(rhouse);
        }


        /// <summary>
        /// Altera Dados de uma casa.
        /// </summary>
        /// <param name="id" example ="1">O ID da Casa</param>
        /// <returns>Uma lista de Casas.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateHouse(int id, [FromBody] House house)
        {
            if (house == null)
                return BadRequest(ModelState);

            if (!_houseRepo.HouseExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            PostalCode existingPostalCode = _postalCodeRepo.GetPostalCodeById(house.PostalCode.postalCode);

            if (existingPostalCode != null && _houseRepo.PostalCodePropertyExists(existingPostalCode.postalCode, house.propertyAssessment))
                return StatusCode(409, "Já Existe uma casa com esse artigo matricial nesse codigo postal");

            if (existingPostalCode == null)
            {
                existingPostalCode = new PostalCode
                {
                    postalCode = house.PostalCode.postalCode,
                    concelho = house.PostalCode.concelho,
                    district = house.PostalCode.district,
                };
                _postalCodeRepo.CreatePostalCode(existingPostalCode);
            }

            StatusHouse status = _statusHouseRepo.GetStatusHouseById(1);
            var rhouse = await _houseRepo.GetHouseById(id);
            rhouse.name = house.name;
            rhouse.price = house.price;
            rhouse.priceyear = house.priceyear;
            rhouse.codDoor = house.codDoor;
            rhouse.floorNumber = house.floorNumber;
            rhouse.doorNumber = house.doorNumber;
            rhouse.guestsNumber = house.guestsNumber;
            rhouse.rooms = house.rooms;
            rhouse.propertyAssessment = house.propertyAssessment;
            rhouse.road = house.road;
            rhouse.sharedRoom = house.sharedRoom;
            rhouse.StatusHouse = status;
            rhouse.PostalCode = existingPostalCode;

            _houseRepo.UpdateHouse(rhouse);

            return Ok();
        }

        //DELETE
        /// <summary>
        /// Apaga uma Casa
        /// </summary>
        /// <param name="id" example ="1">O ID da Casa</param>
        /// <returns>Exclui uma Casa.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteHouse(int id)
        {
            if (!_houseRepo.HouseExists(id))
            {
                return NotFound();
            }

            var houseToDelete = await _houseRepo.GetHouseById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_houseRepo.DeleteHouse(houseToDelete))
            {
                ModelState.AddModelError("", "Erro ao eliminar a Casa");
            }

            var responseMessage = $"A casa com o ID {id} foi eliminada com sucesso.";
            return Ok(responseMessage);
        }


    }
}

using EzBooking.Dtto;
using EzBooking.Models;
using EzBooking.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EzBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly ImageRepo _imageRepo;
        private readonly HouseRepo _houseRepo;

        public ImageController(HouseRepo houseRepo, ImageRepo imageRepo)
        {
            _houseRepo = houseRepo;
            _imageRepo = imageRepo;
        }



        //CREATES
        /// <summary>
        /// Acopla Imagem a Casa
        /// </summary>
        /// <param name="id_house" example ="1">O ID da Casa</param>
        /// <returns>Criou a imagem com caminho no servidor.</returns>
        /// <response code="200">Retorna imagem.</response>
        [HttpPost("{id_house}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateImage(IFormFile[] imageFiles,int id_house)
        {
            int imagesProcessed = 0;

            foreach (var imageFile in imageFiles)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uniqueFileName = $"{id_house}_{imagesProcessed}_{Path.GetFileName(imageFile.FileName)}"; 

                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Houses", uniqueFileName);

                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }


                    House existhouse = await _houseRepo.GetHouseById(id_house);

                    var newimage = new Images
                    {
                        image = uniqueFileName,
                        House = existhouse,
                    };
                    await _imageRepo.CreateImage(newimage);
                    imagesProcessed++;

                }
            }
            if (imagesProcessed == imageFiles.Length)
            {
                // Se todas as imagens foram processadas, retorna o código 200 (OK)
                return Ok();
            }

            return BadRequest("Inválido.");
        }
    }
}

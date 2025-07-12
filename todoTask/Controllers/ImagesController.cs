using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using todoTask.Models.Domain;
using todoTask.Models.DTO;

namespace todoTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        // Post: api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Uplaod([FromForm] ImageUploadReaquestDto requestDto)
        {
            ValidateFIleUpload(requestDto);
            if (ModelState.IsValid)
            {
                // Convert DTO to Domain model
                var imageDomainModel = new Image 
                { 
                    File = requestDto.File,
                    FileExtension = Path.GetExtension(requestDto.File.FileName),
                    FileSize = requestDto.File.Length,
                    FileName = requestDto.FileName,
                    FileDescription = requestDto.FileDescription,
                };

                // User repsoitory to upload Image




            }
            return BadRequest(ModelState);
        }

        private void ValidateFIleUpload(ImageUploadReaquestDto reaquestDto)
        {
            var allowedExtension = new string[] { ".jpg", ".png", ".jpeg" };
            // Here File is Ifromform file Filename is the propert
            if (!allowedExtension.Contains(Path.GetExtension(reaquestDto.File.FileName)))
            {
                ModelState.AddModelError("file","Unsupported File Extension");
            }

            // Validate the uploaded file Size
            if (reaquestDto.File.Length>10485760)
            {
                ModelState.AddModelError("file","File Size more than 10MB, Please Upload a smaller size File.");
            }

        }
    }
}

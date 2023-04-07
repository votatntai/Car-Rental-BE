using Data.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FileStoragesController : ControllerBase
    {

        private readonly ICloudStorageService _fileStorageService;
        public FileStoragesController(ICloudStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        [HttpPost]
        public async Task<ActionResult<FileViewModel>> UploadFiles(ICollection<IFormFile> files)
        {
            var fileVMs = new List<FileViewModel>();
            try
            {
                foreach (var file in files)
                {
                    var id = Guid.NewGuid();
                    var result = await _fileStorageService.Upload(id, file.ContentType, file.OpenReadStream());
                    if (result != null)
                    {
                        fileVMs.Add(new FileViewModel
                        {
                            ContentType = file.ContentType,
                            Name = file.FileName,
                            Url = result
                        });
                    }
                }
                return Ok(fileVMs);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}

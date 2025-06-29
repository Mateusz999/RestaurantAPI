using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.ComponentModel.DataAnnotations;

namespace RestaurationAPI.Controllers
{
    [ApiController]
    [Route("file")]
    public class FileController : ControllerBase
    {
        /// <summary>
        /// Pobiera plik na podstawie jego nazwy.
        /// </summary>
        [HttpGet]
        [ResponseCache(Duration = 1200, VaryByQueryKeys = new[] { "fileName" })]
        public ActionResult GetFile([FromQuery] string fileName)
        {
            var rootPath = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(rootPath, "PrivateFiles", fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("Plik nie istnieje.");

            var fileContent = System.IO.File.ReadAllBytes(filePath);

            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(fileName, out string? contentType);
            contentType ??= "application/octet-stream";

            return File(fileContent, contentType, fileName);
        }

        /// <summary>
        /// Wysyła plik do serwera.
        /// </summary>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Upload([FromForm] FileUploadDto dto)
        {
            var file = dto.File;

            if (file == null || file.Length == 0)
                return BadRequest("Nie przesłano żadnego pliku.");

            var rootPath = Directory.GetCurrentDirectory();
            var uploadDir = Path.Combine(rootPath, "PrivateFiles");
            Directory.CreateDirectory(uploadDir);

            var filePath = Path.Combine(uploadDir, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Ok("Plik został zapisany.");
        }
    }

    public class FileUploadDto
    {
        [Required]
        public IFormFile File { get; set; } = default!;
    }
}

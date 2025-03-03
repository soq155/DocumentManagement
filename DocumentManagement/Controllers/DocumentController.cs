using DocumentManagement.Models;
using DocumentManagement.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Constructor for DocumentController that helps use IDocumentRepository and IWebHostEnvironment methods in our controller
        public DocumentController(IDocumentRepository documentRepository, IWebHostEnvironment webHostEnvironment)
        {
            _documentRepository = documentRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        // Get All Documents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Document>>> GetAllDocuments()
        {
            var documents = await _documentRepository.GetAllAsync();
            return Ok(documents);
        }

        // Uploads a File
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            string uploadPath = Path.Combine(_webHostEnvironment.ContentRootPath, "UploadedFiles");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            string filePath = Path.Combine(uploadPath, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var document = new Document
            {
                FileName = file.FileName,
                FilePath = filePath,
                FileType = Path.GetExtension(file.FileName).ToLower(),
                UploadDate = DateTime.UtcNow
            };

            await _documentRepository.AddDocumentAsync(document);

            return Ok(new { message = "Upload successful", document });
        }

        // Get a File by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile(int id)
        {
            var document = await _documentRepository.GetByIdAsync(id);
            if (document == null)
                return NotFound();

            var fileBytes = await System.IO.File.ReadAllBytesAsync(document.FilePath);
            return File(fileBytes, "application/octet-stream", document.FileName);
        }

        // Delete a File
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            await _documentRepository.DeleteDocumentAsync(id);
            return NoContent();
        }


        // Looks at a File
        [HttpGet("preview/{id}")]
        public async Task<IActionResult> PreviewFile(int id)
        {
            var document = await _documentRepository.GetByIdAsync(id);
            if (document == null || !System.IO.File.Exists(document.FilePath))
                return NotFound();

            var fileBytes = await System.IO.File.ReadAllBytesAsync(document.FilePath);
            string base64String = Convert.ToBase64String(fileBytes);

            return Ok(new
            {
                document.FileName,
                document.FileType,
                document.UploadDate,
                FileContentBase64 = base64String // Base64 encoded file for preview
            });
        }


    }
}

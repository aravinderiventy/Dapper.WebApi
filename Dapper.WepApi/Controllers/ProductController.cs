using Dapper.Application.Repositories;
using Dapper.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Dapper.WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;
        public ProductController(IUnitOfWork unitOfWork, ILogger<ProductController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;   
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                //Using Serilog Extension the logs will be stored in the file 
                logger.LogInformation("Get All api is called");
                var data = await unitOfWork.ProductRepository.GetAllAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong: {ex}");
                return StatusCode(500, "Internal error");
            }
            
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await unitOfWork.ProductRepository.GetByIdAsync(id);
            if (data == null) return Ok();
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            var data = await unitOfWork.ProductRepository.AddAsync(product);
            return Ok(data);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await unitOfWork.ProductRepository.DeleteAsync(id);
            return Ok(data);
        }
        [HttpPut]
        public async Task<IActionResult> Update(Product product)
        {
            var data = await unitOfWork.ProductRepository.UpdateAsync(product);
            return Ok(data);
        }

        [HttpGet]
        [Route("download")]
        public async Task<IActionResult> Download([FromQuery] string fileUrl)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Resources", "Images", fileUrl);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var memory = new MemoryStream();
            await using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), filePath);
        }

        [HttpGet]
        [Route("getFiles")]
        public async Task<IActionResult> GetFiles()
        {
            try
            {
                var folderName = Path.Combine("Resources", "Images");
                var pathToRead=Path.Combine(Directory.GetCurrentDirectory(),folderName);
                var files=Directory.EnumerateFiles(pathToRead)
                    .Where(IsValidFile)
                    .Select(fullPath => Path.Combine(folderName,Path.GetFileName(fullPath)));

                return Ok(new { files });
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string GetContentType(string path)
        {
            var provider =new  FileExtensionContentTypeProvider();
            string contentType;

            if(!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }

        private bool IsValidFile(string fileName)
        {
            return fileName.EndsWith(".txt",StringComparison.OrdinalIgnoreCase)
                || fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase);
        }
    }
}

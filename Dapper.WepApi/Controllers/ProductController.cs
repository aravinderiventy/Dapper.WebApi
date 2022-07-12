using Dapper.Application.Repositories;
using Dapper.Core;
using Microsoft.AspNetCore.Mvc;


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
    }
}

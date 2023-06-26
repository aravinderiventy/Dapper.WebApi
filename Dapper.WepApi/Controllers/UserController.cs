using AutoMapper;
using Dapper.Application.Repositories;
using Dapper.Core.Entities;
using Dapper.Infrastructure.Repositories;
using Dapper.WepApi.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dapper.WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork unitOfWork, ILogger<UserController> logger, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("api/getUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Get all users api called.");
                var data = await _unitOfWork.UserRepository.GetAllAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(500, "Internal error");
            }
        }

        [HttpPost]
        [Route("api/addUser")]
        public async Task<IActionResult> AddAsync(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            var data= await _unitOfWork.UserRepository.AddAsync(user);
            return Ok(data);
        }

        [HttpPut]
        [Route("api/updateUser")]
        public async Task<IActionResult> UpdateUser(UserDto userDto)
        {
            var user= _mapper.Map<User>(userDto);
            var result=await _unitOfWork.UserRepository.UpdateAsync(user);
            return Ok(result);
        }

        [HttpGet]
        [Route("api/getUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user=await _unitOfWork.UserRepository.GetByIdAsync(id);
            var userDto=_mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpDelete]
        [Route("api/deleteUser")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _unitOfWork.UserRepository.DeleteAsync(id);
            
            return Ok(Convert.ToBoolean(user));
        }
    }
}

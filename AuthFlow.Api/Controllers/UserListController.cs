using AuthFlow.Application.DTOs;
using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Domain.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserListController : ControllerBase
    {
        // Defines an interface for accessing User data in the repository.
        private readonly IUserRepository _usersRepository;
        private readonly IMapper _mapper;

        // Constructor for UserController, injecting the User repository.
        public UserListController(IUserRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        [Authorize]
        // Gets all Users. Endpoint: GET api/User/GetUsersAll bt filter
        [HttpGet("[action]/{pageNumber}/{pageSize}/{filter}")]
        public async Task<IActionResult> GetPageByFilter(int pageNumber, int pageSize, string filter)
        {
            var result = await _usersRepository.GetPageByFilter(pageNumber, pageSize, filter);
            var resultDTO = _mapper.Map<List<User>>(result.Data.ToList());
            return Ok(OperationResult<List<User>>.Success(resultDTO, result.Message));
        }

        [Authorize]
        // Gets all Users. Endpoint: GET api/User/GetUsersAll
        [HttpGet("[action]/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetPage(int pageNumber, int pageSize)
        {
            var result = await _usersRepository.GetPageByFilter(pageNumber, pageSize, string.Empty);
            var resultDTO = _mapper.Map<List<User>>(result.Data.ToList());
            return Ok(OperationResult<List<User>>.Success(resultDTO, result.Message));
        }

        [Authorize]
        // Gets all Users. Endpoint: GET api/User/GetCount by filter
        [HttpGet("[action]/{filter}")]
        public async Task<IActionResult> GetCountByFilter(string filter)
        {
            var result = await _usersRepository.GetCountByFilter(filter);
            var count = result.Data;
            return Ok(OperationResult<int>.Success(count, result.Message));
        }

        [Authorize]
        // Gets all Users. Endpoint: GET api/User/Get Count
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCount()
        {
            var result = await _usersRepository.GetCountByFilter(string.Empty);
            var count = result.Data;
            return Ok(OperationResult<int>.Success(count, result.Message));
        }

        [Authorize]
        // Gets a specific User by ID. Endpoint: GET api/User/GetUserById/{id}
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _usersRepository.GetUserById(id);
            var resultDTO = _mapper.Map<List<User>>(result.Data);
            return Ok(OperationResult<User>.Success(resultDTO.FirstOrDefault(), result.Message));
        }
    }
}

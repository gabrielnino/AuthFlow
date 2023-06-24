using Microsoft.AspNetCore.Mvc;
using AuthFlow.Application.Repositories.Interface;
using AutoMapper;
using AuthFlow.Domain.DTO;
using AuthFlow.Application.DTOs;

namespace AuthFlow.Api.Controllers
{
    // Defines route and declares this class as a controller in the API.
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // Defines an interface for accessing User data in the repository.
        private readonly IUserRepository _usersRepository;
        private readonly IMapper _mapper;

        // Constructor for UserController, injecting the User repository.
        public UserController(IUserRepository usersRepository, IConfiguration configuration, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
            
        }

        // Gets all Users. Endpoint: GET api/User/GetUsersAll
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUsersAll()
        {
            var result = await _usersRepository.GetAll();
            var resultDTO = _mapper.Map<List<Domain.DTO.User>>(result.Data.ToList());
            return Ok(OperationResult<List<User>>.Success(resultDTO, result.Message));
        }

        // Gets a specific User by ID. Endpoint: GET api/User/GetUserById/{id}
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _usersRepository.GetAllByFilter(u => u.Id.Equals(id));
            var resultDTO = _mapper.Map<List<User>>(result.Data);
            return Ok(OperationResult<User>.Success(resultDTO.FirstOrDefault(), result.Message));
        }

        // Disables a specific User by ID. Endpoint: GET api/User/DisableUser/{id}
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> DisableUser(int id)
        {
            var result = await _usersRepository.Deactivate(id);
            return Ok(result);
        }

        // Activates a specific User by ID. Endpoint: GET api/User/ActivateUser/{id}
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var result = await _usersRepository.Activate(id);
            return Ok(result);
        }

        // Creates a User. Endpoint: POST api/User
        [HttpPost]
        public async Task<IActionResult> CreateEntity(AddUserRequest addUserRequest)
        {
            var user = new Domain.Entities.User()
            {
                Username = addUserRequest.Username,
                Password = addUserRequest?.Password,
                Email = addUserRequest?.Email,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Active = false
            };
            var result = await _usersRepository.Add(user);
            return Ok(result);
        }

        // Updates a specific User. Endpoint: PUT api/User
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Update(int id, ModifiedUserRequest modifiedUserRequest)
        {
            var user = new Domain.Entities.User()
            {
                Id = id,
                Username = modifiedUserRequest?.Username,
                Password = modifiedUserRequest?.Password,
                Email = modifiedUserRequest?.Email,
            };
            var result = await _usersRepository.Modified(user);
            return Ok(result);
        }

        // Deletes a specific User by ID. Endpoint: DELETE api/User/Delete/{id}
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _usersRepository.Remove(id);
            return Ok(result);
        }
    }
}

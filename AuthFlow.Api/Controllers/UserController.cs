using Microsoft.AspNetCore.Mvc;
using AuthFlow.Domain.Entities;
using AuthFlow.Application.Repositories.Interface;

namespace AuthFlow.Api.Controllers
{
    // Defines route and declares this class as a controller in the API.
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        // Defines an interface for accessing User data in the repository.
        private readonly IUserRepository _usersRepository;

        // Constructor for UserController, injecting the User repository.
        public BaseController(IUserRepository usersRepository, IConfiguration configuration)
        {
            _usersRepository = usersRepository;
        }

        // Gets all Users. Endpoint: GET api/User/GetUsersAll
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUsersAll()
        {
            var result = await _usersRepository.GetAll();
            return Ok(result);
        }

        // Gets a specific User by ID. Endpoint: GET api/User/GetUserById/{id}
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _usersRepository.GetAllByFilter(u => u.Id.Equals(id));
            return Ok(result);
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
        public async Task<IActionResult> CreateEntity([FromBody] User user)
        {
            var result = await _usersRepository.Add(user);
            return Ok(result);
        }

        // Updates a specific User. Endpoint: PUT api/User
        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] User user)
        {
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

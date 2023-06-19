using Microsoft.AspNetCore.Mvc;
using AuthFlow.Domain.Entities;
using AuthFlow.Application.Repositories.Interface;

namespace AuthFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        //private readonly IConfiguration _configuration;
        private readonly IUserRepository _usersRepository;

        public UserController(IUserRepository usersRepository, IConfiguration configuration)
        {
            _usersRepository = usersRepository;
        }

        //[Authorize]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUsersAll()
        {
            var result = await _usersRepository.GetUsersAll();
            return Ok(result);
        }

        //[Authorize]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _usersRepository.GetUsersByFilter(u=>u.Id.Equals(id));
            return Ok(result);

        }

        //[Authorize]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> DisableUser(int id)
        {
            var result = await _usersRepository.DisableUser(id);
            return Ok(result);
        }

        //[Authorize]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var result = await  _usersRepository.ActivateUser(id);
            return Ok(result);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateEntity([FromBody] User user)
        {
            var result = await _usersRepository.CreateUser(user);
            return Ok(result);
        }

        //[Authorize]
        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            var result = await _usersRepository.UpdateUser(user);
            return Ok(result);
        }

        //[Authorize]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _usersRepository.DeleteUser(id);
            return Ok(result);
        }
    }
}

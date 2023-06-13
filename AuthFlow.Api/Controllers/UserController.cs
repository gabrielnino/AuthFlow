using Microsoft.AspNetCore.Mvc;
using AuthFlow.Domain;
using AuthFlow.Application.Repositories.Interface;

namespace AuthFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IUsersRepository _usersRepository;

        public UserController(IUsersRepository usersRepository, IConfiguration configuration)
        {
            // Inject IPeopleRepository instance into controller's constructor
            _usersRepository = usersRepository;
            _configuration = configuration;
        }

        //[Authorize]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                // Retrieve all people from the repository
                var users = _usersRepository.GetAll();
                // Return 200 OK response with the list of people
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateEntity([FromBody] User user)
        {
            try
            {
                if (user == null)
                {
                    // If the request body is null, return a 400 Bad Request response
                    return BadRequest();
                }

                var personByEmail = await _usersRepository.GetEntitiesByFilter(p => p.Email == user.Email);
                if (personByEmail is not null)
                {
                    // If the request body is null, return a 400 Bad Request response
                    return BadRequest();
                }

                // Add the new person to the repository
                _usersRepository.CreateEntity(user);
                // Return 201 Created response with the person and its ID in the Location header
                return Ok(user);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        //[Authorize]
        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            try
            {
                if (user == null)
                {
                    // If the request body is null or the person's ID doesn't match the requested ID, return a 400 Bad Request response
                    return BadRequest();
                }
                // Retrieve the existing person by ID from the repository
                var existingUser = await _usersRepository.GetEntitiesByFilter(p => p.Id == user.Id);
                if (existingUser == null)
                {
                    return NotFound();
                }
                if (existingUser == null)
                {
                    // If the person doesn't exist, return a 404 Not Found response
                    return NotFound();
                }

                // Update the person in the repository
                var result = _usersRepository.UpdateEntity(user);
                // Return a 204 No Content response
                return Ok(result.Result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        //[Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Retrieve the person to delete by ID from the repository
                var user = await _usersRepository.GetEntitiesByFilter(p => p.Id == id);
                if (user == null)
                {
                    // If the person doesn't exist, return a 404 Not Found response
                    return NotFound();
                }
                // Delete the person from the repository
                Task<bool> result = _usersRepository.DeleteEntity(user.First());
                // Return a 204 No Content response
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}

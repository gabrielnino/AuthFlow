using Microsoft.AspNetCore.Mvc;
using AuthFlow.Domain.Entities;
using AuthFlow.Application.Repositories.Interface;

namespace AuthFlow.Api.Controllers
{
    // Defines route and declares this class as a controller in the API.
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        // Defines an interface for accessing Session data in the repository.
        private readonly ISessionRepository _sessionRepository;

        // Constructor for SessionController, injecting the Session repository.
        public SessionController(ISessionRepository sessionRepository, IConfiguration configuration)
        {
            _sessionRepository = sessionRepository;
        }

        // Gets all Sessions. Endpoint: GET api/Session/GetUsersAll
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUsersAll()
        {
            var result = await _sessionRepository.GetSessionsAll();
            return Ok(result);
        }

        // Gets a specific Session by ID. Endpoint: GET api/Session/GetUserById/{id}
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _sessionRepository.GetSessionsByFilter(u => u.Id.Equals(id));
            return Ok(result);
        }

        // Disables a specific Session by ID. Endpoint: GET api/Session/DisableUser/{id}
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> DisableUser(int id)
        {
            var result = await _sessionRepository.DisableSession(id);
            return Ok(result);
        }

        // Activates a specific Session by ID. Endpoint: GET api/Session/ActivateUser/{id}
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var result = await _sessionRepository.ActivateSession(id);
            return Ok(result);
        }

        // Creates a Session. Endpoint: POST api/Session
        [HttpPost]
        public async Task<IActionResult> CreateEntity([FromBody] Session session)
        {
            var result = await _sessionRepository.CreateSession(session);
            return Ok(result);
        }

        // Updates a specific Session. Endpoint: PUT api/Session
        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] Session session)
        {
            var result = await _sessionRepository.UpdateSession(session);
            return Ok(result);
        }

        // Deletes a specific Session by ID. Endpoint: DELETE api/Session/Delete/{id}
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sessionRepository.DeleteSession(id);
            return Ok(result);
        }
    }
}

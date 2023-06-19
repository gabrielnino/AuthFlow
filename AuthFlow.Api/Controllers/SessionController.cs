using Microsoft.AspNetCore.Mvc;
using AuthFlow.Domain.Entities;
using AuthFlow.Application.Repositories.Interface;

namespace AuthFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {

        //private readonly IConfiguration _configuration;
        private readonly ISessionRepository _sessionRepository;

        public SessionController(ISessionRepository sessionRepository, IConfiguration configuration)
        {
            _sessionRepository = sessionRepository;
        }

        //[Authorize]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUsersAll()
        {
            var result = await _sessionRepository.GetSessionsAll();
            return Ok(result);
        }

        //[Authorize]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _sessionRepository.GetSessionsByFilter(u=>u.Id.Equals(id));
            return Ok(result);

        }

        //[Authorize]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> DisableUser(int id)
        {
            var result = await _sessionRepository.DisableSession(id);
            return Ok(result);
        }

        //[Authorize]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var result = await _sessionRepository.ActivateSession(id);
            return Ok(result);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateEntity([FromBody] Session session)
        {
            var result = await _sessionRepository.CreateSession(session);
            return Ok(result);
        }

        //[Authorize]
        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] Session session)
        {
            var result = await _sessionRepository.UpdateSession(session);
            return Ok(result);
        }

        //[Authorize]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sessionRepository.DeleteSession(id);
            return Ok(result);
        }
    }
}

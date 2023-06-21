using Microsoft.AspNetCore.Mvc;
using AuthFlow.Domain.Entities;
using AuthFlow.Application.Repositories.Interface;

namespace AuthFlow.Api.Controllers
{
    // Defines route and declares this class as a controller in the API.
    [Route("api/[controller]")]
    [ApiController]
    public class AccessTokenController : ControllerBase
    {
        // Defines an interface for accessing AccessToken data in the repository.
        private readonly IAccessTokenRepository _accessTokenRepository;

        // Constructor for AccessTokenController, injecting the AccessToken repository.
        public AccessTokenController(IAccessTokenRepository accessTokenRepository, IConfiguration configuration)
        {
            _accessTokenRepository = accessTokenRepository;
        }

        // Gets all AccessTokens. Endpoint: GET api/AccessToken/GetUsersAll
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUsersAll()
        {
            var result = await _accessTokenRepository.GetAccessTokensAll();
            return Ok(result);
        }

        // Gets a specific AccessToken by ID. Endpoint: GET api/AccessToken/GetUserById/{id}
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _accessTokenRepository.GetAccessTokensByFilter(u => u.Id.Equals(id));
            return Ok(result);
        }

        // Disables a specific AccessToken by ID. Endpoint: GET api/AccessToken/DisableUser/{id}
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> DisableUser(int id)
        {
            var result = await _accessTokenRepository.DisableAccessToken(id);
            return Ok(result);
        }

        // Activates a specific AccessToken by ID. Endpoint: GET api/AccessToken/ActivateUser/{id}
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var result = await _accessTokenRepository.ActivateAccessToken(id);
            return Ok(result);
        }

        // Creates an AccessToken. Endpoint: POST api/AccessToken
        [HttpPost]
        public async Task<IActionResult> CreateEntity([FromBody] AccessToken accessToken)
        {
            var result = await _accessTokenRepository.CreateAccessToken(accessToken);
            return Ok(result);
        }

        // Updates a specific AccessToken. Endpoint: PUT api/AccessToken
        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] AccessToken accessToken)
        {
            var result = await _accessTokenRepository.UpdateAccessToken(accessToken);
            return Ok(result);
        }

        // Deletes a specific AccessToken by ID. Endpoint: DELETE api/AccessToken/Delete/{id}
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _accessTokenRepository.DeleteAccessToken(id);
            return Ok(result);
        }
    }
}

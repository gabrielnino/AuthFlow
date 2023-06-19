using Microsoft.AspNetCore.Mvc;
using AuthFlow.Domain.Entities;
using AuthFlow.Application.Repositories.Interface;

namespace AuthFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessTokenController : ControllerBase
    {

        //private readonly IConfiguration _configuration;
        private readonly IAccessTokenRepository _accessTokenRepository;

        public AccessTokenController(IAccessTokenRepository accessTokenRepository, IConfiguration configuration)
        {
            _accessTokenRepository = accessTokenRepository;
        }

        //[Authorize]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUsersAll()
        {
            var result = await _accessTokenRepository.GetAccessTokensAll();
            return Ok(result);
        }

        //[Authorize]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _accessTokenRepository.GetAccessTokensByFilter(u=>u.Id.Equals(id));
            return Ok(result);

        }

        //[Authorize]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> DisableUser(int id)
        {
            var result = await _accessTokenRepository.DisableAccessToken(id);
            return Ok(result);
        }

        //[Authorize]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var result = await _accessTokenRepository.ActivateAccessToken(id);
            return Ok(result);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateEntity([FromBody] AccessToken accessToken)
        {
            var result = await _accessTokenRepository.CreateAccessToken(accessToken);
            return Ok(result);
        }

        //[Authorize]
        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] AccessToken accessToken)
        {
            var result = await _accessTokenRepository.UpdateAccessToken(accessToken);
            return Ok(result);
        }

        //[Authorize]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _accessTokenRepository.DeleteAccessToken(id);
            return Ok(result);
        }
    }
}

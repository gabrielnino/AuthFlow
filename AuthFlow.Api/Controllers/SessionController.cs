using Microsoft.AspNetCore.Mvc;
using AuthFlow.Domain.DTO;
using AuthFlow.Application.Repositories.Interface;
using AutoMapper;
using AuthFlow.Application.DTOs;
using AuthFlow.Infraestructure.Repositories;

namespace AuthFlow.Api.Controllers
{
    // Defines route and declares this class as a controller in the API.
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        // Defines an interface for accessing Session data in the repository.
        private readonly ISessionRepository _sessionRepository;

        private readonly IMapper _mapper;

        // Constructor for SessionController, injecting the Session repository.
        public SessionController(ISessionRepository sessionRepository,IUserRepository userRepository , IConfiguration configuration, IMapper mapper)
        {
            _sessionRepository = sessionRepository;
            _sessionRepository._userRepository = userRepository;
            _mapper = mapper;
        }

        // Gets all Sessions. Endpoint: GET api/Session/GetUsersAll
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _sessionRepository.GetAll();
            var resultDTO = _mapper.Map<List<Session>>(result.Data);
            return Ok(OperationResult<List<Session>>.Success(resultDTO, result.Message));
        }

        // Gets a specific Session by ID. Endpoint: GET api/Session/GetUserById/{id}
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetAllByFilter(int id)
        {
            var result = await _sessionRepository.GetAllByFilter(u => u.Id.Equals(id));
            var resultDTO = _mapper.Map<List<Session>>(result.Data);
            return Ok(OperationResult<Session>.Success(resultDTO.FirstOrDefault(), result.Message));
        }

        // Disables a specific Session by ID. Endpoint: GET api/Session/DisableUser/{id}
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _sessionRepository.Deactivate(id);
            return Ok(result);
        }

        // Creates a Session. Endpoint: POST api/Session
        [HttpPost]
        public async Task<IActionResult> Add(AddSessionRequest addSessionRequest)
        {
            var session = new Domain.Entities.Session()
            {
                UserId = addSessionRequest.UserId,
                Token = addSessionRequest.Token,
                CreatedAt = DateTime.Now,
                Expiration = DateTime.Now.AddMinutes(20), //add a parameter to parametrizece
            };

            var result = await _sessionRepository.Add(session);
            return Ok(result);
        }
    }
}

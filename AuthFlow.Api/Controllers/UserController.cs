using Microsoft.AspNetCore.Mvc;
using AuthFlow.Application.Repositories.Interface;
using AutoMapper;
using AuthFlow.Domain.DTO;
using AuthFlow.Application.DTOs;
using AuthFlow.Application.Interfaces;
using AuthFlow.Application.Use_cases.Interface.Operations;
using Microsoft.AspNetCore.Authorization;

namespace AuthFlow.Api.Controllers
{
    // Defines route and declares this class as a controller in the API.
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // Defines an interface for accessing User data in the repository.
        private readonly IUserRepository _usersRepository;
        private readonly IOtpService _otpService;
        private readonly IReCaptchaService _reCaptchaService;
        private readonly IMapper _mapper;

        // Constructor for UserController, injecting the User repository.
        public UserController(IUserRepository usersRepository, IOtpService otpService, IReCaptchaService reCaptchaService, IConfiguration configuration, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _otpService = otpService;
            _reCaptchaService = reCaptchaService;
            _mapper = mapper;
        }

        // Gets all Users. Endpoint: GET api/User/GetUsersAll
        [HttpGet("[action]/{pageNumber}/{pageSize}/{filter}")]
        public async Task<IActionResult> GetPageByFilter(int pageNumber, int pageSize, string filter)
        {
            var result = await _usersRepository.GetPageByFilter(pageNumber, pageSize, filter);
            var resultDTO = _mapper.Map<List<User>>(result.Data.ToList());
            return Ok(OperationResult<List<User>>.Success(resultDTO, result.Message));
        }

        // Gets all Users. Endpoint: GET api/User/GetUsersAll
        [HttpGet("[action]/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetPage(int pageNumber, int pageSize)
        {
            var result = await _usersRepository.GetPageByFilter(pageNumber, pageSize, string.Empty);
            var resultDTO = _mapper.Map<List<User>>(result.Data.ToList());
            return Ok(OperationResult<List<User>>.Success(resultDTO, result.Message));
        }

        // Gets all Users. Endpoint: GET api/User/GetUsersAll
        [HttpGet("[action]/{filter}")]
        public async Task<IActionResult> GetCountByFilter(string filter)
        {
            var result = await _usersRepository.GetCountByFilter(filter);
            var count = result.Data;
            return Ok(OperationResult<int>.Success(count, result.Message));
        }

        // Gets all Users. Endpoint: GET api/User/GetUsersAll
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCount()
        {
            var result = await _usersRepository.GetCountByFilter(string.Empty);
            var count = result.Data;
            return Ok(OperationResult<int>.Success(count, result.Message));
        }

        // Gets a specific User by ID. Endpoint: GET api/User/GetUserById/{id}
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _usersRepository.GetAllByFilter(u => u.Id.Equals(id));
            var resultDTO = _mapper.Map<List<User>>(result.Data);
            return Ok(OperationResult<User>.Success(resultDTO.FirstOrDefault(), result.Message));
        }



        // Gets a specific User by ID. Endpoint: GET api/User/GetUserById/{id}
        [HttpGet("[action]/{email}")]
        public async Task<IActionResult> GenerateOtp(string email)
        {
            var result = await _otpService.GenerateOtp(email);
            return Ok(result);
        }

        // Gets a specific User by ID. Endpoint: GET api/User/GetUserById/{id}
        [HttpGet("[action]/{email}/{otp}")]
        public async Task<IActionResult> ValidateOtp(string email, string otp)
        {
            var result = await _otpService.ValidateOtp(email, otp);
            return Ok(result);
        }

        [Authorize]
        // Creates a User. Endpoint: POST api/User
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateEntity(AddUserRequest addUserRequest)
        {
            var password = addUserRequest?.Password;
            var user = new Domain.Entities.User()
            {
                Username = addUserRequest.Username,
                Password = password,
                Email = addUserRequest?.Email,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Active = false
            };
            var result = await _usersRepository.Add(user);
            return Ok(result);
        }

       
        // Creates a User. Endpoint: POST api/User
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(Credential credential)
        {
            var result = await _usersRepository.Login(credential.Username, credential.Password);
            return Ok(result);
        }

        // Creates a User. Endpoint: POST api/User
        [HttpPost("[action]")]
        public async Task<IActionResult> ReCaptcha(ReCaptcha token)
        {
            var result = await _reCaptchaService.Validate(token);
            return Ok(result);
        }

        // Updates a specific User. Endpoint: PUT api/User
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Update(int id, ModifiedUserRequest modifiedUserRequest)
        {
            var password = modifiedUserRequest?.Password;
            var user = new Domain.Entities.User()
            {
                Id = id,
                Username = modifiedUserRequest?.Username,
                Password = password,
                Email = modifiedUserRequest?.Email,
            };
            var result = await _usersRepository.Modified(user);
            return Ok(result);
        }

        // Activates a specific User by ID. Endpoint: GET api/User/ActivateUser/{id}
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var result = await _usersRepository.Activate(id);
            return Ok(result);
        }

        // Disables a specific User by ID. Endpoint: GET api/User/DisableUser/{id}
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> DisableUser(int id)
        {
            var result = await _usersRepository.Deactivate(id);
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

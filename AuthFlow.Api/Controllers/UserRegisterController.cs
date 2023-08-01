using Microsoft.AspNetCore.Mvc;
using AuthFlow.Application.Repositories.Interface;
using AutoMapper;
using AuthFlow.Domain.DTO;
using AuthFlow.Application.Interfaces;
using AuthFlow.Application.Use_cases.Interface.Operations;
using Microsoft.AspNetCore.Authorization;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegisterController : ControllerBase
    {
        // Defines an interface for accessing User data in the repository.
        private readonly IUsersRepository _usersRepository;
        private readonly IOTPServices _otpService;
        private readonly IReCaptchaService _reCaptchaService;

        // Constructor for UserController, injecting the User repository.
        public UserRegisterController(IUsersRepository usersRepository, IOTPServices otpService, IReCaptchaService reCaptchaService, IConfiguration configuration, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _otpService = otpService;
            _reCaptchaService = reCaptchaService;
        }

        [Authorize]
        // Gets a specific User by ID. Endpoint: GET api/User/GetUserById/{id}
        [HttpGet("[action]/{email}")]
        public async Task<IActionResult> GenerateOtp(string email)
        {
            var result = await _otpService.GenerateOtp(email);
            return Ok(result);
        }

        //LoginOtp
        [Authorize]
        // Gets a specific User by ID. Endpoint: GET api/User/GetUserById/{id}
        [HttpGet("[action]/{email}/{otp}")]
        public async Task<IActionResult> ValidateOtp(string email, string otp)
        {
            var result = await _otpService.ValidateOtp(email, otp);
            return Ok(result);
        }


        [Authorize]
        // Activates a specific User by ID. Endpoint: GET api/User/ActivateUser/{id}
        [HttpGet("[action]/{email}")]
        public async Task<IActionResult> ValidateEmail(string? email)
        {
            var result = await _usersRepository.ValidateEmail(email);
            return Ok(result);
        }

        [Authorize]
        // Creates a User. Endpoint: POST api/User
        [HttpPost("[action]")]
        public async Task<IActionResult> ReCaptcha(ReCaptcha token)
        {
            var result = await _reCaptchaService.Validate(token);
            return Ok(result);
        }

        [Authorize]
        // Activates a specific User by ID. Endpoint: GET api/User/ActivateUser/{id}
        [HttpGet("[action]/{username}")]
        public async Task<IActionResult> ValidateUsername(string username)
        {
            var result = await _usersRepository.ValidateUsername(username);
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
                Username = addUserRequest?.Username,
                Password = password,
                Email = addUserRequest?.Email,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Active = false
            };
            var result = await _usersRepository.Add(user);
            return Ok(result);
        }

        [Authorize]
        // Activates a specific User by ID. Endpoint: GET api/User/ActivateUser/{id}
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var result = await _usersRepository.Activate(id);
            return Ok(result);
        }
        //SetNewPassword

        [Authorize]
        // Creates a User. Endpoint: POST api/User
        [HttpPost("[action]")]
        public async Task<IActionResult> SetNewPassword(Credential credential)
        {
            var result = await _usersRepository.SetNewPassword(credential.Username, credential.Password);
            return Ok(result);
        }
    }
}

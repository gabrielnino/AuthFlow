using AuthFlow.Application.DTOs;
using AuthFlow.Application.Interfaces;
using AuthFlow.Application.Repositories.Interface;
using AuthFlow.Application.Use_cases.Interface.Operations;
using AuthFlow.Domain.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        // Defines an interface for accessing User data in the repository.
        private readonly IUserRepository _usersRepository;

        // Constructor for UserController, injecting the User repository.
        public UserLoginController(IUserRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        // Creates a User. Endpoint: POST api/User
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(Credential credential)
        {
            var result = await _usersRepository.Login(credential.Username, credential.Password);
            return Ok(result);
        }
    }
}

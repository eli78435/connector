using Edc.Common;
using Edc.Identity.Managers;
using Edc.Identity.Models;
using Edc.Identity.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edc.Identity.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuthController : Controller
{
    private readonly ILogger _logger;
    private readonly IUsersManager _usersManager;
    private readonly IIdGenerator _idGenerator;

    public AuthController(ILogger<AuthController> logger,
        IUsersManager usersManager,
        IIdGenerator idGenerator)
    {
        _logger = logger;
        _usersManager = usersManager;
        _idGenerator = idGenerator;
    }

    [AllowAnonymous]
    [HttpPost("operations/addUser")]
    public async Task<IActionResult> AddNewUser([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest();
        }
        
        var user = new User(_idGenerator.GenerateId(), 
                    request.FirstName,
                    request.LastName,
            request.UserName, 
            request.Email,
            request.Password, 
            Roles.User);
        
        try
        {
            await _usersManager.AddUser(user);
            return Ok($"User {request.UserName} registered successfully.");
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }

    [AllowAnonymous]
    [HttpPost("operations/loginByUserNameAndPassword")]
    public async Task<IActionResult> LoginByUserNameAndPassword([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest();
        }

        try
        {
            var token = await _usersManager.LoginByUserNameAndPassword(request.UserName, request.Password);
            if (string.IsNullOrWhiteSpace(token))
            {
                return Unauthorized("Invalid credentials");
            }
            else
            {
                return Ok(token);
            }
        }
        catch (Exception e)
        {
            return Unauthorized("Invalid credentials");
        }
    }
}
// using Edc.Identity.WebApi.Requests;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using MongoDB.Bson;
// using MongoDB.Driver;
//
// namespace Edc.Identity.WebApi.Controllers;
//
// [ApiController]
// [Authorize]
// [Route("api/[controller]")]
// public class UsersDetailsController : Controller
// {
//     private readonly ILogger _logger;
//     // private readonly IMongoCollectionHolder<UserDetailsDal> _userDetailsCollection;_userDetailsCollection
//
//     public UsersDetailsController(ILogger<UsersDetailsController> logger, IMongoCollectionHolder<UserDetailsDal> userDetailsCollection)
//     {
//         _logger = logger;
//         _userDetailsCollection = userDetailsCollection;
//     }
//     
//     [HttpGet]
//     public async Task<IActionResult> GetUsersDetails()
//     {
//         var details = await _userDetailsCollection.Collection
//             .Find(FilterDefinition<UserDetailsDal>.Empty)
//             .ToListAsync();
//         return Ok("hello new user");
//     }
//     
//     [HttpGet("{id}")]
//     public IActionResult GetUserDetail(string id)
//     {
//         return Ok($"hello new user {id}");
//     }
//     
//     [HttpPost]
//     public async Task<IActionResult> AddUserDetail([FromBody] AddUserRequest request)
//     {
//         var details = new UserDetailsDal
//         {
//             Id = new ObjectId(), 
//             FirstName = request.FirstName,
//             LastName = request.LastName
//         };
//         
//         await _userDetailsCollection.Collection.InsertOneAsync(details);
//         _logger.LogInformation("add new user with id {Id} name {FirstName} {LastName}", details.Id.ToString(), request.FirstName, request.LastName);
//         return Ok("id");
//     }
//     
//     [HttpDelete("{id}")]
//     public IActionResult DeleteUserDetail(string id)
//     {
//         _logger.LogInformation("remove user with id {UserDetailsId}", id);
//         return Ok("id");
//     }
// }
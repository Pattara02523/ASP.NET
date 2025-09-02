using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Dtos;
using MyAPI.Interfaces;

namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICourseLibraryRepository _repo;
        private readonly IMapper _mapper;
        public UserController(ICourseLibraryRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("register")]
        public IActionResult actionResult([FromBody] UserCreateDto userCreateDto)
        {
            if(userCreateDto == null)
            {
                return BadRequest(); // 400
            }        
            if(_repo.IsUserExist(userCreateDto.Username))
            {
                return BadRequest("Username already exists"); // 400
            }
            var user = _mapper.Map<Models.User>(userCreateDto);
            _repo.Register(user);
            _repo.save();
            user.Password = null;
            return StatusCode(201 , user);  // Created
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(string username, string password)
        {
            var user = _repo.Authenticate(username, password);
            if(user == null)
                return Unauthorized(); // 401
            return Ok(user); // 200
        }

    }
}

using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Dtos;
using MyAPI.Extensions;
using MyAPI.Interfaces;

namespace MyAPI.Controllers
{
    // ตัวควบคุม (Controller) สำหรับจัดการ Author
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Authorize]
    public class AuthorController : ControllerBase
    {
        private readonly ICourseLibraryRepository _repo;
        private readonly IMapper _mapper;
        public AuthorController(ICourseLibraryRepository repo , IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException
            (nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException
            (nameof(mapper));
        }

        /// <summary>
        /// สรุป: เมธอด GET สำหรับดึงข้อมูล Author ทั้งหมด
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAuthors()
        {
            var data = _repo.GetAuthors();
            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(data));
        }

        /// <summary>
        /// สรุป: เมธอด GET สำหรับค้นหา Author โดยใช้ authorId
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        [HttpGet("{authorId}")]
        public IActionResult GetAuthors(int authorId)
        {
            if (!_repo.AuthorExists(authorId))
                return NotFound();
            var data = _repo.GetAuthor(authorId);

            return Ok(_mapper.Map<AuthorDto>(data));

        }

        /// <summary>
        /// สรุป: เมธอด GET สำหรับค้นหา Author โดยใช้ mainCategory และ searchQuery
        /// </summary>
        /// <param name="mainCategory"></param>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public IActionResult GetAuthors([FromQuery] string? mainCategory, [FromQuery] string? searchQuery)
        {
            var data = _repo.GetAuthors(mainCategory, searchQuery);
            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(data));
        }

        /// <summary>
        /// สรุป: เมธอด POST สำหรับสร้าง Author ใหม่
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateAuthor([FromBody] AuthorCreateDto author, ApiVersion version)
        {
            if (author == null)
                return BadRequest();
            var authorEntity = _mapper.Map<Models.Author>(author);
            _repo.AddAuthor(authorEntity);
            _repo.save();
            var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);
            return CreatedAtAction("GetAuthors", new {version=version.ToString(), authorId = authorToReturn.Id }, authorToReturn);
        }

        /// <summary>
        /// เมธอด PUT สำหรับอัปเดตข้อมูล Author ที่มีอยู่
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        [HttpPut("{authorId}")]
        public IActionResult UpdateAuthor(int authorId, [FromBody] AuthorCreateDto author)
        {
            if (author == null)
                return BadRequest();
            if (!_repo.AuthorExists(authorId))
                return NotFound();
            var authorEntity = _repo.GetAuthor(authorId);
            if (authorEntity == null)
                return NotFound();
            _mapper.Map(author, authorEntity);
            _repo.UpdateAuthor(authorEntity);
            _repo.save();
            return NoContent();
        }

        /// <summary>
        /// เมธอด DELETE สำหรับลบ Author โดยใช้ authorId
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        [HttpDelete("{authorId}")]
        public IActionResult DeleteAuthor(int authorId)
        {
            if (!_repo.AuthorExists(authorId))
                return NotFound();
            var authorEntity = _repo.GetAuthor(authorId);
            if (authorEntity == null)
                return NotFound();

            if (_repo.GetCourses(authorId).Count() > 0)
            {
                return BadRequest("Can't delete author with existing courses");
            }
            _repo.DeleteAuthor(authorEntity);
            _repo.save();
            return NoContent();
        }

        

    }

}

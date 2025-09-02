using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Dtos;
using MyAPI.Interfaces;

namespace MyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/author")]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName = "v2")]
    [ApiController]
    public class Author2Controller : ControllerBase
    {
        private readonly ICourseLibraryRepository _repo;
        private readonly IMapper _mapper;
        public Author2Controller(ICourseLibraryRepository repo, IMapper mapper)
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
            return Ok(_mapper.Map<IEnumerable<Author2Dto>>(data));
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
            
            return Ok(_mapper.Map<Author2Dto>(data));

        }


    }
}

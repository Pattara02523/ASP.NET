using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Dtos;
using MyAPI.Interfaces;
using MyAPI.Models;

namespace MyAPI.Controllers
{
    [Route("api/author/{authorId}/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseLibraryRepository _repo;
        private readonly IMapper _mapper;
        public CourseController(ICourseLibraryRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException
            (nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException
            (nameof(mapper));
        }

        /// <summary>
        /// สรุป: เมธอด GET สำหรับดึงข้อมูล Course ของ Author ที่ระบุโดย authorId
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCourses(int authorId)
        {
            if (!_repo.AuthorExists(authorId))
                return NotFound();
            var data = _repo.GetCourses(authorId);
            return Ok(_mapper.Map<IEnumerable<CourseDto>>(data));
        }

        /// <summary>
        /// เมธอด GET สำหรับค้นหา Course โดยใช้ courseId ของ Author ที่ระบุโดย authorId
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpGet("{courseId}")]
        public IActionResult GetCourse(int authorId, int courseId)
        {
            if (!_repo.AuthorExists(authorId))
                return NotFound();
            var data = _repo.GetCourse(authorId, courseId);
            if (data == null)
                return NotFound();
            return Ok(_mapper.Map<CourseDto>(data));
        }

        /// <summary>
        /// เมธอด POST สำหรับสร้าง Course ใหม่สำหรับ Author ที่ระบุโดย authorId
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateCourse(int authorId, [FromBody] CourseCreateDto course)
        {
            if (course == null)
                return BadRequest();
            if (!_repo.AuthorExists(authorId))
                return NotFound();
            var courseEntity = _mapper.Map<Models.Course>(course);
            _repo.AddCourse(authorId, courseEntity);
            _repo.save();
            var courseToReturn = _mapper.Map<CourseDto>(courseEntity);
            return CreatedAtAction(nameof(GetCourse),
                new { authorId = authorId, courseId = courseToReturn.Id },
                courseToReturn);
        }

        /// <summary>
        /// เมธอด PUT สำหรับอัปเดตข้อมูล Course ที่มีอยู่สำหรับ Author ที่ระบุโดย authorId
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="courseId"></param>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpPut("{courseId}")]
        public IActionResult UpdateCourse(int authorId, int courseId, [FromBody] CourseCreateDto course)
        {
            if (course == null)
                return BadRequest();
            if (!_repo.AuthorExists(authorId))
                return NotFound();
            var courseEntity = _repo.GetCourse(authorId, courseId);
            if (courseEntity == null)
                return NotFound();
            _mapper.Map(course, courseEntity);
            _repo.UpdateCourse(courseEntity);
            _repo.save();
            return NoContent();
        }

        /// <summary>
        /// เมธอด PATCH สำหรับอัปเดตบางส่วนของ Course ที่มีอยู่สำหรับ Author ที่ระบุโดย authorId
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="courseId"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("{courseId}")]
        public IActionResult PartiallyUpdateCourse(int authorId, int courseId, [FromBody] JsonPatchDocument<Course> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            if (!_repo.AuthorExists(authorId))
                return NotFound();

            var courseEntity = _repo.GetCourse(authorId, courseId);

            if (courseEntity == null)
                return NotFound();

            patchDoc.ApplyTo(courseEntity, ModelState);
            if (!TryValidateModel(courseEntity))
                return ValidationProblem(ModelState);

            _repo.UpdateCourse(courseEntity);
            _repo.save();
            return NoContent();
        }

        /// <summary>
        /// เมธอด DELETE สำหรับลบ Course ที่มีอยู่สำหรับ Author ที่ระบุโดย authorId 
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpDelete("{courseId}")]
        public IActionResult DeleteCourse(int authorId, int courseId)
        {
            if (!_repo.AuthorExists(authorId))
                return NotFound();
            var courseEntity = _repo.GetCourse(authorId, courseId);
            if (courseEntity == null)
                return NotFound();

            _repo.DeleteCourse(courseEntity);
            _repo.save();
            return NoContent();
        }

    }
}
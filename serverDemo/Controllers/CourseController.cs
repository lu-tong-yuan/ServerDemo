using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using serverDemo.Dtos;
using serverDemo.Services;
using serverDemo.Validators;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace serverDemo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseService _courseService;

        public CourseController(CourseService courseService) 
        {
            _courseService = courseService;
        }

        //[HttpGet("testCourse")]
        //public string Test()
        //{
        //    return "成功連結course route...";
        //}
        // GET: api/<CourseController>
        [HttpGet]
        public IActionResult Get()
        {
            var result = _courseService.GetCourses();

            if (result == null || result.Count() == 0)
            {
                return NotFound("沒有課程");
            }

            return Ok(result);
        }

        // GET api/<CourseController>/5
        [HttpGet("{id}")]
        public IActionResult GetCourseByID(Guid id)
        {
            var result = _courseService.GetCourse(id);

            if (result == null)
            {
                return NotFound("找不到課程");
            }

            return Ok(result);
        }


        [HttpGet("findByName/{name}")]
        public IActionResult GetCourseByName(string name)
        {
            var result = _courseService.GetCourseByName(name);

            if (result == null)
            {
                return NotFound("找不到課程");
            }

            return Ok(result);
        }

        [HttpGet("instructor/{_instructor_id}")]
        public IActionResult GetCourseByTeacher(Guid _instructor_id)
        {
            var result = _courseService.GetCourse(_instructor_id);

            if (result == null)
            {
                return NotFound("找不到課程");
            }

            return Ok(result);
        }

        [HttpGet("student/{_student_id}")]
        public IActionResult GetCourseByStudent(Guid _student_id)
        {
            var result = _courseService.GetCourseByStudent(_student_id);

            if (result == null)
            {
                return NotFound("找不到課程");
            }

            return Ok(result);
        }

        // POST api/<CourseController>
        [Authorize(Roles = "instructor")]
        [HttpPost]
        public IActionResult PostCourse([FromBody] CourseDto value)
        {
            var validator = new CourseValidator();
            var result = validator.Validate(value);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            if (_courseService.CreateCourse(value))
            {
                return Ok("新課程已經保存");
            }
            return StatusCode(500, "無法創建課程。。。");
        }

        [Authorize(Roles = "student")]
        [HttpPost("enroll/{id}")]
        public IActionResult enroll(Guid id)
        {
            if (_courseService.Enroll(id))
            {
                return Ok("註冊完成");
            }
            return StatusCode(500, "無法註冊課程。。。");
        }

        [Authorize(Roles = "instructor")]
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] CourseDto value)
        {
            var validator = new CourseValidator();
            var validatorResult = validator.Validate(value);
            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors);
            }

            var result = _courseService.UpdateCourse(id, value);
            if (result == null)
            {
                return NotFound("可能找不到課程，或是你不是此課程的講師");
            }

            return Ok(new { msg = "課程已經被更新成功。", Course = result });
        }

        // DELETE api/<CourseController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (_courseService.DeleteCourse(id) == 0)
            {
                return NotFound("找不到刪除的課程");
            }

            return NoContent();
        }
    }
}

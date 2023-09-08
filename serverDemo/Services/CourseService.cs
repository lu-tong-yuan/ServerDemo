using Microsoft.EntityFrameworkCore;
using serverDemo.Dtos;
using serverDemo.Models;
using System.Security.Claims;

namespace serverDemo.Services
{
    public class CourseService
    {
        private readonly ServerDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CourseService(ServerDBContext serverDBContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = serverDBContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public Boolean CreateCourse(CourseDto value)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var userID = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            value.Instructor = Guid.Parse(userID);
            Course insert = new Course();
            _dbContext.Course.Add(insert).CurrentValues.SetValues(value);
            _dbContext.SaveChanges();
            return true;
        }

        public List<CourseGetDto> GetCourses()
        {
            var result = _dbContext.Course.Include(a => a.InstructorNavigation).ToList();

            return result.Select(a => CourseToCourseGetDto(a)).ToList();
        }

        public CourseGetDto GetCourse(Guid id)
        {
            var result = (from a in _dbContext.Course
                          where a.CourseID == id
                          select a).Include(a => a.InstructorNavigation).SingleOrDefault();

            if (result != null)
            {
                return CourseToCourseGetDto(result);
            }

            return null;
        }

        public CourseGetDto GetCourseByName(string name)
        {
            var result = (from a in _dbContext.Course
                          where a.Title == name
                          select a).Include(a => a.InstructorNavigation).SingleOrDefault();

            if (result != null)
            {
                return CourseToCourseGetDto(result);
            }

            return null;
        }

        public CourseGetDto GetCourseByInstructor(Guid id)
        {
            var result = (from a in _dbContext.Course
                          where a.Instructor == id
                          select a).Include(a => a.InstructorNavigation).SingleOrDefault();

            if (result != null)
            {
                return CourseToCourseGetDto(result);
            }

            return null;
        }

        public CourseGetDto GetCourseByStudent(Guid id)
        {
            var result = (from a in _dbContext.CourseDetail
                          where a.UserID == id
                          select a).Include(a => a.Course).ThenInclude(b => b.InstructorNavigation).SingleOrDefault();

            if (result != null)
            {
                return CourseDetailToCourseGetDto(result);
            }

            return null;
        }

        public Boolean Enroll(Guid id)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var userID = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            CourseDetailPostDto value = new CourseDetailPostDto
            {
                CourseID = id,
                UserID = Guid.Parse(userID)
            };
            CourseDetail insert = new CourseDetail();
            _dbContext.CourseDetail.Add(insert).CurrentValues.SetValues(value);
            _dbContext.SaveChanges();
            return true;
        }

        public CourseGetDto UpdateCourse(Guid id, CourseDto value)
        {
            var update = (from a in _dbContext.Course
                          where a.CourseID == id
                          select a).Include(a => a.InstructorNavigation).SingleOrDefault();

            var httpContext = _httpContextAccessor.HttpContext;
            var userID = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (update == null || update.Instructor != Guid.Parse(userID)) 
            {
                return null;
            }
            value.Instructor = Guid.Parse(userID);
            _dbContext.Course.Update(update).CurrentValues.SetValues(value);
            _dbContext.SaveChanges();
            CourseGetDto courseGetDto = new CourseGetDto 
            {
                CourseID = id,
                Title = update.Title,
                Description = update.Description,
                Price = update.Price,
                Instructor = new CourseGetDto.User
                {
                    Name = update.InstructorNavigation.Name,
                    Email = update.InstructorNavigation.Email,
                }
            };
            return courseGetDto;

        }

        public int DeleteCourse(Guid id)
        {
            var delete = (from a in _dbContext.Course
                          where a.CourseID == id
                          select a).Include(b => b.CourseDetail).SingleOrDefault();

            if (delete != null)
            {
                _dbContext.Course.Remove(delete);
            }

            return _dbContext.SaveChanges();
        }

        private static CourseGetDto CourseToCourseGetDto(Course a) 
        {
            return new CourseGetDto
            {
                CourseID = a.CourseID,
                Title = a.Title,
                Description = a.Description,
                Price = a.Price,
                Instructor = new CourseGetDto.User {
                    Name = a.InstructorNavigation.Name,
                    Email = a.InstructorNavigation.Email,
                }
            };
        }

        private static CourseGetDto CourseDetailToCourseGetDto(CourseDetail a)
        {
            return new CourseGetDto
            {
                CourseID = a.CourseID,
                Title = a.Course.Title,
                Description = a.Course.Description,
                Price = a.Course.Price,
                Instructor = new CourseGetDto.User
                {
                    Name = a.Course.InstructorNavigation.Name,
                    Email = a.Course.InstructorNavigation.Email,
                }
            };
        }



    }
}

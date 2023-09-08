using serverDemo.Models;

namespace serverDemo.Dtos
{
    public class CourseGetDto
    {
        public Guid CourseID { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Price { get; set; } = null!;

        public User Instructor { get; set; } = null!;

        public class User
        {
            public string Name { get; set; } = null!;

            public string Email { get; set; } = null!;
        }
    }

    
}

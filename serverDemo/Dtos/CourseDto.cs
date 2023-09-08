namespace serverDemo.Dtos
{
    public class CourseDto
    {
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Price { get; set; } = null!;

        public Guid Instructor { get; set; }
    }
}

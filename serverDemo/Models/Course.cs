using System;
using System.Collections.Generic;

namespace serverDemo.Models;

public partial class Course
{
    public Guid CourseID { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Price { get; set; } = null!;

    public Guid Instructor { get; set; }

    public virtual ICollection<CourseDetail> CourseDetail { get; set; } = new List<CourseDetail>();

    public virtual User InstructorNavigation { get; set; } = null!;
}

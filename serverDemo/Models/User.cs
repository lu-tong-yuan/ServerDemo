using System;
using System.Collections.Generic;

namespace serverDemo.Models;

public partial class User
{
    public Guid UserID { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime Date { get; set; }

    public virtual ICollection<Course> Course { get; set; } = new List<Course>();

    public virtual ICollection<CourseDetail> CourseDetail { get; set; } = new List<CourseDetail>();
}

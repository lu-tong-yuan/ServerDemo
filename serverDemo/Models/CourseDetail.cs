using System;
using System.Collections.Generic;

namespace serverDemo.Models;

public partial class CourseDetail
{
    public Guid CourseID { get; set; }

    public Guid UserID { get; set; }

    public DateTime Date { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

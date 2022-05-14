using System;
using System.Collections.Generic;

namespace Coaching.Data.Core.Coaching.Entities
{
    public partial class Course
    {
        public Course()
        {
            CourseLesson = new HashSet<CourseLesson>();
        }

        public int Id { get; set; }
        public string Video { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Process { get; set; } = null!;
        public int SpecialityLevelId { get; set; }
        public int Order { get; set; }

        public virtual SpecialityLevel SpecialityLevel { get; set; } = null!;
        public virtual ICollection<CourseLesson> CourseLesson { get; set; }
    }
}

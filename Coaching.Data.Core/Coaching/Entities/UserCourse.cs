using System;
using System.Collections.Generic;

namespace Coaching.Data.Core.Coaching.Entities
{
    public partial class UserCourse
    {
        public UserCourse()
        {
            UserCourseLesson = new HashSet<UserCourseLesson>();
        }

        public int Id { get; set; }
        public int CourseId { get; set; }
        public int UserSpecialityLevelId { get; set; }
        public bool IsFinish { get; set; }
        public int? Time { get; set; }
        public int UserId { get; set; }

        public virtual UserSpecialityLevel UserSpecialityLevel { get; set; } = null!;
        public virtual ICollection<UserCourseLesson> UserCourseLesson { get; set; }
    }
}

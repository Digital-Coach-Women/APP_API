using System;
using System.Collections.Generic;

namespace Coaching.Data.Core.Coaching.Entities
{
    public partial class UserCourseLesson
    {
        public int Id { get; set; }
        public int UserCourseId { get; set; }
        public bool IsFinish { get; set; }
        public int Order { get; set; }
        public int UserId { get; set; }

        public virtual UserCourse UserCourse { get; set; } = null!;
    }
}

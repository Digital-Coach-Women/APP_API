using System;
using System.Collections.Generic;

namespace Coaching.Data.Core.Coaching.Entities
{
    public partial class NotificationUser
    {
        public int Id { get; set; }
        public bool SendCourses { get; set; }
        public bool SendFollow { get; set; }
        public bool SendAdvice { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
    }
}

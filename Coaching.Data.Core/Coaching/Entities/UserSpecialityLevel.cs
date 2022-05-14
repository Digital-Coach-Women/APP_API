using System;
using System.Collections.Generic;

namespace Coaching.Data.Core.Coaching.Entities
{
    public partial class UserSpecialityLevel
    {
        public UserSpecialityLevel()
        {
            UserCourse = new HashSet<UserCourse>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int SpecialityLevelId { get; set; }
        public bool IsFinish { get; set; }

        public virtual SpecialityLevel SpecialityLevel { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<UserCourse> UserCourse { get; set; }
    }
}

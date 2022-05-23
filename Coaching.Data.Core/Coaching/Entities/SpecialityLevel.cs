using System;
using System.Collections.Generic;

namespace Coaching.Data.Core.Coaching.Entities
{
    public partial class SpecialityLevel
    {
        public SpecialityLevel()
        {
            Course = new HashSet<Course>();
            SpecialityLevelCertificate = new HashSet<SpecialityLevelCertificate>();
            UserSpecialityLevel = new HashSet<UserSpecialityLevel>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int SpecialityId { get; set; }
        public string CupImage { get; set; } = null!;
        public int Order { get; set; }
        public bool IsBasic { get; set; }
        public string Level { get; set; } = null!;

        public virtual Speciality Speciality { get; set; } = null!;
        public virtual ICollection<Course> Course { get; set; }
        public virtual ICollection<SpecialityLevelCertificate> SpecialityLevelCertificate { get; set; }
        public virtual ICollection<UserSpecialityLevel> UserSpecialityLevel { get; set; }
    }
}

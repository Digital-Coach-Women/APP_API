using System;
using System.Collections.Generic;

namespace Coaching.Data.Core.Coaching.Entities
{
    public partial class Speciality
    {
        public Speciality()
        {
            SpecialityLevel = new HashSet<SpecialityLevel>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;

        public virtual ICollection<SpecialityLevel> SpecialityLevel { get; set; }
    }
}

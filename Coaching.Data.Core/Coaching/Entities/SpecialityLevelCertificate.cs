using System;
using System.Collections.Generic;

namespace Coaching.Data.Core.Coaching.Entities
{
    public partial class SpecialityLevelCertificate
    {
        public int Id { get; set; }
        public string Uri { get; set; } = null!;
        public string Company { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int SpecialityLevelId { get; set; }

        public virtual SpecialityLevel SpecialityLevel { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;

namespace Coaching.Data.Core.Coaching.Entities
{
    public partial class SuccessStoires
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Profession { get; set; } = null!;
        public int Age { get; set; }
        public string City { get; set; } = null!;
        public string Appointment { get; set; } = null!;
    }
}

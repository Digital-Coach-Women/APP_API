using System;
using System.Collections.Generic;

namespace Coaching.Data.Core.Coaching.Entities
{
    public partial class Chat
    {
        public int Id { get; set; }
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
        public DateTime LastCommunicateDate { get; set; }

        public virtual User UserId1Navigation { get; set; } = null!;
        public virtual User UserId2Navigation { get; set; } = null!;
    }
}

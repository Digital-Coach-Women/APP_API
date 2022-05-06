using System;
using System.Collections.Generic;

namespace Coaching.Data.Core.Coaching.Entities
{
    public partial class ChatSession
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public int ChatId { get; set; }

        public virtual Chat Chat { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}

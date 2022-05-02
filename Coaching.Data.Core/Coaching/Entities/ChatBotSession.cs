using System;
using System.Collections.Generic;

namespace Coaching.Data.Core.Coaching.Entities
{
    public partial class ChatBotSession
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public bool IsBot { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ChatBotId { get; set; }

        public virtual ChatBot ChatBot { get; set; } = null!;
    }
}

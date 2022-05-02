using System;
using System.Collections.Generic;

namespace Coaching.Data.Core.Coaching.Entities
{
    public partial class ChatBot
    {
        public ChatBot()
        {
            ChatBotSession = new HashSet<ChatBotSession>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<ChatBotSession> ChatBotSession { get; set; }
    }
}

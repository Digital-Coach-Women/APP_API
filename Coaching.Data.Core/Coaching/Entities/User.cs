using System;
using System.Collections.Generic;

namespace Coaching.Data.Core.Coaching.Entities
{
    public partial class User
    {
        public User()
        {
            ChatBot = new HashSet<ChatBot>();
            ChatSession = new HashSet<ChatSession>();
            ChatUserId1Navigation = new HashSet<Chat>();
            ChatUserId2Navigation = new HashSet<Chat>();
            NotificationUser = new HashSet<NotificationUser>();
            UserSpecialityLevel = new HashSet<UserSpecialityLevel>();
        }

        public int Id { get; set; }
        public string Names { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string MotherLastName { get; set; } = null!;
        public DateTime Birthdate { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Token { get; set; }
        public string Linkedin { get; set; } = null!;
        public int UserLevel { get; set; }
        public string? FcmToken { get; set; }

        public virtual ICollection<ChatBot> ChatBot { get; set; }
        public virtual ICollection<ChatSession> ChatSession { get; set; }
        public virtual ICollection<Chat> ChatUserId1Navigation { get; set; }
        public virtual ICollection<Chat> ChatUserId2Navigation { get; set; }
        public virtual ICollection<NotificationUser> NotificationUser { get; set; }
        public virtual ICollection<UserSpecialityLevel> UserSpecialityLevel { get; set; }
    }
}

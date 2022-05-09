using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coaching.Helper.FirestoreModel
{
    [FirestoreData]
    public class ChatModel
    {
        [FirestoreProperty(Name = "userId")]
        public int UserId { get; set; }

        [FirestoreProperty(Name = "userName")]
        public string UserName { get; set; }

        [FirestoreProperty(Name = "message")]
        public string Message { get; set; }

        [FirestoreProperty(Name = "date")]
        public DateTime Date { get; set; }
    }
}

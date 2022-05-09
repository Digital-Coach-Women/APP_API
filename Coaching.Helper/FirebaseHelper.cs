using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;

namespace Coaching.Helper
{
    public static class FirebaseHelper
    {
        private static FirestoreDb _db;
        static FirebaseHelper()
        {
            if (_db == null)
            {
                var client = new FirestoreClientBuilder
                {
                    JsonCredentials = Resource.firebase
                }.Build();
                _db = FirestoreDb.Create("coaching-27078", client);
            }
        }

        public static async Task<bool> AddChat(string document, int userId, string userName, string message)
        {
            try
            {
                DocumentReference docRef = _db.Collection("chats").Document(document);
                Dictionary<string, object> service = new Dictionary<string, object>
            {
                { "userId", userId },
                { "userName", userName},
                { "date",  DateTime.Today},
                { "message", message},
            };
                await docRef.SetAsync(service);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

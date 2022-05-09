using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coaching.Helper
{
    public static class FirebaseAuthHelper
    {
        private static bool init = false;
        static FirebaseAuthHelper()
        {
            if (!init)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromJson(Resource.firebase)
                });
                init = true;    
            }
        }

        public static async Task<bool> RegisterUser(string email, string password)
        {
            try
            {
                UserRecordArgs args = new UserRecordArgs()
                {
                    Email = email,
                    Password = password,
                    EmailVerified = false,
                    Disabled = false,
                };
                UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(args);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<string> GetTokenByEmail(string email)
        {
            try
            {
                var user = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);
                var token = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(user.Uid);
                return token;
            }
            catch
            {
                throw new ArgumentException("El usuario no existe (fib).");
            }
        }
    }
}

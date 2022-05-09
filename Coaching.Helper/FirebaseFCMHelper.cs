using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Coaching.Helper
{
    public static class FirebaseFCMHelper
    {
        private static string serverKey = "AAAAXyKlwr4:APA91bFwJ4hBhE5_i6-jKMq0_UKvfIx8y9vKC_iR2lYjnT0ijUG6VLE1YHGpxOYlrAbD8UEc1n-vMCvMKgobYbgxxwzK8p5eYpYTD6eGMi_IUZVAPoCt6IySSMsfz8ijTsG72RbhVF51";
        private static string senderId = "408603181758";
        private static string webAddr = "https://fcm.googleapis.com/fcm/send";

        public static bool SendPushNotification(string DeviceToken, string title, string msg)
        {
            try
            {
                var result = "-1";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                httpWebRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                httpWebRequest.Method = "POST";

                var payload = new
                {
                    to = DeviceToken,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        body = msg,
                        title = title
                    },
                };
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(payload);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}

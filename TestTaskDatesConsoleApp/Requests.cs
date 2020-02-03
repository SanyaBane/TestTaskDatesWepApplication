using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TestTaskDatesCommon.Models;
using TestTaskDatesCommon.Payloads;

namespace TestTaskDatesConsoleApp
{
    public class Requests
    {
        public const string HOST_ADDRESS = "https://localhost:5001";

        public const int WEB_TIMEOUT = 5000;

        public List<DateRange> GetAllDateRanges()
        {
            string url = HOST_ADDRESS + "/api/Date/get/all";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = WEB_TIMEOUT;
            request.Method = "GET";

            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                var encoding = ASCIIEncoding.ASCII;
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    string responseText = reader.ReadToEnd();

                    var allDatesList = JsonConvert.DeserializeObject<List<DateRange>>(responseText);

                    return allDatesList;
                }
            }
        }

        public LoginResultPayload TryToLogin(User user)
        {
            string url = HOST_ADDRESS + "/api/Account/Token";

            string postData = "";
            postData += nameof(user.Login) + "=" + Uri.EscapeDataString(user.Login);
            postData += "&" + nameof(user.Password) + "=" + Uri.EscapeDataString(user.Password);
            var postDataArray = Encoding.ASCII.GetBytes(postData);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = WEB_TIMEOUT;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataArray.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(postDataArray, 0, postDataArray.Length);
            }

            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                var encoding = ASCIIEncoding.ASCII;
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    string responseText = reader.ReadToEnd();

                    var loginResultPayload = JsonConvert.DeserializeObject<LoginResultPayload>(responseText);

                    return loginResultPayload;
                }
            }
        }

        public GeneralResponsePayload TryToRegister(User user)
        {
            string url = HOST_ADDRESS + "/api/Account/Register";

            string postData = "";
            postData += nameof(user.Login) + "=" + Uri.EscapeDataString(user.Login);
            postData += "&" + nameof(user.Password) + "=" + Uri.EscapeDataString(user.Password);
            var postDataArray = Encoding.ASCII.GetBytes(postData);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = WEB_TIMEOUT;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataArray.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(postDataArray, 0, postDataArray.Length);
            }

            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                var encoding = ASCIIEncoding.ASCII;
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    string responseText = reader.ReadToEnd();

                    var reponsePayload = JsonConvert.DeserializeObject<GeneralResponsePayload>(responseText);

                    return reponsePayload;
                }
            }
        }

        public GeneralResponsePayload TryToInsertNewDateRange(DateRange dateRange, string authToken)
        {
            string url = HOST_ADDRESS + "/api/Date/insert_date_range";

            string postData = "";
            postData += nameof(dateRange.Start) + "=" + dateRange.Start.ToShortDateString();
            postData += "&" + nameof(dateRange.End) + "=" + dateRange.End.ToShortDateString();
            var postDataArray = Encoding.ASCII.GetBytes(postData);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = WEB_TIMEOUT;
            request.Method = "PUT";
            request.Headers.Add("Authorization", "Bearer " + authToken);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataArray.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(postDataArray, 0, postDataArray.Length);
            }

            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                var encoding = ASCIIEncoding.ASCII;
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    string responseText = reader.ReadToEnd();

                    var reponsePayload = JsonConvert.DeserializeObject<GeneralResponsePayload>(responseText);

                    return reponsePayload;
                }
            }
        }

        public List<DateRange> GetDateRangeIntersects(DateRange dateRange, string authToken)
        {
            string url = HOST_ADDRESS + "/api/Date/get_date_range_intersect";

            string postData = "";
            postData += nameof(dateRange.Start) + "=" + dateRange.Start.ToShortDateString();
            postData += "&" + nameof(dateRange.End) + "=" + dateRange.End.ToShortDateString();
            var postDataArray = Encoding.ASCII.GetBytes(postData);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = WEB_TIMEOUT;
            request.Method = "POST";
            request.Headers.Add("Authorization", "Bearer " + authToken);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataArray.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(postDataArray, 0, postDataArray.Length);
            }

            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                var encoding = ASCIIEncoding.ASCII;
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    string responseText = reader.ReadToEnd();

                    var allDatesList = JsonConvert.DeserializeObject<List<DateRange>>(responseText);

                    return allDatesList;
                }
            }
        }
    }
}

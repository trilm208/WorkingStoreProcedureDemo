using DataAccess;
using Xamarin.Forms;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;


namespace DataAccess
{
    public class HttpDataServices
    {
        
        public string HttpURL { get; private set; }

        public CookieContainer HttpCookies { get; private set; }

        public string HttpUserAgent { get; private set; }

        private static ManualResetEvent allDone = new ManualResetEvent(false);

        HttpClient client;

        public HttpDataServices(string webServiceURL)
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            this.HttpURL =String.Format(webServiceURL);
            this.HttpCookies = new CookieContainer();
            this.HttpUserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";

        }

        public async Task<DataSet> ExecuteAsync(RequestCollection requests)
        {
            var xml = requests.ToXml();
            var text = await DependencyService.Get<IHttpPost>().HttpPostAsync(xml, this);
            var bytes = Convert.FromBase64String(text);
            var ds = DependencyService.Get<ISerializer>().Decompress<DataSet>(bytes);
            return ds;
        }

        /// <summary>
        /// If you dont need Zip support and dont want use DependencyService
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="httpDataServices"></param>
        /// <returns></returns>
        private async Task<String> HttpPostAsync(string xml, HttpDataServices httpDataServices)
        {
            try
            {
                var request = (HttpWebRequest)HttpWebRequest.Create(HttpURL);
                request.CookieContainer = HttpCookies;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                //request.UserAgent = HttpUserAgent;
                //request.AllowAutoRedirect = true;
                //request.Headers.Add("Accept-Encoding", "gzip,deflate");
                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);

                using (var requestStream = await Task.Factory.FromAsync(request.BeginGetRequestStream, request.EndGetRequestStream, request))
                {
                    await requestStream.WriteAsync(data, 0, data.Length);
                }
                WebResponse responseObject = await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, request);
                var responseStream = responseObject.GetResponseStream();
                var sr = new StreamReader(responseStream);
                string received = await sr.ReadToEndAsync();

                return received;
            }
            catch(Exception ex)
            {
                MyDebugger.Log(ex.Message);
                return "";
            }
           
            //using (var writer = new StreamWriter(request.GetRequestStream()))
            //{
            //    if (string.IsNullOrEmpty(data) == false)
            //    {
            //        writer.Write(data);
            //    }
            //}
            //try
            //{
            //    var webResponse = await request.GetResponseAsync();

            //    using (var stream = webResponse.GetResponseStream())
            //    {
            //        using (var reader = new StreamReader(stream))
            //        {
            //            var result = reader.ReadToEnd();
            //            return result;
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
        }

        
    }
}
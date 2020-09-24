using Newtonsoft.Json;
using QuanLyNoiBo.ViewModels.System.Users;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNoiBoAdminApp.Services
{
    public class UserAPI : IUserAPI
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public UserAPI(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<string> Authenticate(LoginRequest loginRequest)
        {
            //var client = new RestClient("https://localhost:5001/api/GetEntities?function=all&data1=Conek&data2=ON");
            //client.Timeout = -1;
            //var request1 = new RestRequest(Method.GET);
            //IRestResponse response = client.Execute(request1);
            string html = string.Empty;
            string url = @"https://localhost:5001/api/GetEntities?function=all&data1=Conek&data2=ON";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            var token = html;
            return token;
        }
    }
}

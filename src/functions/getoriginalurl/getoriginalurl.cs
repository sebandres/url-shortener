using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Net;

namespace getoriginalurl
{
    public class Program
    {
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req)
        {
            //log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            string name = req.RequestUri.Query.Split('&')
                .FirstOrDefault(q => string.Compare(q.Split('=')[0], "name", true) == 0)
                .Split('=')[1];

            // Get request body
            var data = await req.Content.ReadAsStringAsync();

            // Set name to query string or body data
            name = name ?? data;

            return name == null
                ? new HttpResponseMessage(HttpStatusCode.BadRequest)
                : new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}

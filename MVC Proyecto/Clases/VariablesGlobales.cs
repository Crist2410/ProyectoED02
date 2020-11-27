using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MVC_Proyecto
{
    public class VariablesGlobales
    {
        public static HttpClient WebApiClient = new HttpClient();

        static VariablesGlobales()
        {
            WebApiClient.BaseAddress = new Uri("http://8a8adfec37ba.ngrok.io/api/");
            WebApiClient.DefaultRequestHeaders.Clear();
            WebApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}

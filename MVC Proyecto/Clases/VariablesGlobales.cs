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
            //WebApiClient.BaseAddress = new Uri("http://a2379cfd6145.ngrok.io/api/");
            WebApiClient.BaseAddress = new Uri("http://localhost:61258/api/");
            WebApiClient.DefaultRequestHeaders.Clear();
            WebApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}

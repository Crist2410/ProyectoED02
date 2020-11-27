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
<<<<<<< HEAD
            WebApiClient.BaseAddress = new Uri("http://001aa2ae8dbc.ngrok.io/api/");
=======
            //WebApiClient.BaseAddress = new Uri("http://5362ff4b1f21.ngrok.io/api/");
            WebApiClient.BaseAddress = new Uri("http://localhost:61258/api/");
>>>>>>> 0705121d3eb2bc4ef40dc3d64559f531cf70907d
            WebApiClient.DefaultRequestHeaders.Clear();
            WebApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}

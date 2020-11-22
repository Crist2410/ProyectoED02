using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MVC_Proyecto.Clases
{
    public class ContextoGlobal
    {
        public static HttpClient WeClient = new HttpClient();

        static ContextoGlobal()
        {
            WeClient.BaseAddress = new Uri("http://localhost:61258/api/usuarios");
            WeClient.DefaultRequestHeaders.Clear();
            WeClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            WeClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
        }
    }
}

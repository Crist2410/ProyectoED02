﻿using System;
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
<<<<<<< HEAD
<<<<<<< HEAD
            WebApiClient.BaseAddress = new Uri("http://001aa2ae8dbc.ngrok.io/api/");
=======
            //WebApiClient.BaseAddress = new Uri("http://5362ff4b1f21.ngrok.io/api/");
=======
            //WebApiClient.BaseAddress = new Uri("http://a2379cfd6145.ngrok.io/api/");
>>>>>>> bba14478a6eefd79cee8dc9c482ce4fd823aaf05
            WebApiClient.BaseAddress = new Uri("http://localhost:61258/api/");
>>>>>>> 0705121d3eb2bc4ef40dc3d64559f531cf70907d
=======
            WebApiClient.BaseAddress = new Uri("http://5a0f0664351e.ngrok.io/api/");
           // WebApiClient.BaseAddress = new Uri("http://localhost:61258/api/");
>>>>>>> fa14a4b776af3ae0f72097fa107289d3a06f38da
            WebApiClient.DefaultRequestHeaders.Clear();
            WebApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}

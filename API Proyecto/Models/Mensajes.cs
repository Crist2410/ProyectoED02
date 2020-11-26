using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Proyecto.Models
{
    public class Mensajes
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        public string Texto { get; set; }
        public string Emisor { get; set; }
        public string Receptor { get; set; }
        public string Chat { get; set; }
        public DateTime Fecha { get; set; }
    }
}

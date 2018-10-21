using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml.Linq;

namespace Seminario.Models
{
    [DataContract]
    public class Ejemplo
    {
        public string Nombre { get; set; }

        public int Numero { get; set; }
    }
}
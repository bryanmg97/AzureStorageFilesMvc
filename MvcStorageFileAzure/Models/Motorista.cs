using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcStorageFileAzure.Models
{
    public class Motorista
    {
        public String Nombre { get; set; }
        public String Dorsal { get; set; }
        public String Titulos { get; set; }
        public String Debut { get; set; }
        public List<Equipo> Equipo { get; set; }
    }
}
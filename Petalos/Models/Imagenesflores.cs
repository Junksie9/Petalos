using System;
using System.Collections.Generic;

#nullable disable

namespace Petalos.Models
{
    public partial class Imagenesflores
    {
        public uint Idimagen { get; set; }
        public string Nombreimagen { get; set; }
        public uint Idflor { get; set; }

        public virtual Datosflores IdflorNavigation { get; set; }
    }
}

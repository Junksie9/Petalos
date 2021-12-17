using System;
using System.Collections.Generic;

#nullable disable

namespace Petalos.Models
{
    public partial class Datosflores
    {
        public Datosflores()
        {
            Imagenesflores = new HashSet<Imagenesflores>();
        }

        public uint Idflor { get; set; }
        public string Nombrecientifico { get; set; }
        public string Nombrecomun { get; set; }
        public string Origen { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Imagenesflores> Imagenesflores { get; set; }
    }
}

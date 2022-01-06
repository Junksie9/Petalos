using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Petalos.Models;

namespace Petalos.Areas.Admin.Models
{
    public class FloresViewModel
    {
        public IEnumerable<Datosflores> df { get; set; }
        public IEnumerable<Imagenesflores> Imagenesflores { get; set;  }
        public Datosflores Datosflores { get; set; }

        public Imagenesflores imagenesflores { get; set; }
    }
}

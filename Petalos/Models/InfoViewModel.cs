using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Petalos.Models;

namespace Petalos.Models
{
    public class InfoViewModel
    {
        public Datosflores Datosflores { get; set; }
        public IEnumerable<Datosflores> CuatroF { get; set; }
    }
}

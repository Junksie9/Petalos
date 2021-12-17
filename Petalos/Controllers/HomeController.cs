using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Petalos.Models;

namespace Petalos.Controllers
{
    public class HomeController : Controller
    {
        public floresContext Context { get; }

        public HomeController(floresContext context)
        {
            Context = context;
        }
        public IActionResult Index()
        {
            var f = Context.Datosflores.OrderBy(x => x.Nombre);
            return View(f);
        }

        [Route("{id}/Datos")]
        public IActionResult Flor(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Replace("-", " ");
                var flor = Context.Datosflores.Include(x => x.Imagenesflores).FirstOrDefault(x => x.Nombre == id);
                if (flor == null)
                {
                    return RedirectToAction("Index");
                }
                InfoViewModel vm = new InfoViewModel();
                vm.Datosflores = flor;
                Random ra = new Random();
                vm.CuatroF = Context.Datosflores
                    .Where(x => x.Idflor != flor.Idflor)
                    .ToList()
                    .OrderBy(x => ra.Next())
                    .Take(4)
                    .Select(x => new Datosflores { Idflor = x.Idflor, Nombre = x.Nombre });
                return View(vm);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}
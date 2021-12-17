using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Petalos.Models;
using Petalos.Areas.Admin.Models;

namespace Petalos.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        public floresContext Context { get; }

        public AdminController(floresContext context)
        {
            Context = context;
        }
        [Route("admin/Home")]
        [Route("admin/Home/Index")]
        [Route("admin")]
        public IActionResult Index()
        {
            var f = Context.Datosflores.OrderBy(x => x.Nombre);
            return View(f);
        }

        [Route("admin/AgregarFlor")]
        public IActionResult AgregarFlor()
        {
            return View();
        }

        [Route("admin/AgregarFlor")]
        [HttpPost]
        public IActionResult AgregarFlor(FloresViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Datosflores.Nombre)| string.IsNullOrWhiteSpace(vm.Datosflores.Nombrecientifico) || 
                string.IsNullOrWhiteSpace(vm.Datosflores.Nombrecomun) || string.IsNullOrWhiteSpace(vm.Datosflores.Origen) || 
                string.IsNullOrWhiteSpace(vm.Datosflores.Descripcion))
            {
                ModelState.AddModelError("", "No puede haber ningun espacio en blanco");
            }
            else
            {
                vm.df = Context.Datosflores.OrderBy(x => x.Nombre);
                Context.Add(vm.Datosflores);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
           
            return View(vm);
        }

        [Route("admin/AgregarImagenes")]
        public IActionResult AgregarImagenes()
        {
            return View();
        }

        [Route("admin/EditarFlor")]
        [HttpGet]
        public IActionResult EditarFlor(uint id)
        {
            FloresViewModel vm = new FloresViewModel();
            var flor = Context.Datosflores.FirstOrDefault(x=>x.Idflor == id);
            if (flor == null)
            {
                return RedirectToAction("Index");
            }
           
            vm.Datosflores = flor;
            return View(vm);
        }

        [Route("admin/EditarFlor")]
        [HttpPost]
        public IActionResult EditarFlor(FloresViewModel vm)
        {
           
            return View();
        }

    }

    
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Petalos.Models;
using Petalos.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;

namespace Petalos.Controllers
{
    [Area("Admin")]
    [Authorize]
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
            if (string.IsNullOrWhiteSpace(vm.Datosflores.Nombre) | string.IsNullOrWhiteSpace(vm.Datosflores.Nombrecientifico) ||
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

        [Route("admin/AgregarImagenes/{id}")]
        public IActionResult AgregarImagenes(uint id)
        {
            FloresViewModel vm = new FloresViewModel();
            var flor = Context.Imagenesflores.FirstOrDefault(x => x.Idflor == id);
            if (flor == null)
            {
                return RedirectToAction("Index");
            }
            vm.imagenesflores = flor;
            return View(vm);
        }

        [Route("admin/AgregarImagenes")]
        [HttpPost]
        public IActionResult AgregarImagenes()
        {
            return View();
        }

        [Route("admin/EditarFlor/{id}")]
        [HttpGet]
        public IActionResult EditarFlor(uint id)
        {
            FloresViewModel vm = new FloresViewModel();
            var flor = Context.Datosflores.FirstOrDefault(x => x.Idflor == id);
            if (flor == null)
            {
                return RedirectToAction("Index");
            }
            vm.Datosflores = flor;
            return View(vm);
        }

        [Route("admin/EditarFlor/{id}")]
        [HttpPost]
        public IActionResult EditarFlor(FloresViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Datosflores.Nombre) | string.IsNullOrWhiteSpace(vm.Datosflores.Nombrecientifico) ||
               string.IsNullOrWhiteSpace(vm.Datosflores.Nombrecomun) || string.IsNullOrWhiteSpace(vm.Datosflores.Origen) ||
               string.IsNullOrWhiteSpace(vm.Datosflores.Descripcion))
            {
                ModelState.AddModelError("", "No puede haber ningun espacio en blanco");
            }
            else
            {
                var editarflor = Context.Datosflores.FirstOrDefault(x => x.Idflor == vm.Datosflores.Idflor);
                if (editarflor == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    editarflor.Idflor = vm.Datosflores.Idflor;
                    editarflor.Nombre = vm.Datosflores.Nombre;
                    editarflor.Nombrecientifico = vm.Datosflores.Nombrecientifico;
                    editarflor.Nombrecomun = vm.Datosflores.Nombrecomun;
                    editarflor.Origen = vm.Datosflores.Origen;
                    editarflor.Descripcion = vm.Datosflores.Descripcion;
                    Context.Update(editarflor);
                    Context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(vm);
        }

        [Route("admin/Eliminar/{id}")]
        public IActionResult Eliminar(uint id)
        {
            var flor = Context.Datosflores.FirstOrDefault(x => x.Idflor == id);
            if (flor == null)
            {
                return RedirectToAction("Index");
            }
            return View(flor);
        }

        [Route("admin/Eliminar/{id}")]
        [HttpPost]
        public IActionResult Eliminar(Datosflores df)
        {
            var flor = Context.Datosflores.FirstOrDefault(x => x.Idflor == df.Idflor);

            Context.Remove(flor);
            Context.SaveChanges();
            return RedirectToAction("Index");

        }
    }


}
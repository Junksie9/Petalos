using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Petalos.Models;
using Petalos.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Petalos.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminController : Controller
    {
        public floresContext Context { get; }
        public IWebHostEnvironment Host { get; }

        public AdminController(floresContext context, IWebHostEnvironment host)
        {
            Context = context;
            Host = host;
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
            //vm.Imagenesflores = Context.Imagenesflores.OrderBy(x => x.Nombreimagen).Where(x => x.Idflor == id);
            //var flor = Context.Imagenesflores.Include(x => x.IdflorNavigation).FirstOrDefault(x => x.Idflor == id);
            //vm.Datosflores = Context.Datosflores.FirstOrDefault(x => x.Idflor == id);
            //var f = Context.Datosflores.Include(x=>x.Imagenesflores).FirstOrDefault(x => x.Idflor == id);
            //if (f == null)
            //{
            //    return RedirectToAction("Index");
            //}
            //vm.imagenesflores = flor;
            //vm.Datosflores = f;
            //return View(vm);

            var f = Context.Datosflores.Include(x => x.Imagenesflores).FirstOrDefault(x => x.Idflor == id);
            if (f == null)
            {
                return RedirectToAction("Index");
            }

            vm.imagenesflores = Context.Imagenesflores.FirstOrDefault(x => x.Idflor == f.Idflor);
            vm.Datosflores = f;

            vm.df = Context.Datosflores.OrderBy(x => x.Nombre);

            return View(vm);

        }

        [Route("admin/AgregarImagenes")]
        [HttpPost]
        public IActionResult AgregarImagenes(FloresViewModel vm, IFormFile imagen1)
        {
            if (imagen1 == null)
            {



                vm.df = Context.Datosflores.OrderBy(x => x.Nombre);
                ModelState.AddModelError("", "No hay ninguna fotografia");
                return View(vm);
            }
            if (imagen1 != null)
            {
                if (imagen1.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("", "Solo imagenes jpeg");
                }
                if (imagen1.Length > 1024 * 1024 * 5)
                {
                    ModelState.AddModelError("", "Archivo muy grande");
                }
            }
            vm.imagenesflores.Nombreimagen = imagen1.FileName;
            Context.Add(vm.imagenesflores);
            Context.SaveChanges();
            if (imagen1 != null)
            {
                var flor = Context.Imagenesflores.FirstOrDefault(x => x.Idimagen == vm.imagenesflores.Idimagen);
                if (flor == null)
                {
                    return RedirectToAction("Index");
                }
                var path = Host.WebRootPath + "/images/" + imagen1.FileName;
                FileStream fs = new FileStream(path, FileMode.Create);
                imagen1.CopyTo(fs);
                fs.Close();

                return RedirectToAction("Index");
            }

            return View(vm);
        }
        [Route("Admin/AgregarImagenes/{id}/EliminarImagen")]
        public IActionResult EliminarImagen(uint id)
        {
            var imagen = Context.Imagenesflores.FirstOrDefault(x => x.Idimagen == id);
            if (imagen == null)
            {
                return RedirectToAction("Index");
            }
            return View(imagen);
        }
        [Route("Admin/AgregarImagenes/{id}/EliminarImagen/")]
        [HttpPost]
        public IActionResult EliminarImagen(Imagenesflores i)
        {
            var imagen = Context.Imagenesflores.FirstOrDefault(x => x.Idimagen == i.Idimagen);
            if (imagen == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var path = Host.WebRootPath + "/images/" + i.Nombreimagen;
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                Context.Remove(imagen);
                Context.SaveChanges();

                return RedirectToAction("Index");
            }
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
            var img = Context.Imagenesflores.FirstOrDefault(x => x.Idflor == id);
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
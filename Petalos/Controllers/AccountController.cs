using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Petalos.Models;
using Petalos.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Petalos.Controllers
{
    public class AccountController : Controller
    {

        public AccountController(floresContext context)
        {
            Context = context;
        }

        public floresContext Context { get; }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {

            var hash = Helpers.Cifrado.GetHash(password);
            var user = Context.Usuarios.SingleOrDefault(x => x.NombreUsuario == username && x.Password == hash);
            if (user != null)
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user.NombreReal));
                claims.Add(new Claim(ClaimTypes.Role, user.Rol));
                claims.Add(new Claim("Id", user.Id.ToString()));

                var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(new ClaimsPrincipal(identidad));

                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else
            {
                ModelState.AddModelError("", "Usuario o Contraseña incorrectos");
                return View();
            }

        }
    }
}

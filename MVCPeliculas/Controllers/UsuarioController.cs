using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCPeliculas.Context;
using MVCPeliculas.Migrations;
using MVCPeliculas.Models;

namespace MVCPeliculas.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly PeliculaDatabaseContext _context;

        public UsuarioController(PeliculaDatabaseContext context)
        {
            _context = context;
        }

        // GET: Usuario/Registro
        [AllowAnonymous]  
        public IActionResult Registro()
        {
            return View();
        }

        // POST: Usuario/Registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Registro([Bind("Id,Nombre,Apellido,Mail,Contrasenia,Rol")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                bool mailExistente = _context.Usuario.Any(c => c.Mail == usuario.Mail);

                if (mailExistente)
                {

                    ModelState.AddModelError("Mail", "Este correo ya se ecuentra registrado");

                    return View(usuario);
                }

                _context.Add(usuario);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Usuario");
            }
            return View(usuario);
        }

        // GET: Usuario/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Usuario/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Mail == loginViewModel.Mail && u.Contrasenia == loginViewModel.Contrasenia);

                if (usuario != null)
                {
                    ClaimsIdentity identidad = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                    identidad.AddClaim(new Claim(ClaimTypes.Email, usuario.Mail));
                    identidad.AddClaim(new Claim(ClaimTypes.GivenName, usuario.Nombre));
                    identidad.AddClaim(new Claim(ClaimTypes.Role, usuario.Rol.ToString()));
                    identidad.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()));

                    ClaimsPrincipal principal = new ClaimsPrincipal(identidad);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Pelicula");
                }
            }

            ModelState.AddModelError(string.Empty, "Usuario y/o contraseña incorrectos");

            return View();
        }

        // GET: Usuario/Logout
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        // GET: Usuario
        [Authorize(Roles = nameof(Rol.Admin))]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuario.ToListAsync());
        }

        // GET: Usuario/Details/1
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FirstOrDefaultAsync(m => m.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuario/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            var usuarioActualId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var usuarioActuaRol = User.FindFirstValue(ClaimTypes.Role);

            if (usuario.Id.ToString() != usuarioActualId || usuarioActuaRol != nameof(Rol.Admin))
            {
                return Forbid();
            }

            return View(usuario);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Mail,Contrasenia,Rol")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Pelicula");
            }
            return RedirectToAction("Index", "Pelicula");
        }

        // GET: Usuario/Delete/5
        [Authorize(Roles = nameof(Rol.Admin))]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FirstOrDefaultAsync(m => m.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(Rol.Admin))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = nameof(Rol.Admin))]
        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.Id == id);
        }
    }
}

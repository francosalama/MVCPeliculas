using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCPeliculas.Context;
using MVCPeliculas.Models;

namespace MVCPeliculas.Controllers
{
    [Authorize]
    public class PeliculaController : Controller
    {
        private readonly PeliculaDatabaseContext _context;

        public PeliculaController(PeliculaDatabaseContext context)
        {
            _context = context;
        }

        // GET: Pelicula
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pelicula.ToListAsync());
        }

        // GET: Pelicula/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Pelicula
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // GET: Pelicula/Create
        [Authorize(Roles = nameof(Rol.Admin))]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pelicula/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(Rol.Admin))]
        public async Task<IActionResult> Create([Bind("Id,Nombre,LinkFoto,Valoracion,Anio,Sinopsis,Genero,Plataforma")] Pelicula pelicula)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pelicula);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pelicula);
        }

        // GET: Pelicula/Edit/5
        [Authorize(Roles = nameof(Rol.Admin))]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Pelicula.FindAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }
            return View(pelicula);
        }

        // POST: Pelicula/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(Rol.Admin))]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,LinkFoto,Valoracion,Anio,Sinopsis,Genero,Plataforma")] Pelicula pelicula)
        {
            if (id != pelicula.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pelicula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeliculaExists(pelicula.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pelicula);
        }

        // GET: Pelicula/Delete/5
        [Authorize(Roles = nameof(Rol.Admin))]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Pelicula
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // POST: Pelicula/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(Rol.Admin))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pelicula = await _context.Pelicula.FindAsync(id);
            _context.Pelicula.Remove(pelicula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Pelicula/AgregarPeliculaVista/5
        [Authorize]
        public async Task<IActionResult> AgregarPeliculaVista(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Pelicula
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // POST: Pelicula/AgregarPeliculaVista/5
        [HttpPost, ActionName("AgregarPeliculaVista")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AgregarPeliculaVistaConfirmed(int id)
        {
            var idUsuario = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var peliculaDB = await _context.PeliculaVista.Where(p => p.UsuarioId == idUsuario && p.PeliculaId == id).Include(p => p.Pelicula).FirstOrDefaultAsync();
            if (peliculaDB == null)
            {
                PeliculaVista peliculaVista = new PeliculaVista();
                peliculaVista.UsuarioId = idUsuario;
                peliculaVista.PeliculaId = id;
                _context.PeliculaVista.Add(peliculaVista);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.ErrorPelicula = "Esta pelicula ya fue agregada como vista";
                return View(peliculaDB.Pelicula);
            }
        }

        // GET: Pelicula/AgregarPeliculaDeseada/5
        [Authorize]
        public async Task<IActionResult> AgregarPeliculaDeseada(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Pelicula
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // POST: Pelicula/AgregarPeliculaDeseada/5
        [HttpPost, ActionName("AgregarPeliculaDeseada")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AgregarPeliculaDeseadaConfirmed(int id)
        {
            var idUsuario = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var peliculaDB = await _context.PeliculaDeseada.Where(p => p.UsuarioId == idUsuario && p.PeliculaId == id).Include(p => p.Pelicula).FirstOrDefaultAsync();
            if (peliculaDB == null)
            {
                PeliculaDeseada peliculaDeseada = new PeliculaDeseada();
                peliculaDeseada.UsuarioId = idUsuario;
                peliculaDeseada.PeliculaId = id;
                _context.PeliculaDeseada.Add(peliculaDeseada);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.ErrorPelicula = "Esta pelicula ya fue agregada como deseada";
                return View(peliculaDB.Pelicula);
            }
        }

        [Authorize]
        public async Task<IActionResult> Buscar(string nombre)
        {
            var peliculas = await _context.Pelicula.Where(p => p.Nombre.StartsWith(nombre)).ToListAsync();
            return View(peliculas);
        }

        [Authorize]
        public async Task<IActionResult> Filtrar(int valoracion, int genero)
        {
            List<Pelicula> peliculas = new List<Pelicula>();
            if (valoracion != 0 && genero != -1)
            {
                peliculas = await _context.Pelicula.Where(p => p.Genero == (Genero) Enum.GetValues(typeof(Genero)).GetValue(genero) && p.Valoracion == valoracion).ToListAsync();
            }
            else if (valoracion != 0)
            {
                peliculas = await _context.Pelicula.Where(p => p.Valoracion == valoracion).ToListAsync();
            }
            else if (genero != -1)
            {
                peliculas = await _context.Pelicula.Where(p => p.Genero == (Genero) Enum.GetValues(typeof(Genero)).GetValue(genero)).ToListAsync();
            }

            return View(peliculas);
        }

        [Authorize(Roles = nameof(Rol.Admin))]
        private bool PeliculaExists(int id)
        {
            return _context.Pelicula.Any(e => e.Id == id);
        }
    }
}

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
    public class PeliculaDeseadaController : Controller
    {
        private readonly PeliculaDatabaseContext _context;

        public PeliculaDeseadaController(PeliculaDatabaseContext context)
        {
            _context = context;
        }

        // GET: PeliculaDeseada
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var idUsuario = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var peliculaDatabaseContext = _context.PeliculaDeseada.Where(p => p.UsuarioId == idUsuario).Include(p => p.Pelicula).Include(p => p.Usuario);
            return View(await peliculaDatabaseContext.ToListAsync());
        }

        // GET: PeliculaDeseada/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peliculaDeseada = await _context.PeliculaDeseada
                .Include(p => p.Pelicula)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (peliculaDeseada == null)
            {
                return NotFound();
            }

            return View(peliculaDeseada);
        }

        // GET: PeliculaDeseada/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peliculaDeseada = await _context.PeliculaDeseada
                .Include(p => p.Pelicula)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (peliculaDeseada == null)
            {
                return NotFound();
            }

            return View(peliculaDeseada);
        }

        // POST: PeliculaDeseada/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var peliculaDeseada = await _context.PeliculaDeseada.FirstOrDefaultAsync(m => m.Id == id);
            _context.PeliculaDeseada.Remove(peliculaDeseada);
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
        [Authorize]
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
                var peliculaDeseada = await _context.PeliculaDeseada.Where(p => p.UsuarioId == idUsuario && p.PeliculaId == id).Include(p => p.Pelicula).FirstOrDefaultAsync();
                _context.PeliculaVista.Add(peliculaVista);
                _context.PeliculaDeseada.Remove(peliculaDeseada);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.ErrorPelicula = "Esta pelicula ya fue agregada como vista";
                return View(peliculaDB.Pelicula);
            }
        }
        [Authorize]
        private bool PeliculaDeseadaExists(int id)
        {
            return _context.PeliculaDeseada.Any(e => e.UsuarioId == id);
        }
    }
}

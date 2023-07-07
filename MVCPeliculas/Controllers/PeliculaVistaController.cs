using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCPeliculas.Context;
using MVCPeliculas.Models;

namespace MVCPeliculas.Controllers
{
    public class PeliculaVistaController : Controller
    {
        private readonly PeliculaDatabaseContext _context;

        public PeliculaVistaController(PeliculaDatabaseContext context)
        {
            _context = context;
        }

        // GET: PeliculaVista
        public async Task<IActionResult> Index()
        {
            var idUsuario = 1;
            var peliculaDatabaseContext = _context.PeliculaVista.Where(p => p.UsuarioId == idUsuario).Include(p => p.Pelicula).Include(p => p.Usuario);
            return View(await peliculaDatabaseContext.ToListAsync());
        }

        // GET: PeliculaVista/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peliculaVista = await _context.PeliculaVista
                .Include(p => p.Pelicula)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (peliculaVista == null)
            {
                return NotFound();
            }

            return View(peliculaVista);
        }


        // GET: PeliculaVista/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peliculaVista = await _context.PeliculaVista
                .Include(p => p.Pelicula)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (peliculaVista == null)
            {
                return NotFound();
            }

            return View(peliculaVista);
        }

        // POST: PeliculaVista/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var peliculaVista = await _context.PeliculaVista.FirstOrDefaultAsync(m => m.Id == id);
            _context.PeliculaVista.Remove(peliculaVista);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeliculaVistaExists(int id)
        {
            return _context.PeliculaVista.Any(e => e.UsuarioId == id);
        }
    }
}

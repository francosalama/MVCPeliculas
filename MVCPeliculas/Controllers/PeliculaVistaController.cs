using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCPeliculas.Context;

namespace MVCPeliculas.Controllers
{
    [Authorize]
    public class PeliculaVistaController : Controller
    {
        private readonly PeliculaDatabaseContext _context;

        public PeliculaVistaController(PeliculaDatabaseContext context)
        {
            _context = context;
        }

        // GET: PeliculaVista
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var idUsuario = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var peliculaDatabaseContext = _context.PeliculaVista.Where(p => p.UsuarioId == idUsuario).Include(p => p.Pelicula).Include(p => p.Usuario);
            return View(await peliculaDatabaseContext.ToListAsync());
        }

        // GET: PeliculaVista/Details/5
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var peliculaVista = await _context.PeliculaVista.FirstOrDefaultAsync(m => m.Id == id);
            _context.PeliculaVista.Remove(peliculaVista);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        private bool PeliculaVistaExists(int id)
        {
            return _context.PeliculaVista.Any(e => e.UsuarioId == id);
        }
    }
}

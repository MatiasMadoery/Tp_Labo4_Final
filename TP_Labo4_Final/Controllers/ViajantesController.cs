using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP_Labo4_Final.Models;

namespace TP_Labo4_Final.Controllers
{
    public class ViajantesController : Controller
    {
        private readonly AppDbContext _context;

        public ViajantesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Viajantes
        public async Task<IActionResult> Index()
        {
            //Incluir lo clientes y sus pedidos en cada viajante
            var viajantes = await _context.Viajantes
                .Include(v => v.Clientes!)
                .ThenInclude(c => c.Pedidos!)
                .ToListAsync();
            return View(viajantes);
        }

        // GET: Viajantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viajante = await _context.Viajantes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (viajante == null)
            {
                return NotFound();
            }

            return View(viajante);
        }

        // GET: Viajantes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Viajantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,Cuit,Direccion,Altura,Departamento,Piso,Localidad,Provincia,Pais,Telefono,Email,Estado,PedidoId")] Viajante viajante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(viajante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viajante);
        }

        // GET: Viajantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viajante = await _context.Viajantes.FindAsync(id);
            if (viajante == null)
            {
                return NotFound();
            }
            return View(viajante);
        }

        // POST: Viajantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Cuit,Direccion,Altura,Departamento,Piso,Localidad,Provincia,Pais,Telefono,Email,Estado,PedidoId")] Viajante viajante)
        {
            if (id != viajante.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viajante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ViajanteExists(viajante.Id))
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
            return View(viajante);
        }

        // GET: Viajantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viajante = await _context.Viajantes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (viajante == null)
            {
                return NotFound();
            }

            return View(viajante);
        }

        // POST: Viajantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var viajante = await _context.Viajantes.FindAsync(id);
            if (viajante != null)
            {
                _context.Viajantes.Remove(viajante);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ViajanteExists(int id)
        {
            return _context.Viajantes.Any(e => e.Id == id);
        }
    }
}

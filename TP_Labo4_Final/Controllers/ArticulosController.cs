using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP_Labo4_Final.Models;
using System.IO;

namespace TP_Labo4_Final.Controllers
{
    public class ArticulosController : Controller
    {
        private readonly AppDbContext _context;

        //Publicar una imagen
        private readonly IWebHostEnvironment _env;

        public ArticulosController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Articulos
        public async Task<IActionResult> Index()
        {
            //Incluir las categorias en cada articulo.
            var articulos = await _context.Articulos
                .Include(v => v.Categoria!)                
                .ToListAsync();
            return View(articulos);
        }

        // GET: Articulos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos
                .Include(a => a.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }

        // GET: Articulos/Create
        public IActionResult Create()
        {
            // Cargar las categorías disponibles en el ViewBag
            ViewBag.CategoriaId = new SelectList(_context.Categorias, "Id", "Nombre");
            return View();
        }

        // POST: Articulos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Descripcion,Precio,Stock,NombreImagen,CategoriaId")] Articulo articulo)
        {
            if (ModelState.IsValid)
            {
                //Para subir imagen
                var archivos = HttpContext.Request.Form.Files;//Subir archivos de imagen
                if (archivos != null && archivos.Count > 0)
                {
                    var archivoFoto = archivos[0];
                    if (archivoFoto.Length > 0)
                    {
                        //Generar nombre aleatoria foto
                        var pathDestino = Path.Combine(_env.WebRootPath, "img\\images");

                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "");

                        var extension = Path.GetExtension(archivoFoto.FileName);
                        archivoDestino += extension;


                        using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
                        {
                            archivoFoto.CopyTo(filestream);
                            articulo.NombreImagen = archivoDestino;
                        }
                    }
                }
                //fin subir imagen               

                _context.Add(articulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Volver a cargar las categorías si hay algún error de validación
            ViewBag.CategoriaId = new SelectList(_context.Categorias, "Id", "Nombre", articulo.CategoriaId);
            return View(articulo);
        }

        // GET: Articulos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null)
            {
                return NotFound();
            }

            // Cargar todas las categorías en un SelectList para mostrarlas en la vista
            ViewBag.CategoriaId = new SelectList(_context.Categorias, "Id", "Nombre", articulo.CategoriaId);

            return View(articulo);
        }

        // POST: Articulos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Descripcion,Precio,Stock,CategoriaId,NombreImagen")] Articulo articulo)
        {
            if (id != articulo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //Para editar imagen
                var archivos = HttpContext.Request.Form.Files;//Subir archivos de imagen
                if (archivos != null && archivos.Count > 0)
                {
                    var archivoFoto = archivos[0];
                    if (archivoFoto.Length > 0)
                    {
                        var pathDestino = Path.Combine(_env.WebRootPath, "img\\images");

                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "");

                        var extension = Path.GetExtension(archivoFoto.FileName);
                        archivoDestino += extension;


                        using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
                        {
                            archivoFoto.CopyTo(filestream);
                            if (articulo.NombreImagen != null)
                            {
                                var archivoViejo = Path.Combine(pathDestino, articulo.NombreImagen);
                                if (System.IO.File.Exists(archivoViejo))
                                {
                                    System.IO.File.Delete(archivoViejo);
                                }
                            }
                            articulo.NombreImagen = archivoDestino;
                        }
                    }
                }
                //fin editar imagen
                try
                {
                    _context.Update(articulo);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Articulo modificado correctamente!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticuloExists(articulo.Id))
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

            // Si hay un error, volver a cargar la lista de categorías
            ViewBag.CategoriaId = new SelectList(_context.Categorias, "Id", "Nombre", articulo.CategoriaId);

            return View(articulo);
        }

        // GET: Articulos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }

        // POST: Articulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo != null)
            {
                _context.Articulos.Remove(articulo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticuloExists(int id)
        {
            return _context.Articulos.Any(e => e.Id == id);
        }
    }
}

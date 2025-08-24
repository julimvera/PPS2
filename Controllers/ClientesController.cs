using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp_Peluqueria.Data;
using WebApp_Peluqueria.Models;

namespace WebApp_Peluquería.Controllers
{
    public class ClientesController : Controller
    {
        private readonly PeluqueriaContext _context;

        public ClientesController(PeluqueriaContext context)
        {
            _context = context;
        }


        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            _context.Add(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            return View(cliente);
        }

        // Index con filtro de búsqueda
        public async Task<IActionResult> Index(string buscar)
        {
            // Si no se envía búsqueda, muestra todos los clientes
            var clientesQuery = _context.Cliente.AsQueryable();

            if (!string.IsNullOrEmpty(buscar))
            {
                // Filtrar por nombre o apellido que contenga el texto buscado (insensible a mayúsculas)
                clientesQuery = clientesQuery.Where(c =>
                    c.Nombre.Contains(buscar) || c.Apellido.Contains(buscar));
            }

            var clientes = await clientesQuery.ToListAsync();

            ViewBag.Buscar = buscar;

            return View(clientes);
        }



        // GET: Clientes/Detalle/5
        public async Task<IActionResult> Detalle(int id)
        {
            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null) return NotFound();

            cliente.Tratamientos = await _context.Tratamiento
                .Where(t => t.ClienteId == id)
                .OrderByDescending(t => t.Fecha)
                .ToListAsync();

            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarTratamiento(Tratamiento tratamiento)
        {
            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(c => c.Id == tratamiento.ClienteId);

            if (cliente == null)
                return NotFound();

            // Si no se seleccionó fecha, poner hoy
            if (tratamiento.Fecha == default(DateTime))
            {
                tratamiento.Fecha = DateTime.Today;
            }

            _context.Tratamiento.Add(tratamiento);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "✅ Tratamiento agregado con éxito.";

            return RedirectToAction("Detalle", new { id = tratamiento.ClienteId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _context.Cliente
                .Include(c => c.Tratamientos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
                return NotFound();

            if (cliente.Tratamientos != null && cliente.Tratamientos.Any())
            {
                _context.Tratamiento.RemoveRange(cliente.Tratamientos);
            }

            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: Clientes/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Cliente cliente)
        {
     

            var clienteExistente = await _context.Cliente.FindAsync(cliente.Id);
            if (clienteExistente == null)
            {
                return NotFound();
            }

        
            clienteExistente.Nombre = cliente.Nombre;
            clienteExistente.Apellido = cliente.Apellido;
            clienteExistente.Telefono = cliente.Telefono;
            clienteExistente.Observaciones = cliente.Observaciones;
           

            try
            {
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Cliente actualizado con éxito.";
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al guardar los cambios.");
                return View(cliente);
            }

            return RedirectToAction("Index");
        }
    }

}




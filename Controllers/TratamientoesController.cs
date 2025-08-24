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
    public class TratamientosController : Controller
    {
        private readonly PeluqueriaContext _context;

        public TratamientosController(PeluqueriaContext context)
        {
            _context = context;
        }

        // Editar tratamiento
        public async Task<IActionResult> Editar(int id)
        {
            var tratamiento = await _context.Tratamiento.FindAsync(id);
            if (tratamiento == null) return NotFound();

            ViewBag.Tipos = new List<string> { "Tintura", "Corte", "Peinado", "Shock de keratina" };
            return View(tratamiento);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Tratamiento t)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Tipos = new List<string> { "Tintura", "Corte", "Peinado", "Shock de keratina" };
                return View(t);
            }

            _context.Tratamiento.Update(t);
            await _context.SaveChangesAsync();

            return RedirectToAction("Detalle", "Clientes", new { id = t.ClienteId });
        }

        // Eliminar tratamiento (confirmación simple)
        public async Task<IActionResult> Eliminar(int id)
        {
            var tratamiento = await _context.Tratamiento.FindAsync(id);
            if (tratamiento == null) return NotFound();
            return View(tratamiento);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var tratamiento = await _context.Tratamiento.FindAsync(id);
            if (tratamiento == null) return NotFound();

            _context.Tratamiento.Remove(tratamiento);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "🗑️ Tratamiento eliminado con éxito.";

            return RedirectToAction("Detalle", "Clientes", new { id = tratamiento.ClienteId });
        }
    }
}

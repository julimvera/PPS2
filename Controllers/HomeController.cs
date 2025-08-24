using WebApp_Peluqueria.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp_Peluqueria.Data;
using WebApp_Peluqueria.Models;
using WebApp_Peluquería.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp_Peluquería.Controllers
{
    public class HomeController : Controller
    {
        private readonly PeluqueriaContext _context;

        public HomeController(PeluqueriaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var hoy = DateTime.Today;

            var tratamientos = await _context.Tratamiento
                .Include(t => t.Cliente)
                .Where(t => t.Fecha.Date == hoy)
                .ToListAsync();

            var ventas = await _context.VentaProducto
                .Include(v => v.Cliente)
                .Where(v => v.Fecha.Date == hoy)
                .ToListAsync();

            var planilla = tratamientos.Select(t => new PlanillaDiaViewModel
            {
                Cliente = t.Cliente.Nombre + " " + t.Cliente.Apellido,
                Tratamiento = t.Tipo,
                Producto = null,
                Precio = t.Precio
            }).ToList();

            planilla.AddRange(ventas.Select(v => new PlanillaDiaViewModel
            {
                Cliente = v.Cliente.Nombre + " " + v.Cliente.Apellido,
                Tratamiento = null,
                Producto = v.Producto,
                Precio = v.Precio
            }));

            ViewBag.Total = planilla.Sum(p => p.Precio);

            return View(planilla);
        }

        public IActionResult ResumenFinanciero(int? anio, int? mes)
        {
            anio ??= DateTime.Today.Year;
            mes ??= DateTime.Today.Month;

            // Obtener tratamientos del mes seleccionado
            var tratamientos = _context.Tratamiento
                .Include(t => t.Cliente)  // Esto trae el cliente
                .Where(t => t.Fecha.Year == anio && t.Fecha.Month == mes)
                .ToList();

            // Calcular totales
            ViewBag.TotalMes = tratamientos.Sum(t => t.Precio);
            ViewBag.TotalAnio = _context.Tratamiento
                .Where(t => t.Fecha.Year == anio)
                .Sum(t => t.Precio);

            ViewBag.Anio = anio;
            ViewBag.Mes = mes;

            return View(tratamientos);
        }
    }
}
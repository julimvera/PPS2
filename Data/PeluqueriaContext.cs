using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApp_Peluqueria.Models;
using WebApp_Peluquería.Models;

namespace WebApp_Peluqueria.Data
{
    public class PeluqueriaContext : DbContext
    {
        public PeluqueriaContext(DbContextOptions<PeluqueriaContext> options) : base(options) { }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Tratamiento> Tratamiento { get; set; }
        public DbSet<VentaProducto> VentaProducto { get; set; }
        public DbSet<CierreDiario> CierresDiarios { get; set; }
    }
}

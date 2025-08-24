using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp_Peluqueria.Models
{
    public class VentaProducto
    {
        public int Id { get; set; }
        public string Producto { get; set; }
        [Precision(10, 2)]
        public decimal Precio { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}
   

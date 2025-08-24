using Microsoft.EntityFrameworkCore;

namespace WebApp_Peluqueria.Models
{
    public class Tratamiento
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public string Tipo { get; set; } // Seleccionable: corte, tintura, etc.
        public string Descripcion { get; set; } // Texto libre: tintura 8.3 + oxidante, etc.
        [Precision(10, 2)]
        public decimal Precio { get; set; } // Ingresado a mano
        public DateTime Fecha { get; set; } // Cargado automáticamente o manual
    }
}

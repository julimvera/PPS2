using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp_Peluqueria.Models
{
    public class CierreDiario
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalGanancia { get; set; }
    }
}


﻿namespace WebApp_Peluqueria.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string ?Observaciones { get; set; }

        public List<Tratamiento> Tratamientos { get; set; }
    }
}
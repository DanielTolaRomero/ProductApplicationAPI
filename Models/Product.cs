using System.ComponentModel.DataAnnotations;
using System.Data;

namespace WebApplicationPractica.Models
{
    public class Product
    {
        public int id { get; set; }
        [Required (ErrorMessage = "El nombre del producto es obligatorio")]
        [MinLength(3)]
        public string  name { get; set; }
        [Required (ErrorMessage = "La categoria es obligatorio")]
        [MinLength(3)]
        public string category { get; set; }
        [Required (ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El valor debe ser un número positivo mayor a 0.")]
        public decimal price { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public bool active { get; set; } = true;
    }
}

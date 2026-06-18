using System.ComponentModel.DataAnnotations;
using System.Data;

namespace WebApplicationPractica.Models
{
    public class Product
    {
        public int id { get; set; }

        public string  name { get; set; }

        public decimal price { get; set; }

        public Category category { get; set; } = null;
        public int categoryId { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public bool active { get; set; } = true;
    }
}
